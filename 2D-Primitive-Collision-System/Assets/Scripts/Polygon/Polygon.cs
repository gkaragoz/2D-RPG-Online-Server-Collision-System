using System.Collections.Generic;
using UnityEngine;

public class Polygon {

	private List<Vector2> points = new List<Vector2>();
	private List<Vector2> edges = new List<Vector2>();

	public void BuildEdges() {
		Vector2 p1;
		Vector2 p2;
		edges.Clear();
		for (int i = 0; i < points.Count; i++) {
			p1 = points[i];
			if (i + 1 >= points.Count) {
				p2 = points[0];
			} else {
				p2 = points[i + 1];
			}
			edges.Add(p2 - p1);
		}
	}

	public List<Vector2> Edges {
		get { return edges; }
	}

	public List<Vector2> Points {
		get { return points; }
	}

	public Vector2 Center {
		get {
			float totalX = 0;
			float totalY = 0;
			for (int i = 0; i < points.Count; i++) {
				totalX += points[i].x;
				totalY += points[i].y;
			}

			return new Vector2(totalX / (float)points.Count, totalY / (float)points.Count);
		}
	}

	public void Offset(Vector2 v) {
		Offset(v.x, v.y);
	}

	public void Offset(float x, float y) {
		for (int i = 0; i < points.Count; i++) {
			Vector2 p = points[i];
			points[i] = new Vector2(p.x + x, p.y + y);
		}
	}

	public override string ToString() {
		string result = "";

		for (int i = 0; i < points.Count; i++) {
			if (result != "") result += " ";
			result += "{" + ToString(points[i], true)+ "}";
		}

		return result;
    }

    public string ToString(Vector2 vector, bool rounded) {
        if (rounded) {
            return (int)Mathf.Round(vector.x) + ", " + (int)Mathf.Round(vector.y);
        } else {
            return vector.x + ", " + vector.y;
        }
    }

}


