using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public int mapWidth = 100;
    public int mapHeight = 100;
    public float noiseScale = 20f;

    public int octaves = 4;
    [Range(0, 1)]
    public float persistence = 0.5f;
    public float lacunarity = 2f;

    public int seed;
    public Vector2 offset;
}

public class WorldGenerator : MonoBehaviour
{
    [SerializeField]
    private NoiseSettings noiseSettings = new NoiseSettings();

    private Texture2D noiseTexture;

    void OnValidate()
    {
        ValidateSettings();
        GenerateWorld();
    }

    private void ValidateSettings()
    {
        if (noiseSettings.mapWidth < 1) noiseSettings.mapWidth = 1;
        if (noiseSettings.mapHeight < 1) noiseSettings.mapHeight = 1;
        if (noiseSettings.lacunarity < 1) noiseSettings.lacunarity = 1;
        if (noiseSettings.octaves < 0) noiseSettings.octaves = 0;
    }

    public void GenerateWorld()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(noiseSettings);
        DrawNoiseMap(noiseMap);
    }

    private void DrawNoiseMap(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        noiseTexture = new Texture2D(width, height);
        Color[] colourMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }
        noiseTexture.SetPixels(colourMap);
        noiseTexture.Apply();
    }

    public NoiseSettings GetNoiseSettings()
    {
        return noiseSettings;
    }

    public Texture2D GetNoiseTexture()
    {
        return noiseTexture;
    }
}
