using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState state;

    public Microwave microwave;
    public CameraTriggers cameraTriggers;
    public Transform plateSpinner;
    public Transform foodMicrowaveLocation;
    public Transform foodCameraLocation;

    //EVENTS
    public event EventHandler OnStateChanged;
    public Action OnAtMicrowave;
    public Action OnAtFridge;
    public Action OnFoodPlaced;
    public Action OnFoodGrabbed;
    public Action OnStartEvaluation;

    public Action<FoodData> OnShowDescription;
    public Action OnHideDescription;

    private Food selectedObject;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        state = GameState.Selection;
    }

    private void Update()
    {
        if (state == GameState.Selection)
        {
            if (selectedObject != null && Input.GetMouseButtonDown(0))
            {
                SetGameState(GameState.Preperation);
                OnHideDescription?.Invoke();
            }
        }
        else if (state == GameState.Playing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SetGameState(GameState.GameFinished);
            }
        }
        else if (state == GameState.Resetting)
        {
            if (selectedObject != null) selectedObject = null;
        }
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
