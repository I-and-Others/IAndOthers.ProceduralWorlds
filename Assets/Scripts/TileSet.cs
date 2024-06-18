using UnityEngine;

[CreateAssetMenu(fileName = "TileSet", menuName = "Hexagon/Tile Set", order = 1)]
public class TileSet : ScriptableObject
{
    public string tileSetName;
    public GameObject hexPrefab;
    public FaceTypeEnum east;
    public FaceTypeEnum southEast;
    public FaceTypeEnum southWest;
    public FaceTypeEnum west;
    public FaceTypeEnum northWest;
    public FaceTypeEnum northEast;

    public FaceTypeEnum GetFaceType(HexFaceDirectionEnum direction, HexRotationEnum rotation)
    {
        int index = ((int)direction + (int)rotation) % 6;
        return GetFaceTypeByIndex(index);
    }

    public FaceTypeEnum GetFaceTypeByIndex(int index)
    {
        switch (index)
        {
            case 0: return east;
            case 1: return southEast;
            case 2: return southWest;
            case 3: return west;
            case 4: return northWest;
            case 5: return northEast;
            default: return FaceTypeEnum.Empty;
        }
    }
}
