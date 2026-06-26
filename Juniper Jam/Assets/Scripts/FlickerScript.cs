using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class FlickerScript : MonoBehaviour
{
    public float minFlickerSpeed = 0.01f;
    public float maxFlickerSpeed = 0.15f;

    private Light lightSource;

    private bool isFlickering = false;

    private void Start()
    {
        lightSource = GetComponent<Light>();
    }

    private void Update()
    {
        if (isFlickering == false && GameManager.Instance.state == GameState.Selection)
        {
            StartCoroutine(FlickeringLight());
        }
    }

    IEnumerator FlickeringLight()
    {
        isFlickering = true;
        float timeDelay;
        lightSource.enabled = false;
        timeDelay = Random.Range(minFlickerSpeed, maxFlickerSpeed);
        yield return new WaitForSeconds(timeDelay);
        lightSource.enabled = true;
        timeDelay = Random.Range(minFlickerSpeed, maxFlickerSpeed);
        yield return new WaitForSeconds(timeDelay);
        isFlickering = false;
    }
}
