using UnityEngine;

[System.Serializable]
public class HexMapSettings
{
    public GameObject hexPrefab;
    public int mapWidth = 6;
    public int mapHeight = 6;
    public float hexSize = 1f;

    public HexOrientationEnum hexOrientation;

    public float hexWidth;
    public float hexHeight;
}