using UnityEngine;

public class CityPlacer : MonoBehaviour
{
    public GameObject cityPrefab;

    public void PlaceCities(int numberOfCities, HexGridGenerator gridGenerator)
    {
        for (int i = 0; i < numberOfCities; i++)
        {
            int x = Random.Range(0, gridGenerator.gridWidth);
            int y = Random.Range(0, gridGenerator.gridHeight);
            Vector3 cityPos = gridGenerator.CalculateHexPosition(x, y);
            Instantiate(cityPrefab, cityPos, Quaternion.identity, gridGenerator.transform);
        }
    }
}
