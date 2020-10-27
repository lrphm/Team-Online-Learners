using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportEmotions : MonoBehaviour
{
    //public GameObject emotionalState;
    private string participantName;
    private float timer;
    private float xScale = -1, yScale = -1, yPos = -10, yHeight = -1;
    // Start is called before the first frame update

    private List<Emotion> emotionTrack = new List<Emotion>();
    private List<GameObject> graphBars = new List<GameObject>();
    // Lists populated in code inspector
    public List<string> emotionalSequence;
    private Emotion nextEmotion, previousEmotion;
    private ScenarioManager parentScript;
    private int emotionTrackIndex = 0;

    private static Dictionary<EmotionName, string> suggestions = new Dictionary<EmotionName, string>()
    {
        { EmotionName.scared, "If the cause is clear, perhaps you could help mitigate this." },
        { EmotionName.angry, "If the cause is clear, it might be helpful to address it. Otherwise, perhaps a cooldown period would help." },
        { EmotionName.sad, "If the cause is clear, it might be helpful to address it. Otherwise, reassurance or a compassionate inquiry might help." }
    };

    private EmotionGraph emotionGraph;
    
    void Start()
    {

    }
    public void Init()
    {
        parentScript = this.gameObject.transform.parent.gameObject.GetComponent<ScenarioManager>();
        participantName = this.gameObject.name;
        timer = 0.0f;
        emotionTrackIndex = 0;
        emotionTrack.Clear();
        graphBars.Clear();
        GetGraphBars();

        if (xScale == -1 && yScale == -1)
        {
            xScale = graphBars[0].transform.localScale.x;
            yScale = graphBars[0].transform.localScale.y;
            yPos = graphBars[0].transform.position.y;
            yHeight = graphBars[0].GetComponent<SpriteRenderer>().bounds.size.y;
        }
        PopulateEmotionalTrack();
        previousEmotion = new Emotion(EmotionName.neutral, 0f, 0f);
        nextEmotion = emotionTrack[emotionTrackIndex];
        emotionGraph = new EmotionGraph(30, nextEmotion, graphBars, xScale, yScale, yPos, yHeight);
        StartCoroutine(EmotionScript());        
    }

    public void Disable()
    {
        StopAllCoroutines();
    }

    public void GetGraphBars()
    {
        GameObject emotionGraph = this.gameObject.transform.Find("EmotionGraph").gameObject;
        foreach (Transform barTrans in emotionGraph.transform)
        {
            graphBars.Add(barTrans.gameObject);
            // set z value
            barTrans.localPosition = new Vector3(barTrans.localPosition.x, barTrans.localPosition.y, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleEmotionGraphColours()
    {
        this.emotionGraph.ToggleColours();
    }

    IEnumerator EmotionScript()
    {
        while (true)
        {
            if (timer >= nextEmotion.time)
            {
                if(previousEmotion.emotionDef.emotionName == EmotionName.neutral && nextEmotion.emotionDef.emotionName != EmotionName.neutral)
                {
                    parentScript.AddAlertBox($"{participantName} has become {nextEmotion.emotionDef.emotionName}.\n{suggestions[nextEmotion.emotionDef.emotionName]}");
                }
                previousEmotion = nextEmotion;
                emotionGraph.addToStream(previousEmotion);
                //emotionalMarker.color = nextEmotion.colour;
                //emotionalState.transform.localScale = new Vector3(xScale, yScale * nextEmotion.magnitude);

                emotionTrackIndex++;
                if (emotionTrackIndex < emotionTrack.Count)
                    nextEmotion = emotionTrack[emotionTrackIndex];
                else
                    nextEmotion = new Emotion(nextEmotion.emotionDef.emotionName, nextEmotion.magnitude, timer);
                //emotionTrackIndex++;
                //if(emotionTrackIndex < emotionTrack.Count)
                //    nextEmotion = emotionTrack[++emotionTrackIndex];
            }
            else if (previousEmotion.Equals(nextEmotion))
            {
                float lerpMagnitude = Emotion.MagLerp(timer, previousEmotion, nextEmotion);
                emotionGraph.addToStream(new Emotion(nextEmotion.emotionDef.emotionName, lerpMagnitude, timer));
                //emotionalState.transform.localScale = new Vector3(xScale, yScale * lerpMagnitude);
            }

            timer += Time.deltaTime;
            //Debug.Log(timer);
            yield return null;
        }
    }

    public void PopulateEmotionalTrack()
    {
        foreach(String emotionText in emotionalSequence)
        {
            float offset = ScenarioManager.offsets[ParamTracker.scenario - 1];
            string[] args = emotionText.Split(' ');
            // if not legit emotion, set to neutral
            bool legitEmotion = Enum.TryParse(args[0], out EmotionName emotionName);
            if (!legitEmotion)
                emotionName = EmotionName.neutral;

            float.TryParse(args[1], out float magnitude);
            float.TryParse(args[2], out float time);
            time = time - offset;
            if(time >= 0)
                emotionTrack.Add(new Emotion(emotionName, magnitude, time));
        }
    }

    private class EmotionGraph
    {
        private int rangeSeconds;
        private int numBars; // trailing bars
        public Emotion currentEmotion;
        private List<Emotion> emotionStream;
        private List<Emotion> graph;
        private List<GameObject> graphBars;
        private float xScale, yScale, yPos, yHeight;

        public EmotionGraph(int rangeSeconds, Emotion startEmotion, List<GameObject> graphBars, float xScale, float yScale, float yPos, float yHeight)
        {
            this.rangeSeconds = rangeSeconds;
            this.numBars = graphBars.Count - 1;
            this.currentEmotion = startEmotion;
            this.graphBars = graphBars;
            emotionStream = new List<Emotion>();
            graph = new List<Emotion>(numBars + 1); // add one for head
            // initialise graph
            for (int i = 0; i < graph.Capacity; i++)
            {
                graph.Add(currentEmotion);
            }

            this.xScale = xScale;
            this.yScale = yScale;
            this.yPos = yPos;
            this.yHeight = yHeight;
        }

        public void addToStream(Emotion emotion)
        {
            this.emotionStream.Add(emotion);
            this.currentEmotion = emotion;
            int i = 0;
            // truncate stream, prevent from getting massive
            while(emotionStream.Count > 0 && emotionStream[i].time < currentEmotion.time - rangeSeconds - 5)
            {
                emotionStream.RemoveAt(i);
            }

            this.UpdateGraph();
        }

        public void UpdateGraph()
        {
            // what to do about head
            graph[graph.Count - 1] = currentEmotion;
            float subjectTime;
            int streamIndex = emotionStream.Count - 1;
            for(int i = numBars - 1; i >= 0; i--)
            {
                subjectTime = currentEmotion.time - rangeSeconds + ((float)rangeSeconds * (float)i / (float)numBars);
                if (subjectTime < 0)
                    graph[i] = Emotion.GetUndefined(subjectTime);
                else
                {
                    while(emotionStream[streamIndex].time > subjectTime)
                    {
                        streamIndex--;
                    }
                    graph[i] = emotionStream[streamIndex];
                }
            }
            this.Redraw();
        }

        public void Redraw()
        {
            for(int i = 0; i < graph.Count; i++)
            {
                graph[i].Draw(this.graphBars[i], xScale, yScale, yPos, yHeight);
            }
        }

        public void ToggleColours()
        {
            for (int i = 0; i < graph.Count; i++)
            {
                graph[i].ToggleColour(this.graphBars[i]);
            }
        }
    }

    private class Emotion
    {
        public EmotionDefinition emotionDef;
        public float magnitude;
        public float time;

        public Color colour
        {
            get { return this.emotionDef.Colour; }
        }
        
        public Emotion(EmotionName emotionName, float magnitude, float time)
        {
            this.emotionDef = new EmotionDefinition(emotionName);
            this.magnitude = emotionName == EmotionName.neutral ? 0.1f : magnitude;
            this.time = time;
        }

        public bool Equals(Emotion otherEmotion)
        {
            return this.emotionDef.emotionName == otherEmotion.emotionDef.emotionName;
        }

        public void Draw(GameObject graphBar, float xScale, float yScale, float yPos, float yHeight)
        {
            SpriteRenderer sprite = graphBar.GetComponent<SpriteRenderer>();
            graphBar.transform.localScale = new Vector3(xScale, yScale * this.magnitude);
            float yPosNew = yPos + (sprite.bounds.size.y - yHeight) / 2;
            graphBar.transform.position = new Vector3(graphBar.transform.position.x, yPosNew);
            sprite.color = this.colour;
        }

        public void ToggleColour(GameObject graphBar)
        {
            SpriteRenderer sprite = graphBar.GetComponent<SpriteRenderer>();
            sprite.color = this.colour;
        }

        public static float MagLerp(float currentTime, Emotion firstEmotion, Emotion secondEmotion)
        {
            float y0, y1, x0, x1;
            y0 = firstEmotion.magnitude;
            y1 = secondEmotion.magnitude;
            x0 = firstEmotion.time;
            x1 = secondEmotion.time;

            return (y0 * (x1 - currentTime) + y1 * (currentTime - x0)) / (x1 - x0);
        }

        public static Emotion GetUndefined(float time)
        {
            return new Emotion(EmotionName.neutral, 0.1f, time);
        }
    } 

    public struct EmotionDefinition
    {
        public EmotionName emotionName;
        public Color Colour {
            get
            {
                Color colour;
                switch (emotionName)
                {
                    case EmotionName.neutral:
                        colour = ScenarioManager.colourBlind ? new Color(1, 0.690f, 0) : Color.grey;
                        break;
                    case EmotionName.sad:
                        colour = ScenarioManager.colourBlind ? new Color(0.392f, 0.560f, 1) : Color.blue;
                        break;
                    case EmotionName.angry:
                        colour = ScenarioManager.colourBlind ? new Color(0.862f, 0.149f, 0.498f) : Color.red;
                        break;
                    case EmotionName.frustrated:
                        colour = ScenarioManager.colourBlind ? new Color(0.470f, 0.368f, 0.941f) : new Color(0.5143642f, 0.1058823f, 0.8392157f); // purple
                        break;
                    case EmotionName.scared:
                        colour = ScenarioManager.colourBlind ? new Color(0.996f, 0.380f, 0) : new Color(0.8396226f, 0.5578821f, 0.1069331f); // orange
                        break;
                    default:
                        colour = Color.green;
                        break;
                }
                return colour;
            }
        }

        public EmotionDefinition(EmotionName emotionName)
        {
            this.emotionName = emotionName;
        }

        public static EmotionDefinition GetEmotionDefinition(EmotionName emotionName)
        {
            return new EmotionDefinition(emotionName);
        }

    }

    public enum EmotionName
    {
        neutral,
        sad,
        frustrated,
        angry,
        scared
    }
}
