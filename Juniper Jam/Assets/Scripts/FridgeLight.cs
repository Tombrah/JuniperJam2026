using System;
using System.Collections;
using UnityEngine;

public class FridgeLight : MonoBehaviour
{
    private Light fridgeLight;

    private void Start()
    {
        fridgeLight = GetComponent<Light>();
        GameManager.Instance.OnStartScreen += StartScreen;
        fridgeLight.enabled = false;
    }

    private void StartScreen()
    {
        StartCoroutine(TurnOnLightAfterNSeconds(0.7f));
    }

    private IEnumerator TurnOnLightAfterNSeconds(float n)
    {
        yield return new WaitForSeconds(n);

        fridgeLight.enabled = true;
    }
}
