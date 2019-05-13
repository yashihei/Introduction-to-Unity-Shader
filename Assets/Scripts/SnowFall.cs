using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SnowFall : MonoBehaviour
{
    const int SNOW_NUM = 16000;
    private Vector3[] _vertices;
    private int[] _triangles;
    private Color[] _colors;
    private Vector2[] _uvs;

    private float _range;
    private float _rangeR;
    private Vector3 _move = Vector3.zero;

    private void Start()
    {
        _range = 16f;
        _rangeR = 1.0f / _range;
        
        // Build vertices
        _vertices = new Vector3[SNOW_NUM * 4];
        for (var i = 0; i < SNOW_NUM; i++)
        {
            float x = Random.Range(-_range, _range);
            float y = Random.Range(-_range, _range);
            float z = Random.Range(-_range, _range);
            var point = new Vector3(x, y, z);
            _vertices[i * 4 + 0] = point;
            _vertices[i * 4 + 1] = point;
            _vertices[i * 4 + 2] = point;
            _vertices[i * 4 + 3] = point;
        }

        // Build triangles
        _triangles = new int[SNOW_NUM * 6];
        for (int i = 0; i < SNOW_NUM; i++)
        {
            _triangles[i * 6 + 0] = i * 4 + 0;
            _triangles[i * 6 + 1] = i * 4 + 1;
            _triangles[i * 6 + 2] = i * 4 + 2;
            _triangles[i * 6 + 3] = i * 4 + 2;
            _triangles[i * 6 + 4] = i * 4 + 1;
            _triangles[i * 6 + 5] = i * 4 + 3;
        }

        // Build uvs
        _uvs = new Vector2[SNOW_NUM * 4];
        for (var i = 0; i < SNOW_NUM; i++)
        {
            _uvs[i * 4 + 0] = new Vector2(0f, 0f);
            _uvs[i * 4 + 1] = new Vector2(1f, 0f);
            _uvs[i * 4 + 2] = new Vector2(0f, 1f);
            _uvs[i * 4 + 3] = new Vector2(1f, 1f);
        }
        
        // Build mesh
        Mesh mesh = new Mesh();
        mesh.name = "MeshSnowFlakes";
        mesh.vertices = _vertices;
        mesh.triangles = _triangles;
        mesh.colors = _colors;
        mesh.uv = _uvs;
        // Frustum Culling回避の為 https://helpdesk.unity3d.co.jp/hc/ja/articles/204550764
        mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 99999999);
        var mf = GetComponent<MeshFilter>();
        mf.sharedMesh = mesh;
    }

    private void LateUpdate()
    {
        var targetPosition = Camera.main.transform.TransformPoint(Vector3.forward * _range);
        var mr = GetComponent<Renderer>();
        mr.material.SetFloat("_Range", _range);
        mr.material.SetFloat("_RangeR", _rangeR);
        mr.material.SetFloat("_Size", 0.1f);
        mr.material.SetVector("_MoveTotal", _move);
        mr.material.SetVector("_CamUp", Camera.main.transform.up);
        mr.material.SetVector("_TargetPosition", targetPosition);

        float x = (Mathf.PerlinNoise(0f, Time.time * 0.1f) - 0.5f) * 10f;
        float y = -2f;
        float z = (Mathf.PerlinNoise(Time.time * 0.1f, 0f) - 0.5f) * 10f;
        _move += new Vector3(x, y, z) * Time.deltaTime;
        _move.x = Mathf.Repeat(_move.x, _range * 2f);
        _move.y = Mathf.Repeat(_move.y, _range * 2f);
        _move.z = Mathf.Repeat(_move.z, _range * 2f);
    }
}