﻿using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class GridSystem : MonoBehaviour {

    public bool IsCreated {
        get {
            if (_grids != null) {
                return true;
            }
            return false;
        }
    }

    [Header("Initialization")]
    [SerializeField]
    private GameObject _gridPrefab;
    [SerializeField]
    private Transform[] _targets;

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
    float SCALE_X = 1;
    [SerializeField]
    float SCALE_Y = 1;
    [SerializeField]
    private Grid[,] _grids;

    private Transform _parent;

    public Grid GetRandomGrid() {
        return _grids[Random.Range(0, ROW), Random.Range(0, COLUMN)];
    }

    public void CreateGrids() {
        DeleteGrids();

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
            }
        }

        for (int ii = 0; ii < ROW; ii++) {
            for (int jj = 0; jj < COLUMN; jj++) {
                SetNeighbours(_grids[ii, jj]);
            }
        }
    }

    public void DeleteGrids() {
        GameObject _parentObject = GameObject.Find("Parent");
        if (_parentObject != null) {
            DestroyImmediate(_parentObject);
        }

        _grids = null;
    }

    public List<Grid> GetNeighbours(params Grid[] grids) {
        List<Grid> allNeighbours = new List<Grid>();

        for (int ii = 0; ii < grids.Length; ii++) {
            allNeighbours = allNeighbours.Union(grids[ii].Neighbours).ToList();
        }
        for (int ii = 0; ii < grids.Length; ii++) {
            allNeighbours.Remove(grids[ii]);
        }

        return allNeighbours;
    }

    private void LateUpdate() {
        if (IsCreated) {
            SearchTarget();
        }
    }

    private void SetNeighbours(Grid grid) {
        List<Grid> neighbours = new List<Grid>();

        for (int jj = -1; jj <= 1; jj++) {
            for (int kk = -1; kk <= 1; kk++) {
                if (jj == 0 && kk == 0)
                    continue;

                int checkX = (int)grid.Matrix.x + jj;
                int checkY = (int)grid.Matrix.y + kk;

                if (checkX >= 0 && checkX < ROW && checkY >= 0 && checkY < COLUMN) {
                    neighbours.Add(_grids[checkX, checkY]);
                }
            }
        }

        grid.Neighbours = neighbours;
    }

    private void SearchTarget() {
        for (int ii = 0; ii < _targets.Length; ii++) {
            Vector2 targetScale = new Vector2(_targets[ii].localScale.x, _targets[ii].localScale.z);
            Vector2 targetPosition = new Vector2(_targets[ii].localPosition.x, _targets[ii].localPosition.z);

            Vector2 minDecisionPosition = targetPosition - (targetScale * 0.5f);
            Vector2 maxDecisionPosition = targetPosition + (targetScale * 0.5f);

            Vector2 minGridIndexRatio = minDecisionPosition / new Vector2(SCALE_X, SCALE_Y);
            Vector2 maxGridIndexRatio = maxDecisionPosition / new Vector2(SCALE_X, SCALE_Y);

            Vector2 reelMinIndex = new Vector2(Mathf.FloorToInt(minGridIndexRatio.x), Mathf.FloorToInt(minGridIndexRatio.y));
            Vector2 reelMaxIndex = new Vector2(Mathf.FloorToInt(maxGridIndexRatio.x), Mathf.FloorToInt(maxGridIndexRatio.y));

            for (int jj = (int)reelMinIndex.x; jj <= reelMaxIndex.x; jj++) {
                for (int kk = (int)reelMinIndex.y; kk <= reelMaxIndex.y; kk++) {
                    _grids[jj, kk].Paint();
                }
            }
        }
    }

    private void OnValidate() {
        if (IsCreated) {
            SearchTarget();
        }
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
        GUILayout.Space(10f);
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("GET NEIGHBOURS")) {
            Grid grid = gridSystem.GetRandomGrid();
            Grid grid2 = gridSystem.GetRandomGrid();
            grid.PaintObstacle();
            grid2.PaintObstacle();

            Debug.Log("Selected Grid1: " + grid.Matrix.ToString());
            Debug.Log("Selected Grid2: " + grid2.Matrix.ToString());
            List<Grid> neighbours = gridSystem.GetNeighbours(grid, grid2);

            for (int ii = 0; ii < neighbours.Count; ii++) {
                Debug.Log(neighbours[ii].Matrix.ToString());
                neighbours[ii].Paint();
            }

            EditorWindow view = EditorWindow.GetWindow<SceneView>();
            view.Repaint();
        }
        GUI.backgroundColor = Color.cyan;

        base.OnInspectorGUI();
    }
}
#endif
