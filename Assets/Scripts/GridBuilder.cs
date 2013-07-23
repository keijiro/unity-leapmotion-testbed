using UnityEngine;
using System.Collections;

public class GridBuilder : MonoBehaviour
{
    int width = 20;
    int height = 10;

    void Awake ()
    {
        var vertices = new Vector3[(width + 1) * 2 + (height + 1) * 2];
        var offs = 0;

        for (var h = 0; h <= height; h++) {
            vertices [offs++] = new Vector3 (0, h, 0);
            vertices [offs++] = new Vector3 (width, h, 0);
        }

        for (var w = 0; w <= width; w++) {
            vertices [offs++] = new Vector3 (w, 0, 0);
            vertices [offs++] = new Vector3 (w, height, 0);
        }

        var indices = new int[(width + 1) * 2 + (height + 1) * 2];

        for (var i = 0; i < offs; i++) {
            indices [i] = i;
        }

        var mesh = new Mesh ();
        mesh.vertices = vertices;
        mesh.SetIndices (indices, MeshTopology.Lines, 0);

        GetComponent<MeshFilter> ().mesh = mesh;
    }
}
