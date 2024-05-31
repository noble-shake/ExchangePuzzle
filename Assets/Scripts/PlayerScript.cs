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



}
