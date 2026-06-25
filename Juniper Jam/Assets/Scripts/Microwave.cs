using UnityEngine;

public class Microwave : MonoBehaviour
{
    private const string OPEN = "Open";
    private const string CLOSE = "Close";

    private Animator ani;

    private void Start()
    {
        ani = GetComponent<Animator>();
        GameManager.Instance.OnFoodPlaced += OnFoodPlaced;
        GameManager.Instance.OnStateChanged += OnStateChanged;
    }

    private void OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.state == GameState.GameFinished)
        {
            ani.SetTrigger(OPEN);
        }
    }

    private void OnFoodPlaced()
    {
        ani.SetTrigger(CLOSE);
    }

    public void StartGame()
    {
        GameManager.Instance.SetGameState(GameState.Playing);
    }

    public void BeginEvaluation()
    {
        GameManager.Instance.OnStartEvaluation?.Invoke();
    }
}
