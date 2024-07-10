using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    // material -> change color
    // make connection with collider changed
    // player owner
    // block connection on/off
    [Header("Block Inspector")]
    [SerializeField] BlockType blockType;

    [Header("Moving Type Block Setting")]
    [SerializeField] bool MoveInverse;
    [SerializeField] float MoveSpeed;
    [SerializeField] Vector3 moveToPos;
    [SerializeField] List<GameObject> MovePoints;
    [SerializeField] Queue<Vector3> MoveQueue;

    [Header("Block Material Setup")]
    [SerializeField] Color originColor;
    [SerializeField] Material blockMat;
    [SerializeField] Material currentMat;
    [SerializeField] MeshRenderer meshRenderer;

    [Header("Block Collider Setup")]
    [SerializeField] BoxCollider blockColl;
    [SerializeField] GameObject dimensionCollObj;
    [SerializeField] GameObject dimensionCollRenderObj;

    [Header("Portal Functions")]
    [SerializeField] bool isConnected;
    [SerializeField] Vector3 WarpDirection;
    [SerializeField] BlockScript ConnectedBlockObject;
    bool isBlock;
    public bool BlockCheck { get { return isBlock; } set { isBlock = value; } }

    [Header("External Setup")]
    [SerializeField] int OwnerPlayerID = -1;


    public enum EnumWarpDirection
    { 
        UP,
        DOWN,
        LEFT,
        RIGHT,
        NONE
    }


    public BlockType getBlockType()
    {
        return blockType;
    }

    void Start()
    {
        if (blockType == BlockType.Normal || blockType == BlockType.Moving)
        {
            originColor = blockMat.color;
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material = Instantiate(blockMat);
            currentMat = meshRenderer.material;

        }


        if (blockType == BlockType.Moving)
        {
            MoveQueue = new Queue<Vector3>();
        }

        dimensionCollObj.SetActive(false);
    }

    void Update()
    {
        BlockMoving();
    }

    private void BlockMoving()
    {
        if (blockType != BlockType.Moving) return;

        if (MoveQueue.Count == 0 && !MoveInverse)
        {
            int mCount = MovePoints.Count;
            for (int inum = 0; inum < mCount; inum++)
            {
                MoveQueue.Enqueue(MovePoints[inum].transform.position);
            }
            MoveInverse = true;
            moveToPos = MoveQueue.Dequeue();
        }
        else if (MoveQueue.Count == 0 && MoveInverse)
        {
            int mCount = MovePoints.Count;
            for (int inum = mCount; inum > 0; inum--)
            {
                MoveQueue.Enqueue(MovePoints[inum - 1].transform.position);
            }
            MoveInverse = false;
            moveToPos = MoveQueue.Dequeue();
        }

        if (MoveQueue.Count != 0)
        {
            if (Vector3.Distance(moveToPos, transform.position) < 0.001f)
            {
                moveToPos = MoveQueue.Dequeue();
            }

            transform.position = Vector3.MoveTowards(transform.position, moveToPos, MoveSpeed * Time.deltaTime);
        }
    }

    public void changeColorFromPlayer(PlayerTag _playerID)
    {
        if (PlayerTag.Player1 == _playerID)
        {
            currentMat.color = BlockColoring.GetBlockColor(BlockColorTag.Player1);
            dimensionCollRenderObj.GetComponent<MeshRenderer>().material.color = BlockColoring.GetBlockColor(BlockColorTag.Player1);


        }
        else if (PlayerTag.Player2 == _playerID)
        {
            currentMat.color = BlockColoring.GetBlockColor(BlockColorTag.Player2);
            dimensionCollRenderObj.GetComponent<MeshRenderer>().material.color = BlockColoring.GetBlockColor(BlockColorTag.Player2);
        }
        else
        {
            currentMat.color = BlockColoring.GetBlockColor(BlockColorTag.Normal);
            dimensionCollRenderObj.GetComponent<MeshRenderer>().material.color = BlockColoring.GetBlockColor(BlockColorTag.Normal);
        }
    }

    public void changeColorFromSwitch(Color _color)
    {
        GetComponent<MeshRenderer>().material.color = _color;
    }

    public void createDimension(Vector3 _norm)
    {
        Vector3 DimensionPostion;
        EnumDimension dimm = DimensionClass.GetFromDynamicDimension(_norm);
        DimensionPostion = DimensionClass.GetFromDimensionDirection(dimm);

        WarpDirection = DimensionPostion;

        if (ConnectedBlockObject == null) return;

        openDimenstion();
    }

    public void openDimenstion()
    {

        if (WarpDirection == Vector3.zero) return;
        // blockColl.enabled = false;
        dimensionCollObj.transform.position = transform.position + WarpDirection;
        dimensionCollObj.SetActive(true);

        if (WarpDirection == Vector3.left)
        {
            dimensionCollRenderObj.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (WarpDirection == Vector3.right)
        {
            dimensionCollRenderObj.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
    }

    public void closeDimension()
    {
        dimensionCollObj.SetActive(false);
    }

    public void shutDownDimension()
    {
        BlockCheck = false;
        OwnerPlayerID = -1;
        dimensionCollObj.transform.position = transform.position;
        dimensionCollRenderObj.transform.rotation = transform.rotation;
        WarpDirection = Vector3.zero;
        UnSyncronizeBlock();
        changeColorFromPlayer(PlayerTag.None);
        dimensionCollObj.SetActive(false);
    }

    public Vector3 WarpToOtherSide()
    {
        return dimensionCollObj.transform.position;
    }

    public Vector3 getWarpDirection()
    {
        return WarpDirection;
    }

    public void SynchronizeBlock(BlockScript _otherBlock)
    {
        ConnectedBlockObject = _otherBlock;
        isConnected = true;
    }

    public void UnSyncronizeBlock()
    {
        ConnectedBlockObject = null;
        isConnected = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player") && blockType == BlockType.Trap)
        {
            PlayerManager.instance.EventHappend = true;
            GameManager.instance.StageGameOver();
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("block"))
    //    {
    //        BlockCheck = true;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("block"))
    //    {
    //        BlockCheck = false;
    //    }
    //}


    public void TriggerOnDimensionObject(Collider other)
    {

        if (!other.GetComponent<PlayerScript>().passedCheck()) return;

        if (ConnectedBlockObject.GetComponent<BlockScript>().BlockCheck || BlockCheck) return;

        Vector3 TargetPos = ConnectedBlockObject.GetComponent<BlockScript>().WarpToOtherSide();
        other.transform.position = TargetPos;

        Vector3 currentRigid = other.GetComponent<Rigidbody>().velocity;

        Vector3 TargetWarpDirection = ConnectedBlockObject.GetComponent<BlockScript>().getWarpDirection();

        if (TargetWarpDirection == Vector3.up)
        {
            other.transform.position += (Vector3.up / 2f);
        }
        else if (TargetWarpDirection == Vector3.down)
        {
            other.transform.position += (Vector3.down / 2f);
        }

        /*
         UP UP
        DOWN UP
        LEFT UP
        RIGHT UP

        UP DOWN
        DOWN DOWN
        LEFT DOWN
        RIGHT DOWN
         */

        float x = currentRigid.x > 0 ? Mathf.Clamp(currentRigid.x, 2f, 20f) : Mathf.Clamp(currentRigid.x, -20f, -2f);
        float y = currentRigid.y > 0 ? Mathf.Clamp(currentRigid.y, 2f, 20f) : Mathf.Clamp(currentRigid.y, -20f, -2f);
        currentRigid = new Vector3(x, y, 0f);
        Rigidbody rigid = other.GetComponent<Rigidbody>();


        if (WarpDirection == Vector3.up && TargetWarpDirection == Vector3.up)
        {
            currentRigid.y = (Mathf.Abs(currentRigid.x) + Mathf.Abs(currentRigid.y));
            currentRigid.x = 0f;
            rigid.velocity = Vector3.zero;
            rigid.AddForce(currentRigid + Vector3.up, ForceMode.Impulse);
        }
        else if (WarpDirection == Vector3.down && TargetWarpDirection == Vector3.up)
        {
            currentRigid.y = Mathf.Abs(currentRigid.x) + Mathf.Abs(currentRigid.y);
            currentRigid.x = 0f;
            rigid.velocity = Vector3.zero;
            rigid.AddForce(currentRigid + Vector3.up, ForceMode.Impulse);
        }
        else if (WarpDirection == Vector3.left && TargetWarpDirection == Vector3.up)
        {
            currentRigid.y = Mathf.Abs(currentRigid.x) + Mathf.Abs(currentRigid.y);
            currentRigid.x = 0f;

            rigid.velocity = Vector3.zero;
            rigid.AddForce(currentRigid + Vector3.up, ForceMode.Impulse);
        }
        else if (WarpDirection == Vector3.right && TargetWarpDirection == Vector3.up)
        {
            currentRigid.y = Mathf.Abs(currentRigid.x) + Mathf.Abs(currentRigid.y);
            currentRigid.x = 0f;
            currentRigid = -currentRigid;
            rigid.velocity = Vector3.zero;
            rigid.AddForce(currentRigid + Vector3.up, ForceMode.Impulse);
        }
        else if (WarpDirection == Vector3.up && TargetWarpDirection == Vector3.down)
        {
            other.GetComponent<PlayerScript>().InfinityDimension = 0.1f;
            currentRigid.y = -(Mathf.Abs(currentRigid.x) + Mathf.Abs(currentRigid.y));
            currentRigid.x = 0f;
            rigid.velocity = Vector3.zero;
            rigid.AddForce(currentRigid + Vector3.down, ForceMode.Impulse);
        }
        else if (WarpDirection == Vector3.down && TargetWarpDirection == Vector3.down)
        {
            currentRigid.y = -(Mathf.Abs(currentRigid.x) + Mathf.Abs(currentRigid.y));
            currentRigid.x = 0f;
            rigid.velocity = Vector3.zero;
            rigid.AddForce(currentRigid + Vector3.down, ForceMode.Impulse);
        }
        else if (WarpDirection == Vector3.left && TargetWarpDirection == Vector3.down)
        {
            currentRigid.y = Mathf.Abs(currentRigid.x) + Mathf.Abs(currentRigid.y);
            currentRigid.x = 0f;
            currentRigid = -currentRigid;
            rigid.velocity = Vector3.zero;
            rigid.AddForce(currentRigid + Vector3.down, ForceMode.Impulse);
        }
        else if (WarpDirection == Vector3.right && TargetWarpDirection == Vector3.down)
        {
            currentRigid.y = Mathf.Abs(currentRigid.x) + Mathf.Abs(currentRigid.y);
            currentRigid.x = 0f;
            currentRigid = -currentRigid;
            rigid.velocity = Vector3.zero;
            rigid.AddForce(currentRigid + Vector3.down, ForceMode.Impulse);
        }
        else if (WarpDirection == Vector3.up && TargetWarpDirection == Vector3.left)
        {
            currentRigid.x = -(Mathf.Abs(currentRigid.x) + Mathf.Abs(currentRigid.y));
            currentRigid.y = 0f;
            rigid.velocity = Vector3.zero;
            rigid.AddForce(currentRigid + Vector3.left * 3f, ForceMode.Impulse);
        }
        else if (WarpDirection == Vector3.down && TargetWarpDirection == Vector3.left)
        {
            currentRigid.x = -(Mathf.Abs(currentRigid.x) + Mathf.Abs(currentRigid.y));
            currentRigid.y = 0f;
            rigid.velocity = Vector3.zero;
            rigid.AddForce(currentRigid + Vector3.left, ForceMode.Impulse);
        }
        else if (WarpDirection == Vector3.left && TargetWarpDirection == Vector3.left)
        {
            currentRigid.x = -(Mathf.Abs(currentRigid.x) + Mathf.Abs(currentRigid.y));
            currentRigid.y = 0f;

            rigid.velocity = currentRigid + new Vector3(-4f, 0);
        }
        else if (WarpDirection == Vector3.right && TargetWarpDirection == Vector3.left)
        {
            currentRigid.x = -(Mathf.Abs(currentRigid.x) + Mathf.Abs(currentRigid.y));
            currentRigid.y = 0f;
            rigid.velocity = Vector3.zero;
            rigid.AddForce(currentRigid + Vector3.left, ForceMode.Impulse);
        }
        else if (WarpDirection == Vector3.up && TargetWarpDirection == Vector3.right)
        {

            currentRigid.x = Mathf.Abs(currentRigid.x) + Mathf.Abs(currentRigid.y);
            currentRigid.y = 0f;
            rigid.velocity = Vector3.zero;
            rigid.AddForce(currentRigid + Vector3.right * 1f, ForceMode.Impulse);
        }
        else if (WarpDirection == Vector3.down && TargetWarpDirection == Vector3.right)
        {
            currentRigid.x = Mathf.Abs(currentRigid.x) + Mathf.Abs(currentRigid.y);
            currentRigid.y = 0f;
            rigid.velocity = Vector3.zero;
            rigid.AddForce(currentRigid + Vector3.right * 1f, ForceMode.Impulse);
        }
        else if (WarpDirection == Vector3.left && TargetWarpDirection == Vector3.right)
        {
            currentRigid.x = Mathf.Abs(currentRigid.x) + Mathf.Abs(currentRigid.y);
            currentRigid.y = 0f;
            rigid.velocity = Vector3.zero;
            rigid.AddForce(currentRigid + Vector3.right * 1f, ForceMode.Impulse);
        }
        else if (WarpDirection == Vector3.right && TargetWarpDirection == Vector3.right)
        {
            currentRigid.x = Mathf.Abs(currentRigid.x) + Mathf.Abs(currentRigid.y);
            currentRigid.y = 0f;
            rigid.velocity = Vector3.zero;
            rigid.AddForce(currentRigid + Vector3.right * 4f, ForceMode.Impulse);
        }
    }
}
