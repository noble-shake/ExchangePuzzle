using System;
using System.Collections;
using System.Collections.Generic;
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

    [Header("External Setup")]
    [SerializeField] Camera mainCam;
    [SerializeField] GameObject ChracterPortrait;

    [Header("Player Control")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] bool isMovable;
    [SerializeField] bool isFirstChar;
    [SerializeField] float exCool;
    [SerializeField] float exCurCool;
    [SerializeField] bool isAiming;

    [Header("Player Object")]
    [SerializeField] PlayerScript Player1Object;
    [SerializeField] PlayerScript Player2Object;
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
    public void StageInit()
    {
        // position setup
        Player1Object.GetComponent<PlayerScript>().playerCam.SetActive(true);
        Player2Object.GetComponent<PlayerScript>().playerCam.SetActive(false);
        Player1Object.GetComponent<Rigidbody>().mass = 1f;
        Player2Object.GetComponent<Rigidbody>().mass = 1000f;
        TargetObject = Player1Object;
        rigid = Player1Object.GetComponent<Rigidbody>();
    }

    void Start()
    {
        StageInit();
        
    }


    void Update()
    {
        CharacterExchange();
        CharacterMove();
        CharacterJump();
        CharacterAim();
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
                Player1Object.GetComponent<PlayerScript>().playerCam.SetActive(false);
                Player1Object.GetComponent<PlayerScript>().CharacterSelect = false; // isSelect On/Off
                Player2Object.GetComponent<PlayerScript>().playerCam.SetActive(true);
                Player2Object.GetComponent<PlayerScript>().CharacterSelect = true;
                Player1Object.GetComponent<Rigidbody>().mass = 1000f;
                Player2Object.GetComponent<Rigidbody>().mass = 1f;
                TargetObject = Player2Object;
                rigid = Player2Object.GetComponent<Rigidbody>();
            }
            else
            {
                Player1Object.GetComponent<PlayerScript>().playerCam.SetActive(true);
                Player1Object.GetComponent<PlayerScript>().CharacterSelect = false;
                Player2Object.GetComponent<PlayerScript>().playerCam.SetActive(false);
                Player2Object.GetComponent<PlayerScript>().CharacterSelect = true;
                Player1Object.GetComponent<Rigidbody>().mass = 1f;
                Player2Object.GetComponent<Rigidbody>().mass = 1000f;
                TargetObject = Player1Object;
                rigid = Player1Object.GetComponent<Rigidbody>();
            }
        }



    }

    private void CharacterMove() {
        if (isAiming) return;
        // if (!isMovable) return;

        float vert = Input.GetAxis("Vertical");
        float hori = Input.GetAxis("Horizontal");

        Vector3 moveDir = Vector3.zero;
        moveDir.x = hori * moveSpeed;
        moveDir.y = rigid.velocity.y;
        rigid.velocity = TargetObject.transform.rotation * moveDir;
    }

    private void CharacterJump() {

        if (Input.GetKeyDown(KeyCode.X) && TargetObject.GroundCheck)
        {
            rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void CharacterAim()
    {
        //// exchange icon cool time change
        //exCurCool -= Time.deltaTime;
        //if (exCurCool < 0f)
        //{
        //    exCurCool = 0f;
        //    // isMovable = true;
        //}

        //if (exCurCool > 0f) return;

        if (Input.GetKeyDown(KeyCode.Z) && TargetObject.GroundCheck && !isAiming)
        {
            TargetObject.GetComponent<PlayerScript>().playerAimCam.SetActive(true);
            isAiming = true;
        }
        else if (Input.GetKeyDown(KeyCode.Z) && isAiming)
        {
            TargetObject.GetComponent<PlayerScript>().playerCam.SetActive(true);
            isAiming = false;
        }
        
    }
}
