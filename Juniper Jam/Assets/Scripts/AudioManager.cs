using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Microwave")]
    [SerializeField] private AudioSource microwave_open;
    [SerializeField] private AudioSource microwave_close;
    [SerializeField] private AudioSource microwave_start;
    [SerializeField] private AudioSource microwave_on;

    [Header("Popcorn")]
    [SerializeField] private AudioSource popcorn;

    [Header("After Cooking")]
    [SerializeField] private AudioSource[] afterCooking;

    private GameManager gm; 

    private void Start()
    {
        gm = GameManager.Instance;
        gm.OnStateChanged += OnStateChanged;
        gm.OnFoodPlaced += OnFoodPlaced;
        gm.OnCooked += Evaluate;
    }

    private void OnFoodPlaced()
    {
        microwave_close.Play();
    }

    private void Evaluate(CookedState state)
    {
        afterCooking[(int)state].Play();
    }

    private void OnStateChanged(object sender, System.EventArgs e)
    {
        switch (gm.state)
        {
            case GameState.Selection:
                break;
            case GameState.Preperation:
                break;
            case GameState.Playing:
                microwave_start.Play();
                microwave_on.Play();
                if (GameManager.Instance.GetSelectedObject().GetData().Type == FoodType.Popcorn) popcorn.Play();
                break;
            case GameState.CookFinished:
                microwave_on.Stop();
                if (GameManager.Instance.GetSelectedObject().GetData().Type == FoodType.Popcorn) popcorn.Stop();
                microwave_open.Play();
                break;
            case GameState.Resetting:
                break;
            case GameState.GameOver:
                break;
            default:
                break;
        }
    }
}
