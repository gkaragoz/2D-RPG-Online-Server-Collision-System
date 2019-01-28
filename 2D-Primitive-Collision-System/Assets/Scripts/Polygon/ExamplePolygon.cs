using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExamplePolygon : MonoBehaviour {

    public Polygon polygon;

    public void Draw(List<Vector2> vertices, Material material) {
        Vector3[] vector3D = new Vector3[vertices.Count];
        for (int ii = 0; ii < vertices.Count; ii++) {
            vector3D[ii] = new Vector3(vertices[ii].x, vertices[ii].y, 0f);
        }

        // Use the triangulator to get indices for creating triangles
        var triangulator = new Triangulator(vertices);
        var indices = triangulator.Triangulate();

        // Generate a color for each vertex
        var colors = Enumerable.Range(0, vertices.Count)
            .Select(i => Random.ColorHSV())
            .ToArray();

        // Create the mesh
        var mesh = new Mesh {
            vertices = vector3D,
            triangles = indices,
            colors = colors
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Set up game object with mesh;
        var meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(material);

        var filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = mesh;
    }

}
