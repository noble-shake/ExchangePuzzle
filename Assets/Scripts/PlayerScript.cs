using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Character Physics")]
    [SerializeField] Rigidbody rigid;
    [SerializeField] CapsuleCollider coll;
    [SerializeField] GameObject PlayerCam;  // Cinemachine Object
    [SerializeField] GameObject PlayerAimCam;  // Cinemachine Object
    [SerializeField]
    public GameObject playerCam
    { 
        get {
            PlayerAimCam.SetActive(false);
            return PlayerCam; 
        }
        set { PlayerCam = value; }
    }

    public GameObject playerAimCam
    {
        get {
            PlayerCam.SetActive(false);
            return PlayerAimCam; 
        }
        set { PlayerCam = value; }
    }

    [SerializeField] bool isGround;
    [SerializeField] public bool GroundCheck
    { get { return isGround; } set { isGround = value; } }

    [Header("Character Control")]
    [SerializeField] bool isSelect;
    public bool CharacterSelect
    { get { return isSelect; } set { isSelect = value; } }

    // Start is called before the first frame update
    void Start()
    {
        PlayerAimCam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        JumpCheck();
    }

    private void JumpCheck() {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 1.1f, LayerMask.GetMask("Ground")))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }


<<<<<<< HEAD
=======
    public void SightChange(int direction)
    {
        Vector3 scaleVec = SpriteChild.localScale;
        Vector3 rotateVec = PlayerAimCam.transform.rotation.eulerAngles;
        Debug.Log(rotateVec);
        if (direction == 1)
        {
            scaleVec.x = Mathf.Abs(scaleVec.x);
            // rotateVec.x = Mathf.Abs(rotateVec.x);

            SpriteChild.localScale = scaleVec;
            // PlayerAimCam.transform.localScale = camVec;
        }
        else if (direction == -1)
        {
            scaleVec.x = -Mathf.Abs(scaleVec.x);
            // camVec.x = -Mathf.Abs(camVec.x);

            SpriteChild.localScale = scaleVec;
            // PlayerAimCam.transform.localScale = camVec;
        }

    }

>>>>>>> parent of 3cf564b (2024-05-31)

}
