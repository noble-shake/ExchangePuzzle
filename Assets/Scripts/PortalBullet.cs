using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBullet : MonoBehaviour
{
    [Header("External Setup")]
    [SerializeField] Camera mainCam;
    [SerializeField] GameObject SpriteObject;
    [SerializeField] SpriteRenderer sprRenderer;
    [SerializeField] Sprite PlayerBulletSprite;

    [Header("Owner Setup")]
    [SerializeField] PlayerTag PlayerID;
    [SerializeField] public PlayerTag pid { get { return PlayerID; } set { PlayerID = value; } }
    [SerializeField] CapsuleCollider bulletColl;
    [SerializeField] float bulletSpeed;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        // sprRenderer = GetComponent<SpriteRenderer>();
        // sprRenderer.sprite = PlayerBulletSprite;

        Destroy(gameObject, 10f);
    }

    public void SetBulletInspector(PlayerTag _PlayerID)
    { 
        PlayerID = _PlayerID;
        switch (_PlayerID) {
            case PlayerTag.Player1:
                sprRenderer.color = BlockColoring.GetBlockColor(BlockColorTag.Player1);
                break;
            case PlayerTag.Player2:
                sprRenderer.color = BlockColoring.GetBlockColor(BlockColorTag.Player2);
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
        // SpriteObject.transform.LookAt(mainCam.transform.position);
        SpriteObject.transform.forward = mainCam.transform.forward;

        transform.position = transform.position + transform.forward * Time.deltaTime * bulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("block"))
        {
            if (other.GetComponent<BlockScript>().getBlockType() == BlockType.Normal || other.GetComponent<BlockScript>().getBlockType() == BlockType.Moving)
            {
                Vector3 errorVec = ((Vector2)transform.position - (Vector2)other.gameObject.transform.position);
                float angle = Mathf.Atan2(errorVec.y, errorVec.x) * Mathf.Rad2Deg + 180f;

                if (-45f > angle && angle >= 45f)
                { 
                    
                }
                else if (135f > angle && angle >= f)
                {

                }
                if (-45f > angle && angle >= 45f)
                {

                }
                if (-45f > angle && angle >= 45f)
                {

                }


                Vector3 collisionPoint;
                if (Mathf.Abs(errorVec.x) >= Mathf.Abs(errorVec.y))
                {
                    if (errorVec.x > 0) { collisionPoint = Vector3.right; }
                    else { collisionPoint = Vector3.left; }

                }
                else
                {

                    if (errorVec.y > 0) { collisionPoint = Vector3.up; }
                    else { collisionPoint = Vector3.down; }

                }


                //Vector3 collisionPoint = other.ClosestPoint(transform.position);
                //Vector3 collisionNormal = transform.position - collisionPoint;

                GameManager.instance.RegistryBlock(other.gameObject, pid);
                BlockScript go = other.GetComponent<BlockScript>();
                go.createDimension(collisionPoint);

                Destroy(gameObject);
            }
            else if (other.GetComponent<BlockScript>().getBlockType() == BlockType.NonChanged || other.GetComponent<BlockScript>().getBlockType() == BlockType.NonChangedMoving)
            {
                Destroy(gameObject);
            }
            else if (other.GetComponent<BlockScript>().getBlockType() == BlockType.Passed)
            {
                // not Activated.
            }
            else 
            {
                Destroy(gameObject);
            }


        }
    }

}
