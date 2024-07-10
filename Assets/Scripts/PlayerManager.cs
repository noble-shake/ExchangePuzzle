using UnityEngine;

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
    [SerializeField] GameObject AimUI;
    [SerializeField] GameObject TracingPushBlock;

    public bool isAimUIActive { get { return AimUI.activeSelf; } }

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
    bool eventHappend;

    public bool EventHappend { get { return eventHappend; } set { eventHappend = value; } }

    [Header("Player Object")]
    [SerializeField] PlayerScript Player1Object;
    [SerializeField] PlayerScript Player2Object;
    [SerializeField] PlayerScript Player1Prefab;
    [SerializeField] PlayerScript Player2Prefab;

    [SerializeField] PlayerTag curPlayerID;
    [SerializeField] PlayerScript TargetObject;
    [SerializeField] Rigidbody rigid;

    public PlayerScript RegistryPlayer1 {  get { return Player1Object; } set { Player1Object = value; } }
    public PlayerScript RegistryPlayer2 {  get { return Player2Object; } set { Player2Object = value; } }

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
        TargetObject = Player1Object;
        CurrentAimCam = Player1Object.GetComponent<PlayerScript>().GetAimCameraObject();
        curPlayerID = PlayerTag.Player1;
        rigid = Player1Object.GetComponent<Rigidbody>();
        TracingPushBlock.GetComponent<PreventPushObject>().Tracing = Player2Object;
    }

    public void StageInitForPlayer2()
    {
        // position setup
        Player2Object.GetComponent<PlayerScript>().playerCam.SetActive(true);
        Player1Object.GetComponent<PlayerScript>().playerCam.SetActive(false);
        TargetObject = Player2Object;
        CurrentAimCam = Player2Object.GetComponent<PlayerScript>().GetAimCameraObject();
        curPlayerID = PlayerTag.Player2;
        rigid = Player2Object.GetComponent<Rigidbody>();
        TracingPushBlock.GetComponent<PreventPushObject>().Tracing = Player1Object;
    }

    void Start()
    {
        AimUI.SetActive(false);
        sequenceInstance = SequenceManager.instance;
    }

    public void GeneratePlayers()
    {
        Player1Object = Instantiate(Player1Prefab);
        Player1Object.gameObject.SetActive(false);

        Player2Object = Instantiate(Player2Prefab);
        Player2Object.gameObject.SetActive(false);
    }


    void Update()
    {
        if (sequenceInstance.SequenceProcessing) return;
        if (TargetObject == null) return;
        if (eventHappend) return;

        CharacterExchange();
        CharacterMove();
        CharacterJump();
        CharacterAimMode();
        CharacterAimRotate();
        CharacterAimSpriteCheck();
        CharacterShoot();
        
    }

    private void CharacterAimSpriteCheck()
    {
        if (!isAiming)
        {
            if(Player1Object.LookAtFront) Player1Object.LookAtFront = false;
            if(Player1Object.LookAtBack) Player1Object.LookAtBack = false;
            if(Player2Object.LookAtFront) Player2Object.LookAtFront = false;
            if(Player2Object.LookAtBack) Player2Object.LookAtBack= false;

            return;
        }

        switch (curPlayerID)
        { 
            case PlayerTag.Player1:
                if (Player2Object.transform.position.x >= Player1Object.transform.position.x)
                {
                    if (Player2Object.DirectionCheck)
                    {
                        Player2Object.LookAtFront = true;
                        if (Player2Object.LookAtBack)
                        {
                            Player2Object.LookAtBack = false;
                        }
                    }
                    else
                    {
                        Player2Object.LookAtBack = true;
                        if (Player2Object.LookAtFront)
                        {
                            Player2Object.LookAtFront= false;
                        }
                    }
                }
                else
                {
                    if (Player2Object.DirectionCheck)
                    {
                        Player2Object.LookAtBack = true;
                        if (Player2Object.LookAtFront)
                        {
                            Player2Object.LookAtFront= false;
                        }
                    }
                    else
                    {
                        Player2Object.LookAtFront= true;
                        if (Player2Object.LookAtBack)
                        {
                            Player2Object.LookAtBack= false;
                        }
                    }
                }

                break;
            case PlayerTag.Player2:
                if (Player1Object.transform.position.x >= Player2Object.transform.position.x)
                {
                    if (Player1Object.DirectionCheck)
                    {
                        Player1Object.LookAtFront = true;
                        if (Player1Object.LookAtBack)
                        {
                            Player1Object.LookAtBack = false;
                        }
                    }
                    else
                    {
                        Player1Object.LookAtBack = true;
                        if (Player1Object.LookAtFront)
                        {
                            Player1Object.LookAtFront = false;
                        }
                    }
                }
                else
                {
                    if (Player1Object.DirectionCheck)
                    {
                        Player1Object.LookAtBack = true;
                        if (Player1Object.LookAtFront)
                        {
                            Player1Object.LookAtFront = false;
                        }
                    }
                    else
                    {
                        Player1Object.LookAtFront = true;
                        if (Player1Object.LookAtBack)
                        {
                            Player1Object.LookAtBack = false;
                        }
                    }
                }
                break;
        }

    }

    private void CharacterShoot()
    {
        if (!isAiming) return;
        if (GameManager.instance.PauseEvent) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(CurrentAimCam.transform.position, CurrentAimCam.transform.forward, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {

                if (!TargetObject.GetComponent<PlayerScript>().DirectionCheck)
                {
                    TargetObject.ShotPointRight.transform.LookAt(hit.point);
                }
                else {
                    TargetObject.ShotPointLeft.transform.LookAt(hit.point);
                }

                
                
            }

            if (!TargetObject.GetComponent<PlayerScript>().DirectionCheck)
            {
                GameObject go = Instantiate(BulletPrefab, TargetObject.ShotPointRight.position, TargetObject.ShotPointRight.rotation);
                go.GetComponent<PortalBullet>().SetBulletInspector(curPlayerID);
            }
            else
            {
                GameObject go = Instantiate(BulletPrefab, TargetObject.ShotPointLeft.position, TargetObject.ShotPointLeft.rotation);
                go.GetComponent<PortalBullet>().SetBulletInspector(curPlayerID);
            }

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
            TargetObject.GetComponent<PlayerScript>().animWalk = false;
            isFirstChar = !isFirstChar;
            exCurCool = exCool;
            // isMovable = false;

            // Camera Change
            // Character Controller Change
            if (isFirstChar)
            {
                curPlayerID = PlayerTag.Player2;
                Player1Object.GetComponent<PlayerScript>().playerCam.SetActive(false);
                Player1Object.GetComponent<PlayerScript>().CharacterSelect = false; // isSelect On/Off
                Player2Object.GetComponent<PlayerScript>().playerCam.SetActive(true);
                Player2Object.GetComponent<PlayerScript>().CharacterSelect = true;
                // Player1Object.GetComponent<Rigidbody>().mass = 1000f;
                // Player2Object.GetComponent<Rigidbody>().mass = 1f;
                TargetObject = Player2Object;
                CurrentAimCam = Player2Object.GetComponent<PlayerScript>().GetAimCameraObject();
                // Player1Object.GetComponent<Rigidbody>().velocity = rigid.velocity;
                rigid = Player2Object.GetComponent<Rigidbody>();
                TracingPushBlock.GetComponent<PreventPushObject>().Tracing = Player1Object;
            }
            else
            {
                curPlayerID = PlayerTag.Player1;
                Player1Object.GetComponent<PlayerScript>().playerCam.SetActive(true);
                Player1Object.GetComponent<PlayerScript>().CharacterSelect = false;
                Player2Object.GetComponent<PlayerScript>().playerCam.SetActive(false);
                Player2Object.GetComponent<PlayerScript>().CharacterSelect = true;
                // Player1Object.GetComponent<Rigidbody>().mass = 1f;
                // Player2Object.GetComponent<Rigidbody>().mass = 1000f;
                TargetObject = Player1Object;
                CurrentAimCam = Player1Object.GetComponent<PlayerScript>().GetAimCameraObject();
                // Player2Object.GetComponent<Rigidbody>().velocity = rigid.velocity;
                rigid = Player1Object.GetComponent<Rigidbody>();
                TracingPushBlock.GetComponent<PreventPushObject>().Tracing = Player2Object;
            }
        }
    }

    public void CharacterExchangeEvent(PlayerTag _player)
    {
        if (curPlayerID == _player) return;

        if (_player == PlayerTag.Player2)
        {
            curPlayerID = PlayerTag.Player2;
            Player1Object.GetComponent<PlayerScript>().playerCam.SetActive(false);
            Player1Object.GetComponent<PlayerScript>().CharacterSelect = false; // isSelect On/Off
            Player2Object.GetComponent<PlayerScript>().playerCam.SetActive(true);
            Player2Object.GetComponent<PlayerScript>().CharacterSelect = true;
            // Player1Object.GetComponent<Rigidbody>().mass = 1000f;
            // Player2Object.GetComponent<Rigidbody>().mass = 1f;
            TargetObject = Player2Object;
            CurrentAimCam = Player2Object.GetComponent<PlayerScript>().GetAimCameraObject();
            rigid = Player2Object.GetComponent<Rigidbody>();
        }
        else
        {
            curPlayerID = PlayerTag.Player1;
            Player1Object.GetComponent<PlayerScript>().playerCam.SetActive(true);
            Player1Object.GetComponent<PlayerScript>().CharacterSelect = false;
            Player2Object.GetComponent<PlayerScript>().playerCam.SetActive(false);
            Player2Object.GetComponent<PlayerScript>().CharacterSelect = true;
            // Player1Object.GetComponent<Rigidbody>().mass = 1f;
            // Player2Object.GetComponent<Rigidbody>().mass = 1000f;
            TargetObject = Player1Object;
            CurrentAimCam = Player1Object.GetComponent<PlayerScript>().GetAimCameraObject();
            rigid = Player1Object.GetComponent<Rigidbody>();
        }
    }

    private void CharacterMove() {
        float vert = Input.GetAxis("Vertical");
        float hori = Input.GetAxis("Horizontal");

        if (hori != 0)
        {
            TargetObject.animWalk = true;

            if (hori > 0.2f && TargetObject.DirectionCheck || hori < -0.2f && !TargetObject.DirectionCheck)
            {
                rotateTime = 90f;
            }
            TargetObject.SightChange(hori);
        }
        else
        {
            TargetObject.animWalk = false;
            return;
        }
        

        if (isAiming) return;

        if (TargetObject.PassedCheck) return;

        // if (TargetObject.WallCheck) return;

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
            AimUI.SetActive(true);
            aimCurCool = aimCool;
            TargetObject.GetComponent<PlayerScript>().playerAimCam.SetActive(true);
            isAiming = true;
            GameManager.instance.PlayerUIChange();
        }
        else if (Input.GetKeyDown(KeyCode.Z) && isAiming)
        {
            AimUI.SetActive(false);
            aimCurCool = aimCool;
            TargetObject.GetComponent<PlayerScript>().playerCam.SetActive(true);
            isAiming = false;
            GameManager.instance.PlayerUIChange();
            TargetObject.GunModelReset();
        }


    }

    private void CharacterAimRotate()
    {

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
            TargetObject.GetComponent<PlayerScript>().CharacterHead.rotation = Quaternion.Euler(0f, 0f, -rotateValue.x);
        }
        else
        {
            TargetObject.GetComponent<PlayerScript>().CharacterArm.rotation = Quaternion.Euler(0f, 0f, rotateValue.x);
            TargetObject.GetComponent<PlayerScript>().CharacterHead.rotation = Quaternion.Euler(0f, 0f, rotateValue.x);
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

    public void AimColorChange(PlayerTag _playerID, bool _active)
    {
        AimColorTag targetAimObj = _playerID == PlayerTag.Player1 ? AimColorTag.Blue : AimColorTag.Yellow;
        AimUI.GetComponent<AimEffectScript>().AlphaChange(targetAimObj, _active);
    }

}
