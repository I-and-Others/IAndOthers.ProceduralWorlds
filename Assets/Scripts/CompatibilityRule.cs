using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CompatibilityRule", menuName = "Hex/CompatibilityRule")]
public class CompatibilityRule : ScriptableObject
{
    public int prefabId;
    public Dictionary<HexFaceDirectionEnum, List<FaceConnection>> faceConnections;

    [System.Serializable]
    public struct FaceConnection
    {
        public int targetPrefabId;
        public HexFaceDirectionEnum targetFaceDirection;
        public HexRotationEnum rotation;
    }
}
