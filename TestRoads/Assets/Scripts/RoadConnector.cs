using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadConnector : MonoBehaviour
{
    public GameObject Road, Road90, RoadT, RoadX;
    private GameObject Model;
    private Building building;
    public bool North, West, East, South;
    public int numOfIntersections;

    private void Awake()
    {
        Model = gameObject.transform.GetChild(0).gameObject;
        building = gameObject.GetComponent<Building>();
    }

    private void FixedUpdate()
    {
        CalculateIntersections();
        ChangeModel();
    }

    private void ChangeModel()
    {
        switch (numOfIntersections)
        {            
            case 1:
                SwitchModel(Road);
                if(West || East)
                {
                    Model.transform.rotation = Quaternion.Euler(0, 0, 90);
                }
                break;
            case 2:
                if((North && South) || (West && East))
                {                    
                    SwitchModel(Road);
                    if(West && East)
                        Model.transform.rotation = Quaternion.Euler(0, 0, 90);
                }
                else
                {
                    SwitchModel(Road90);
                    if (North && West) { }
                    if (North && East)
                        Model.transform.rotation = Quaternion.Euler(0, 0, -90);
                    if (South && West)
                        Model.transform.rotation = Quaternion.Euler(0, 0, 90);
                    if (South && East)
                        Model.transform.rotation = Quaternion.Euler(0, 0, 180);
                }
                break;
            case 3:
                SwitchModel(RoadT);
                if (North && West && East)
                    Model.transform.rotation = Quaternion.Euler(0, 0, 0);
                if (North && West && South)
                    Model.transform.rotation = Quaternion.Euler(0, 0, 90);
                if(North && South && East)
                    Model.transform.rotation = Quaternion.Euler(0, 0, -90);
                if (West && South && East)
                    Model.transform.rotation = Quaternion.Euler(0, 0, 180);

                break;
            case 4:
                SwitchModel(RoadX);
                break;
            default:
                System.Console.WriteLine("Something wrong with directions");
                break;
        }
    }

    private void SwitchModel(GameObject model)
    {
        
        if(model == Road90)
        {
            Road.SetActive(false);
            Road90.SetActive(true);
            RoadT.SetActive(false);
            RoadX.SetActive(false);
            building.numOfRenderer = 1;
        }
        else if(model == RoadT)
        {
            Road.SetActive(false);
            Road90.SetActive(false);
            RoadT.SetActive(true);
            RoadX.SetActive(false);
            building.numOfRenderer = 2;
        }
        else if(model == RoadX)
        {
            Road.SetActive(false);
            Road90.SetActive(false);
            RoadT.SetActive(false);
            RoadX.SetActive(true);
            building.numOfRenderer = 3;
        }
        else
        {
            Road.SetActive(true);
            Road90.SetActive(false);
            RoadT.SetActive(false);
            RoadX.SetActive(false);
            building.numOfRenderer = 0;
        }
    }
    
    private void CalculateIntersections()
    {
        int i = 0;
        if (North)
            i += 1;
        if (West)
            i += 1;
        if (East)
            i += 1;
        if (South)
            i += 1;
        numOfIntersections = i;
    }
}
