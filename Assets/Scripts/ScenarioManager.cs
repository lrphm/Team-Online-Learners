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
                        emotionalSeq.Add("happy 1 0");
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
                        emotionalSeq.Add("scared 3 36");
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
