using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class TutorialSequence1 : Sequences
{
    [Header("Sequence Setup")]
    [SerializeField] GameObject StartUI;
    [SerializeField] GameObject Camera1;
    [SerializeField] GameObject Camera2;
    [SerializeField] GameObject Player1;
    [SerializeField] GameObject Player2;
    [SerializeField] GameObject GenerateEffect1;  
    [SerializeField] GameObject GenerateEffect2;  


    [SerializeField] List<IEnumerator> functions = new List<IEnumerator>();
    [SerializeField] List<float> delays = new List<float>();

    public void makeSeqeunce()
    {
        functions.Add(SeqActive(StartUI, true));
        delays.Add(1f);

        functions.Add(SeqActive(StartUI, false));
        delays.Add(1f);

        functions.Add(SeqActive(StartUI, true));
        delays.Add(1f);

        functions.Add(SeqActive(StartUI, false));
        delays.Add(1f);

        functions.Add(composite());
        delays.Add(1f);

        functions.Add(SeqActive(GenerateEffect1, true));
        delays.Add(1f);

        functions.Add(InputKeyboardSequence(Player1));
        delays.Add(1f);

        functions.Add(SeqActive(GenerateEffect2, true));
        delays.Add(1f);

        GenerateSequence(functions, delays);
    }

    protected IEnumerator composite()
    {
        Debug.Log(QueSeqeunce.Count);
        StartCoroutine(SeqActive(Camera1, false));
        StartCoroutine(SeqActive(Camera2, true));
        yield return null;
    }

    protected IEnumerator InputKeyboardSequence(GameObject _player)
    {
        float rotateTime;
        float MovedTime = 0f;

        while (MovedTime > 2f)
        {
            float vert = Input.GetAxis("Vertical");
            float hori = Input.GetAxis("Horizontal");

            if (hori != 0)
            {
                if (hori > 0.5f && _player.GetComponent<PlayerScript>().DirectionCheck || hori < -0.5f && !_player.GetComponent<PlayerScript>().DirectionCheck)
                {
                    rotateTime = 90f;
                }
                _player.GetComponent<PlayerScript>().SightChange(hori);

                MovedTime += Time.deltaTime;
            }

            // if (!isMovable) return;

            Vector3 moveDir = Vector3.zero;
            moveDir.x = hori * _player.GetComponent<PlayerManager>().PlayerSpeed;
            moveDir.y = _player.GetComponent<PlayerScript>().GetRigidbody.velocity.y;
            _player.GetComponent<PlayerScript>().GetRigidbody.velocity = _player.transform.rotation * moveDir;

            yield return null;
        }



    }

}
