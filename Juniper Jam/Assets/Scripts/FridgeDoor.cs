using System;
using UnityEngine;

public class FridgeDoor : MonoBehaviour
{
    private Animator ani;

    private void Start()
    {
        ani = GetComponent<Animator>();
        GameManager.Instance.OnStartScreen += StartScreen;
    }

    private void StartScreen()
    {
        ani.SetTrigger("Open");
    }
}
