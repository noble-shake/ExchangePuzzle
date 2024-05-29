using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerManager: MonoBehaviour
{
    /*
     * Movement 
     * Jump
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
    
    }

    void Start()
    {
        mainCam = Camera.main;
        rigid = Player1Object.GetComponent<Rigidbody>();
    }


    void Update()
    {
        
    }
}
