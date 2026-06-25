using System;
using UnityEngine;

public class FoodItem : MonoBehaviour
{
    public CookedState state;

    [SerializeField] private float cookTime = 5;
    [SerializeField] private float burnThreshold = 1.5f;
    [SerializeField] private float percentage;

    public event EventHandler OnStateChanged;

    private Morpher morpher;

    private void Start()
    {
        state = CookedState.Raw;
        TryGetComponent<Morpher>(out morpher);
    }

    public void Cook()
    {
        if (state == CookedState.Burnt) return;

        percentage += Time.deltaTime / cookTime;
        if (morpher != null) morpher.SetSlider(percentage);

        TryUpdateState();
    }

    public void TryUpdateState()
    {
        if (percentage > 1 && state == CookedState.Raw)
        {
            state = CookedState.Cooked;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
        else if (percentage > burnThreshold && state != CookedState.Burnt)
        {
            state = CookedState.Burnt;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

public enum CookedState
{
    Raw,
    Cooked,
    Burnt
}
