using System.Collections.Generic;
using UnityEngine;

public class WaveFunctionCollapse : MonoBehaviour
{
    public List<CompatibilityRule> rules;

    public void GenerateMap()
    {
        Hex[,] hexGrid = HexMapGenerator.Instance.hexGrid;
        List<Hex> hexList = new List<Hex>();

        foreach (var hex in hexGrid)
        {
            hexList.Add(hex);
        }

        while (hexList.Count > 0)
        {
            Hex hex = hexList[Random.Range(0, hexList.Count)];
            hexList.Remove(hex);

            CollapseHex(hex);
        }
    }

    private void CollapseHex(Hex hex)
    {
        // Select a random tile and rotation that fits the current constraints
        var possibleTiles = GetPossibleTiles(hex);
        if (possibleTiles.Count > 0)
        {
            var selectedTile = possibleTiles[Random.Range(0, possibleTiles.Count)];
            ApplyTile(hex, selectedTile);
        }
    }

    private List<CompatibilityRule> GetPossibleTiles(Hex hex)
    {
        List<CompatibilityRule> possibleTiles = new List<CompatibilityRule>();

        foreach (var rule in rules)
        {
            bool isCompatible = true;

            for (int i = 0; i < 6; i++)
            {
                var neighbor = hex.neighbors[i];
                if (neighbor != null)
                {
                    var neighborFace = (HexFaceDirectionEnum)i;
                    var neighborState = neighbor.faceStates[i];

                    if (neighborState == FaceStateEnum.Locked)
                    {
                        isCompatible = false;
                        break;
                    }

                    var neighborRule = rules.Find(r => r.prefabId == neighbor.id);
                    if (neighborRule != null)
                    {
                        bool faceCompatible = false;
                        foreach (var connection in neighborRule.faceConnections[neighborFace])
                        {
                            if (connection.targetPrefabId == rule.prefabId)
                            {
                                faceCompatible = true;
                                break;
                            }
                        }

                        if (!faceCompatible)
                        {
                            isCompatible = false;
                            break;
                        }
                    }
                }
            }

            if (isCompatible)
            {
                possibleTiles.Add(rule);
            }
        }

        return possibleTiles;
    }

    private void ApplyTile(Hex hex, CompatibilityRule rule)
    {
        hex.id = rule.prefabId;
        for (int i = 0; i < 6; i++)
        {
            var faceDirection = (HexFaceDirectionEnum)i;
            foreach (var connection in rule.faceConnections[faceDirection])
            {
                var neighbor = hex.neighbors[i];
                if (neighbor != null)
                {
                    neighbor.faceStates[(i + 3) % 6] = FaceStateEnum.Filled;
                }
            }
        }
    }
}
