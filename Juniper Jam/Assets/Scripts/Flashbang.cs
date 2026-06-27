using System.Collections;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class Flashbang : MonoBehaviour
{
    private Image img;

    private void Start()
    {
        img = GetComponent<Image>();
        GameManager.Instance.OnFlashbang += FlashbangBoom;
        gameObject.SetActive(false);
    }

    private void FlashbangBoom()
    {
        Debug.Log("turn on");
        gameObject.SetActive(true);
        img.color = Color.white;
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        float percentage = 1;

        while (percentage > 0)
        {
            percentage -= Time.deltaTime / 4;
            img.color = new Color(1, 1, 1, percentage);
            yield return null;
        }
        percentage = 0;
        img.color = new Color(1, 1, 1, percentage);
        gameObject.SetActive(false);
    }
}
