using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HexRuleSet", menuName = "Hexagon/Hex Rule Set", order = 1)]
public class HexRuleSet : ScriptableObject
{
    public HexRule hexRule = new HexRule(); // Single HexRule instance
}

[System.Serializable]
public class HexRule
{
    public string name;
    public GameObject hexPrefab;
    public List<HexFaceConnection> connections;

    public HexRule()
    {
        connections = new List<HexFaceConnection>
        {
            new HexFaceConnection { faceDirection = HexFaceDirectionEnum.East },
            new HexFaceConnection { faceDirection = HexFaceDirectionEnum.SouthEast },
            new HexFaceConnection { faceDirection = HexFaceDirectionEnum.SouthWest },
            new HexFaceConnection { faceDirection = HexFaceDirectionEnum.West },
            new HexFaceConnection { faceDirection = HexFaceDirectionEnum.NorthWest },
            new HexFaceConnection { faceDirection = HexFaceDirectionEnum.NorthEast }
        };
    }
}

[System.Serializable]
public class HexFaceConnection
{
    public HexFaceDirectionEnum faceDirection;
    public List<NeighborRule> allowedNeighborRules = new List<NeighborRule>(); // Reference to other HexRule sets
}

[System.Serializable]
public class NeighborRule
{
    public HexRuleSet hexRuleSet;
    public List<HexFaceDirectionEnum> neighborFaceDirections = new List<HexFaceDirectionEnum>();
}
