using UnityEngine;

public class TextureChanger : MonoBehaviour
{
    [SerializeField] private Material[] materials;
    private MeshRenderer meshRenderer;
    private FoodItem foodItem;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        foodItem = GetComponent<FoodItem>();
        foodItem.OnStateChanged += UpdateMesh;
    }

    private void UpdateMesh(object sender, System.EventArgs e)
    {
        if (materials.Length != 3) return;
        meshRenderer.sharedMaterial = materials[(int)foodItem.state];
    }
}
