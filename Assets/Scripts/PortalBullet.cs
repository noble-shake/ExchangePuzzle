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
    [SerializeField] Sprite Player1BulletSprite;
    [SerializeField] Sprite Player2BulletSprite;

    [Header("Owner Setup")]
    [SerializeField] int PlayerID;
    [SerializeField] public int pid { get { return PlayerID; } set { PlayerID = value; } }
    [SerializeField] CapsuleCollider bulletColl;
    [SerializeField] float bulletSpeed;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;

        Destroy(gameObject, 10f);
    }

    public void SetBulletInspector(int _PlayerID)
    { 
        PlayerID = _PlayerID;
        switch (_PlayerID) {
            case 0:
                sprRenderer.sprite = Player1BulletSprite;
                break;
            case 1:
                sprRenderer.sprite = Player2BulletSprite;
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
                Vector3 collisionPoint = other.ClosestPoint(transform.position);
                Vector3 collisionNormal = transform.position - collisionPoint;
                // Debug.Log(collisionPoint);
                // Debug.Log(collisionNormal);
                // change Block

                GameManager.instance.RegistryBlock(other.gameObject, pid);
                BlockScript go = other.GetComponent<BlockScript>();
                go.createDimension(collisionNormal);

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
