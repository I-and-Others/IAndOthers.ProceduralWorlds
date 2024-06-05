using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexMapGenerator))]
public class HexMapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HexMapGenerator generator = (HexMapGenerator)target;

        if (GUILayout.Button("Generate Hex Map"))
        {
            generator.GenerateHexMap();
        }

        if (GUILayout.Button("Start Wave Function Collapse"))
        {
            generator.StartWaveFunctionCollapse();
        }
    }
}
