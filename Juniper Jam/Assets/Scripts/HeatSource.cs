using UnityEngine;

public class HeatSource : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<FoodItem>(out FoodItem item))
        {
            item.Cook();
        }
    }
}
