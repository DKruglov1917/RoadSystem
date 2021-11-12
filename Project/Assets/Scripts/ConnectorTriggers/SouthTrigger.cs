using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SouthTrigger : MonoBehaviour
{
    public RoadConnector rc;

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Road")
        {
            rc.South = true;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Road")
        {
            rc.South = false;
        }
    }
}
