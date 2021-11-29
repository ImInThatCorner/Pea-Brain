using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFOV : MonoBehaviour
{
    public float fov = 60f;
    public int rayCount = 30;

    public Transform player;
    public Camera cam;

    [SerializeField]
    private LayerMask layerMask;

    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Update()
    {
        float angle = CalculateStartingAngle();
        float angleIncrease = fov / rayCount;
        float maxViewDistance = 50f;

        // FOV mesh properties
        Vector3[] vertices = new Vector3[rayCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = player.position;

        int vertexIndex = 1;
        int triangleIndex = 0;
        // draw all the triangles in the FOV mesh
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 offset = GetVectorFromAngle(angle);
            Vector3 vertex;

            RaycastHit hit;
            Ray ray = new Ray(player.position, GetVectorFromAngle(angle));
            Physics.Raycast(ray, out hit, maxViewDistance, layerMask);
            if (hit.collider == null)
            {
                vertex = player.position + offset * maxViewDistance;
            }
            else
            {
                vertex = hit.point;
            }


            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }
            vertexIndex++;

            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

    }

    private Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = Mathf.Deg2Rad * angle;
        return new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad));
    }

    private float GetAngleFromVector(Vector3 vector)
    {
        vector = vector.normalized;
        float angle = Mathf.Atan2(vector.z, vector.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        return angle;
    }

    private float CalculateStartingAngle()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerPosInScreen = cam.WorldToScreenPoint(player.position);

        Vector3 orientation = (Vector2)(mousePos - playerPosInScreen);
        float startingAngle = GetAngleFromVector(new Vector3(orientation.x, 0, orientation.y)) + fov / 2f;

        return startingAngle;
    }
}
