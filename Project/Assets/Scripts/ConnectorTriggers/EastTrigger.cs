using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EastTrigger : MonoBehaviour
{
    public RoadConnector rc;

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Road")
        {
            rc.East = true;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Road")
        {
            rc.East = false;
        }
    }
}
