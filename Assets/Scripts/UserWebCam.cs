using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserWebCam : MonoBehaviour
{
    static WebCamTexture webCam;
    // Start is called before the first frame update
    void Start()
    {
        if (webCam is null)
            webCam = new WebCamTexture();

        this.gameObject.transform.Find("Video Screen").GetComponent<UnityEngine.UI.RawImage>().texture = webCam;

        if (!webCam.isPlaying)
            webCam.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
