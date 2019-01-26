using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class Grid : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI _txtIndex;

    [SerializeField]
    private int _index;
    [SerializeField]
    private Vector2 _size;

    public int Index { get => _index; set => _index = value; }
    public Vector2 Size { get => _size; set => _size = value; }

    private void Start() {
        Size = transform.localScale;
        _txtIndex.text = _index.ToString();
    }

}
