using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Grid : MonoBehaviour {

    [SerializeField]
    private int _index;
    [SerializeField]
    private Vector2 _size;

    public Vector2 Size { get => _size; set => _size = value; }

    private void Awake() {
        Size = transform.localScale;
    }

}
