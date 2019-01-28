using System.Collections.Generic;
using UnityEngine;
using static PolygonSystem;

public class MapGenerator : MonoBehaviour {

    List<ExamplePolygon> examplePolygons = new List<ExamplePolygon>();
    List<Polygon> polygons = new List<Polygon>();
    GameObject playerObject;

    private void Start() {
        Polygon p = new Polygon();
        p.Points.Add(new Vector2(100, 0));
        p.Points.Add(new Vector2(150, 50));
        p.Points.Add(new Vector2(100, 150));
        p.Points.Add(new Vector2(0, 100));

        polygons.Add(p);

        p = new Polygon();
        p.Points.Add(new Vector2(50, 50));
        p.Points.Add(new Vector2(100, 0));
        p.Points.Add(new Vector2(150, 150));
        p.Offset(80, 80);

        polygons.Add(p);

        p = new Polygon();
        p.Points.Add(new Vector2(0, 50));
        p.Points.Add(new Vector2(50, 0));
        p.Points.Add(new Vector2(150, 80));
        p.Points.Add(new Vector2(160, 200));
        p.Points.Add(new Vector2(-10, 190));
        p.Offset(300, 300);

        polygons.Add(p);

        foreach (Polygon polygon in polygons) polygon.BuildEdges();

        playerObject = CreatePolygon(polygons[0]);
        playerObject.AddComponent<PlayerController>().movementSpeed = 10;
        playerObject.name = "Player";

        for (int ii = 1; ii < polygons.Count; ii++) {
            CreatePolygon(polygons[ii]);
        }
    }

    public GameObject CreatePolygon(Polygon polygon) {
        GameObject polygonObject = new GameObject();
        ExamplePolygon examplePolygon = polygonObject.AddComponent<ExamplePolygon>();
        examplePolygon.polygon = polygon;
        examplePolygon.Draw(polygon.Points);

        return polygonObject;
    }

    public Vector2 ProcessMovement(ExamplePolygon examplePolygon, Vector3 direction) {
        Vector2 playerTranslation = new Vector2(direction.x, direction.z);

        foreach (Polygon polygon in polygons) {
            if (polygon == examplePolygon.polygon) continue;

            PolygonCollisionResult r = PolygonCollision(examplePolygon.polygon, polygon, playerTranslation);

            if (r.WillIntersect) {
                playerTranslation = playerTranslation + r.MinimumTranslationVector;
                break;
            }
        }

        examplePolygon.polygon.Offset(playerTranslation);
        return playerTranslation;
    }

}
