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
        WarpDirection = _norm;

        if (ConnectedBlockObject == null) return;

        // dimension prefab
        // collider on/off
        blockColl.enabled = false;
        dimensionCollObj.SetActive(true);
    }

    public void shutDownDimension() 
    {
        blockColl.enabled = true;
        isConnected = false;
        ConnectedBlockObject = null;
        changeColorFromPlayer(-1);
        dimensionCollObj.SetActive(false);
    }

    public Vector3 WarpToOtherSide() { 
        return ConnectedBlockObject.transform.position + WarpDirection;
    }

    public void SynchronizeBlocks(BlockScript _otherBlock) {
        ConnectedBlockObject = _otherBlock;
    }

    public void TriggerOnDimensionObject(Collider other) {
        Debug.Log("Collider");
    }

    public void hitFromPortalBullet(PortalBullet bullet) 
    {
        // check direction

        int playerID = bullet.pid;
        shutDownDimension();

        if (isConnected) 
        {
            // already connected
            
        }

        // other change already allocated
        

        // no allocated


        changeColorFromPlayer(playerID);

    }

}
