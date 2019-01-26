using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class GridSystem : MonoBehaviour {

    public bool IsCreated { get { return _createdGrids.Count > 0 ? true : false; } }

    [Header("Initialization")]
    [SerializeField]
    private GameObject _gridPrefab;
    [SerializeField]
    private Transform _target;

    [Header("Settings")]
    [SerializeField]
    private int MAP_WIDTH = 10;
    [SerializeField]
    private int MAP_HEIGHT = 5;

    [SerializeField]
    private int ROW = 3;
    [SerializeField]
    private int COLUMN = 3;
    [SerializeField]
    float SCALE_X = 1;
    [SerializeField]
    float SCALE_Y = 1;

    [Header("Debug")]
    [SerializeField]
    private Grid[,] _grids;

    private List<Grid> _createdGrids = new List<Grid>();

    private Transform _parent;

    private void Update() {
        if (IsCreated) {
            Vector2 targetScale = new Vector2(_target.localScale.x, _target.localScale.z);
            Vector2 targetPosition = new Vector2(_target.localPosition.x, _target.localPosition.z);

            Vector2 minDecisionPosition = targetPosition - (targetScale * 0.5f);
            Vector2 maxDecisionPosition = targetPosition + (targetScale * 0.5f);

            Vector2 minGridIndexRatio = minDecisionPosition / new Vector2(SCALE_X, SCALE_Y);
            Vector2 maxGridIndexRatio = maxDecisionPosition / new Vector2(SCALE_X, SCALE_Y);

            Vector2 reelMinIndex = new Vector2(Mathf.FloorToInt(minGridIndexRatio.x), Mathf.FloorToInt(minGridIndexRatio.y));
            Vector2 reelMaxIndex = new Vector2(Mathf.FloorToInt(maxGridIndexRatio.x), Mathf.FloorToInt(maxGridIndexRatio.y));

            for (int ii = (int)reelMinIndex.x; ii <= reelMaxIndex.x; ii++) {
                for (int jj = (int)reelMinIndex.y; jj <= reelMaxIndex.y; jj++) {
                    _grids[ii, jj].Paint();
                }
            }
        }
    }

    public void CreateGrids() {
        _grids = new Grid[ROW, COLUMN];

        SCALE_X = MAP_WIDTH / COLUMN;
        SCALE_Y = MAP_HEIGHT / ROW;

        if (_parent == null) {
            _parent = new GameObject("Parent").transform;
        }

        for (int ii = 0; ii < ROW; ii++) {
            for (int jj = 0; jj < COLUMN; jj++) {
                Grid grid = Instantiate(_gridPrefab, new Vector3(SCALE_X * 0.5f + (ii * SCALE_X), 0f, SCALE_Y * 0.5f + (jj * SCALE_Y)), Quaternion.identity).GetComponent<Grid>();
                grid.Size = new Vector2(SCALE_X, SCALE_Y);
                grid.Matrix = new Vector2(ii, jj);
                grid.transform.localScale = new Vector3(grid.Size.x, 1f, grid.Size.y);
                grid.transform.SetParent(_parent);

                _grids[ii, jj] = grid;
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
