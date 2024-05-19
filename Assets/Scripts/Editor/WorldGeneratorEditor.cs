using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WorldGenerator))]
public class WorldGeneratorEditor : Editor
{
    bool showPerlinNoiseSettings = true;
    bool showHexMapSettings = true;
    bool showRegionSettings = true;

    public override void OnInspectorGUI()
    {
        WorldGenerator worldGen = (WorldGenerator)target;

        // Perlin Noise Settings Foldout
        showPerlinNoiseSettings = EditorGUILayout.Foldout(showPerlinNoiseSettings, "Perlin Noise Settings");
        if (showPerlinNoiseSettings)
        {
            EditorGUI.indentLevel++;
            NoiseSettings settings = worldGen.GetNoiseSettings();
            settings.mapWidth = EditorGUILayout.IntField("Map Width", settings.mapWidth);
            settings.mapHeight = EditorGUILayout.IntField("Map Height", settings.mapHeight);
            settings.noiseScale = EditorGUILayout.FloatField("Noise Scale", settings.noiseScale);
            settings.octaves = EditorGUILayout.IntField("Octaves", settings.octaves);
            settings.persistence = EditorGUILayout.Slider("Persistence", settings.persistence, 0, 1);
            settings.lacunarity = EditorGUILayout.FloatField("Lacunarity", settings.lacunarity);
            settings.seed = EditorGUILayout.IntField("Seed", settings.seed);
            settings.offset = EditorGUILayout.Vector2Field("Offset", settings.offset);

            // Noise Map Preview
            GUILayout.Space(10);
            GUILayout.Label("Noise Map Preview", EditorStyles.boldLabel);

            Texture2D noiseTexture = worldGen.GetNoiseTexture();
            if (noiseTexture != null)
            {
                // Calculate the size of the preview texture based on the inspector window size
                float inspectorWidth = noiseTexture.width;
                float aspectRatio = (float)noiseTexture.height / noiseTexture.width;
                float previewHeight = inspectorWidth * aspectRatio;

                // Center the preview texture
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(noiseTexture, GUILayout.Width(inspectorWidth), GUILayout.Height(previewHeight));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;
        }

        // Hex Map Settings Foldout
        HexMapGenerator hexMapGen = HexMapGenerator.Instance;
        showHexMapSettings = EditorGUILayout.Foldout(showHexMapSettings, "Hex Map Settings");
        if (showHexMapSettings)
        {
            EditorGUI.indentLevel++;
            hexMapGen.Settings.hexPrefab = (GameObject)EditorGUILayout.ObjectField("Hex Prefab", hexMapGen.Settings.hexPrefab, typeof(GameObject), false);
            hexMapGen.Settings.mapWidth = EditorGUILayout.IntField("Map Width", hexMapGen.Settings.mapWidth);
            hexMapGen.Settings.mapHeight = EditorGUILayout.IntField("Map Height", hexMapGen.Settings.mapHeight);
            hexMapGen.Settings.hexSize = EditorGUILayout.FloatField("Hex Size", hexMapGen.Settings.hexSize);
            hexMapGen.Settings.hexOrientation = (HexOrientationEnum)EditorGUILayout.EnumPopup("Hex Orientation", hexMapGen.Settings.hexOrientation);

            // Display calculated hex width and height
            EditorGUILayout.LabelField("Calculated Hex Width", hexMapGen.Settings.hexWidth.ToString("F2"));
            EditorGUILayout.LabelField("Calculated Hex Height", hexMapGen.Settings.hexHeight.ToString("F2"));
            EditorGUI.indentLevel--;
        }

        // Region Settings Foldout
        showRegionSettings = EditorGUILayout.Foldout(showRegionSettings, "Region Settings");
        if (showRegionSettings)
        {
            EditorGUI.indentLevel++;
            worldGen.SetRegionSettings((RegionSettings)EditorGUILayout.ObjectField("Region Settings", worldGen.GetRegionSettings(), typeof(RegionSettings), false));
            EditorGUI.indentLevel--;
        }


        // Generate Hex Map Button
        if (GUILayout.Button("Generate Hex Map"))
        {
        }

        // Auto update when GUI changes
        if (GUI.changed)
        {
            worldGen.GenerateWorld();
            EditorUtility.SetDirty(hexMapGen);
        }
    }
}