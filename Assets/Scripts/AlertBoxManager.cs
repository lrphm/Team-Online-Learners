using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertBoxManager : MonoBehaviour
{
    public void onClick()
    {
        ScenarioManager script = this.gameObject.transform.parent.gameObject.GetComponent<ScenarioManager>();
        script.RemoveAlertBox(this.gameObject);
    }
}
