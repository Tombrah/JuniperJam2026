using UnityEngine;

public class HeatSource : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (GameManager.Instance.state != GameState.Playing) return;

        if (other.gameObject.TryGetComponent<FoodItem>(out FoodItem item))
        {
            item.Cook();
        }
    }
}
