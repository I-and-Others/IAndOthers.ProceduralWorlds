using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WFCManager))]
public class WFCManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WFCManager wfcManager = (WFCManager)target;
        if (GUILayout.Button("Initialize Possible Tile Sets"))
        {
            wfcManager.StartWaveFunctionCollapse();
        }

        if (GUILayout.Button("Collapse Next"))
        {
            wfcManager.CollapseNext();
        }
    }
}
