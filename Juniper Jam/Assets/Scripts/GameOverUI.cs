using System;
using UnityEngine;
using DG.Tweening;

public class GameOverUI : MonoBehaviour
{

    private void Start()
    {
        GameManager.Instance.OnEndScreen += EndScreen;
    }

    private void EndScreen()
    {
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.DOLocalMoveY(0, 1f);
    }
}
