using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NorthTrigger : MonoBehaviour
{
    public RoadConnector rc;

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Road")
        {
            rc.North = true;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Road")
        {
            rc.North = false;
        }
    }
}
