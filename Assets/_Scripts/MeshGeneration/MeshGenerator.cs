using System.Collections.Generic;
using UnityEngine;

namespace MeshGeneration
{
    public static class MeshGenerator
    {
        private static Vector3[] _directions = new Vector3[6]
        {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back,
        };

        public static Mesh GetCubeSphere(int resolution, float size)
        {
            List<Vector3> vertices = new();
            List<Vector3> normals = new();
            List<int> triangles = new();
            List<Vector2> uvs = new();
            Dictionary<Vector3, int> vertexMap = new();

            Mesh mesh = new Mesh();
            mesh.name = "CubeSphere";

            for (int i = 0; i < _directions.Length; i++)
            {
                int[,] vertexIndices = new int[resolution + 1, resolution + 1];

                // Used to determine plane of the face using main direction
                Vector3 planeAxisA = Vector3.Cross(_directions[i], Vector3.up);
                if (planeAxisA == Vector3.zero) planeAxisA = Vector3.right;
                Vector3 planeAxisB = Vector3.Cross(_directions[i], planeAxisA);

                for (int x = 0; x < resolution + 1; x++)
                {
                    for (int y = 0; y < resolution + 1; y++)
                    {
                        var point = _directions[i] * (size / 2f) 
                            + (-size / 2f + size / resolution * x) * planeAxisA
                            + (-size / 2f + size / resolution * y) * planeAxisB;

                        point = point.normalized * (size / 2f);

                        if (!vertexMap.ContainsKey(point))
                        {
                            vertexMap[point] = vertices.Count;
                            vertices.Add(point);
                            normals.Add(point);

                            Vector3 normal = point.normalized;
                            float u = 0.5f + Mathf.Atan2(normal.z, normal.x) / (2f * Mathf.PI);
                            float v = 0.5f - Mathf.Asin(normal.y) / Mathf.PI;
                            uvs.Add(new Vector2(u, v));
                        }

                        vertexIndices[x, y] = vertexMap[point];
                    }
                }

                for (int x = 0; x < resolution; x++)
                {
                    for (int y = 0; y < resolution; y++)
                    {
                        int i00 = vertexIndices[x, y];
                        int i10 = vertexIndices[x + 1, y];
                        int i01 = vertexIndices[x, y + 1];
                        int i11 = vertexIndices[x + 1, y + 1];

                        triangles.Add(i00);
                        triangles.Add(i10);
                        triangles.Add(i11);

                        triangles.Add(i00);
                        triangles.Add(i11);
                        triangles.Add(i01);
                    }
                }
            }

            // Cone
            Vector3 coneTip = Vector3.forward * (size / 2f + 0.7f);
            int tipIndex = vertices.Count;
            vertices.Add(coneTip);
            normals.Add(Vector3.forward);
            uvs.Add(new Vector2(0.5f, 1f));

            List<int> baseIndices = new();

            for (int i = 0; i <= resolution; i++)
            {
                float angle = 2f * Mathf.PI * i / resolution;
                float x = Mathf.Cos(angle);
                float y = Mathf.Sin(angle);
                Vector3 dir = new Vector3(x, y, 0);
                Vector3 basePoint = dir * (size / 4f);

                int baseIndex = vertices.Count;
                vertices.Add(basePoint);
                normals.Add((basePoint - coneTip).normalized);
                uvs.Add(new Vector2((float)i / resolution, 0));
                baseIndices.Add(baseIndex);
            }

            for (int i = 0; i < resolution; i++)
            {
                int current = baseIndices[i];
                int next = baseIndices[(i + 1) % resolution];

                triangles.Add(tipIndex);
                triangles.Add(current);
                triangles.Add(next);
            }

            mesh.SetVertices(vertices);
            mesh.SetNormals(normals);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            return mesh;
        }

        public static Mesh GetTorus(int mainAngleResolution, int sectionResolution, float tubeRadius, float mainRadius)
        {
            List<Vector3> vertices = new();
            List<Vector3> normals = new();
            List<int> triangles = new();
            List<Vector2> uvs = new();

            Mesh mesh = new Mesh();
            mesh.name = "Torus";

            int[,] vertexIndices = new int[mainAngleResolution, sectionResolution];

            for (int i = 0; i < mainAngleResolution; i++)
            {
                float mainAngle = 2f * Mathf.PI * i / mainAngleResolution;

                float x = Mathf.Cos(mainAngle);
                float y = Mathf.Sin(mainAngle);
                Vector3 midPoint = new Vector3(x, 0, y) * mainRadius;

                for (int j = 0; j < sectionResolution; j++)
                {
                    float torusAngle = 2f * Mathf.PI * j / sectionResolution;

                    Vector3 point = new Vector3(
                    (mainRadius + tubeRadius * Mathf.Cos(torusAngle)) * Mathf.Cos(mainAngle),
                    tubeRadius * Mathf.Sin(torusAngle),
                    (mainRadius + tubeRadius * Mathf.Cos(torusAngle)) * Mathf.Sin(mainAngle));

                    vertexIndices[i, j] = vertices.Count;
                    vertices.Add(point);
                    normals.Add((point - midPoint).normalized);
                    uvs.Add(new Vector2((float)i / mainAngleResolution, (float)j / sectionResolution));
                }

            }

            for(int i = 0; i < mainAngleResolution; i++)
            {
                int nextI = (i + 1) % mainAngleResolution;

                for (int j = 0; j < sectionResolution; j++)
                {
                    int nextJ = (j + 1) % sectionResolution;

                    int i00 = vertexIndices[i, j];
                    int i10 = vertexIndices[nextI, j];
                    int i01 = vertexIndices[i, nextJ];
                    int i11 = vertexIndices[nextI, nextJ];

                    triangles.Add(i00);
                    triangles.Add(i11);
                    triangles.Add(i10);

                    triangles.Add(i00);
                    triangles.Add(i01);
                    triangles.Add(i11);
                }
            }

            mesh.SetVertices(vertices);
            mesh.SetNormals(normals);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}