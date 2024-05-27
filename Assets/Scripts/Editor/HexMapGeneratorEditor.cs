using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexMapGenerator))]
public class HexMapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HexMapGenerator hexMapGenerator = (HexMapGenerator)target;
        if (GUILayout.Button("Generate Hex Map"))
        {
            // Schedule the generation for the next frame
            EditorApplication.delayCall += () => hexMapGenerator.GenerateHexMap();
        }
    }
}
