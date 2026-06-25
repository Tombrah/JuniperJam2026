using System.Data.SqlTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodDescriptionUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        GameManager.Instance.OnShowDescription += Show;
        GameManager.Instance.OnHideDescription += Hide;
        Hide();
    }

    private void Show(FoodData data) 
    {
        if (data.Icon != null) image.sprite = data.Icon;
        if (data.Description != null) text.text = data.Description;
        gameObject.SetActive(true); 
    }

    private void Hide() { gameObject.SetActive(false); }
}
