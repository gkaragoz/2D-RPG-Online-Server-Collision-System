using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ObjTalker : MonoBehaviour {

    private const string FILE_NAME = "mapData.json";
    private ObjData _objData;

    private void Start() {
        _objData = new ObjData();

        Process();
        WriteToJson(JsonUtility.ToJson(_objData));
    }

    private void Process() {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        _objData.meshName = gameObject.name;

        for (int ii = 0; ii < mesh.vertices.Length; ii++) {
            if (mesh.vertices[ii].y + transform.localScale.y / 2 == 0) {
                _objData.exportedVertices.Add(mesh.vertices[ii]);
            }
        }

        _objData.exportedVertices = _objData.exportedVertices.Distinct().ToList();

        for (int ii = 0; ii < _objData.exportedVertices.Count; ii++) {
            Debug.Log(_objData.exportedVertices[ii]);
        }
    }

    public static void WriteToJson(string logString) {
        string path = "Assets/Resources/" + FILE_NAME;

        if (!File.Exists(path)) {
            Debug.Log("[LogFile] WRITE EVENT>> " + FILE_NAME + " file is creating on path: " + path);
        }

        FileStream file = File.Open(path, FileMode.Append, FileAccess.Write);

        StreamWriter writer = new StreamWriter(file);
        writer.WriteLine(logString);
        writer.Close();
    }

}
