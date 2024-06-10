using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Character Info")]
    [SerializeField] Transform SpriteChild;
    [SerializeField] Transform AimTracer;
    [SerializeField] Transform Muzzle;
    public Transform ShotPoint { get { return Muzzle; } }

    [Header("Character Physics")]
    [SerializeField] Rigidbody rigid;
    [SerializeField] CapsuleCollider coll;
    [SerializeField] GameObject PlayerCam;  // Cinemachine Object
    [SerializeField] GameObject PlayerAimCam;  // Cinemachine Object
    [SerializeField] bool isGround;
    public GameObject playerCam { get {PlayerAimCam.SetActive(false); return PlayerCam;} set { PlayerCam = value; }}
    public GameObject playerAimCam { get {PlayerCam.SetActive(false); return PlayerAimCam;} set { PlayerCam = value; }}
    public bool GroundCheck { get {return isGround;} set {isGround = value;} }

    [Header("Character Control")]
    [SerializeField] bool isSelect;
    [SerializeField] bool isLeft;
    public bool CharacterSelect { get { return isSelect; } set { isSelect = value; } }
    public bool DirectionCheck { get { return isLeft; } set { isLeft = value; } }

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

    public void SightChange(int direction)
    {
        Vector3 scaleVec = SpriteChild.localScale;
        Vector3 scaleAimVec = AimTracer.localScale;
        // Vector3 rotateVec = PlayerAimCam.transform.rotation.eulerAngles;
        if (direction == 1 && isLeft)
        {
            isLeft = false;
            scaleVec.x = Mathf.Abs(scaleVec.x);
            scaleAimVec.x = Mathf.Abs(scaleAimVec.x);
            // rotateVec.y = 90f;

            SpriteChild.localScale = scaleVec;
            AimTracer.localScale = scaleAimVec;
            // PlayerAimCam.transform.rotation = Quaternion.Euler(rotateVec);
        }
        else if (direction == -1 && !isLeft)
        {
            isLeft = true;
            scaleVec.x = -Mathf.Abs(scaleVec.x);
            scaleAimVec.x = -Mathf.Abs(scaleAimVec.x);
            // rotateVec.y = -90f;

            SpriteChild.localScale = scaleVec;
            AimTracer.localScale = scaleAimVec;
            // PlayerAimCam.transform.rotation = Quaternion.Euler(rotateVec);
        }

    }

    public GameObject GetAimCameraObject() {
        return PlayerAimCam;
    }

    public GameObject GetCameraObject()
    {
        return PlayerCam;
    }

}
