using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public enum SequenceMethods
{
    SeqObjectWalk,
    SeqDialPlay,
    SeqGenerate,
    SeqActive,
    SeqComposite,
    SeqCustom,
}

public class Sequences: MonoBehaviour
{
    [Header("Internal Setup")]
    [SerializeField] bool queTrigger;
    protected Queue<IEnumerator> QueSeqeunce = new Queue<IEnumerator>();

    protected IEnumerator SequencePlay()
    {
        while (QueSeqeunce.Count > 0)
        {   
            queTrigger = true;
            yield return StartCoroutine(QueSeqeunce.Dequeue());
            yield return new WaitUntil(() => queTrigger == false);
        }
        SequenceManager.instance.SequenceProcessing = false;

    }

    protected void GenerateSequence(List<IEnumerator> methods, List<float> delays)
    {
        SequenceManager.instance.SequenceProcessing = true;
        int count = methods.Count;

        for (int inum = 0; inum < count; inum++)
        {
            QueSeqeunce.Enqueue(Task(methods[inum], delays[inum]));
        }
        queTrigger = true;

        StartCoroutine(SequencePlay());
    }

    protected IEnumerator Task(IEnumerator _method, float delay)
    {
        yield return new WaitForSeconds(delay);
        yield return StartCoroutine(_method);
    }

    protected IEnumerator SeqObjectWalk(GameObject _object, Vector3? _pos = null, float speed = 10f)
    {
        while (Vector3.Distance(_object.transform.position, _pos.Value) > 0.001f)
        {
            Vector3.MoveTowards(_object.transform.position, _pos.Value, speed * Time.deltaTime);
            yield return null;
        }
        queTrigger = false;
    }

    protected IEnumerator SeqDialPlay(int _id)
    {
        SequenceManager.instance.DialogSystemStart(_id);
        yield return null;
        queTrigger = false;
    }

    protected IEnumerator SeqActive(GameObject _object, bool _active)
    {
        _object.SetActive(_active);
        yield return null;
        queTrigger = false;
    }
}
