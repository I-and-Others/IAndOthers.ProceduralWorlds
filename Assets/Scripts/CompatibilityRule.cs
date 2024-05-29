using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CompatibilityRule", menuName = "ScriptableObjects/CompatibilityRule", order = 1)]
public class CompatibilityRule : ScriptableObject
{
    public int prefabId; // Unique ID for the prefab
    public List<FaceCompatibility> faceCompatibilities; // List of face compatibilities

    [System.Serializable]
    public struct FaceCompatibility
    {
        public int faceIndex; // Index of the face (0-5)
        public List<int> compatiblePrefabIds; // List of compatible prefab IDs for this face
    }
}
