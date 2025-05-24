using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject quad;  
    public int gridWidth;   
    public int gridHeight;   
    public float cellSize = 2f;  

    private Dictionary<Vector3, float> gridQValues;  

    void Start()
    {
        gridQValues = new Dictionary<Vector3, float>();
        SetGridDimensions();
        GenerateGrid();
    }

 
    void SetGridDimensions()
    {
       
        MeshRenderer quadRenderer = quad.GetComponent<MeshRenderer>();
        if (quadRenderer != null)
        {
            Vector3 quadSize = quadRenderer.bounds.size;
            gridWidth = Mathf.CeilToInt(quadSize.x / cellSize);  
            gridHeight = Mathf.CeilToInt(quadSize.z / cellSize); 
        }
        else
        {
            Debug.LogError("Quad-ul nu are un MeshRenderer!");
        }
    }


    void GenerateGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                Vector3 cellPosition = new Vector3(x * cellSize - (gridWidth * cellSize) / 2, 0, z * cellSize - (gridHeight * cellSize) / 2);
                gridQValues[cellPosition] = 0f; 
            }
        }
    }

    public float GetQValue(Vector3 position)
    {
        Vector3 closestCell = GetClosestCellPosition(position);
        if (gridQValues.ContainsKey(closestCell))
        {
            return gridQValues[closestCell];
        }
        return 0f;
    }

    public void UpdateQValue(Vector3 position, float value)
    {
        Vector3 closestCell = GetClosestCellPosition(position);
        if (gridQValues.ContainsKey(closestCell))
        {
            gridQValues[closestCell] = value;
            Debug.Log($"Updated Q value for cell at {closestCell} to: {value}");

        }
        else
        {
            gridQValues.Add(closestCell, value);
            Debug.Log($"Added new cell at {closestCell} with Q value: {value}");

        }
    }

    private Vector3 GetClosestCellPosition(Vector3 position)
    {
        float closestDistance = Mathf.Infinity;
        Vector3 closestCell = Vector3.zero;

        foreach (var cellPosition in gridQValues.Keys)
        {
            float distance = Vector3.Distance(position, cellPosition);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCell = cellPosition;
            }
        }

        return closestCell;
    }

    void OnDrawGizmos()
    {
        if (gridQValues == null) return;

        Gizmos.color = Color.green;
        foreach (var cell in gridQValues.Keys)
        {
            Gizmos.DrawWireCube(cell, new Vector3(cellSize, 0.2f, cellSize));
        }
    }
}
