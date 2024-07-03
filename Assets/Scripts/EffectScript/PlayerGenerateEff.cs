using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerateEffect : MonoBehaviour
{
    [SerializeField] PlayerScript OwnPlayer;


    void Start()
    {
        
    }

    private void OnEnable()
    {
        Invoke("GeneratePlayer", 2f);
    }

    public void GeneratePlayer()
    {
        PlayerScript go = Instantiate(OwnPlayer);

        if (go.player == PlayerTag.Player1)
        {
            PlayerManager.instance.RegistryPlayer1 = go;
        }
        else
        {
            PlayerManager.instance.RegistryPlayer2 = go;

        }

        
        
        
        go.transform.position = transform.position;
        go.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
