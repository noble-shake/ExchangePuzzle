using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game UI")]
    [SerializeField] GameObject PauseUI;
    [SerializeField] GameObject WarpConnectBlock1;
    [SerializeField] GameObject WarpConnectBlock2;

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

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        PauseUI.SetActive(false);
    }


    void Update()
    {
        PauseGame();
    }

    private void PauseGame() {
        if (Input.GetKeyDown(KeyCode.Escape) && !PauseUI.activeSelf)
        {
            Time.timeScale = 0f;
            PauseUI.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && PauseUI.activeSelf)
        {
            Time.timeScale = 1f;
            PauseUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void WaprConnect(GameObject _block, int _playerID)
    { 
        GameObject target = _playerID == 0 ? WarpConnectBlock1 : WarpConnectBlock2;
        if (target == null)
        {
            target = _block;
            // color change;
            // direction check;
        }
        else
        {
            // target block reset;
            // target parent check;
            // color change;
            // direction check;
            target = _block;
        }
    }
}
