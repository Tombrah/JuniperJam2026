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
        GameManager.Instance.OnFoodGrabbed += OnFoodGrabbed;
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
