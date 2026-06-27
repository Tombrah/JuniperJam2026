using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        startButton.onClick.AddListener(() =>
        {
            GameManager.Instance.OnStartScreen?.Invoke();
        });

        quitButton.onClick.AddListener(() =>
        {
            Debug.Log("Quit");
#if UNITY_WEBGL && !UNITY_EDITOR
            Application.OpenURL("https://tombrah.itch.io/lunch-break");
            
#endif
        });
    }
}
