using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Vertex Pair maps two verices to each other, used to find the nearest vertex to a given vertex.
/// </summary>
public class VertexPair
{
    public Vector3 Vertex1 { get; set; }
    public Vector3 Vertex2 { get; set; }

    public VertexPair(Vector3 vertex1, Vector3 vertex2)
    {
        Vertex1 = vertex1;
        Vertex2 = vertex2;
    }
}


/// <summary>
/// Morpher is responsible for morphing between two meshe.
/// It uses the slider value to determine the percentage of the morphing.
/// </summary>
public class Morpher : MonoBehaviour
{
    const string TEXTUREMAP = "_BaseMap";

    const string RAWTEX = "_RawTex";
    const string COOKEDTEX = "_CookedTex";
    const string BURNTTEX = "_BurntTex";
    const string RAWCOOKEDLERP = "_lerp1";
    const string COOKEDBURNTLERP = "_lerp2";

    public bool IsDeforming = true;

    [Tooltip("Mesh should be read/write enabled from the model import settings")]
    [SerializeField] private Mesh _oldMesh;
    [Tooltip("Mesh should be read/write enabled from the model import settings")]
    [SerializeField] private Mesh _newMesh;
    [SerializeField] private Mesh _newestMesh;

    private MeshFilter _meshFilter;
    private Renderer _renderer;

    [Range(0f, 2f)]
    [SerializeField] private float _slider;


    private Vector3[] _oldVertices;
    private Vector3[] _newVertices;
    private Vector3[] _newestVertices;

    private int[] _oldTriangles;
    private int[] _newTriangles;
    private int[] _newestTriangles;

    private List<Vector3> _finalVertices;

    private Mesh _interpolatedMesh;
    private List<VertexPair> _pairsOfVertices1;
    private List<VertexPair> _pairsOfVertices2;


    private Material _finalMaterial;

    void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _renderer = GetComponent<Renderer>();

        _interpolatedMesh = new Mesh();
        _interpolatedMesh.MarkDynamic();

        if (_meshFilter != null)
        {
            _meshFilter.mesh = _interpolatedMesh;
        }

        _oldVertices = _oldMesh.vertices;
        _newVertices = _newMesh.vertices;
        _newestVertices = _newestMesh.vertices;

        _oldTriangles = _oldMesh.triangles;
        _newTriangles = _newMesh.triangles;
        _newestTriangles = _newestMesh.triangles;

        _finalVertices = new List<Vector3>(_oldVertices);

        CreatePairs1();
        CreatePairs2();

        _finalMaterial = _renderer.material;

        GameManager.Instance.OnStateChanged += OnStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnStateChanged -= OnStateChanged;
    }

    private void OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.state == GameState.Playing) IsDeforming = true;
        else IsDeforming = false;
    }

    /// <summary>
    /// Create pairs of vertices, each pair contains a vertex from the old mesh and a vertex from the new mesh.
    /// Vertices are mapped by the nearest one to each other.
    /// </summary>
    private void CreatePairs1()
    {
        _pairsOfVertices1 = new List<VertexPair>();

        for (int i = 0; i < _oldVertices.Length; i++)
        {
            var oldVertex = _oldVertices[i];

            var nearestToOldVertex = _newVertices.OrderBy(v => Vector3.Distance(v, oldVertex)).FirstOrDefault();

            _pairsOfVertices1.Add(new VertexPair(oldVertex, nearestToOldVertex));
        }
    }


    /// <summary>
    /// Create pairs of vertices, each pair contains a vertex from the new mesh and a vertex from the old mesh.
    /// Vertices are mapped by the nearest one to each other.
    /// </summary>
    private void CreatePairs2()
    {
        _pairsOfVertices2 = new List<VertexPair>();

        for (int i = 0; i < _newVertices.Length; i++)
        {
            var oldVertex = _newVertices[i];

            var nearestToOldVertex = _newestVertices.OrderBy(v => Vector3.Distance(v, oldVertex)).FirstOrDefault();

            _pairsOfVertices2.Add(new VertexPair(oldVertex, nearestToOldVertex));
        }
    }


    void Update()
    {
        if (IsDeforming)
        {
            Deform();
        }
    }

    public void SetSlider(float slider)
    {
        _slider = slider;
    }


    /// <summary>
    /// Deform the mesh based on the slider value.
    /// </summary>
    private void Deform()
    {
        float extendedThreshold = 1 + ((GetComponent<FoodItem>().GetBurnThreshold() - 1) * 0.5f);
        float extendedSlider = (_slider - 1) / (GetComponent<FoodItem>().GetBurnThreshold() - 1);

        if (_slider < 1)
        {
            _finalVertices = _pairsOfVertices1.Select(p => Vector3.Lerp(p.Vertex1, p.Vertex2, _slider)).ToList();
        }
        else if (_slider < extendedThreshold)
        {
            _finalVertices = _pairsOfVertices2.Select(p => Vector3.Lerp(p.Vertex1, p.Vertex2, extendedSlider)).ToList();
        }
        else
        {
            _finalVertices = _newestVertices.ToList();
        }

        _interpolatedMesh.Clear();

        _interpolatedMesh.SetVertices(_finalVertices);
        if (_slider < 1) _interpolatedMesh.triangles = _oldTriangles;
        else if (_slider < extendedThreshold) _interpolatedMesh.triangles = _newTriangles;
        else _interpolatedMesh.triangles = _newestTriangles;


        if (_slider < 1)
        {
            _interpolatedMesh.bounds = _oldMesh.bounds;
            _interpolatedMesh.uv = _oldMesh.uv;
            _interpolatedMesh.uv2 = _oldMesh.uv2;
            _interpolatedMesh.uv3 = _oldMesh.uv3;
        }
        else if (_slider < extendedThreshold)
        {
            _interpolatedMesh.bounds = _newMesh.bounds;
            _interpolatedMesh.uv = _newMesh.uv;
            _interpolatedMesh.uv2 = _newMesh.uv2;
            _interpolatedMesh.uv3 = _newMesh.uv3;
        }
        else
        {
            _interpolatedMesh.bounds = _newestMesh.bounds;
            _interpolatedMesh.uv = _newestMesh.uv;
            _interpolatedMesh.uv2 = _newestMesh.uv2;
            _interpolatedMesh.uv3 = _newestMesh.uv3;
        }

        _interpolatedMesh.RecalculateNormals();

        if (_slider < 1)
        {
            _finalMaterial.SetFloat(RAWCOOKEDLERP, _slider);
        }
        else
        {
            _finalMaterial.SetFloat(COOKEDBURNTLERP, extendedSlider);
        }
    }

}