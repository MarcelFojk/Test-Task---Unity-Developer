using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace MeshGeneration
{
    public class MeshGenerator : MonoBehaviour
    {
        private List<Vector3> _vertices = new();   
        private List<Vector3> _normals = new();   
        private List<int> _triangles = new();
        private List<Vector2> _uvs = new();

        private Vector3[] _directions = new Vector3[6]
        {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back,
        };

        Dictionary<Vector3, int> _vertexMap = new Dictionary<Vector3, int>();

        public Mesh GetCubeSphere(int resolution, float size)
        {
            _vertices.Clear();
            _normals.Clear();
            _triangles.Clear();
            _uvs.Clear();
            _vertexMap.Clear();

            Mesh mesh = new Mesh();
            mesh.name = "CubeSphere";

            for (int i = 0; i < _directions.Length; i++)
            {
                int[,] vertexIndices = new int[resolution + 1, resolution + 1];

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

                        if (!_vertexMap.ContainsKey(point))
                        {
                            _vertexMap[point] = _vertices.Count;
                            _vertices.Add(point);
                            _normals.Add(point);
                            _uvs.Add(new Vector2((float)x / resolution, (float)y / resolution));
                        }

                        vertexIndices[x, y] = _vertexMap[point];
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

                        _triangles.Add(i00);
                        _triangles.Add(i10);
                        _triangles.Add(i11);

                        _triangles.Add(i00);
                        _triangles.Add(i11);
                        _triangles.Add(i01);
                    }
                }
            }

            // Cone
            Vector3 coneTip = Vector3.forward * (size / 2f + 1f);
            int tipIndex = _vertices.Count;
            _vertices.Add(coneTip);
            _normals.Add(Vector3.forward);
            _uvs.Add(new Vector2(0.5f, 1f));

            List<int> baseIndices = new();

            for (int i = 0; i <= resolution; i++)
            {
                float angle = 2f * Mathf.PI * i / resolution;
                float x = Mathf.Cos(angle);
                float y = Mathf.Sin(angle);
                Vector3 dir = new Vector3(x, y, 0);
                Vector3 basePoint = dir * (size / 2f);

                int baseIndex = _vertices.Count;
                _vertices.Add(basePoint);
                _normals.Add((basePoint - coneTip).normalized);
                _uvs.Add(new Vector2((float)i / resolution, 0));
                baseIndices.Add(baseIndex);
            }

            for (int i = 0; i < resolution; i++)
            {
                int current = baseIndices[i];
                int next = baseIndices[(i + 1) % resolution];

                _triangles.Add(tipIndex);
                _triangles.Add(current);
                _triangles.Add(next);
            }

            mesh.SetVertices(_vertices);
            mesh.SetNormals(_normals);
            mesh.SetUVs(0, _uvs);
            mesh.SetTriangles(_triangles, 0);
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            return mesh;
        }

        public Mesh GetTorus(int mainAngleResolution, int sectionResolution, float tubeRadius, float mainRadius)
        {
            _vertices.Clear();
            _normals.Clear();
            _triangles.Clear();
            _uvs.Clear();

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

                    vertexIndices[i, j] = _vertices.Count;
                    _vertices.Add(point);
                    _normals.Add((point - midPoint).normalized);
                    _uvs.Add(new Vector2((float)i / mainAngleResolution, (float)j / sectionResolution));
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

                    _triangles.Add(i00);
                    _triangles.Add(i11);
                    _triangles.Add(i10);

                    _triangles.Add(i00);
                    _triangles.Add(i01);
                    _triangles.Add(i11);
                }
            }

            mesh.SetVertices(_vertices);
            mesh.SetNormals(_normals);
            mesh.SetUVs(0, _uvs);
            mesh.SetTriangles(_triangles, 0);
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}