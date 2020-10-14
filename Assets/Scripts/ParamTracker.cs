using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamTracker : MonoBehaviour
{
    public static int scenario = 1;

    void Awake()
    {
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
}
