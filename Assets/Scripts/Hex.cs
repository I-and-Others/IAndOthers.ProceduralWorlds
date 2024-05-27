using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public Vector2Int coordinates; // Hex coordinates
    public FaceStateEnum[] faceStates = new FaceStateEnum[6]; // Array to store the state of each face

    public void Initialize(Vector2Int coords)
    {
        coordinates = coords;
        // Initialize all faces to Empty by default
        for (int i = 0; i < faceStates.Length; i++)
        {
            faceStates[i] = FaceStateEnum.Empty;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 0.1f);

        Vector3[] facePositions = {
            Quaternion.Euler(0, 30, 0) * new Vector3(0, 0, 0.5f), // A
            Quaternion.Euler(0, 30, 0) * new Vector3(0.433f, 0, 0.25f), // B
            Quaternion.Euler(0, 30, 0) * new Vector3(0.433f, 0, -0.25f), // C
            Quaternion.Euler(0, 30, 0) * new Vector3(0, 0, -0.5f), // D
            Quaternion.Euler(0, 30, 0) * new Vector3(-0.433f, 0, -0.25f), // E
            Quaternion.Euler(0, 30, 0) * new Vector3(-0.433f, 0, 0.25f) // F
        };

        Color[] faceColors = {
            Color.red,    // A
            Color.green,  // B
            Color.blue,   // C
            Color.yellow, // D
            Color.magenta,// E
            Color.cyan    // F
        };

        for (int i = 0; i < facePositions.Length; i++)
        {
            Gizmos.color = faceColors[i];
            Gizmos.DrawLine(transform.position, transform.position + facePositions[i]);

            switch (faceStates[i])
            {
                case FaceStateEnum.Empty:
                    Gizmos.color = Color.white;
                    Gizmos.DrawWireSphere(transform.position + facePositions[i], 0.1f);
                    break;
                case FaceStateEnum.Filled:
                    Gizmos.color = faceColors[i];
                    Gizmos.DrawSphere(transform.position + facePositions[i], 0.1f);
                    break;
                case FaceStateEnum.Locked:
                    Gizmos.color = Color.black;
                    Gizmos.DrawSphere(transform.position + facePositions[i], 0.1f);
                    break;
            }
        }
    }
}