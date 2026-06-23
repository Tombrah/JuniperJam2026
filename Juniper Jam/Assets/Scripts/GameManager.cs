using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState state;

    public event EventHandler OnStateChanged;

    private Food selectedObject;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (state == GameState.Selection)
        {
            if (selectedObject != null && Input.GetMouseButtonDown(0))
            {
                SetGameState(GameState.Preperation);
            }
        }
        else if (state == GameState.Playing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                selectedObject = null;
                SetGameState(GameState.GameFinished);
            }
        }
    }

    private void Start()
    {
        state = GameState.Selection;
    }

    public void SetGameState(GameState newState)
    {
        state = newState;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedObject(Food food)
    {
        selectedObject = food;
    }

    public Food GetSelectedObject()
    {
        return selectedObject;
    }
}

public enum GameState
{
    Selection,
    Preperation,
    Playing,
    GameFinished,
    Resetting
}
