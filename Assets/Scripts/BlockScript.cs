using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    // material -> change color
    // make connection with collider changed
    // player owner
    // block connection on/off
    [Header("Block Material Setup")]
    [SerializeField] Color originColor;
    [SerializeField] Material blockMat;
    [SerializeField] Material currentMat;
    [SerializeField] MeshRenderer meshRenderer;

    [Header("Block Collider Setup")]
    [SerializeField] BoxCollider blockColl;
    [SerializeField] GameObject dimensionCollObj;

    [Header("Portal Functions")]
    [SerializeField] bool isConnected;
    [SerializeField] Vector3 WarpDirection;
    [SerializeField] BlockScript ConnectedBlockObject;

    [Header("External Setup")]
    [SerializeField] int OwnerPlayerID = -1;


    void Start()
    {
        originColor = blockMat.color;
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = Instantiate(blockMat);
        currentMat = meshRenderer.material;
        dimensionCollObj.SetActive(false);
    }

    void Update()
    {

    }

    public void changeColorFromPlayer(int _playerID) {
        if (_playerID == 0)
        {
            // currentMat.SetColor("_Color", Color.black);
            // blockMat.color = Color.red;
            currentMat.color = BlockColoring.GetBlockColor(BlockColorTag.Player1);
        }
        else if (_playerID == 1)
        {
            // currentMat.SetColor("_Color", new Color(80f / 255f, 188f / 255f, 223f / 255f, 1f));
            currentMat.color = BlockColoring.GetBlockColor(BlockColorTag.Player2);
        }
        else {
            currentMat.color = BlockColoring.GetBlockColor(BlockColorTag.Normal);
        }
    }

    public void createDimension(Vector3 _norm) 
    {
        Vector3 DimensionPostion;

        if (Mathf.Abs(_norm.x) > Mathf.Abs(_norm.y))
        {
            if (_norm.x > 0)
            {
                DimensionPostion = Vector3.left / 2;
            }
            else
            {
                DimensionPostion = Vector3.right / 2;
            }
        }
        else
        {
            if (_norm.y < 0)
            {
                DimensionPostion = Vector3.down / 2;
            }
            else
            {
                DimensionPostion = Vector3.up/ 2;
            }
        }

        WarpDirection = DimensionPostion;

        if (ConnectedBlockObject == null) return;

        openDimenstion();
    }

    public void openDimenstion()
    {
        
        if (WarpDirection == Vector3.zero) return;
        Debug.Log(WarpDirection);
        blockColl.enabled = false;
        dimensionCollObj.transform.position += WarpDirection; 
        dimensionCollObj.SetActive(true);
    }

    public void shutDownDimension() 
    {
        OwnerPlayerID = -1;
        blockColl.enabled = true;
        dimensionCollObj.transform.position = transform.position;
        WarpDirection = Vector3.zero;
        UnSyncronizeBlock();
        changeColorFromPlayer(-1);
        dimensionCollObj.SetActive(false);
    }

    public Vector3 WarpToOtherSide() { 
        return dimensionCollObj.transform.position;
    }

    public void SynchronizeBlock(BlockScript _otherBlock) {
        ConnectedBlockObject = _otherBlock;
        isConnected = true;
    }

    public void UnSyncronizeBlock()
    {
        ConnectedBlockObject = null;
        isConnected = false;
    }

    public void TriggerOnDimensionObject(Collider other) {
        Debug.Log("Collider");
        // plaeyer warped
        Vector3 TargetPos = ConnectedBlockObject.GetComponent<BlockScript>().WarpToOtherSide();
        other.transform.position = TargetPos;
    }

    public void hitFromPortalBullet(PortalBullet bullet) 
    {
        // check direction

        int playerID = bullet.pid;
        // shutDownDimension();

        if (OwnerPlayerID == -1)
        {
            OwnerPlayerID = playerID;
        }
        else if (OwnerPlayerID == playerID)
        { 
            shutDownDimension();
        }


        if (isConnected) 
        {
            // already connected
            
        }

        // other change already allocated
        

        // no allocated


        changeColorFromPlayer(playerID);

    }

}
