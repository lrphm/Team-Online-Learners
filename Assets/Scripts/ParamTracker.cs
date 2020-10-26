using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamTracker : MonoBehaviour
{
    public static int scenario = 1;
    private bool useWebCam = false;
    public GameObject startCanvas, meetingCanvas;

    void Awake()
    {
        UserWebCam.useWebCam = this.useWebCam;
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropdownChanged(int index)
    {
        scenario = index + 1;
    }

    public void JoinMeeting()
    {
        startCanvas.SetActive(false);
        meetingCanvas.SetActive(true);
        meetingCanvas.GetComponent<ScenarioManager>().Init();
    }

    public void LeaveMeeting()
    {
        meetingCanvas.GetComponent<ScenarioManager>().Disable();
        startCanvas.SetActive(true);
    }

    public void SetWebCamUse(bool useWebCam)
    {
        this.useWebCam = useWebCam;
        UserWebCam.useWebCam = this.useWebCam;
    }
}
