using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Microwave")]
    [SerializeField] private AudioSource microwave_open;
    [SerializeField] private AudioSource microwave_close;
    [SerializeField] private AudioSource microwave_start;
    [SerializeField] private AudioSource microwave_on;

    [Header("Food Sounds")]
    [SerializeField] private AudioSource popcorn;
    [SerializeField] private AudioSource bowlBreak;

    [Header("After Cooking")]
    [SerializeField] private AudioSource[] afterCooking;
    [SerializeField] private AudioSource gameOver;

    private GameManager gm;

    private int index = -1;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gm = GameManager.Instance;
        gm.OnStateChanged += OnStateChanged;
        gm.OnFoodPlaced += OnFoodPlaced;
        gm.OnCooked += Evaluate;
        gm.OnBowlBreak += BowlBreak;
    }

    private void Update()
    {
        if (index != -1)
        {
            if (!afterCooking[index].isPlaying)
            {
                index = -1;
                gm.SetGameState(GameState.Resetting);
            }
        }
    }

    public void SetPitch(float pitch)
    {
        pitch = Mathf.Clamp(pitch, 0.8f, 1.2f);
        microwave_on.pitch = pitch;
    }

    public float GetPitch()
    {
        return microwave_on.pitch;
    }

    private void BowlBreak()
    {
        bowlBreak.Play();
    }

    private void OnFoodPlaced()
    {
        microwave_close.Play();
    }

    private void Evaluate(CookedState state)
    {
        afterCooking[(int)state].Play();
        index = (int)state;
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
