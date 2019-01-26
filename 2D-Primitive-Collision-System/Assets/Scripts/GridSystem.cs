using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour {

    public GameObject groundObject;

    [SerializeField]
    private List<Grid> _grids = new List<Grid>();

    [SerializeField]
    private float WIDTH = 10;
    [SerializeField]
    private float HEIGHT = 10;

    private void Awake() {
        Mesh mesh = groundObject.GetComponent<MeshFilter>().mesh;

        WIDTH = mesh.bounds.size.x;
        HEIGHT = mesh.bounds.size.z;
    }

}
