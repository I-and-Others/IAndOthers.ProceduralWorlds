using UnityEngine;

public class Hex : MonoBehaviour
{
    public Vector2Int coordinates; // Hex coordinates
    public float noiseValue; // Noise value from Perlin noise

    public void Initialize(Vector2Int coords, float noise = 1f)
    {
        coordinates = coords;
        noiseValue = noise;
        // Optionally set visual appearance based on noise value
    }
}
