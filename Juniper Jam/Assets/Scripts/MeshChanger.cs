using UnityEngine;

[RequireComponent (typeof(FoodItem))]
public class MeshChanger : MonoBehaviour
{
    [SerializeField] private Mesh[] meshes;
    private MeshFilter filter;
    private FoodItem foodItem;

    private void Start()
    {
        filter = GetComponent<MeshFilter>();
        foodItem = GetComponent<FoodItem>();
        foodItem.OnStateChanged += UpdateMesh;
    }

    private void UpdateMesh(object sender, System.EventArgs e)
    {
        if (meshes.Length != 3) return;
        filter.mesh = meshes[(int)foodItem.state];
    }
}
