using System.Collections.Generic;
using UnityEngine;

public class WFCManager : MonoBehaviour
{
    public HexMapGenerator hexMapGenerator;
    public TileSet[] tileSets;

    private bool initialized = false;

    private void Start()
    {
        if (hexMapGenerator.hexGrid == null || hexMapGenerator.hexGrid.Length == 0)
        {
            Debug.LogError("Hex map is not generated. Please generate the hex map first.");
            return;
        }

        InitializePossibleTileSets();
        initialized = true;
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
                    List<TileSet> possibleTileSets = new List<TileSet>(tileSets);
                    hex.possibleTileSets = possibleTileSets;
                }
            }
        }
    }

    public void CompleteWaveFunctionCollapse()
    {
        while (true)
        {
            CollapseNext();
            if (IsWaveFunctionCollapsed())
            {
                break;
            }
        }
    }

    private bool IsWaveFunctionCollapsed()
    {
        foreach (var hex in hexMapGenerator.hexGrid)
        {
            if (hex != null && hex.possibleTileSets.Count > 1)
            {
                return false; // Found a hex that hasn't collapsed to a single state
            }
        }
        return true; // All hexes have collapsed to a single state
    }


    public void StartWaveFunctionCollapse()
    {
        InitializePossibleTileSets();
        initialized = true;
    }

    public void CollapseNext()
    {
        if (!initialized)
        {
            Debug.LogError("Wave Function Collapse has not been initialized. Call StartWaveFunctionCollapse first.");
            return;
        }

        Hex hexToCollapse = Observe();
        if (hexToCollapse == null)
        {
            Debug.Log("No more hexagons to collapse.");
            return;
        }
        Collapse(hexToCollapse);
        Propagate(hexToCollapse);
    }

    private Hex Observe()
    {
        Hex minEntropyHex = null;
        float minEntropy = float.MaxValue;

        foreach (var hex in hexMapGenerator.hexGrid)
        {
            if (hex != null && hex.possibleTileSets.Count > 1)
            {
                float entropy = CalculateEntropy(hex);
                if (entropy < minEntropy)
                {
                    minEntropy = entropy;
                    minEntropyHex = hex;
                }
            }
        }

        return minEntropyHex;
    }

    private float CalculateEntropy(Hex hex)
    {
        return hex.possibleTileSets.Count;
    }

    private void Collapse(Hex hex)
    {
        List<TileSet> possibleTileSets = hex.possibleTileSets;
        TileSet selectedTileSet = possibleTileSets[Random.Range(0, possibleTileSets.Count)];

        hex.currentTileSet = selectedTileSet;

        // Set the mesh filter of the hex to the selected tile set's mesh
        MeshFilter meshFilter = hex.GetComponent<MeshFilter>();
        if (meshFilter != null && selectedTileSet.hexPrefab != null)
        {
            MeshFilter prefabMeshFilter = selectedTileSet.hexPrefab.GetComponent<MeshFilter>();
            if (prefabMeshFilter != null)
            {
                meshFilter.mesh = prefabMeshFilter.sharedMesh;
            }
        }
        // Set the rotation of the hex to the selected tile set's rotation
        var hexRotation = selectedTileSet.rotationDegree;
        hex.currentRotation = hexRotation;
        hex.transform.rotation = Quaternion.Euler(0, (int)hexRotation, 0);

        hex.possibleTileSets.Clear();
    }

    private void Propagate(Hex collapsedHex)
    {
        Queue<Hex> propagationQueue = new Queue<Hex>();
        propagationQueue.Enqueue(collapsedHex);

        while (propagationQueue.Count > 0)
        {
            Hex currentHex = propagationQueue.Dequeue();

            foreach (HexMainDirectionEnum direction in System.Enum.GetValues(typeof(HexMainDirectionEnum)))
            {
                Hex neighbor = currentHex.neighbors[(int)direction];
                if (neighbor == null || neighbor.possibleTileSets == null || neighbor.possibleTileSets.Count == 0)
                {
                    continue;
                }

                int oppositeDirectionInt = ((int)direction + 3) % 6;
                HexMainDirectionEnum oppositeDirection = (HexMainDirectionEnum)oppositeDirectionInt;

                if (currentHex.currentTileSet == null)
                {
                    continue;
                }

                List<TileSet> validTileSets = new List<TileSet>();
                foreach (var possibleTileSet in neighbor.possibleTileSets)
                {
                    var requiredFaceType = currentHex.currentTileSet?.GetFaceType(direction) ?? HexDirectionConnectionTypeEnum.None;
                    var faceType = possibleTileSet.GetFaceType(oppositeDirection);

                    if (requiredFaceType == faceType)
                    {
                        validTileSets.Add(possibleTileSet);
                    }
                }

                if (validTileSets.Count != neighbor.possibleTileSets.Count)
                {
                    neighbor.possibleTileSets = validTileSets;
                    propagationQueue.Enqueue(neighbor);
                }

                if (validTileSets.Count == 1)
                {
                    Collapse(neighbor);
                    propagationQueue.Enqueue(neighbor);
                }
            }
        }
    }
}