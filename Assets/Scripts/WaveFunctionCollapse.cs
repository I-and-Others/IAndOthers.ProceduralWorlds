using System.Collections.Generic;
using UnityEngine;

public class WaveFunctionCollapse
{
    private int mapWidth;
    private int mapHeight;
    private List<HexTile> hexTiles;
    private HexTile[,] map;

    public WaveFunctionCollapse(int width, int height, List<HexTile> tiles)
    {
        mapWidth = width;
        mapHeight = height;
        hexTiles = tiles;
        map = new HexTile[width, height];
    }

    public HexTile[,] GenerateMap()
    {
        InitializeMap();
        while (!IsCollapsed())
        {
            var cell = GetLowestEntropyCell();
            if (cell == null)
                break;

            CollapseCell(cell.Value.Item1, cell.Value.Item2);
        }

        return map;
    }

    private void InitializeMap()
    {
        // Initialize all cells with all possible hex tiles
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                map[x, y] = null;
            }
        }
    }

    private bool IsCollapsed()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (map[x, y] == null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private (int, int)? GetLowestEntropyCell()
    {
        // In this example, we simply return the first empty cell we find
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (map[x, y] == null)
                {
                    return (x, y);
                }
            }
        }
        return null;
    }

    private void CollapseCell(int x, int y)
    {
        var possibleTiles = GetPossibleTilesForCell(x, y);

        if (possibleTiles.Count == 0)
        {
            Debug.LogError($"No possible tiles for cell at ({x}, {y}).");
            return;
        }

        HexTile selectedTile = possibleTiles[Random.Range(0, possibleTiles.Count)];
        map[x, y] = selectedTile;

        UpdateNeighbors(x, y, selectedTile);
    }

    private List<HexTile> GetPossibleTilesForCell(int x, int y)
    {
        var possibleTiles = new List<HexTile>(hexTiles);

        // Check neighbors and reduce possible tiles accordingly
        if (x > 0 && map[x - 1, y] != null)
        {
            possibleTiles = FilterPossibleTiles(possibleTiles, map[x - 1, y], HexSide.D);
        }
        if (x < mapWidth - 1 && map[x + 1, y] != null)
        {
            possibleTiles = FilterPossibleTiles(possibleTiles, map[x + 1, y], HexSide.A);
        }
        if (y > 0 && map[x, y - 1] != null)
        {
            possibleTiles = FilterPossibleTiles(possibleTiles, map[x, y - 1], HexSide.F);
        }
        if (y < mapHeight - 1 && map[x, y + 1] != null)
        {
            possibleTiles = FilterPossibleTiles(possibleTiles, map[x, y + 1], HexSide.B);
        }

        return possibleTiles;
    }

    private List<HexTile> FilterPossibleTiles(List<HexTile> possibleTiles, HexTile neighborTile, HexSide side)
    {
        var filteredTiles = new List<HexTile>();

        foreach (var tile in possibleTiles)
        {
            if (IsCompatible(tile, neighborTile, side))
            {
                filteredTiles.Add(tile);
            }
        }

        return filteredTiles;
    }

    private bool IsCompatible(HexTile tile, HexTile neighborTile, HexSide side)
    {
        var neighborSide = GetOppositeSide(side);

        foreach (var face in tile.faces)
        {
            if (face.side == side)
            {
                foreach (var neighborOption in face.possibleNeighbors)
                {
                    if (neighborOption.hexTile == neighborTile)
                    {
                        foreach (var compatibleFace in neighborOption.compatibleFaces)
                        {
                            foreach (var neighborFace in neighborTile.faces)
                            {
                                if (neighborFace.side == neighborSide && compatibleFace == neighborFace.side)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
        }

        return false;
    }

    private HexSide GetOppositeSide(HexSide side)
    {
        switch (side)
        {
            case HexSide.A: return HexSide.D;
            case HexSide.B: return HexSide.E;
            case HexSide.C: return HexSide.F;
            case HexSide.D: return HexSide.A;
            case HexSide.E: return HexSide.B;
            case HexSide.F: return HexSide.C;
            default: return HexSide.A;
        }
    }

    private void UpdateNeighbors(int x, int y, HexTile selectedTile)
    {
        // Update neighbor constraints based on the selected tile
        // This part can be more complex depending on your needs
    }
}
