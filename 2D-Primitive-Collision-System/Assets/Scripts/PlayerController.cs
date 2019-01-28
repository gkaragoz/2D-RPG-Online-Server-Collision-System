using UnityEditor;
using UnityEngine;
using static PolygonSystem;

public class PlayerController : MonoBehaviour {

    MapGenerator mapGenerator;
    ExamplePolygon examplePolygon;

    private void Start() {
        examplePolygon = GetComponent<ExamplePolygon>();
        mapGenerator = GameObject.FindObjectOfType<MapGenerator>();
    }

    private void Update() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical) * movementSpeed;

        if(direction != Vector3.zero) {
            transform.Translate(mapGenerator.ProcessMovement(examplePolygon, direction));
        }
    }

    public enum Direction {
        UP,
        DOWN,
        RIGHT,
        LEFT,
        UPPER_RIGHT,
        UPPER_LEFT,
        LOWER_RIGHT,
        LOWER_LEFT
    }

    public float movementSpeed = 1f;

    public void Move(Direction direction) {
        Vector3 directionVector = Vector3.zero;
        switch (direction) {
            case Direction.UP:
                directionVector = Vector3.forward * movementSpeed;
                break;
            case Direction.DOWN:
                directionVector = Vector3.back * movementSpeed;
                break;
            case Direction.RIGHT:
                directionVector = Vector3.right * movementSpeed;
                break;
            case Direction.LEFT:
                directionVector = Vector3.left * movementSpeed;
                break;
            case Direction.UPPER_RIGHT:
                directionVector = (Vector3.forward + Vector3.right) * movementSpeed;
                break;
            case Direction.UPPER_LEFT:
                directionVector = (Vector3.forward + Vector3.left) * movementSpeed;
                break;
            case Direction.LOWER_RIGHT:
                directionVector = (Vector3.back + Vector3.right) * movementSpeed;
                break;
            case Direction.LOWER_LEFT:
                directionVector = (Vector3.back + Vector3.left) * movementSpeed;
                break;
            default:
                break;
        }

        transform.Translate(mapGenerator.ProcessMovement(examplePolygon, directionVector));
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor {
    public PlayerController playerController;

    public void OnEnable() {
        playerController = (PlayerController)target;
    }

    public override void OnInspectorGUI() {

        base.OnInspectorGUI();
        GUILayout.Space(10f);

        GUILayout.BeginHorizontal();
        GUISkin skin = GUI.skin;

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("UPPER_LEFT", GUILayout.ExpandWidth(false), GUILayout.MaxHeight(150f), GUILayout.MaxWidth(150f))) {
            playerController.Move(PlayerController.Direction.UPPER_LEFT);
        }
        if (GUILayout.Button("UP", GUILayout.ExpandWidth(false), GUILayout.MaxHeight(150f), GUILayout.MaxWidth(150f))) {
            playerController.Move(PlayerController.Direction.UP);
        }
        if (GUILayout.Button("UPPER_RIGHT", GUILayout.ExpandWidth(false), GUILayout.MaxHeight(150f), GUILayout.MaxWidth(150f))) {
            playerController.Move(PlayerController.Direction.UPPER_RIGHT);
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("LEFT", GUILayout.ExpandWidth(false), GUILayout.MaxHeight(150f), GUILayout.MaxWidth(150f))) {
            playerController.Move(PlayerController.Direction.LEFT);
        }
        GUILayout.Button("", skin.label, GUILayout.ExpandWidth(false), GUILayout.MaxHeight(150f), GUILayout.MaxWidth(150f));
        if (GUILayout.Button("RIGHT", GUILayout.ExpandWidth(false), GUILayout.MaxHeight(150f), GUILayout.MaxWidth(150f))) {
            playerController.Move(PlayerController.Direction.RIGHT);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("LOWER_LEFT", GUILayout.ExpandWidth(false), GUILayout.MaxHeight(150f), GUILayout.MaxWidth(150f))) {
            playerController.Move(PlayerController.Direction.LOWER_LEFT);
        }
        if (GUILayout.Button("DOWN", GUILayout.ExpandWidth(false), GUILayout.MaxHeight(150f), GUILayout.MaxWidth(150f))) {
            playerController.Move(PlayerController.Direction.DOWN);
        }
        if (GUILayout.Button("LOWER_RIGHT", GUILayout.ExpandWidth(false), GUILayout.MaxHeight(150f), GUILayout.MaxWidth(150f))) {
            playerController.Move(PlayerController.Direction.LOWER_RIGHT);
        }

        GUILayout.EndHorizontal();
    }
}
#endif
