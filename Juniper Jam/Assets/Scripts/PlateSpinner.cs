using UnityEngine;

public class PlateSpinner : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        transform.Rotate(transform.up * speed * Time.deltaTime);
    }
}
