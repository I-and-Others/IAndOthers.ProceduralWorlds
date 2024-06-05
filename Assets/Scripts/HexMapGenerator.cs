using System.Collections.Generic;
using UnityEngine;

public class HexMapGenerator : Singleton<HexMapGenerator>
{
    public HexMapSettings Settings;
    public HexRuleSet[] hexRuleSets; // Array of hex rule sets
    public Hex[,] hexGrid;
    private Dictionary<Hex, HexRule> hexToRuleMap = new Dictionary<Hex, HexRule>();

    public void GenerateHexMap()
    {
        CalculateHexDimensions();
        ClearMap();

        hexGrid = new Hex[Settings.mapWidth, Settings.mapHeight];

        // Create hex tiles and store them in the array
        for (int x = 0; x < Settings.mapWidth; x++)
        {
            for (int y = 0; y < Settings.mapHeight; y++)
            {
                Vector3 position = CalculateHexPosition(x, y);
                Quaternion rotation = Quaternion.identity;

                if (Settings.hexOrientation == HexOrientationEnum.OddQ || Settings.hexOrientation == HexOrientationEnum.EvenQ)
                {
                    rotation = Quaternion.Euler(0, 30, 0);
                }

                GameObject hexGO = Instantiate(Settings.hexPrefab, position, rotation, transform);
                hexGO.name = $"Hex_{x}_{y}";
                Hex hex = hexGO.GetComponent<Hex>();
                hex.Initialize(new Vector2Int(x, y));
                hexGrid[x, y] = hex;
            }
        }

        // Update neighbors
        for (int x = 0; x < Settings.mapWidth; x++)
        {
            for (int y = 0; y < Settings.mapHeight; y++)
            {
                SetNeighbors(hexGrid, x, y);
            }
        }
    }

