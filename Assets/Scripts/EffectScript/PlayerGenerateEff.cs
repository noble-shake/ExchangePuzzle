using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerateEffect : MonoBehaviour
{
    public PlayerTag playerTag;

    private void OnEnable()
    {
        Invoke("GeneratePlayer", 2f);
    }

    public void GeneratePlayer()
    {

        if (playerTag == PlayerTag.Player1)
        {
            PlayerManager.instance.StageInitForPlayer1();
            PlayerScript OwnPlayer = PlayerManager.instance.RegistryPlayer1;
            OwnPlayer.transform.position = transform.position;
            OwnPlayer.gameObject.SetActive(true);
        }
        else
        {
            PlayerManager.instance.StageInitForPlayer2();
            PlayerScript OwnPlayer = PlayerManager.instance.RegistryPlayer2;
            OwnPlayer.transform.position = transform.position;
            OwnPlayer.gameObject.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}
