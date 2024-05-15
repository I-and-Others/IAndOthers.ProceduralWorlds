using UnityEngine;

public class TerrainTypeAssigner : MonoBehaviour
{
    public GameObject waterPrefab;
    public GameObject grassPrefab;
    public GameObject mountainPrefab;

    public void AssignTerrainTypes(float[,] noiseMap, HexGridGenerator gridGenerator)
    {
        for (int x = 0; x < gridGenerator.gridWidth; x++)
        {
            for (int y = 0; y < gridGenerator.gridHeight; y++)
            {
                float noiseValue = noiseMap[x, y];
                GameObject prefab = ChooseHexPrefab(noiseValue);
                Vector3 pos = gridGenerator.CalculateHexPosition(x, y);
                Instantiate(prefab, pos, Quaternion.identity, gridGenerator.transform);
            }
        }
    }

    private GameObject ChooseHexPrefab(float noiseValue)
    {
        if (noiseValue < 1.5f) return waterPrefab;
        else if (noiseValue < 3f) return grassPrefab;
        else return mountainPrefab;
    }
}
