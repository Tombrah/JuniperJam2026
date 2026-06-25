using UnityEngine;
using UnityEngine.InputSystem;

public class PlateSpinner : MonoBehaviour
{
    [Header("Base Spin")]
    [SerializeField] private float baseSpeed = 35f;

    [Header("Forward")]
    [SerializeField] private float forwardBoost = 15f;
    [SerializeField] private float maxForwardSpeed = 100f;

    [Header("Reverse")]
    [SerializeField] private float reverseBoost = 8f;
    [SerializeField] private float maxReverseSpeed = -25f;

    [Header("Return")]
    [SerializeField] private float returnRate = 50f;

    private float currentSpeed;

    private void Start()
    {
        currentSpeed = 0;
        GameManager.Instance.OnStateChanged += GameStateChanged;
    }

    private void GameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.state == GameState.Playing) currentSpeed = baseSpeed;
        else currentSpeed = 0;
    }

    private void Update()
    {
        if (GameManager.Instance.state != GameState.Playing) return;

        float scroll = 0f;

        if (Mouse.current != null)
            scroll = Mouse.current.scroll.ReadValue().y;

        if (scroll > 0f)
        {
            currentSpeed += forwardBoost;
        }
        else if (scroll < 0f)
        {
            currentSpeed -= reverseBoost;
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, baseSpeed, returnRate * Time.deltaTime);
        }

        currentSpeed = Mathf.Clamp(currentSpeed, maxReverseSpeed, maxForwardSpeed);

        transform.Rotate(Vector3.forward * currentSpeed * Time.deltaTime, Space.Self);
    }
}