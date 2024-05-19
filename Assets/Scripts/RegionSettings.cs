using UnityEngine;

[CreateAssetMenu(fileName = "RegionSettings", menuName = "Hex/RegionSettings")]
[System.Serializable]
public class RegionSettings : ScriptableObject
{
    public Region[] regions;
}
