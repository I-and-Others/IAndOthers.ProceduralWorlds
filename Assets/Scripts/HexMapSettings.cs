using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HexMapSettings", menuName = "Hex/HexMapSettings")]
public class HexMapSettings : ScriptableObject
{
    public GameObject hexPrefab;
    public int mapWidth;
    public int mapHeight;
    public float hexSize;
    public HexOrientationEnum hexOrientation;
    public float hexWidth;
    public float hexHeight;
    public List<HexTile> hexTiles;
}
