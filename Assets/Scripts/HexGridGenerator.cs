using UnityEngine;

public class HexGridGenerator : MonoBehaviour
{
    public GameObject[] hexPrefabs; // Array to hold different hex prefabs (water, grass, mountains, etc.)
    public int gridWidth = 20;
    public int gridHeight = 20;
    public float hexWidth = 1f;
    public float hexHeight = 1f;

    private void Start()
    {
        GenerateHexGrid();
    }

    public void GenerateHexGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 pos = CalculateHexPosition(x, y);
                Instantiate(hexPrefabs[0], pos, Quaternion.identity, this.transform); // Temporarily using the first prefab
            }
        }
    }

    public Vector3 CalculateHexPosition(int x, int y)
    {
        float xPos = x * hexWidth;
        if (y % 2 == 1)
        {
            xPos += hexWidth / 2f;
        }
        float zPos = y * hexHeight * 0.75f;
        return new Vector3(xPos, 0, zPos);
    }
}
