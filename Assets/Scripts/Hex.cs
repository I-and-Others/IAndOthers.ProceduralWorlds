using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class Hex : MonoBehaviour
{
    public Vector2Int coordinates;
    public Vector2Int axialCoordinates;
    public Hex[] neighbors = new Hex[6];
    public TileSet currentTileSet;
    public HexRotationEnum currentRotation;
    [SerializeField]
    public List<PossibleTileSet> possibleTileSets = new List<PossibleTileSet>();

    public void Initialize(Vector2Int coords)
    {
        coordinates = coords;
        axialCoordinates = OffsetToAxial(coords);
    }

    private Vector2Int OffsetToAxial(Vector2Int offset)
    {
        int q = offset.x - (offset.y - (offset.y & 1)) / 2;
        int r = offset.y;
        return new Vector2Int(q, r);
    }

    private void OnDrawGizmos()
    {
        Vector3[] facePositions =
        {
            Quaternion.Euler(0, 30, 0) * new Vector3(0, 0, 0.5f),
            Quaternion.Euler(0, 30, 0) * new Vector3(0.433f, 0, 0.25f),
            Quaternion.Euler(0, 30, 0) * new Vector3(0.433f, 0, -0.25f),
            Quaternion.Euler(0, 30, 0) * new Vector3(0, 0, -0.5f),
            Quaternion.Euler(0, 30, 0) * new Vector3(-0.433f, 0, -0.25f),
            Quaternion.Euler(0, 30, 0) * new Vector3(-0.433f, 0, 0.25f)
        };

        Color[] faceColors =
        {
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow,
            Color.magenta,
            Color.cyan,
        };

        string[] directionLabels =
        {
            "NE", "E", "SE", "SW", "W", "NW"
        };

        for (int i = 0; i < facePositions.Length; i++)
        {
            Gizmos.color = faceColors[i];
            Gizmos.DrawLine(transform.position, transform.position + facePositions[i]);
            Gizmos.color = Color.white;
            Handles.Label(transform.position + facePositions[i] * 1.5f, directionLabels[i], new GUIStyle { normal = { textColor = faceColors[i] } });
        }

        Handles.Label(transform.position + Quaternion.Euler(0, 0, 0) * new Vector3(-0.1f, 0, 1.0f) * 0.8f, $"({coordinates.x}, {coordinates.y})", new GUIStyle { normal = { textColor = Color.white } });
        if (possibleTileSets != null && possibleTileSets.Count > 0)
        {
            Handles.Label(transform.position + Quaternion.Euler(0, 0, 0) * new Vector3(-0.2f, 0, 0.8f) * 0.8f, $"Possible: {possibleTileSets.Count}", new GUIStyle { normal = { textColor = Color.white } });
        }
    }
}
