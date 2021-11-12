using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Renderer RoadRenderer, Road90Renderer,
                    RoadTRenderer, RoadXRenderer;

    private List<Renderer> renderers = new List<Renderer>();

    public Vector2Int Size = Vector2Int.one;

    public int numOfRenderer;

    private void Awake()
    {
        numOfRenderer = 0;
        AddRenderers();
    }

    private void Start()
    {
        SetNormal();
    }

    public void SetTransparent(bool available)
    {
        if (available)
        {
            renderers[numOfRenderer].material.color = Color.green;
        }
        else
        {
            renderers[numOfRenderer].material.color = Color.red;
        }
    }

    public void SetNormal()
    {
        renderers[numOfRenderer].material.color = Color.white;
    }

    private void AddRenderers()
    {
        renderers.Add(RoadRenderer);
        renderers.Add(Road90Renderer);
        renderers.Add(RoadTRenderer);
        renderers.Add(RoadXRenderer);
    }
}