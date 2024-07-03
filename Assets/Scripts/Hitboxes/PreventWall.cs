using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventWall : MonoBehaviour
{

    PlayerScript player;

    void Start()
    {
        player = GetComponentInParent<PlayerScript>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "player" || collision.tag == "block") player.WallTriggerEnter(collision);
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "player" || collision.tag == "block") player.WallTriggerExit(collision);
    }
}
