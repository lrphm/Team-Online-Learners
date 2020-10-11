using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportEmotions : MonoBehaviour
{
    public GameObject emotionalState;
    private float timer;
    private SpriteRenderer emotionalMarker;
    private float xScale, yScale;
    // Start is called before the first frame update

    private List<Emotion> emotionTrack = new List<Emotion>();
    private Emotion nextEmotion, previousEmotion;
    private int emotionTrackIndex = 0;
    void Start()
    {
        timer = 0.0f;
        emotionalMarker = emotionalState.GetComponent<SpriteRenderer>();
        xScale = emotionalState.transform.localScale.x;
        yScale = emotionalMarker.transform.localScale.y;
        emotionTrack.Add(new Emotion(EmotionName.happy, 3, 0));
        emotionTrack.Add(new Emotion(EmotionName.happy, 3, 8));
        emotionTrack.Add(new Emotion(EmotionName.happy, 1, 10));
        emotionTrack.Add(new Emotion(EmotionName.angry, 1, 10));
        emotionTrack.Add(new Emotion(EmotionName.angry, 3, 11));

        nextEmotion = emotionTrack[emotionTrackIndex];
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= nextEmotion.time)
        {
            previousEmotion = nextEmotion;
            emotionalMarker.color = nextEmotion.colour;
            emotionalState.transform.localScale = new Vector3(xScale, yScale * nextEmotion.magnitude);
            nextEmotion = emotionTrack[++emotionTrackIndex];
        }
        else if(previousEmotion.Equals(nextEmotion))
        {
            float lerpMagnitude = Emotion.MagLerp(timer, previousEmotion, nextEmotion);
            emotionalState.transform.localScale = new Vector3(xScale, yScale * lerpMagnitude);
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

        public static float MagLerp(float currentTime, Emotion firstEmotion, Emotion secondEmotion)
        {
            float y0, y1, x0, x1;
            y0 = firstEmotion.magnitude;
            y1 = secondEmotion.magnitude;
            x0 = firstEmotion.time;
            x1 = secondEmotion.time;

            return (y0 * (x1 - currentTime) + y1 * (currentTime - x0)) / (x1 - x0);
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
