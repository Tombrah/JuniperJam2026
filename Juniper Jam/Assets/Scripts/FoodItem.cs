using System;
using UnityEngine;

public class FoodItem : MonoBehaviour
{
    public CookedState state;

    [SerializeField] private float cookTime = 5;
    [SerializeField] private float burnThreshold = 1.5f;
    [SerializeField] private float percentage;
    [SerializeField] private Mesh brokenMesh;

    public event EventHandler OnStateChanged;
    public event EventHandler OnCooked;
    public event EventHandler OnBurning;

    private Morpher morpher;

    private Food parent;

    private void Start()
    {
        state = CookedState.Raw;
        TryGetComponent<Morpher>(out morpher);
        parent = transform.parent.GetComponent<Food>();
    }

    private void Update()
    {
        if (GameManager.Instance.state == GameState.Playing && parent.GetData().Type == FoodType.Egg && GameManager.Instance.GetSelectedObject() == parent)
        {
            Cook();
        }
    }

    public void Cook()
    {
        if (state == CookedState.Burnt) return;

        percentage += Time.deltaTime / cookTime;
        if (morpher != null)
        {
            if (parent.GetData().Type != FoodType.Popcorn) morpher.SetSlider(percentage);
            else morpher.SetSlider(percentage * 2f);
        }

        TryUpdateState();
    }

    public void TryUpdateState()
    {
        if (percentage > 1 && state == CookedState.Raw)
        {
            state = CookedState.Cooked;
            OnCooked?.Invoke(this, EventArgs.Empty);
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
        else if (percentage > burnThreshold && state != CookedState.Burnt)
        {
            state = CookedState.Burnt;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
            if (parent.GetData().Type == FoodType.Soup)
            {
                GetComponent<Morpher>().IsDeforming = false;
                GetComponent<MeshFilter>().mesh = brokenMesh;
                GetComponent<MeshFilter>().mesh.RecalculateNormals();
                GameManager.Instance.OnBowlBreak?.Invoke();
            }
        }
    }

    public float GetBurnThreshold()
    {
        return burnThreshold;
    }
}

public enum CookedState
{
    Raw,
    Cooked,
    Burnt
}
