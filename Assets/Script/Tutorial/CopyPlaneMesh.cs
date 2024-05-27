using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CreatePlaneWithHole : MonoBehaviour
{
    public Vector2 holeSize = new Vector2(1f, 1f); // 직사각형 구멍의 크기 (가로, 세로)
    public Vector2 holeCenter = new Vector2(0f, 0f); // 직사각형 구멍의 중심 좌표

    void Start()
    {
        Mesh originalMesh = CreateOriginalPlaneMesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for (int i = 0; i < originalMesh.triangles.Length; i += 3)
        {
            Vector3 v0 = originalMesh.vertices[originalMesh.triangles[i]];
            Vector3 v1 = originalMesh.vertices[originalMesh.triangles[i + 1]];
            Vector3 v2 = originalMesh.vertices[originalMesh.triangles[i + 2]];

            Vector2 v0UV = originalMesh.uv[originalMesh.triangles[i]];
            Vector2 v1UV = originalMesh.uv[originalMesh.triangles[i + 1]];
            Vector2 v2UV = originalMesh.uv[originalMesh.triangles[i + 2]];

            // 삼각형의 모든 점이 직사각형 구멍 내부에 있지 않은 경우에만 추가
            if (!IsInsideRectangle(v0UV) || !IsInsideRectangle(v1UV) || !IsInsideRectangle(v2UV))
            {
                int index = vertices.Count;
                vertices.Add(v0);
                vertices.Add(v1);
                vertices.Add(v2);
                triangles.Add(index);
                triangles.Add(index + 1);
                triangles.Add(index + 2);

                uvs.Add(v0UV);
                uvs.Add(v1UV);
                uvs.Add(v2UV);
            }
        }

        Mesh newMesh = new Mesh();
        newMesh.vertices = vertices.ToArray();
        newMesh.triangles = triangles.ToArray();
        newMesh.uv = uvs.ToArray();

        newMesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = newMesh;
    }

    Mesh CreateOriginalPlaneMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = {
            new Vector3(-1f, 0,  1f),
            new Vector3( 1f, 0,  1f),
            new Vector3( 1f, 0, -1f),
            new Vector3(-1f, 0, -1f)
        };

        Vector2[] uvs = {
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, 0)
        };

        int[] triangles = {
            0, 1, 2,
            0, 2, 3
        };

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        return mesh;
    }

    bool IsInsideRectangle(Vector2 point)
    {
        return (point.x > holeCenter.x - holeSize.x / 2 &&
                point.x < holeCenter.x + holeSize.x / 2 &&
                point.y > holeCenter.y - holeSize.y / 2 &&
                point.y < holeCenter.y + holeSize.y / 2);
    }
}
