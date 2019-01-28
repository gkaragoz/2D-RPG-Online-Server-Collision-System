using System.Linq;
using UnityEngine;

public class ExamplePolygon : MonoBehaviour {

    public void Draw(Vector2[] vertices) {
        // Create Vector2 vertices
        var vertices2D = vertices;

        var vertices3D = System.Array.ConvertAll<Vector2, Vector3>(vertices2D, v => v);

        // Use the triangulator to get indices for creating triangles
        var triangulator = new Triangulator(vertices2D);
        var indices = triangulator.Triangulate();

        // Generate a color for each vertex
        var colors = Enumerable.Range(0, vertices3D.Length)
            .Select(i => Random.ColorHSV())
            .ToArray();

        // Create the mesh
        var mesh = new Mesh {
            vertices = vertices3D,
            triangles = indices,
            colors = colors
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Set up game object with mesh;
        var meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));

        var filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = mesh;
    }

    //void Form1_KeyDown(object sender, KeyEventArgs e) {
    //    int i = 14;
    //    Vector velocity = new Vector();

    //    switch (e.KeyValue) {

    //        case 32: //SPACE

    //            break;

    //        case 38: // UP

    //            velocity = new Vector(0, -i);
    //            break;

    //        case 40: // DOWN

    //            velocity = new Vector(0, i);
    //            break;

    //        case 39: // RIGHT

    //            velocity = new Vector(i, 0);
    //            break;

    //        case 37: // LEFT

    //            velocity = new Vector(-i, 0);
    //            break;

    //    }

    //    Vector playerTranslation = velocity;

    //    foreach (Polygon polygon in polygons) {
    //        if (polygon == player) continue;

    //        PolygonCollisionResult r = PolygonCollision(player, polygon, velocity);

    //        if (r.WillIntersect) {
    //            playerTranslation = velocity + r.MinimumTranslationVector;
    //            break;
    //        }
    //    }

    //    player.Offset(playerTranslation);

    //}

}
