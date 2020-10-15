using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserWebCam : MonoBehaviour
{
    static WebCamTexture webCam;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Init()
    {
        if (webCam is null)
            webCam = new WebCamTexture();

        this.gameObject.transform.Find("Video Screen").GetComponent<UnityEngine.UI.RawImage>().texture = webCam;

        Play();
    }

    public void Disable()
    {
        if (webCam.isPlaying)
            webCam.Stop();
    }

    private void Play()
    {
        try
        {
            if (!webCam.isPlaying)
            {
                try
                {
                    webCam.Play();
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
