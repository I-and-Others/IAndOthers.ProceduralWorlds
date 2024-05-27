using UnityEngine;

public class Hex : MonoBehaviour
{
    public Vector2Int coordinates; // Hex coordinates

    public void Initialize(Vector2Int coords)
    {
        coordinates = coords;
    }
}
