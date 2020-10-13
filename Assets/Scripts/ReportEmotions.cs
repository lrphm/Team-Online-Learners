using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportEmotions : MonoBehaviour
{
    //public GameObject emotionalState;
    private float timer;
    private SpriteRenderer emotionalMarker;
    private static float xScale, yScale;
    // Start is called before the first frame update

    private List<Emotion> emotionTrack = new List<Emotion>();
    private List<GameObject> graphBars = new List<GameObject>();
    // Lists populated in code inspector
    public List<string> emotionalSequence;
    private Emotion nextEmotion, previousEmotion;
    private int emotionTrackIndex = 0;

    private EmotionGraph emotionGraph;
    void Start()
    {
        timer = 0.0f;
        GetGraphBars();
        //emotionalMarker = emotionalState.GetComponent<SpriteRenderer>();
        xScale = graphBars[0].transform.localScale.x;
        yScale = graphBars[0].transform.localScale.y;
        //emotionTrack.Add(new Emotion(EmotionName.happy, 3, 0));
        //emotionTrack.Add(new Emotion(EmotionName.happy, 3, 8));
        //emotionTrack.Add(new Emotion(EmotionName.happy, 1, 10));
        //emotionTrack.Add(new Emotion(EmotionName.angry, 1, 10));
        //emotionTrack.Add(new Emotion(EmotionName.angry, 3, 11));
        //emotionTrack.Add(new Emotion(EmotionName.angry, 3, 15));
        //emotionTrack.Add(new Emotion(EmotionName.angry, 1, 25));
        //emotionTrack.Add(new Emotion(EmotionName.disgusted, 1, 25));
        //emotionTrack.Add(new Emotion(EmotionName.disgusted, 2, 30));
        //emotionTrack.Add(new Emotion(EmotionName.disgusted, 1, 40));
        //emotionTrack.Add(new Emotion(EmotionName.disgusted, 1, 60));
        //emotionTrack.Add(new Emotion(EmotionName.happy, 1, 60));
        //emotionTrack.Add(new Emotion(EmotionName.happy, 2, 70));
        PopulateEmotionalTrack();
        nextEmotion = emotionTrack[emotionTrackIndex];
        emotionGraph = new EmotionGraph(30, nextEmotion, graphBars);
    }

    public void GetGraphBars()
    {
        GameObject emotionGraph = this.gameObject.transform.Find("EmotionGraph").gameObject;
        foreach (Transform barTrans in emotionGraph.transform)
        {
            graphBars.Add(barTrans.gameObject);
        }
        Debug.Log(graphBars.Count);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= nextEmotion.time)
        {
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
        else if(previousEmotion.Equals(nextEmotion))
        {
            float lerpMagnitude = Emotion.MagLerp(timer, previousEmotion, nextEmotion);
            emotionGraph.addToStream(new Emotion(nextEmotion.emotionDef.emotionName,lerpMagnitude,timer));
            //emotionalState.transform.localScale = new Vector3(xScale, yScale * lerpMagnitude);
        }
    }

    public void PopulateEmotionalTrack()
    {
        foreach(String emotionText in emotionalSequence)
        {
            string[] args = emotionText.Split(' ');
            Enum.TryParse(args[0], out EmotionName emotionName);
            float.TryParse(args[1], out float magnitude);
            float.TryParse(args[2], out float time);

            emotionTrack.Add(new Emotion(emotionName, magnitude, time));
        }

        //int index = 0;
        //while(index <= emotionTrack.Count - 2)
        //{
        //    Emotion currentEmotion = emotionTrack[index];
        //    Emotion followingEmotion = emotionTrack[index + 1];
        //    if (!currentEmotion.Equals(followingEmotion) && currentEmotion.time != followingEmotion.time)
        //        return;
        //}
    }

    private class EmotionGraph
    {
        private int rangeSeconds;
        private int numBars; // trailing bars
        public Emotion currentEmotion;
        private List<Emotion> emotionStream;
        private List<Emotion> graph;
        private List<GameObject> graphBars;

        public EmotionGraph(int rangeSeconds, Emotion startEmotion, List<GameObject> graphBars)
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
                graph[i].Draw(this.graphBars[i]);
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
            get { return this.emotionDef.colour; }
        }
        
        public Emotion(EmotionName emotionName, float magnitude, float time)
        {
            this.emotionDef = new EmotionDefinition(emotionName);
            this.magnitude = magnitude;
            this.time = time;
        }

        public bool Equals(Emotion otherEmotion)
        {
            return this.emotionDef.emotionName == otherEmotion.emotionDef.emotionName;
        }

        public void Draw(GameObject graphBar)
        {
            graphBar.transform.localScale = new Vector3(xScale, yScale * this.magnitude);
            graphBar.GetComponent<SpriteRenderer>().color = this.colour;
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
            return new Emotion(EmotionName.happy, 0, time);
        }
    } 

    private struct EmotionDefinition
    {
        public EmotionName emotionName;
        public Color colour;

        public EmotionDefinition(EmotionName emotionName, Color colour)
        {
            this.emotionName = emotionName;
            this.colour = colour;
        }

        public EmotionDefinition(EmotionName emotionName)
        {
            this.emotionName = emotionName;
            switch(emotionName)
            {
                case EmotionName.happy:
                    colour = Color.green;
                    break;
                case EmotionName.sad:
                    colour = Color.blue;
                    break;
                case EmotionName.angry:
                    colour = Color.red;
                    break;
                case EmotionName.disgusted:
                    colour = Color.magenta;
                    break;
                case EmotionName.scared:
                    colour = Color.yellow;
                    break;
                case EmotionName.confused:
                    colour = Color.grey;
                    break;
                default:
                    colour = Color.green;
                    break;
            }
        }
    }

    private enum EmotionName
    {
        happy,
        sad,
        disgusted,
        angry,
        scared,
        confused
    }
}
