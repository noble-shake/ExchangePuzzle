using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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

    [Header("External Setup")]
    Camera mainCam;

    [Header("Player Control")]
    [SerializeField] bool isLeftChar = true;
    [SerializeField] float exCool;
    [SerializeField] float exCurCool;

    [Header("Player Object")]
    [SerializeField] PlayerScript Player1Object;
    [SerializeField] PlayerScript Player2Object;
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
    }

    void Start()
    {
        mainCam = Camera.main;
        rigid = Player1Object.GetComponent<Rigidbody>();
    }


    void Update()
    {
        CharacterExchange();
    }

    private void CharacterExchange()
    {
        // exchange icon cool time change
        exCurCool -= Time.deltaTime;
        if (exCurCool < 0f)
        { 
            exCurCool = 0f;
        }

        if (exCurCool > 0f) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            isLeftChar = !isLeftChar;
            exCurCool = exCool;
        }

        // Camera Change
        // Character Controller Change
        if (isLeftChar)
        {

        }
        else
        { 
            
        }

    }
}
