using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game UI")]
    [SerializeField] GameObject PauseUI;
    [SerializeField] GameObject WarpConnectBlock1;
    [SerializeField] GameObject WarpConnectBlock2;
    public GameObject WarpBlock1 { get { return WarpConnectBlock1; } set { WarpConnectBlock1 = value; } }
    public GameObject WarpBlock2 { get { return WarpConnectBlock2; } set { WarpConnectBlock2 = value; } }

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
        // PauseGame();
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

    public void RegistryBlock(GameObject _block, int _playerID)
    {
        switch (_playerID)
        {
            case 0:
                if (WarpBlock1 != null)
                {
                    WarpBlock1.GetComponent<BlockScript>().shutDownDimension();
                    WarpBlock1 = null;
                }
                WarpBlock1 = _block;
                if (WarpBlock2 != null)
                {
                    WarpBlock1.GetComponent<BlockScript>().SynchronizeBlock(WarpBlock2.GetComponent<BlockScript>());
                    WarpBlock2.GetComponent<BlockScript>().SynchronizeBlock(WarpBlock1.GetComponent<BlockScript>());
                    WarpBlock1.GetComponent<BlockScript>().openDimenstion();
                    WarpBlock2.GetComponent<BlockScript>().openDimenstion();
                }
                break;
            case 1:
                if (WarpBlock2 != null)
                {
                    WarpBlock2.GetComponent<BlockScript>().shutDownDimension();
                    WarpBlock2 = null;
                }
                WarpBlock2 = _block;
                if (WarpBlock1 != null)
                {
                    WarpBlock1.GetComponent<BlockScript>().SynchronizeBlock(WarpBlock2.GetComponent<BlockScript>());
                    WarpBlock2.GetComponent<BlockScript>().SynchronizeBlock(WarpBlock1.GetComponent<BlockScript>());
                    WarpBlock1.GetComponent<BlockScript>().openDimenstion();
                    WarpBlock2.GetComponent<BlockScript>().openDimenstion();
                }
                break;
        }
    }

    public void WaprConnect(GameObject _block, int _playerID)
    {

        // GameObject target = _playerID == 0 ? WarpConnectBlock1 : WarpConnectBlock2;
        //if (target == null)
        //{
        //    // color change;
        //    // direction check;
        //}
        //else
        //{
        //    // target block reset;
        //    // target parent check;
        //    // color change;
        //    // direction check;
        //}

    }

}
