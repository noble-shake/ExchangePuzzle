using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSequence1 : Sequences
{
    [Header("Sequence Setup")]
    [SerializeField] GameObject StartUI;
    [SerializeField] GameObject Camera1;
    [SerializeField] GameObject Camera2;
    PlayerScript Player1;
    PlayerScript Player2;
    [SerializeField] GameObject GenerateEffect1;  
    [SerializeField] GameObject GenerateEffect2;
    [SerializeField] GameObject MoveGaugeUI;
    [SerializeField] Slider MoveGauge;
    [SerializeField] GameObject KeyX;
    [SerializeField] GameObject KeyC;
    [SerializeField] GameObject KeyZ;
    [SerializeField] GameObject KeyMouse;

    [SerializeField] List<IEnumerator> functions;
    [SerializeField] List<float> delays;


    private void Start()
    {
        GameManager.instance.StageInit();
        Stage1TutorialSequence1();
    }


    public void Stage1TutorialSequence1()
    {
        Player1 = PlayerManager.instance.RegistryPlayer1;
        Player2 = PlayerManager.instance.RegistryPlayer2;

        functions = new List<IEnumerator>();
        delays = new List<float>();

        MoveGauge.value = 0f;
        MoveGauge.maxValue = 2f;

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

        Vector3 TargetPos = new Vector3(GenerateEffect1.transform.position.x, GenerateEffect1.transform.position.y, Camera2.transform.position.z);
        functions.Add(SeqObjectWalk(Camera2, TargetPos, 10f));
        delays.Add(0.3f);

        functions.Add(SeqDialPlay(0, gameObject));
        delays.Add(0.3f);

        functions.Add(SeqActive(GenerateEffect1, true));
        delays.Add(1f);

        functions.Add(CameraLive(PlayerTag.Player1));
        delays.Add(0.5f);

        functions.Add(SeqActive(MoveGaugeUI, true));
        delays.Add(0f);

        functions.Add(SeqDialPlay(1, gameObject));
        delays.Add(0.3f);

        functions.Add(InputKeyboardSequence(Player1.gameObject));
        delays.Add(1f);

        functions.Add(SeqDialPlay(2, gameObject));
        delays.Add(0.3f);

        functions.Add(SeqActive(MoveGauge.gameObject, false));
        delays.Add(0f);

        functions.Add(CameraLive(PlayerTag.Player2));
        delays.Add(0.5f);

        TargetPos = new Vector3(GenerateEffect2.transform.position.x, GenerateEffect2.transform.position.y, Camera2.transform.position.z);
        functions.Add(SeqObjectWalk(Camera2, TargetPos, 10f));
        delays.Add(0.3f);

        functions.Add(SeqActive(GenerateEffect2, true));
        delays.Add(1f);

        functions.Add(SeqActive(KeyX, true));
        delays.Add(0f);

        functions.Add(SeqActive(KeyC, true));
        delays.Add(0f);

        functions.Add(SeqDialPlay(3, gameObject));
        delays.Add(0.3f);

        GenerateSequence(functions, delays);
    }

    protected IEnumerator composite()
    {
        StartCoroutine(SeqActive(Camera1, false));
        StartCoroutine(SeqActive(Camera2, true));
        yield return null;
    }

    protected IEnumerator CameraLive(PlayerTag _player)
    {
        switch (_player)
        {
            case PlayerTag.Player1:
                PlayerManager.instance.StageInitForPlayer1();
                break;
            case PlayerTag.Player2:
                PlayerManager.instance.StageInitForPlayer2();
                break;
        }

        yield return null;
        queTrigger = false;
    }

    protected IEnumerator InputKeyboardSequence(GameObject _player)
    {
        float rotateTime;
        float MovedTime = 0f;

        while (MovedTime < 2f)
        {
            float vert = Input.GetAxis("Vertical");
            float hori = Input.GetAxis("Horizontal");

            if (hori != 0)
            {
                _player.GetComponent<PlayerScript>().animWalk = true;

                if (hori > 0.5f && _player.GetComponent<PlayerScript>().DirectionCheck || hori < -0.5f 
                    && !_player.GetComponent<PlayerScript>().DirectionCheck)
                {
                    rotateTime = 90f;
                }
                _player.GetComponent<PlayerScript>().SightChange(hori);

                MovedTime += Time.deltaTime;
                MoveGauge.value = MovedTime;
                if (MovedTime > 2f)
                {
                    MoveGauge.value = 2f;
                }

                Vector3 moveDir = Vector3.zero;
                moveDir.x = hori * PlayerManager.instance.PlayerSpeed;
                moveDir.y = _player.GetComponent<PlayerScript>().GetRigidbody.velocity.y;
                _player.GetComponent<PlayerScript>().GetRigidbody.velocity = _player.transform.rotation * moveDir;
            }
            else
            {
                _player.GetComponent<PlayerScript>().animWalk = false;
            }

            yield return null;
        }
        queTrigger = false;

    }

}
