using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WestTrigger : MonoBehaviour
{
    public RoadConnector rc;

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Road")
        {
            rc.West = true;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Road")
        {
            rc.West = false;
        }
    }
}
