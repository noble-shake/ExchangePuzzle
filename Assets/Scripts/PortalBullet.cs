using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBullet : MonoBehaviour
{
    [Header("External Setup")]
    [SerializeField] MeshRenderer m_Renderer;
    [SerializeField] Material ReferenceMat;
    [SerializeField] Material BulletMat;

    [Header("Owner Setup")]
    [SerializeField] int PlayerID;
    [SerializeField] public int pid { get { return PlayerID; } set { PlayerID = value; } }
    [SerializeField] CapsuleCollider bulletColl;
    [SerializeField] float bulletSpeed;

    // Start is called before the first frame update
    void Start()
    {
        m_Renderer = GetComponent<MeshRenderer>();
        m_Renderer.material = Instantiate(ReferenceMat);
        BulletMat = m_Renderer.material;
        BulletMat.color = BlockColoring.GetBlockColor(BlockColorTag.Normal);

        Destroy(gameObject, 5f);
    }

    public void SetBulletInspector(int _PlayerID)
    { 
        PlayerID = _PlayerID;
        switch (_PlayerID) {
            case 0:
                BulletMat.color = BlockColoring.GetBlockColor(BlockColorTag.Player1);
                break;
            case 1:
                BulletMat.color = BlockColoring.GetBlockColor(BlockColorTag.Player2);
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + transform.forward * Time.deltaTime * bulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("block"))
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
    }
}
