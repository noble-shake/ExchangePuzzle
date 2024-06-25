using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager: MonoBehaviour
{
    /*
     * Movement 
     * Jump / ground check
     * Character Change
     * Change Camera Mode
     * Gun Shoot
     * Portal Warp
     * Game Over
     * 
     */


    public static PlayerManager instance;
    private SequenceManager sequenceInstance;

    [Header("External Setup")]
    [SerializeField] Camera mainCam;
    [SerializeField] GameObject CurrentArms;
    [SerializeField] GameObject CurrentAimCam;
    [SerializeField] GameObject ChracterPortrait;
    [SerializeField] GameObject BulletPrefab;

    [Header("Player Control")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] bool isMovable;
    [SerializeField] bool isFirstChar;

    public float PlayerSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    [SerializeField] float exCool;
    [SerializeField] float exCurCool;

    [SerializeField] float mouseSensivity;
    [SerializeField] bool isAiming;
    [SerializeField] float aimCool;
    [SerializeField] float aimCurCool;
    [SerializeField] Vector3 rotateValue;
    [SerializeField] float rotateTime;
    [SerializeField] float rotateSpeed;

    [Header("Player Object")]
    [SerializeField] PlayerScript Player1Object;
    [SerializeField] PlayerScript Player2Object;
    [SerializeField] int curPlayerID;
    [SerializeField] PlayerScript TargetObject;
    [SerializeField] Rigidbody rigid;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        { 
            Destroy(gameObject);
        }
    }

    // Stage Initialize
    public void StageInitForPlayer1()
    {
        // position setup
        Player1Object.GetComponent<PlayerScript>().playerCam.SetActive(true);
        Player2Object.GetComponent<PlayerScript>().playerCam.SetActive(false);
        Player1Object.GetComponent<Rigidbody>().mass = 1f;
        Player2Object.GetComponent<Rigidbody>().mass = 1000f;
        TargetObject = Player1Object;
        CurrentAimCam = Player1Object.GetComponent<PlayerScript>().GetAimCameraObject();
        curPlayerID = 0;
        rigid = Player1Object.GetComponent<Rigidbody>();
    }

    public void StageInitForPlayer2()
    {
        // position setup
        Player2Object.GetComponent<PlayerScript>().playerCam.SetActive(true);
        Player1Object.GetComponent<PlayerScript>().playerCam.SetActive(false);
        Player2Object.GetComponent<Rigidbody>().mass = 1f;
        Player1Object.GetComponent<Rigidbody>().mass = 1000f;
        TargetObject = Player2Object;
        CurrentAimCam = Player2Object.GetComponent<PlayerScript>().GetAimCameraObject();
        curPlayerID = 0;
        rigid = Player2Object.GetComponent<Rigidbody>();
    }

    void Start()
    {
        StageInitForPlayer1();
        sequenceInstance = SequenceManager.instance;


    }


    void Update()
    {
        if (sequenceInstance.SequenceProcessing) return;

        CharacterExchange();
        CharacterMove();
        CharacterJump();
        CharacterAimMode();
        CharacterAimRotate();
        CharacterShoot();
    }

    private void CharacterShoot()
    {
        if (!isAiming) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(CurrentAimCam.transform.position, CurrentAimCam.transform.forward, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {

                TargetObject.ShotPoint.transform.LookAt(hit.point);
            }


            GameObject go = Instantiate(BulletPrefab, TargetObject.ShotPoint.position, TargetObject.ShotPoint.rotation);
            go.GetComponent<PortalBullet>().SetBulletInspector(curPlayerID);
        }
    }

    private void CharacterExchange()
    {
        // exchange icon cool time change
        exCurCool -= Time.deltaTime;
        if (exCurCool < 0f)
        { 
            exCurCool = 0f;
            // isMovable = true;
        }

        if (exCurCool > 0f) return;
        if (isAiming) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            isFirstChar = !isFirstChar;
            exCurCool = exCool;
            // isMovable = false;

            // Camera Change
            // Character Controller Change
            if (isFirstChar)
            {
                curPlayerID = 1;
                Player1Object.GetComponent<PlayerScript>().playerCam.SetActive(false);
                Player1Object.GetComponent<PlayerScript>().CharacterSelect = false; // isSelect On/Off
                Player2Object.GetComponent<PlayerScript>().playerCam.SetActive(true);
                Player2Object.GetComponent<PlayerScript>().CharacterSelect = true;
                Player1Object.GetComponent<Rigidbody>().mass = 1000f;
                Player2Object.GetComponent<Rigidbody>().mass = 1f;
                TargetObject = Player2Object;
                CurrentAimCam = Player2Object.GetComponent<PlayerScript>().GetAimCameraObject();
                rigid = Player2Object.GetComponent<Rigidbody>();
            }
            else
            {
                curPlayerID = 0;
                Player1Object.GetComponent<PlayerScript>().playerCam.SetActive(true);
                Player1Object.GetComponent<PlayerScript>().CharacterSelect = false;
                Player2Object.GetComponent<PlayerScript>().playerCam.SetActive(false);
                Player2Object.GetComponent<PlayerScript>().CharacterSelect = true;
                Player1Object.GetComponent<Rigidbody>().mass = 1f;
                Player2Object.GetComponent<Rigidbody>().mass = 1000f;
                TargetObject = Player1Object;
                CurrentAimCam = Player1Object.GetComponent<PlayerScript>().GetAimCameraObject();
                rigid = Player1Object.GetComponent<Rigidbody>();
            }
        }



    }

    private void CharacterMove() {
        float vert = Input.GetAxis("Vertical");
        float hori = Input.GetAxis("Horizontal");

        if(hori != 0)
        {
            if (hori > 0.5f && TargetObject.DirectionCheck || hori < -0.5f && !TargetObject.DirectionCheck)
            {
                rotateTime = 90f;
            }
            TargetObject.SightChange(hori);
        }
        

        if (isAiming) return;

        if (TargetObject.PassedCheck) return;

        // if (!isMovable) return;

        Vector3 moveDir = Vector3.zero;
        moveDir.x = hori * moveSpeed;
        moveDir.y = rigid.velocity.y;
        rigid.velocity = TargetObject.transform.rotation * moveDir;
    }

    private void CharacterJump() {
        if (isAiming) return;

        if (Input.GetKeyDown(KeyCode.X) && TargetObject.GroundCheck)
        {
            rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void CharacterAimMode()
    {
        aimCurCool -= Time.deltaTime;
        if (aimCurCool < 0f)
        {
            aimCurCool = 0f;
            // isMovable = true;
        }

        if (aimCurCool > 0f) return;

        if (Input.GetKeyDown(KeyCode.Z) && !isAiming)
        {
            aimCurCool = aimCool;
            TargetObject.GetComponent<PlayerScript>().playerAimCam.SetActive(true);
            isAiming = true;
            GameManager.instance.PlayerUIChange();
        }
        else if (Input.GetKeyDown(KeyCode.Z) && isAiming)
        {
            aimCurCool = aimCool;
            TargetObject.GetComponent<PlayerScript>().playerCam.SetActive(true);
            isAiming = false;
            GameManager.instance.PlayerUIChange();
            TargetObject.GunModelReset();
        }


    }

    private void CharacterAimRotate()
    {
        // if (!isAiming) return;

        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensivity / 2 * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensivity / 2 * Time.deltaTime;

        rotateValue.x -= (mouseY + mouseX);
        //rotateValue.y += mouseX;
        rotateValue.x = Mathf.Clamp(rotateValue.x, -60f, 60f);
        // DirectionCheck

        rotateTime -= Time.deltaTime * rotateSpeed;
        if (rotateTime < 0f)
        {
            rotateTime = 0f;
        }

        if (TargetObject.DirectionCheck)
        {
            rotateValue.y = -90f + rotateTime;
        }
        else {
            rotateValue.y = 90f - rotateTime;
        }

        //rotateValue.y = Mathf.Clamp(rotateValue.y, 0f, 60f);

        // Character, Camera
        // transform.rotation = Quaternion.Euler(0f, rotateValue.y, 0f);
        CurrentAimCam.transform.rotation = Quaternion.Euler(rotateValue.x, rotateValue.y, 0f);

        if (!TargetObject.GetComponent<PlayerScript>().DirectionCheck)
        {
            TargetObject.GetComponent<PlayerScript>().CharacterArm.rotation = Quaternion.Euler(0f, 0f, -rotateValue.x);
        }
        else
        {
            TargetObject.GetComponent<PlayerScript>().CharacterArm.rotation = Quaternion.Euler(0f, 0f, rotateValue.x);
        }
        

    }

    public void PlayerGenerate(int _gen)
    {
        switch (_gen)
        {
            case 0:
                Player1Object.gameObject.SetActive(true);
                break;
            case 1:
                Player2Object.gameObject.SetActive(true);
                break;
            case 2:
                Player2Object.gameObject.SetActive(true);
                Player1Object.gameObject.SetActive(true);
                break;
        }

    }

}
