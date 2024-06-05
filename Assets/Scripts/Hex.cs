﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public int id; // Unique identifier for each hex prefab
    public Vector2Int coordinates; // Hex coordinates in offset
    public Vector2Int axialCoordinates; // Axial coordinates
    public FaceStateEnum[] faceStates = new FaceStateEnum[6]; // Array to store the state of each face
    public Hex[] neighbors = new Hex[6];

    public void Initialize(Vector2Int coords)
    {
        coordinates = coords;
        axialCoordinates = OffsetToAxial(coords);
        for (int i = 0; i < faceStates.Length; i++)
        {
            faceStates[i] = FaceStateEnum.Empty;
        }
    }

    public void UpdateFaceState(int faceIndex, FaceStateEnum state)
    {
        faceStates[faceIndex] = state;
    }

    private Vector2Int OffsetToAxial(Vector2Int offset)
    {
        int q = offset.x - (offset.y - (offset.y & 1)) / 2;
        int r = offset.y;
        return new Vector2Int(q, r);
    }

    private Vector2Int AxialToOffset(Vector2Int axial)
    {
        int col = axial.x + (axial.y - (axial.y & 1)) / 2;
        int row = axial.y;
        return new Vector2Int(col, row);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 0.1f);

        Vector3[] facePositions = {
            Quaternion.Euler(0, 30, 0) * new Vector3(0, 0, 0.5f), // NorthEast
            Quaternion.Euler(0, 30, 0) * new Vector3(0.433f, 0, 0.25f), // East
            Quaternion.Euler(0, 30, 0) * new Vector3(0.433f, 0, -0.25f), // SouthEast
            Quaternion.Euler(0, 30, 0) * new Vector3(0, 0, -0.5f), // SouthWest
            Quaternion.Euler(0, 30, 0) * new Vector3(-0.433f, 0, -0.25f), // West
            Quaternion.Euler(0, 30, 0) * new Vector3(-0.433f, 0, 0.25f) // NorthWest
        };

        Color[] faceColors = {
            Color.red,    // NorthEast
            Color.green,  // East
            Color.blue,   // SouthEast
            Color.yellow, // SouthWest
            Color.magenta,// West
            Color.cyan    // NorthWest
        };
        string[] directionLabels =
        {
            "NE", // NorthEast
            "E",  // East
            "SE", // SouthEast
            "SW", // SouthWest
            "W",  // West
            "NW"  // NorthWest
        };


        for (int i = 0; i < facePositions.Length; i++)
        {
            Gizmos.color = faceColors[i];
            Gizmos.DrawLine(transform.position, transform.position + facePositions[i]);

            switch (faceStates[i])
            {
                case FaceStateEnum.Empty:
                    Gizmos.color = Color.white;
                    Gizmos.DrawWireSphere(transform.position + facePositions[i], 0.1f);
                    break;
                case FaceStateEnum.Filled:
                    Gizmos.color = faceColors[i];
                    Gizmos.DrawSphere(transform.position + facePositions[i], 0.1f);
                    break;
                case FaceStateEnum.Locked:
                    Gizmos.color = Color.black;
                    Gizmos.DrawSphere(transform.position + facePositions[i], 0.1f);
                    break;
            }

            // Draw the direction label
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.normal.textColor = faceColors[i];
            Handles.Label(transform.position + facePositions[i] * 1.5f, directionLabels[i], labelStyle);

        }

        // Draw the coordinates as text
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        Handles.Label(transform.position + new Vector3(-0.2f, -0.2f, 0.8f), $"({coordinates.x}, {coordinates.y})", style);
    }
}
