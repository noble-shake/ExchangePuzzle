using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage4Sequence : Sequences
{
    [Header("Sequence Setup")]
    [SerializeField] GameObject StartUI;
    [SerializeField] GameObject Camera1;
    [SerializeField] GameObject Camera2;
    [SerializeField] GameObject Camera3;
    [SerializeField] GameObject Camera4;
    PlayerScript Player1;
    PlayerScript Player2;
    [SerializeField] GameObject GenerateEffect1;
    [SerializeField] GameObject GenerateEffect2;

    [SerializeField] List<IEnumerator> functions;
    [SerializeField] List<float> delays;

    [SerializeField] bool SequenceTrigger;


    private void Start()
    {
        GameManager.instance.StageInit();
        StageStartSequence();
    }


    public void StageStartSequence()
    {
        Player1 = PlayerManager.instance.RegistryPlayer1;
        Player2 = PlayerManager.instance.RegistryPlayer2;

        functions = new List<IEnumerator>();
        delays = new List<float>();

        functions.Add(SeqActive(StartUI, true));
        delays.Add(1f);

        functions.Add(SeqActive(StartUI, false));
        delays.Add(1f);

        functions.Add(SeqActive(StartUI, true));
        delays.Add(1f);

        functions.Add(SeqActive(StartUI, false));
        delays.Add(1f);

        functions.Add(compositeCam12());
        delays.Add(1f);

        functions.Add(compositeCam23());
        delays.Add(1f);

        functions.Add(compositeCam34());
        delays.Add(1f);

        Vector3 TargetPos = new Vector3(GenerateEffect1.transform.position.x, GenerateEffect1.transform.position.y, Camera2.transform.position.z);
        functions.Add(SeqObjectWalk(Camera4, TargetPos, 50f));
        delays.Add(0.3f);

        functions.Add(SeqActive(GenerateEffect1, true));
        delays.Add(1f);

        functions.Add(SeqActive(GenerateEffect2, true));
        delays.Add(1f);

        functions.Add(CameraLive(PlayerTag.Player1));
        delays.Add(1);

        functions.Add(SeqDialPlay(9, gameObject));
        delays.Add(0.3f);

        GenerateSequence(functions, delays);
    }

    protected IEnumerator compositeCam12()
    {
        StartCoroutine(SeqActive(Camera1, false));
        StartCoroutine(SeqActive(Camera2, true));
        yield return null;
    }

    protected IEnumerator compositeCam23()
    {
        StartCoroutine(SeqActive(Camera2, false));
        StartCoroutine(SeqActive(Camera3, true));
        yield return null;
    }

    protected IEnumerator compositeCam34()
    {
        StartCoroutine(SeqActive(Camera3, false));
        StartCoroutine(SeqActive(Camera4, true));
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
}