    public void StartWaveFunctionCollapse()
    {
        List<Vector2Int> availablePositions = new List<Vector2Int>();
        for (int x = 0; x < Settings.mapWidth; x++)
        {
            for (int y = 0; y < Settings.mapHeight; y++)
            {
                availablePositions.Add(new Vector2Int(x, y));
            }
        }

        while (availablePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, availablePositions.Count);
            Vector2Int position = availablePositions[randomIndex];
            availablePositions.RemoveAt(randomIndex);

            List<HexRule> possibleRules = GetPossibleRules(position);
            if (possibleRules.Count == 0) continue;

            HexRule selectedRule = possibleRules[Random.Range(0, possibleRules.Count)];
            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 6) * 60, 0);

            GameObject hexGO = Instantiate(selectedRule.hexPrefab, CalculateHexPosition(position.x, position.y), randomRotation, transform);
            Hex hex = hexGO.GetComponent<Hex>();
            hex.Initialize(position);
            hexGrid[position.x, position.y] = hex;
            hexToRuleMap[hex] = selectedRule;

            // Update the face states of the current hex and its neighbors
            UpdateFaceStates(hex, selectedRule, randomRotation);
        }
    }

    private void UpdateFaceStates(Hex hex, HexRule selectedRule, Quaternion rotation)
    {
        for (int i = 0; i < 6; i++)
        {
            HexFaceDirectionEnum faceDirection = (HexFaceDirectionEnum)i;
            Hex neighbor = GetNeighbor(hex, faceDirection);

            if (neighbor != null)
            {
                HexFaceDirectionEnum oppositeFaceDirection = GetOppositeFaceDirection(faceDirection);

                // Apply rules based on the face direction and neighbor's face direction
                // Update the face states of the current hex and its neighbor
                hex.UpdateFaceState(i, FaceStateEnum.Filled);
                int neighborFaceIndex = (int)oppositeFaceDirection;
                neighbor.UpdateFaceState(neighborFaceIndex, FaceStateEnum.Filled);
            }
        }
    }

    private List<HexRule> GetPossibleRules(Vector2Int position)
    {
        List<HexRule> possibleRules = new List<HexRule>();

        foreach (var ruleSet in hexRuleSets)
        {
            HexRule rule = ruleSet.hexRule;
            bool isValid = true;

            for (int i = 0; i < 6; i++)
            {
                HexFaceDirectionEnum faceDirection = (HexFaceDirectionEnum)i;
                Hex neighbor = GetNeighborAtPosition(position, faceDirection);

                if (neighbor != null)
                {
                    HexFaceDirectionEnum oppositeFaceDirection = GetOppositeFaceDirection(faceDirection);

                    if (hexToRuleMap.TryGetValue(neighbor, out HexRule neighborRule))
                    {
                        bool matchFound = false;
                        foreach (var neighborRuleSet in rule.connections[i].allowedNeighborRules)
                        {
                            if (neighborRuleSet.hexRuleSet.hexRule == neighborRule)
                            {
                                if (neighborRuleSet.neighborFaceDirections.Contains(oppositeFaceDirection))
                                {
                                    matchFound = true;
                                    break;
                                }
                            }
                        }

                        if (!matchFound)
                        {
                            isValid = false;
                            break;
                        }
                    }
                }
            }

            if (isValid)
            {
                possibleRules.Add(rule);
            }
        }

        return possibleRules;
    }

    private Hex GetNeighborAtPosition(Vector2Int position, HexFaceDirectionEnum direction)
    {
        Vector2Int neighborAxial = position + DirectionToAxialOffset(direction);
        Vector2Int neighborOffset = AxialToOffset(neighborAxial);

        if (IsWithinBounds(neighborOffset))
        {
            return hexGrid[neighborOffset.x, neighborOffset.y];
        }

        return null;
    }

    private Vector2Int DirectionToAxialOffset(HexFaceDirectionEnum direction)
    {
        switch (direction)
        {
            case HexFaceDirectionEnum.East: return new Vector2Int(1, 0);
            case HexFaceDirectionEnum.SouthEast: return new Vector2Int(1, -1);
            case HexFaceDirectionEnum.SouthWest: return new Vector2Int(0, -1);
            case HexFaceDirectionEnum.West: return new Vector2Int(-1, 0);
            case HexFaceDirectionEnum.NorthWest: return new Vector2Int(-1, 1);
            case HexFaceDirectionEnum.NorthEast: return new Vector2Int(0, 1);
            default: return Vector2Int.zero;
        }
    }

    private HexFaceDirectionEnum GetOppositeFaceDirection(HexFaceDirectionEnum direction)
    {
        switch (direction)
        {
            case HexFaceDirectionEnum.East: return HexFaceDirectionEnum.West;
            case HexFaceDirectionEnum.SouthEast: return HexFaceDirectionEnum.NorthWest;
            case HexFaceDirectionEnum.SouthWest: return HexFaceDirectionEnum.NorthEast;
            case HexFaceDirectionEnum.West: return HexFaceDirectionEnum.East;
            case HexFaceDirectionEnum.NorthWest: return HexFaceDirectionEnum.SouthEast;
            case HexFaceDirectionEnum.NorthEast: return HexFaceDirectionEnum.SouthWest;
            default: return HexFaceDirectionEnum.East;
        }
    }

    private void ClearMap()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    private void CalculateHexDimensions()
    {
        if (Settings.hexOrientation == HexOrientationEnum.OddR || Settings.hexOrientation == HexOrientationEnum.EvenR)
        {
            Settings.hexWidth = Mathf.Sqrt(3) * Settings.hexSize;
            Settings.hexHeight = 2 * Settings.hexSize;
        }
        else if (Settings.hexOrientation == HexOrientationEnum.OddQ || Settings.hexOrientation == HexOrientationEnum.EvenQ)
        {
            Settings.hexWidth = 2 * Settings.hexSize;
            Settings.hexHeight = Mathf.Sqrt(3) * Settings.hexSize;
        }
    }

    private Vector3 CalculateHexPosition(int x, int y)
    {
        float xPos = 0f;
        float zPos = 0f;

        if (Settings.hexOrientation == HexOrientationEnum.OddR || Settings.hexOrientation == HexOrientationEnum.EvenR)
        {
            xPos = x * Settings.hexWidth;
            zPos = y * (Settings.hexHeight * 0.75f);

            if ((Settings.hexOrientation == HexOrientationEnum.OddR && y % 2 == 1) ||
                (Settings.hexOrientation == HexOrientationEnum.EvenR && y % 2 == 0))
            {
                xPos += Settings.hexWidth / 2f;
            }
        }
        else if (Settings.hexOrientation == HexOrientationEnum.OddQ || Settings.hexOrientation == HexOrientationEnum.EvenQ)
        {
            xPos = x * (Settings.hexWidth * 0.75f);
            zPos = y * Settings.hexHeight;

            if ((Settings.hexOrientation == HexOrientationEnum.OddQ && x % 2 == 1) ||
                (Settings.hexOrientation == HexOrientationEnum.EvenQ && x % 2 == 0))
            {
                zPos += Settings.hexHeight / 2f;
            }
        }

        return new Vector3(xPos, 0, zPos);
    }

    private void SetNeighbors(Hex[,] hexGrid, int x, int y)
    {
        Hex hex = hexGrid[x, y];

        Vector2Int[] directions = {
            new Vector2Int(+1, 0), new Vector2Int(+1, -1), new Vector2Int(0, -1),
            new Vector2Int(-1, 0), new Vector2Int(-1, +1), new Vector2Int(0, +1)
        };

        for (int i = 0; i < directions.Length; i++)
        {
            Vector2Int neighborAxial = hex.axialCoordinates + directions[i];
            Vector2Int neighborOffset = AxialToOffset(neighborAxial);

            if (IsWithinBounds(neighborOffset))
            {
                hex.neighbors[i] = hexGrid[neighborOffset.x, neighborOffset.y];
            }
        }
    }

    private bool IsWithinBounds(Vector2Int coordinates)
    {
        return coordinates.x >= 0 && coordinates.x < Settings.mapWidth &&
               coordinates.y >= 0 && coordinates.y < Settings.mapHeight;
    }

    private Vector2Int AxialToOffset(Vector2Int axial)
    {
        int col = axial.x + (axial.y - (axial.y & 1)) / 2;
        int row = axial.y;
        return new Vector2Int(col, row);
    }

    private Hex GetNeighbor(Hex hex, HexFaceDirectionEnum direction)
    {
        Vector2Int neighborAxial = hex.axialCoordinates + DirectionToAxialOffset(direction);
        Vector2Int neighborOffset = AxialToOffset(neighborAxial);

        if (IsWithinBounds(neighborOffset))
        {
            return hexGrid[neighborOffset.x, neighborOffset.y];
        }

        return null;
    }
}
