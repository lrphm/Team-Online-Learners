using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    // Start is called before the first frame update
    private int scenario;
    public static float[] offsets = { 13,0,0};
    public static bool colourBlind = false;
    public List<GameObject> participants;
    public GameObject userCam;
    public GameObject alertBoxPrefab;
    public List<GameObject> alertBoxes = new List<GameObject>();
    private string[] names = { "jason", "carl", "jess" };

    void Start()
    {
        //this.Init();
    }

    public void Init()
    {
        RemoveAllAlertBoxes();
        scenario = ParamTracker.scenario;
        string resourceFolder = $"Scenario {scenario}/";
        GameObject participant;
        for (int i = 0; i < participants.Count; i++)
        {
            participant = participants[i];
            participant.transform.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().clip = Resources.Load(resourceFolder + names[i]) as UnityEngine.Video.VideoClip;
            if (i > 0)
                participant.transform.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().SetDirectAudioMute(0, true);
            participant.GetComponent<ReportEmotions>().emotionalSequence = GetEmotionalSeq(names[i]);
            participant.SetActive(true);
            participant.GetComponent<ReportEmotions>().Init();
        }
        userCam.GetComponent<UserWebCam>().Init();
        SetLegend();
        SetButtonText();
    }

    public void AddAlertBox(string alertText)
    {
        GameObject alertBox = GameObject.Instantiate(alertBoxPrefab, this.gameObject.transform);
        alertBox.GetComponentInChildren<UnityEngine.UI.Text>().text = alertText;
        if (alertBoxes.Count > 0)
        {
            float offset = 8.0f;
            Vector3 offsetPosition = alertBoxes[alertBoxes.Count - 1].transform.localPosition;
            offsetPosition += new Vector3(offset, -offset);
            alertBox.transform.localPosition = offsetPosition;
        }

        alertBoxes.Add(alertBox);

    }

    public void RemoveAlertBox(GameObject closedAlertBox)
    {
        alertBoxes.Remove(closedAlertBox);
        Destroy(closedAlertBox);
    }

    void RemoveAllAlertBoxes()
    {
        for(int i = alertBoxes.Count - 1; i >= 0; i--)
            RemoveAlertBox(alertBoxes[i]);
    }

    void SetLegend()
    {
        ReportEmotions.EmotionName[] emotions = (ReportEmotions.EmotionName[])System.Enum.GetValues(typeof(ReportEmotions.EmotionName));
        List<GameObject> legendObjects = new List<GameObject>();
        
        foreach (Transform child in this.gameObject.transform)
        {
            if (child.gameObject.name.StartsWith("Text"))
                legendObjects.Add(child.gameObject);
        }

        for (int i = 0; i < emotions.Length; i++)
        {
            ReportEmotions.EmotionDefinition newEmotion = ReportEmotions.EmotionDefinition.GetEmotionDefinition(emotions[i]);
            legendObjects[i].GetComponent<UnityEngine.UI.Text>().text = newEmotion.emotionName.ToString();
            legendObjects[i].GetComponentInChildren<SpriteRenderer>().color = newEmotion.Colour;
            // set z value to 0 because for some stupid reason this sometimes changes to massive -ve value
            Transform spriteTrans = legendObjects[i].transform.Find("Square").transform;
            spriteTrans.position = new Vector3(spriteTrans.position.x, spriteTrans.position.y, 0);
        }
    }

    public void ToggleColourPalette()
    {
        ScenarioManager.colourBlind = !ScenarioManager.colourBlind;
        SetButtonText();
        SetLegend();
        for (int i = 0; i < participants.Count; i++)
        {
            participants[i].GetComponent<ReportEmotions>().ToggleEmotionGraphColours();
        }
    }

    void SetButtonText()
    {
        GameObject button = this.gameObject.transform.Find("ChangeColours").gameObject;
        button.GetComponentInChildren<UnityEngine.UI.Text>().text = colourBlind ? "Standard Colour Palette" : "Colour Blind Palette";
    }

    List<string> GetEmotionalSeq(string name)
    {
        List<string> emotionalSeq = new List<string>();

        switch (name)
        {
            case "carl":
                switch(scenario)
                {
                    case 1:
                        emotionalSeq.Add("happy 0.7 13");
                        emotionalSeq.Add("happy 0.7 40");
                        emotionalSeq.Add("happy 0.1 42");
                        emotionalSeq.Add("happy 0.1 56");
                        emotionalSeq.Add("happy 0.4 60");
                        emotionalSeq.Add("happy 0.7 65");
                        break;
                    case 2:
                        emotionalSeq.Add("happy 0.4 0");
                        emotionalSeq.Add("happy 0.4 27");
                        emotionalSeq.Add("happy 1 28");
                        emotionalSeq.Add("happy 0.5 32");
                        emotionalSeq.Add("happy 0.6 45");
                        emotionalSeq.Add("happy 0.3 48");
                        break;
                    case 3:
                        emotionalSeq.Add("happy 0.3 0");
                        emotionalSeq.Add("happy 0.3 10");
                        emotionalSeq.Add("happy 0.6 12");
                        emotionalSeq.Add("happy 0.4 15");
                        emotionalSeq.Add("happy 0.3 20");
                        emotionalSeq.Add("happy 0.1 25");
                        emotionalSeq.Add("angry 0.1 25");
                        emotionalSeq.Add("angry 0.1 28");
                        emotionalSeq.Add("angry 0.5 29");
                        emotionalSeq.Add("angry 0.7 38");
                        emotionalSeq.Add("angry 1 40");
                        emotionalSeq.Add("angry 1 48");
                        emotionalSeq.Add("angry 1.3 50");
                        emotionalSeq.Add("angry 1.3 55");
                        emotionalSeq.Add("angry 1.5 56");
                        break;
                    default:
                        break;
                }
                break;

            case "jason":
                switch (scenario)
                {
                    case 1:
                        emotionalSeq.Add("happy 0.5 13");
                        emotionalSeq.Add("happy 0.5 36");
                        emotionalSeq.Add("happy 0.1 37");
                        emotionalSeq.Add("happy 0.1 58");
                        emotionalSeq.Add("happy 0.5 65");
                        break;
                    case 2:
                        emotionalSeq.Add("happy 0.2 0");
                        emotionalSeq.Add("happy 0.2 29");
                        emotionalSeq.Add("happy 1 30");
                        emotionalSeq.Add("happy 0.4 35");
                        emotionalSeq.Add("happy 0.2 43");
                        emotionalSeq.Add("happy 0.1 45");
                        emotionalSeq.Add("happy 0.1 55");
                        emotionalSeq.Add("happy 0.05 60");
                        emotionalSeq.Add("happy 0.05 62");
                        emotionalSeq.Add("happy 0.1 69");
                        emotionalSeq.Add("happy 0.3 75");
                        emotionalSeq.Add("happy 0.6 78");
                        break;
                    case 3:
                        emotionalSeq.Add("happy 1.5 0");
                        emotionalSeq.Add("happy 1.5 18");
                        emotionalSeq.Add("happy 1 25");
                        emotionalSeq.Add("happy 0.8 34");
                        emotionalSeq.Add("happy 0.8 43");
                        emotionalSeq.Add("happy 0.5 46");
                        emotionalSeq.Add("happy 0.5 53");
                        emotionalSeq.Add("happy 0.7 55");
                        break;
                    default:
                        break;
                }
                break;

            case "jess":
                switch (scenario)
                {
                    case 1:
                        emotionalSeq.Add("happy 0.2 13");
                        emotionalSeq.Add("happy 0.2 17");
                        emotionalSeq.Add("happy 1 19");
                        emotionalSeq.Add("happy 0.6 31");
                        emotionalSeq.Add("scared 0.2 31");
                        emotionalSeq.Add("scared 0.6 35");
                        emotionalSeq.Add("scared 2 36");
                        emotionalSeq.Add("scared 2 40");
                        emotionalSeq.Add("scared 3 42");
                        emotionalSeq.Add("scared 3 48");
                        emotionalSeq.Add("scared 2 56");
                        emotionalSeq.Add("scared 2 62");
                        emotionalSeq.Add("scared 1 65");
                        emotionalSeq.Add("scared 0.2 68");
                        emotionalSeq.Add("scared 0.2 73");
                        emotionalSeq.Add("happy 0.5 73");
                        emotionalSeq.Add("happy 1.5 77");
                        break;
                    case 2:
                        emotionalSeq.Add("sad 0.5 0");
                        emotionalSeq.Add("sad 0.5 33");
                        emotionalSeq.Add("sad 1.6 39");
                        emotionalSeq.Add("sad 1.6 56");
                        emotionalSeq.Add("sad 1 65");
                        emotionalSeq.Add("sad 1 66");
                        emotionalSeq.Add("sad 0.6 71");
                        emotionalSeq.Add("sad 0.1 78");
                        emotionalSeq.Add("happy 0.1 78");
                        emotionalSeq.Add("happy 0.3 83");
                        break;
                    case 3:
                        emotionalSeq.Add("happy 0.3 0");
                        emotionalSeq.Add("happy 0.3 2");
                        emotionalSeq.Add("happy 0.6 3");
                        emotionalSeq.Add("happy 0.3 6");
                        emotionalSeq.Add("happy 0.3 17");
                        emotionalSeq.Add("happy 0.1 21");
                        emotionalSeq.Add("angry 0.1 23");
                        emotionalSeq.Add("angry 0.3 25");
                        emotionalSeq.Add("angry 1 29");
                        emotionalSeq.Add("angry 1.4 34");
                        emotionalSeq.Add("angry 1.4 38");
                        emotionalSeq.Add("angry 1.8 40");
                        emotionalSeq.Add("angry 1.8 48");
                        emotionalSeq.Add("angry 2.1 50");
                        emotionalSeq.Add("angry 2.5 56");
                        break;
                    default:
                        break;
                }
                break;

            default:
                break;

        }
        return emotionalSeq;
    }

    public void Disable()
    {
        foreach (GameObject participant in participants)
        {
            participant.GetComponent<ReportEmotions>().Disable();
            participant.transform.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().Stop();
            participant.transform.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().clip = null;
        }
            
        userCam.GetComponent<UserWebCam>().Disable();
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick()
    {
        ParamTracker.scenario++;
        if (ParamTracker.scenario > 3)
            ParamTracker.scenario = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);        
    }
}
