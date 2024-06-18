using UnityEngine;

[CreateAssetMenu(fileName = "TileSet", menuName = "Hexagon/Tile Set", order = 1)]
public class TileSet : ScriptableObject
{
    public string tileSetName;
    public GameObject hexPrefab;
    public HexRotationEnum rotationDegree;
    public HexDirectionConnectionTypeEnum east;
    public HexDirectionConnectionTypeEnum southEast;
    public HexDirectionConnectionTypeEnum southWest;
    public HexDirectionConnectionTypeEnum west;
    public HexDirectionConnectionTypeEnum northWest;
    public HexDirectionConnectionTypeEnum northEast;

    public HexDirectionConnectionTypeEnum GetFaceType(HexMainDirectionEnum direction)
    {
        switch (direction)
        {
            case HexMainDirectionEnum.East:
                return east;
            case HexMainDirectionEnum.SouthEast:
                return southEast;
            case HexMainDirectionEnum.SouthWest:
                return southWest;
            case HexMainDirectionEnum.West:
                return west;
            case HexMainDirectionEnum.NorthWest:
                return northWest;
            case HexMainDirectionEnum.NorthEast:
                return northEast;
            default:
                return HexDirectionConnectionTypeEnum.None;
        }
    }
}