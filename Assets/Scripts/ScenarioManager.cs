using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int scenario = 1;
    private float[] offsets = { 12,0,0};
    public List<GameObject> participants;
    private string[] names = { "carl", "jason", "jess" };

    void Start()
    {
        string resourceFolder = $"Scenario {scenario}/";
        GameObject participant;
        for (int i = 0; i < participants.Count; i++)
        {
            participant = participants[i];
            participant.transform.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().clip = Resources.Load(resourceFolder + names[i]) as UnityEngine.Video.VideoClip;
            participant.GetComponent<ReportEmotions>().emotionalSequence = GetEmotionalSeq(names[i]);
            participant.SetActive(true);
        }

        ReportEmotions.EmotionName[] emotions = (ReportEmotions.EmotionName[])System.Enum.GetValues(typeof(ReportEmotions.EmotionName));
        List<GameObject> legendObjects = new List<GameObject>();
        foreach(Transform child in this.gameObject.transform)
        {
            if (child.gameObject.name.StartsWith("Text"))
                legendObjects.Add(child.gameObject);
        }

        for(int i = 0; i < emotions.Length; i++)
        {
            ReportEmotions.EmotionDefinition newEmotion = ReportEmotions.EmotionDefinition.GetEmotionDefinition(emotions[i]);
            legendObjects[i].GetComponent<UnityEngine.UI.Text>().text = newEmotion.emotionName.ToString();
            legendObjects[i].GetComponentInChildren<SpriteRenderer>().color = newEmotion.colour;
        }
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
                        emotionalSeq.Add("happy 0.7 0");
                        emotionalSeq.Add("happy 0.7 40");
                        emotionalSeq.Add("happy 0.1 42");
                        emotionalSeq.Add("confused 0.2 42");
                        emotionalSeq.Add("confused 0.8 44");
                        emotionalSeq.Add("confused 0.8 50");
                        emotionalSeq.Add("confused 0.1 56");
                        emotionalSeq.Add("happy 0.1 56");
                        emotionalSeq.Add("happy 0.4 60");
                        emotionalSeq.Add("happy 0.7 65");
                        break;
                    case 2:
                        emotionalSeq.Add("happy 3 0");
                        break;
                    case 3:
                        emotionalSeq.Add("happy 3 0");
                        break;
                    default:
                        break;
                }
                break;

            case "jason":
                switch (scenario)
                {
                    case 1:
                        emotionalSeq.Add("happy 0.5 0");
                        emotionalSeq.Add("happy 0.5 36");
                        emotionalSeq.Add("happy 0.1 37");
                        emotionalSeq.Add("confused 0.1 37");
                        emotionalSeq.Add("confused 1.5 38");
                        emotionalSeq.Add("confused 1.5 43");
                        emotionalSeq.Add("confused 1 47");
                        emotionalSeq.Add("confused 0.4 51");
                        emotionalSeq.Add("confused 0.2 55");
                        emotionalSeq.Add("confused 0.2 58");
                        emotionalSeq.Add("happy 0.1 58");
                        emotionalSeq.Add("happy 0.5 65");
                        break;
                    case 2:
                        emotionalSeq.Add("happy 3 0");
                        break;
                    case 3:
                        emotionalSeq.Add("happy 3 0");
                        break;
                    default:
                        break;
                }
                break;

            case "jess":
                switch (scenario)
                {
                    case 1:
                        emotionalSeq.Add("happy 0.2 0");
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
                        emotionalSeq.Add("happy 3 0");
                        break;
                    case 3:
                        emotionalSeq.Add("happy 3 0");
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
