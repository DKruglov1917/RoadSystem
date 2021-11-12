using UnityEngine;
using TMPro;

public class BuildingsGrid : MonoBehaviour
{
    public Vector2Int GridSize = new Vector2Int(10, 10);    

    private static Building[,] grid;
    private static Building flyingBuilding;

    public TextMeshProUGUI eraseButtonTxt, buildButtonTxt;
    private Camera mainCamera;

    public Building Road;
    public Building cloneBuildingStart, cloneBuildingFinish;

    public int startX, startY;
    public int finishX, finishY;
    private bool bOrigin;
    private bool bErasing;

    public delegate void finishingRoad();
    public static event finishingRoad onFinish;

    private void Awake()
    {
        grid = new Building[GridSize.x, GridSize.y];

        mainCamera = Camera.main;

        bOrigin = true;
    }

    public void StartPlacingBuilding()
    {
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
        }

        flyingBuilding = Instantiate(Road);
        flyingBuilding.tag = "Untagged";
    }

    public void EraseButton()
    {
        bErasing = !bErasing;
        if (bErasing)
        {
            eraseButtonTxt.text = "STOP ERASING";
        }
        else
        {
            eraseButtonTxt.text = "ERASE";
        }
    }    

    public void BuildButton()
    {
        if (bOrigin)
        {
            buildButtonTxt.text = "START ROAD";
        }
        else
        {
            buildButtonTxt.text = "FINISH ROAD";
        }
    }

    private void Update()
    {
        if (flyingBuilding != null)
        {
            var groundPlane = new Plane(Vector3.forward, Vector3.zero);            

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);            

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);

                int x = Mathf.RoundToInt(worldPosition.x);
                int y = Mathf.RoundToInt(worldPosition.y);  

                bool available = true;

                if (x < 0 || x > GridSize.x - flyingBuilding.Size.x) available = false;
                if (y < 0 || y > GridSize.y - flyingBuilding.Size.y) available = false;

                flyingBuilding.transform.position = new Vector3(x, y, 0);
                flyingBuilding.SetTransparent(available);

                if (available && Input.GetMouseButtonDown(0))
                {
                    if (bOrigin)
                    {
                        bOrigin = false;
                        startX = x;
                        startY = y;
                        cloneBuildingStart = flyingBuilding;
                        if (IsPlaceTaken(x, y))
                        {
                            //Destroy(grid[x, y].gameObject);
                            Destroy(flyingBuilding.gameObject.transform.GetChild(0).gameObject);
                            flyingBuilding.gameObject.GetComponent<RoadConnector>().enabled = false;
                            flyingBuilding = null;
                        }
                        else
                        {
                            PlaceFlyingBuilding(x, y);
                        }
                    }
                    else
                    {
                        bOrigin = true;
                        finishX = x;
                        finishY = y;
                        cloneBuildingFinish = flyingBuilding;
                        cloneBuildingStart.transform.gameObject.AddComponent<PathFinder>().Target =
                            cloneBuildingFinish.transform.gameObject;
                        Destroy(flyingBuilding.gameObject);

                        onFinish?.Invoke();

                        PlaceMiddleOfTheRoad();
                    }
                }
            }
        }
        else
        {
            EraseBuilding();
        }

        BuildButton();
    }

    private bool IsPlaceTaken(int placeX, int placeY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                if (grid[placeX + x, placeY + y] != null) return true;
            }
        }

        return false;
    }    

    private void PlaceFlyingBuilding(int placeX, int placeY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                grid[placeX + x, placeY + y] = flyingBuilding;
            }
        }
        flyingBuilding.tag = "Road";
        flyingBuilding.SetNormal();
        flyingBuilding = null;
    }

    private void PlaceMiddleOfTheRoad()
    {
        foreach (var item in PathFinder.PathToTarget)
        {
            if (!IsPlaceTaken((int)item.x, (int)item.y))
            {
                grid[(int)item.x, (int)item.y] = Instantiate(Road, new Vector2(item.x, item.y), Quaternion.identity);
            }
        }
    }

    private void EraseBuilding()
    {
        if (bErasing && Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if(hit.transform.gameObject.tag == "Road")
                {
                    Destroy(hit.transform.gameObject);
                }
            }                
        }
    }
}
