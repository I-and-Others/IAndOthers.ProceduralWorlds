using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HexTile", menuName = "Hex/HexTile")]
public class HexTile : ScriptableObject
{
    public string tileName;
    public GameObject prefab;
    public List<HexFace> faces;
    //public Texture2D referenceImage;

    [System.Serializable]
    public class HexFace
    {
        public HexSide side;
        public List<NeighborOption> possibleNeighbors;
    }

    [System.Serializable]
    public class NeighborOption
    {
        public HexTile hexTile;
        public List<HexSide> compatibleFaces;
    }
}

public enum HexSide
{
    A,
    B,
    C,
    D,
    E,
    F
}
