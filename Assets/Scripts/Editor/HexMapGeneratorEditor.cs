using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexMapGenerator))]
public class HexMapGeneratorEditor : Editor
{
    bool showHexMapSettings = true;

    public override void OnInspectorGUI()
    {
        HexMapGenerator hexMapGen = (HexMapGenerator)target;

        // Hex Map Settings Foldout
        showHexMapSettings = EditorGUILayout.Foldout(showHexMapSettings, "Hex Map Settings");
        if (showHexMapSettings)
        {
            EditorGUI.indentLevel++;
            hexMapGen.hexPrefab = (GameObject)EditorGUILayout.ObjectField("Hex Prefab", hexMapGen.hexPrefab, typeof(GameObject), false);
            hexMapGen.mapWidth = EditorGUILayout.IntField("Map Width", hexMapGen.mapWidth);
            hexMapGen.mapHeight = EditorGUILayout.IntField("Map Height", hexMapGen.mapHeight);
            hexMapGen.hexSize = EditorGUILayout.FloatField("Hex Size", hexMapGen.hexSize);
            hexMapGen.hexOrientation = (HexMapGenerator.HexOrientation)EditorGUILayout.EnumPopup("Hex Orientation", hexMapGen.hexOrientation);

            // Display calculated hex width and height
            EditorGUILayout.LabelField("Calculated Hex Width", hexMapGen.hexWidth.ToString("F2"));
            EditorGUILayout.LabelField("Calculated Hex Height", hexMapGen.hexHeight.ToString("F2"));
            EditorGUI.indentLevel--;
        }

        // Generate Hex Map Button
        if (GUILayout.Button("Generate Hex Map"))
        {
            hexMapGen.GenerateHexMap();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(hexMapGen);
            hexMapGen.GenerateHexMap();
        }
    }
}
