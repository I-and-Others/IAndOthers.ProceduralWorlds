using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveFunctionCollapse : MonoBehaviour
{
    public enum CellState { Empty, Filled, Locked };
    public int gridWidth = 10;
    public int gridHeight = 10;
    private CellState[,] grid;
    private List<Vector2Int> possibleStates;
    private System.Random random = new System.Random();

    void Start()
    {
        InitializeGrid();
        WaveFunctionCollapseAlgorithm();
        OnDrawGizmos();
    }

    void InitializeGrid()
    {
        grid = new CellState[gridWidth, gridHeight];
        possibleStates = new List<Vector2Int>();

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y] = CellState.Empty;
                possibleStates.Add(new Vector2Int(x, y));
            }
        }
    }

    void WaveFunctionCollapseAlgorithm()
    {
        while (possibleStates.Count > 0)
        {
            Vector2Int cell = possibleStates[random.Next(possibleStates.Count)];
            possibleStates.Remove(cell);

            List<CellState> validStates = GetValidStates(cell);

            if (validStates.Count == 0)
            {
                Debug.LogError("Contradiction found!");
                return;
            }

            CellState chosenState = validStates[random.Next(validStates.Count)];
            grid[cell.x, cell.y] = chosenState;

            Propagate(cell);
        }
    }

    List<CellState> GetValidStates(Vector2Int cell)
    {
        // For simplicity, this function returns all states. In a real implementation,
        // you would check the rules to determine which states are valid for this cell.
        return new List<CellState> { CellState.Empty, CellState.Filled, CellState.Locked };
    }

    void Propagate(Vector2Int cell)
    {
        // Implement your propagation logic here based on the rules you have set.
        // This is a placeholder example that doesn't perform any real propagation.
    }

    private void OnDrawGizmos()
    {
        if (grid == null) return;

        Vector3 cellSize = new Vector3(1, 0.1f, 1);

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 position = new Vector3(x, 0, y);

                switch (grid[x, y])
                {
                    case CellState.Empty:
                        Gizmos.color = Color.white;
                        Gizmos.DrawWireCube(position, cellSize);
                        break;
                    case CellState.Filled:
                        Gizmos.color = Color.green;
                        Gizmos.DrawCube(position, cellSize);
                        break;
                    case CellState.Locked:
                        Gizmos.color = Color.black;
                        Gizmos.DrawCube(position, cellSize);
                        break;
                }
            }
        }
    }
}
