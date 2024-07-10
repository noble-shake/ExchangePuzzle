using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Sequence : Sequences
{
    [Header("Sequence Setup")]
    [SerializeField] GameObject StartUI;
    [SerializeField] GameObject Camera1;
    [SerializeField] GameObject Camera2;
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

        functions.Add(composite());
        delays.Add(1f);

        Vector3 TargetPos = new Vector3(GenerateEffect1.transform.position.x, GenerateEffect1.transform.position.y, Camera2.transform.position.z);
        functions.Add(SeqObjectWalk(Camera2, TargetPos, 10f));
        delays.Add(0.3f);

        functions.Add(SeqActive(GenerateEffect1, true));
        delays.Add(1f);

        functions.Add(SeqActive(GenerateEffect2, true));
        delays.Add(1f);

        functions.Add(CameraLive(PlayerTag.Player1));
        delays.Add(0.5f);

        functions.Add(SeqDialPlay(6, gameObject));
        delays.Add(0.3f);

        GenerateSequence(functions, delays);
    }

    public void CollideSequenceStart()
    {
        functions = new List<IEnumerator>();
        delays = new List<float>();

        functions.Add(SeqDialPlay(7, gameObject));
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player")
        {
            if (!SequenceTrigger)
            {
                SequenceTrigger = true;
                CollideSequenceStart();
            }
        }
    }
}
