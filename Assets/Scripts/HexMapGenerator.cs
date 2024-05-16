using UnityEngine;

public class HexMapGenerator : Singleton<HexMapGenerator>
{
    public GameObject hexPrefab;
    public int mapWidth = 6;
    public int mapHeight = 6;
    public float hexSize = 1f;

    public enum HexOrientation { OddR, EvenR, OddQ, EvenQ }
    public HexOrientation hexOrientation;

    public float hexWidth;
    public float hexHeight;

    private void OnValidate()
    {
        CalculateHexDimensions();
    }

    public void GenerateHexMap()
    {
        CalculateHexDimensions();
        ClearMap();

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3 position = CalculateHexPosition(x, y);
                Quaternion rotation = Quaternion.identity;

                // Apply rotation for pointy-top hexagons (OddQ, EvenQ)
                if (hexOrientation == HexOrientation.OddQ || hexOrientation == HexOrientation.EvenQ)
                {
                    rotation = Quaternion.Euler(0, 30, 0);
                }

                GameObject hexGO = Instantiate(hexPrefab, position, rotation, this.transform);
                Hex hex = hexGO.GetComponent<Hex>();
                hex.Initialize(new Vector2Int(x, y));
            }
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
        if (hexOrientation == HexOrientation.OddR || hexOrientation == HexOrientation.EvenR)
        {
            hexWidth = Mathf.Sqrt(3) * hexSize;
            hexHeight = 2 * hexSize;
        }
        else if (hexOrientation == HexOrientation.OddQ || hexOrientation == HexOrientation.EvenQ)
        {
            hexWidth = 2 * hexSize;
            hexHeight = Mathf.Sqrt(3) * hexSize;
        }
    }

    private Vector3 CalculateHexPosition(int x, int y)
    {
        float xPos = 0f;
        float zPos = 0f;

        if (hexOrientation == HexOrientation.OddR || hexOrientation == HexOrientation.EvenR)
        {
            xPos = x * hexWidth;
            zPos = y * (hexHeight * 0.75f);

            if ((hexOrientation == HexOrientation.OddR && y % 2 == 1) ||
                (hexOrientation == HexOrientation.EvenR && y % 2 == 0))
            {
                xPos += hexWidth / 2f;
            }
        }
        else if (hexOrientation == HexOrientation.OddQ || hexOrientation == HexOrientation.EvenQ)
        {
            xPos = x * (hexWidth * 0.75f);
            zPos = y * hexHeight;

            if ((hexOrientation == HexOrientation.OddQ && x % 2 == 1) ||
                (hexOrientation == HexOrientation.EvenQ && x % 2 == 0))
            {
                zPos += hexHeight / 2f;
            }
        }

        return new Vector3(xPos, 0, zPos);
    }
}