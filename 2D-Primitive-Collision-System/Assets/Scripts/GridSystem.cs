using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridSystem : MonoBehaviour {

    public bool IsCreated { get { return _createdGrids.Count > 0 ? true : false; } }

    [Header("Initialization")]
    [SerializeField]
    private GameObject _gridPrefab;

    [Header("Settings")]
    [SerializeField]
    private int MAP_WIDTH = 10;
    [SerializeField]
    private int MAP_HEIGHT = 5;

    [SerializeField]
    private int ROW = 3;
    [SerializeField]
    private int COLUMN = 3;

    [Header("Debug")]
    [SerializeField]
    private Grid[,] grids;
    [SerializeField]
    private Vector3 _offset;

    private List<Grid> _createdGrids = new List<Grid>();

    private Transform _parent;

    public void CreateGrids() {
        grids = new Grid[ROW, COLUMN];

        float SCALE_X = MAP_WIDTH / COLUMN;
        float SCALE_Y = MAP_HEIGHT / ROW;

        if (_parent == null) {
            _parent = new GameObject("Parent").transform;
        }

        _offset = new Vector3(-MAP_WIDTH * 0.5f, 0f, -MAP_HEIGHT * 0.5f);
        int indexer = 0;

        for (int ii = 0; ii < COLUMN; ii++) {
            for (int jj = 0; jj < ROW; jj++) {
                Grid grid = Instantiate(_gridPrefab, _offset + new Vector3(ii * SCALE_X, 0f, jj * SCALE_Y), Quaternion.identity).GetComponent<Grid>();
                grid.Size = new Vector2(SCALE_X, SCALE_Y);
                grid.Index = indexer++;
                grid.transform.localScale = new Vector3 (grid.Size.x, 1f, grid.Size.y);
                grid.transform.SetParent(_parent);
                _createdGrids.Add(grid);
            }
        }
    }

    public void DeleteGrids() {
        for (int ii = 0; ii < _createdGrids.Count; ii++) {
            if (_createdGrids[ii] != null) {
                DestroyImmediate(_createdGrids[ii].gameObject);
            }
        }

        _createdGrids = new List<Grid>();
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(GridSystem))]
public class GridSystemEditor : Editor {
    public GridSystem gridSystem;

    public void OnEnable() {
        gridSystem = (GridSystem)target;
    }

    public override void OnInspectorGUI() {
        GUI.backgroundColor = Color.green;
        GUILayout.Space(10f);
        GUILayout.Label("GRID SYSTEM");
        if (GUILayout.Button("CREATE GRIDS")) {
            if (gridSystem.IsCreated) {
                gridSystem.DeleteGrids();
            }

            gridSystem.CreateGrids();

            EditorWindow view = EditorWindow.GetWindow<SceneView>();
            view.Repaint();
        }
        GUILayout.Space(10f);
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("DELETE GRIDS")) {
            gridSystem.DeleteGrids();

            EditorWindow view = EditorWindow.GetWindow<SceneView>();
            view.Repaint();
        }
        GUI.backgroundColor = Color.cyan;

        base.OnInspectorGUI();
    }
}
#endif
