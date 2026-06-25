using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip microwave_open;
    [SerializeField] private AudioClip microwave_close;

    private GameManager gm; 

    private void Start()
    {
        gm = GameManager.Instance;
        gm.OnStateChanged += OnStateChanged;
        gm.OnFoodPlaced += OnFoodPlaced;
    }

    private void OnFoodPlaced()
    {
        source.clip = microwave_close;
        source.Play();
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
                break;
            case GameState.CookFinished:
                source.clip = microwave_open;
                source.Play();
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
