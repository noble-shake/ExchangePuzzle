using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogs
{
    public int ID;
    public string CharacterName;
    public string Context;
    public string SpriteName;
    public string Comment;
}

[System.Serializable]
public class DialogsRead
{
    public Dialogs info;
    public bool check_read;
}

[System.Serializable]
public class DialogCycle
{
    public List<DialogsRead> info = new List<DialogsRead>();
    public int cycle_index;
    public bool check_cycle_read;
}

public class SequenceManager : MonoBehaviour
{

    public static SequenceManager instance;


    // [SerializeField] InputActionControls sequenceAction;
    bool _interact;

    [Header("json serialize")]
    public TextAsset JsonFile;
    [SerializeField] Dialogs[] TypeWriter;
    [SerializeField] Dictionary<int, List<DialogsRead>> ScriptData;
    [SerializeField] DialogCycle contents;

    [Header("Reading Store")]
    public string t_context;
    public string t_spriteName;
    public string t_voiceName;
    public Queue<string> text_seq = new Queue<string>();
    public Queue<string> sprite_seq = new Queue<string>();
    public Queue<string> voice_seq = new Queue<string>();

    [Header("Dialog Target")]
    private TextMeshProUGUI textName;
    private TextMeshProUGUI textContext;
    public GameObject DialogBarUI; // DialogName, DialogText
    public float delay;
    public GameObject CutsceneUI;

    [Header("Coroutine Definition")]
    [SerializeField] IEnumerator SequanceIEnum;
    [SerializeField] IEnumerator SkipIEnum;


    public bool isEnd;
    public bool SequenceProcessing { get { return isEnd; } set { isEnd = value; } }

    public IEnumerator DialogSystemStart(int _id)
    {
        textName = DialogBarUI.GetComponent<DialogBarScript>().dialogName;
        textContext = DialogBarUI.GetComponent<DialogBarScript>().dialogText;

        DialogBarUI.gameObject.SetActive(true);
        // yield return new WaitForSeconds(0.5f);
        // Cutscene UI active
        for (int i = 0; i < ScriptData[_id].Count; i++)
        {
            ScriptData[_id][i].check_read = false;
        }

        contents = new DialogCycle() { cycle_index = 0, check_cycle_read = false, info = ScriptData[_id] };

        for (int i = 0; i < contents.info.Count; i++)
        {
            text_seq.Enqueue(contents.info[i].info.Context);
            sprite_seq.Enqueue(contents.info[i].info.SpriteName);
        }

        for (int i = 0; i < contents.info.Count; i++)
        {
            textName.text = contents.info[i].info.CharacterName;
            t_context = text_seq.Dequeue();
            t_spriteName = sprite_seq.Dequeue();
            // t_voiceName = voice_seq.Dequeue();

            if (t_spriteName != "-")
            {
                // GameManager.instance.screenFadeInEffect();
                Sprite targetSpr = Resources.Load<Sprite>("Cutscenes/" + t_spriteName);
                GameObject cutSceneObj = CutsceneUI.transform.GetChild(0).gameObject;
                cutSceneObj.GetComponent<Image>().sprite = targetSpr;
                cutSceneObj.SetActive(true);
            }

            SequanceIEnum = SequanceCoroutine(i);
            StartCoroutine(SequanceIEnum);

            yield return new WaitUntil(() =>
            {
                if (contents.info[i].check_read)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });

        }


        isEnd = true;
        CutsceneUI.transform.GetChild(0).gameObject.SetActive(false);
        // GameManager.instance.screenFadeOutEffect();
    }

    public IEnumerator SequanceCoroutine(int index)
    {
        SkipIEnum = SkipSequanceCoroutine(SequanceIEnum, index);
        StartCoroutine(SkipIEnum);
        textContext.text = "";

        foreach (char letter in t_context.ToCharArray())
        {
            textContext.text += letter;
            yield return new WaitForSeconds(delay);
        }

        StopCoroutine(SkipIEnum);
        IEnumerator next = nextContext(index);
        StartCoroutine(next);
    }

    public IEnumerator SkipSequanceCoroutine(IEnumerator _seq, int index)
    {
        yield return new WaitForSecondsRealtime(0.3f);
        // yield return new WaitUntil(() => _interact);
        yield return new WaitUntil(() => Input.GetKey(KeyCode.Z));
        StopCoroutine(_seq);
        textContext.text = t_context;
        IEnumerator nextDialog = nextContext(index);
        StartCoroutine(nextDialog);
    }

    public IEnumerator nextContext(int index)
    {
        StopCoroutine(SequanceIEnum);
        StopCoroutine(SkipIEnum);
        yield return new WaitForSecondsRealtime(0.3f);
        // yield return new WaitUntil(() => _interact);
        yield return new WaitUntil(() => Input.GetKey(KeyCode.Z));
        DisplayNext(index);
    }

    public void DisplayNext(int index)
    {
        if (text_seq.Count == 0)
        {
            DialogBarUI.gameObject.SetActive(false);
        }
        StopCoroutine(SequanceIEnum);
        contents.info[index].check_read = true;
    }

    public bool dialogRead()
    {
        if (!contents.check_cycle_read)
        {
            return true;
        }

        return false;
    }

    public bool getIsEnd()
    {
        return isEnd;
    }


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

    // Start is called before the first frame update
    void Start()
    {
        ScriptData = new Dictionary<int, List<DialogsRead>>();
        JsonDeserialized();
    }

    private void HandleSequence(bool _bool)
    {
        _interact = _bool;
    }

    public void JsonDeserialized()
    {
        var result = JsonConvert.DeserializeObject<Dialogs[]>(JsonFile.ToString());
        foreach (var temp_dialog in result)
        {
            int matchKey = (int)temp_dialog.ID;
            // Debug.Log(matchKey);
            DialogsRead data = new DialogsRead() { info = temp_dialog, check_read = false };

            if (ScriptData.ContainsKey(matchKey))
            {

                ScriptData[matchKey].Add(data);
            }
            else
            {
                ScriptData.Add(matchKey, new List<DialogsRead>());
                ScriptData[matchKey].Add(data);
            }
        }
    }


}
