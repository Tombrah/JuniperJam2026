using JetBrains.Annotations;
using System;
using UnityEngine;

public class CameraTriggers : MonoBehaviour
{
    const string LOOKATMICROWAVE = "Prep";
    const string LOOKATFRIDGE = "Reset";

    private Animator ani;
    private GameManager gameManager;

    private void Start()
    {
        ani = GetComponent<Animator>();
        gameManager = GameManager.Instance;
        gameManager.OnFoodGrabbed += OnFoodGrabbed;
        gameManager.OnStateChanged += Instance_OnStateChanged;
    }

    private void Instance_OnStateChanged(object sender, EventArgs e)
    {
        if (gameManager.state == GameState.Resetting)
        {
            SetTrigger(LOOKATFRIDGE);
        }
    }

    private void OnFoodGrabbed()
    {
        if (gameManager.state == GameState.Preperation)
        {
            SetTrigger(LOOKATMICROWAVE);
        }
    }

    public void StartSelection()
    {
        gameManager.SetGameState(GameState.Selection);
        gameManager.OnAtFridge?.Invoke();
    }

    public void AtMicrowave()
    {
        gameManager.OnAtMicrowave?.Invoke();
    }

    public void SetTrigger(string name)
    {
        ani.SetTrigger(name);
    }
}
