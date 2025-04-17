using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Character Info")]
    [SerializeField] PlayerTag playerTag;
    [SerializeField] List<SpriteRenderer> ColorParts;
    [SerializeField] Transform SpriteChild;
    [SerializeField] bool wallStep;
    [SerializeField] float defaultAimModelPos;

    [SerializeField] Collider LegColl;
    [SerializeField] Transform MuzzleRight;
    [SerializeField] Transform MuzzleLeft;

    [Header("Character Parts")]
    [SerializeField] Transform AimTracer;
    [SerializeField] Transform HeadParts;
    [SerializeField] Animator anim;
    [SerializeField] GameObject FrontSprite;
    [SerializeField] GameObject BackSprite;
    

    [Header("Character Physics")]
    [SerializeField] Rigidbody rigid;
    [SerializeField] CapsuleCollider coll;
    [SerializeField] GameObject PlayerCam;  // Cinemachine Object
    [SerializeField] GameObject PlayerAimCam;  // Cinemachine Object
    [SerializeField] bool isGround;
    [SerializeField] bool isPassed;
    [SerializeField] float passedTime;
    [SerializeField] float passedCurTime;

    public Rigidbody GetRigidbody { get { return rigid; } }

    [Header("Character Control")]
    [SerializeField] bool isSelect;
    [SerializeField] bool isLeft;

    public float InfinityDimension { set { passedCurTime = value; } }

    public Transform ShotPointRight { get { return MuzzleRight; } }
    public Transform ShotPointLeft { get { return MuzzleLeft; } }
    public Transform CharacterArm { get { return AimTracer; } }
    public Transform CharacterHead { get { return HeadParts; } }
    public PlayerTag player { get { return playerTag; } }

    public bool CharacterSelect { get { return isSelect; } set { isSelect = value; } }
    public bool DirectionCheck { get { return isLeft; } set { isLeft = value; } }

    public GameObject playerCam { get { PlayerAimCam.SetActive(false); return PlayerCam; } set { PlayerCam = value; } }
    public GameObject playerAimCam { get { PlayerCam.SetActive(false); return PlayerAimCam; } set { PlayerCam = value; } }
    public bool GroundCheck { get { return isGround; } set { isGround = value; } }
    public bool PassedCheck { get { return isPassed; } }

    public bool animWalk { set { anim.SetBool("OnWalk", value); } }
    public bool animJump { set { anim.SetBool("OnJump", value); } }

    public bool LookAtFront { get { return FrontSprite.activeSelf; } set { FrontSprite.SetActive(value); } }
    public bool LookAtBack { get { return BackSprite.activeSelf; } set { BackSprite.SetActive(value); } }

    public bool WallCheck { get { return wallStep; } set { wallStep = value; } }

    private void Awake()
    {
        // anim = GetComponent<Animator>();
    }

    void Start()
    {
        PlayerAimCam.SetActive(false);
        // defaultAimModelPos = AimModel.transform.position.z;
        colorUpdate();
        

        FrontSprite.SetActive(false);
        BackSprite.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        JumpCheck();
        passedCurTime -= Time.deltaTime;
        if (passedCurTime <= 0)
        {
            isPassed = false;
            passedCurTime = 0f;
        }
    }

    public void colorUpdate()
    {
        foreach (SpriteRenderer sprTarget in ColorParts)
        {
            switch (playerTag)
            {
                case PlayerTag.Player1:
                    sprTarget.color = BlockColoring.GetBlockColor(BlockColorTag.Player1);
                    break;
                case PlayerTag.Player2:
                    sprTarget.color = BlockColoring.GetBlockColor(BlockColorTag.Player2);
                    break;
            }
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
        if (Physics.Raycast(LegColl.transform.position, -transform.up, out RaycastHit hitGround, 1f, LayerMask.GetMask("Ground")))
        {
            animJump = false;
            isGround = true;
            return;
        }
        else if (Physics.Raycast(LegColl.transform.position, -transform.up, out RaycastHit hitWall, 1f, LayerMask.GetMask("Wall")))
        {
            animJump = false;
            isGround = true;
            return;
        }
        else if (Physics.Raycast(LegColl.transform.position, -transform.up, out RaycastHit hitPlayer, 1f, LayerMask.GetMask("Player")))
        {

            if (hitPlayer.collider.GetComponent<PlayerScript>().playerTag != playerTag)
            {
                animJump = false;
                isGround = true;
                return;
            }
            else
            {
                animJump = true;
                isGround = false;
                return;
            }


        }
        else
        {
            animJump = true;
            isGround = false;
        }
    }

    public void SightChange(float direction)
    {
        Vector3 scaleVec = SpriteChild.localScale;

        if (direction > 0.5 && isLeft)
        {
            isLeft = false;
            scaleVec.x = Mathf.Abs(scaleVec.x);


            SpriteChild.localScale = scaleVec;
            FrontSprite.transform.localScale = scaleVec;
            BackSprite.transform.localScale = scaleVec;

        }
        else if (direction < -0.5 && !isLeft)
        {
            isLeft = true;
            scaleVec.x = -Mathf.Abs(scaleVec.x);


            SpriteChild.localScale = scaleVec;
            FrontSprite.transform.localScale = scaleVec;
            BackSprite.transform.localScale = scaleVec;


        }

    }

    public void GunModelSightSwap()
    {
        // Vector3 SightChange = AimModel.transform.position;
        // SightChange.z = -SightChange.z;
        // AimModel.transform.position = SightChange;
    }

    public void GunModelReset()
    {
        // Vector3 SightChange = AimModel.transform.position;
        // SightChange.z = defaultAimModelPos;
        // AimModel.transform.position = SightChange;
    }

    public GameObject GetAimCameraObject() {
        return PlayerAimCam;
    }

    public GameObject GetCameraObject()
    {
        return PlayerCam;
    }

    public void WallTriggerEnter(Collider coll)
    {
        wallStep = true;
    }

    public void WallTriggerExit(Collider coll)
    {
        wallStep = false;
    }

    public void ControlEarn(bool isOn)
    {
        playerCam.SetActive(isOn);
        CharacterSelect = isOn; // isSelect On/Off
    }

}
