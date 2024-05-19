using UnityEngine;

public class HexMapGenerator : Singleton<HexMapGenerator>
{
    public HexMapSettings Settings;

    private void OnValidate()
    {
        CalculateHexDimensions();
    }

    public void GenerateHexMap(float[,] noiseMap, RegionSettings regionSettings)
    {
        CalculateHexDimensions();
        ClearMap();

        for (int x = 0; x < Settings.mapWidth; x++)
        {
            for (int y = 0; y < Settings.mapHeight; y++)
            {
                Vector3 position = CalculateHexPosition(x, y);
                Quaternion rotation = Quaternion.identity;

                // Apply rotation for pointy-top hexagons (OddQ, EvenQ)
                if (Settings.hexOrientation == HexOrientationEnum.OddQ || Settings.hexOrientation == HexOrientationEnum.EvenQ)
                {
                    rotation = Quaternion.Euler(0, 30, 0);
                }

                // Get the noise value for this hex
                float noiseValue = noiseMap[(int)(x * (noiseMap.GetLength(0) / (float)Settings.mapWidth)), (int)(y * (noiseMap.GetLength(1) / (float)Settings.mapHeight))];

                // Determine the appropriate prefab based on the noise value
                GameObject hexPrefab = GetPrefabForNoiseValue(noiseValue, regionSettings);

                GameObject hexGO = Instantiate(hexPrefab, position, rotation, this.transform);
                Hex hex = hexGO.GetComponent<Hex>();
                hex.Initialize(new Vector2Int(x, y));
            }
        }
    }

    private GameObject GetPrefabForNoiseValue(float noiseValue, RegionSettings regionSettings)
    {
        foreach (var region in regionSettings.regions)
        {
            if (noiseValue >= region.minValue && noiseValue <= region.maxValue)
            {
                return region.prefab;
            }
        }
        // Default to base hex prefab if no region matches
        return Settings.hexPrefab;
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
}
