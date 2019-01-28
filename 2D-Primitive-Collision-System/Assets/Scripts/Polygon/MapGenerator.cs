using System.Collections.Generic;
using UnityEngine;

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

        playerObject = CreatePolygon(polygons[0].Points.ToArray());
        playerObject.AddComponent<PlayerController>().movementSpeed = 1;
        playerObject.name = "Player";

        for (int ii = 1; ii < polygons.Count; ii++) {
            CreatePolygon(polygons[ii].Points.ToArray());
        }
    }

    public GameObject CreatePolygon(Vector2[] vertices) {
        GameObject polygonObject = new GameObject();
        ExamplePolygon polygon = polygonObject.AddComponent<ExamplePolygon>();
        polygon.Draw(vertices);

        return polygonObject;
    }
    
}
