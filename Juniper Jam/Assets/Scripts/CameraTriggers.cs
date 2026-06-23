using UnityEngine;

public class CameraTriggers : MonoBehaviour
{
    private Animator ani;

    private void Start()
    {
        ani = GetComponent<Animator>();
        GameManager.Instance.OnStateChanged += GameStateChanged;
    }

    private void GameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.state == GameState.Preperation)
        {
            ani.SetTrigger("Prep");
        }
        else if(GameManager.Instance.state == GameState.GameFinished)
        {
            ani.SetTrigger("Reset");
        }
    }

    public void StartGame()
    {
        GameManager.Instance.SetGameState(GameState.Playing);
    }

    public void StartSelection()
    {
        GameManager.Instance.SetGameState(GameState.Selection);
    }

    public void PlaceFood()
    {
        GameManager.Instance.GetSelectedObject().TweenMove(PlateSpinner.Instance.location.position, 0.4f);
    }
}
