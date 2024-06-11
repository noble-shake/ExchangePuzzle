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
    public Transform CharacterArm { get { return AimTracer; } }

    [Header("Character Physics")]
    [SerializeField] Rigidbody rigid;
    [SerializeField] CapsuleCollider coll;
    [SerializeField] GameObject PlayerCam;  // Cinemachine Object
    [SerializeField] GameObject PlayerAimCam;  // Cinemachine Object
    [SerializeField] bool isGround;
    [SerializeField] bool isPassed;
    [SerializeField] float passedTime;
    [SerializeField] float passedCurTime;
    public GameObject playerCam { get {PlayerAimCam.SetActive(false); return PlayerCam;} set { PlayerCam = value; }}
    public GameObject playerAimCam { get {PlayerCam.SetActive(false); return PlayerAimCam;} set { PlayerCam = value; }}
    public bool GroundCheck { get {return isGround;} set {isGround = value;} }
    public bool PassedCheck { get { return isPassed; } }

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
        passedCurTime -= Time.deltaTime;
        if (passedCurTime < 0)
        {
            passedCurTime = 0f;
        }
    }

    public void dimenstionPassed()
    {
        passedCurTime = passedTime;
    }

    public bool passedCheck()
    {
        if (passedCurTime <= 0f)
        {
            isPassed = true;
            dimenstionPassed();
            return true;
        }
        else
        {
            return false;
        }

    }

    private void JumpCheck() {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hitGround, 1.1f, LayerMask.GetMask("Ground")) || Physics.Raycast(transform.position, -transform.up, out RaycastHit hitPlayer, 1.1f, LayerMask.GetMask("Player")))
        {
            isGround = true;
            isPassed = false;
        }
        else
        {
            isGround = false;
        }
    }

    public void SightChange(float direction)
    {
        Vector3 scaleVec = SpriteChild.localScale;
        Vector3 scaleAimVec = AimTracer.localScale;
        // Vector3 rotateVec = PlayerAimCam.transform.rotation.eulerAngles;
        if (direction > 0.5 && isLeft)
        {
            isLeft = false;
            scaleVec.x = Mathf.Abs(scaleVec.x);
            scaleAimVec.x = Mathf.Abs(scaleAimVec.x);
            // rotateVec.y = 90f;

            SpriteChild.localScale = scaleVec;
            AimTracer.localScale = scaleAimVec;
            // PlayerAimCam.transform.rotation = Quaternion.Euler(rotateVec);
        }
        else if (direction < -0.5 && !isLeft)
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
