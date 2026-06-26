using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    [SerializeField] private Button startButton;

    private void Start()
    {
        startButton.onClick.AddListener(() =>
        {
            GameManager.Instance.OnStartScreen?.Invoke();
        });
    }
}
