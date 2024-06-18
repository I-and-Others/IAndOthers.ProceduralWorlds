using System.Collections.Generic;
using UnityEngine;

public class WFCManager : MonoBehaviour
{
    public HexMapGenerator hexMapGenerator;
    public TileSet[] tileSets;

    private void Start()
    {
        // Ensure the hex map is generated before initializing possible tile sets
        if (hexMapGenerator.hexGrid == null || hexMapGenerator.hexGrid.Length == 0)
        {
            Debug.LogError("Hex map is not generated. Please generate the hex map first.");
            return;
        }

        InitializePossibleTileSets();
    }

    public void InitializePossibleTileSets()
    {
        if (hexMapGenerator.hexGrid == null)
        {
            Debug.LogError("Hex grid is null. Ensure the hex map is generated first.");
            return;
        }

        for (int x = 0; x < hexMapGenerator.hexGrid.GetLength(0); x++)
        {
            for (int y = 0; y < hexMapGenerator.hexGrid.GetLength(1); y++)
            {
                Hex hex = hexMapGenerator.hexGrid[x, y];
                if (hex != null)
                {
                    List<PossibleTileSet> possibleTileSets = new List<PossibleTileSet>();
                    foreach (var tileSet in tileSets)
                    {
                        List<HexRotationEnum> validRotations = new List<HexRotationEnum>();
                        for (int rotation = 0; rotation < 6; rotation++)
                        {
                            validRotations.Add((HexRotationEnum)rotation);
                        }
                        possibleTileSets.Add(new PossibleTileSet(tileSet, validRotations));
                    }
                    hex.possibleTileSets = possibleTileSets;
                }
                else
                {
                    Debug.LogWarning($"Hex at position ({x}, {y}) is null.");
                }
            }
        }
    }

    public void CollapseNext()
    {
        Hex hexToCollapse = GetRandomHexToCollapse();
        if (hexToCollapse == null)
        {
            Debug.LogError("No hex found to collapse.");
            return;
        }

        List<PossibleTileSet> possibleTileSets = CalculatePossibleTileSets(hexToCollapse);
        if (possibleTileSets == null || possibleTileSets.Count == 0)
        {
            Debug.LogError("No possible tile sets found for collapsing.");
            return;
        }

        PossibleTileSet selectedTileSet = possibleTileSets[Random.Range(0, possibleTileSets.Count)];
        CollapseHex(hexToCollapse, selectedTileSet.tileSet, selectedTileSet.rotations[Random.Range(0, selectedTileSet.rotations.Count)]);
        UpdatePossibleTileSets(hexToCollapse);
    }

    private Hex GetRandomHexToCollapse()
    {
        List<Hex> keys = new List<Hex>();
        foreach (var hex in hexMapGenerator.hexGrid)
        {
            if (hex != null && hex.possibleTileSets.Count > 0)
            {
                keys.Add(hex);
            }
        }

        if (keys.Count == 0)
        {
            return null;
        }

        return keys[Random.Range(0, keys.Count)];
    }

    private List<PossibleTileSet> CalculatePossibleTileSets(Hex hex)
    {
        List<PossibleTileSet> possibleTileSets = new List<PossibleTileSet>();

        foreach (var tileSet in tileSets)
        {
            List<HexRotationEnum> validRotations = new List<HexRotationEnum>();
            for (int rotation = 0; rotation < 6; rotation++)
            {
                bool isValid = true;
                HexRotationEnum rotationEnum = (HexRotationEnum)rotation;

                foreach (HexFaceDirectionEnum direction in System.Enum.GetValues(typeof(HexFaceDirectionEnum)))
                {
                    Hex neighbor = hex.neighbors[(int)direction];
                    if (neighbor != null && neighbor.currentTileSet != null)
                    {
                        HexFaceDirectionEnum neighborDirection = (HexFaceDirectionEnum)(((int)direction + 3) % 6);
                        FaceTypeEnum requiredFaceType = neighbor.currentTileSet.GetFaceType(neighborDirection, neighbor.currentRotation);
                        FaceTypeEnum faceType = tileSet.GetFaceType(direction, rotationEnum);

                        if (requiredFaceType != faceType)
                        {
                            isValid = false;
                            break;
                        }
                    }
                }

                if (isValid)
                {
                    validRotations.Add(rotationEnum);
                }
            }

            if (validRotations.Count > 0)
            {
                possibleTileSets.Add(new PossibleTileSet(tileSet, validRotations));
            }
        }

        return possibleTileSets;
    }

    private void CollapseHex(Hex hex, TileSet tileSet, HexRotationEnum rotation)
    {
        hex.currentTileSet = tileSet;
        hex.currentRotation = rotation;
        int rotationDegrees = (int)hex.currentRotation * 60;

        // Set the mesh filter of the hex to the selected tile set's mesh
        MeshFilter meshFilter = hex.GetComponent<MeshFilter>();
        meshFilter.mesh = tileSet.hexPrefab.GetComponent<MeshFilter>().sharedMesh;
        hex.transform.rotation = Quaternion.Euler(0, rotationDegrees, 0);

        for (int i = 0; i < 6; i++)
        {
            hex.faceTypes[i] = tileSet.GetFaceType((HexFaceDirectionEnum)i, hex.currentRotation);
        }

        hex.possibleTileSets.Clear();
    }

    private void UpdatePossibleTileSets(Hex collapsedHex)
    {
        foreach (HexFaceDirectionEnum direction in System.Enum.GetValues(typeof(HexFaceDirectionEnum)))
        {
            Hex neighbor = collapsedHex.neighbors[(int)direction];
            if (neighbor == null || neighbor.possibleTileSets == null || neighbor.possibleTileSets.Count == 0)
            {
                continue;
            }

            int oppositeDirectionInt = ((int)direction + 3) % 6;
            HexFaceDirectionEnum oppositeDirection = (HexFaceDirectionEnum)oppositeDirectionInt;

            FaceTypeEnum requiredFaceType = collapsedHex.currentTileSet.GetFaceType(oppositeDirection, collapsedHex.currentRotation);

            List<PossibleTileSet> validTileSets = new List<PossibleTileSet>();
            foreach (var possibleTileSet in neighbor.possibleTileSets)
            {
                List<HexRotationEnum> validRotations = new List<HexRotationEnum>();
                foreach (var rotation in possibleTileSet.rotations)
                {
                    if (possibleTileSet.tileSet.GetFaceType(RotateDirection(oppositeDirection, rotation), rotation) == requiredFaceType)
                    {
                        validRotations.Add(rotation);
                    }
                }
                if (validRotations.Count > 0)
                {
                    validTileSets.Add(new PossibleTileSet(possibleTileSet.tileSet, validRotations));
                }
            }

            neighbor.possibleTileSets = validTileSets;

            if (validTileSets.Count == 1)
            {
                CollapseHex(neighbor, validTileSets[0].tileSet, validTileSets[0].rotations[0]);
            }
        }
    }

    private HexFaceDirectionEnum RotateDirection(HexFaceDirectionEnum direction, HexRotationEnum rotation)
    {
        int newDirection = ((int)direction + (int)rotation) % 6;
        return (HexFaceDirectionEnum)newDirection;
    }
}
