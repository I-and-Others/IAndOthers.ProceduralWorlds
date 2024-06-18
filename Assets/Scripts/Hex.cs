using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PossibleTileSet
{
    public TileSet tileSet;
    public List<HexRotationEnum> rotations;

    public PossibleTileSet(TileSet tileSet, List<HexRotationEnum> rotations)
    {
        this.tileSet = tileSet;
        this.rotations = rotations;
    }
}

public class Hex : MonoBehaviour
{
    public Vector2Int coordinates;
    public Vector2Int axialCoordinates;
    public FaceTypeEnum[] faceTypes = new FaceTypeEnum[6];
    public Hex[] neighbors = new Hex[6];
    public TileSet currentTileSet;
    public HexRotationEnum currentRotation;
    [SerializeField]
    public List<PossibleTileSet> possibleTileSets = new List<PossibleTileSet>();

    public void Initialize(Vector2Int coords)
    {
        coordinates = coords;
        axialCoordinates = OffsetToAxial(coords);
        for (int i = 0; i < faceTypes.Length; i++)
        {
            faceTypes[i] = FaceTypeEnum.Empty;
        }
    }

    public FaceTypeEnum GetFaceType(HexFaceDirectionEnum direction, HexRotationEnum rotation)
    {
        int index = ((int)direction + (int)rotation) % 6;
        return faceTypes[index];
    }

    public FaceTypeEnum GetFaceType(HexFaceDirectionEnum direction)
    {
        int index = ((int)direction + (int)currentRotation) % 6;
        return faceTypes[index];
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
            Quaternion.Euler(0, 30, 0) * new Vector3(0, 0, 0.5f),
            Quaternion.Euler(0, 30, 0) * new Vector3(0.433f, 0, 0.25f),
            Quaternion.Euler(0, 30, 0) * new Vector3(0.433f, 0, -0.25f),
            Quaternion.Euler(0, 30, 0) * new Vector3(0, 0, -0.5f),
            Quaternion.Euler(0, 30, 0) * new Vector3(-0.433f, 0, -0.25f),
            Quaternion.Euler(0, 30, 0) * new Vector3(-0.433f, 0, 0.25f)
        };

        Color[] faceColors = {
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow,
            Color.magenta,
            Color.cyan
        };

        string[] directionLabels = {
            "NE", "E", "SE", "SW", "W", "NW"
        };

        for (int i = 0; i < facePositions.Length; i++)
        {
            Gizmos.color = faceColors[i];
            Gizmos.DrawLine(transform.position, transform.position + facePositions[i]);
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position + facePositions[i], 0.1f);
            Handles.Label(transform.position + facePositions[i] * 1.5f, directionLabels[i], new GUIStyle { normal = { textColor = faceColors[i] } });
        }

        Handles.Label(transform.position + new Vector3(-0.2f, -0.2f, 0.8f), $"({coordinates.x}, {coordinates.y})", new GUIStyle { normal = { textColor = Color.white } });

        if (possibleTileSets != null && possibleTileSets.Count > 0)
        {
            Handles.Label(transform.position + Vector3.up * 0.5f, $"Possible: {possibleTileSets.Count}", new GUIStyle { normal = { textColor = Color.yellow } });
        }
    }
}
