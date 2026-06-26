using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState state;
    [Header("Food Datas")]
    [SerializeField] private FoodData[] datas;
    [Header("Scipts")]
    public Microwave microwave;
    public CameraTriggers cameraTriggers;
    [Header("Transforms")]
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

    public Dictionary<FoodData, bool> successfulCook;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        state = GameState.Selection;
        successfulCook = new Dictionary<FoodData, bool>();
        foreach (FoodData data in datas)
        {
            successfulCook.Add(data, false);
        }
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
            if (Input.GetMouseButtonDown(0))
            {
                SetGameState(GameState.CookFinished);
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

        if (state == GameState.Resetting)
        {
            bool isComplete = true;
            foreach (FoodData data in datas)
            {
                if (successfulCook[data] == false)
                {
                    isComplete = false;
                    break;
                }
            }

            if (isComplete)
            {
                //GAME OVER YOU WIN!
                state = GameState.GameOver;
            }
        }

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

    public void UpdateCookedState(FoodData food)
    {
        successfulCook[food] = true;
    }
}

public enum GameState
{
    Selection,
    Preperation,
    Playing,
    CookFinished,
    Resetting,
    GameOver
}
