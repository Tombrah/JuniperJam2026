using UnityEngine;
using DG.Tweening;

public class Food : MonoBehaviour
{
    private const string MOVEID = "moveid";

    [SerializeField] private Vector3 fridgePos;

    private GameManager gameManager;

    private void Start()
    {
        fridgePos = transform.position;
        gameManager = GameManager.Instance;
        gameManager.OnStateChanged += GameStateChanged;
        gameManager.OnAtMicrowave += Food_OnAtMicrowave;
    }

    private void Food_OnAtMicrowave()
    {
        if (gameManager.GetSelectedObject() != this) return;

        transform.DOMove(gameManager.foodMicrowaveLocation.position, 0.4f).OnComplete(() =>
        {
            gameManager.OnFoodPlaced?.Invoke();
        });
    }

    private void GameStateChanged(object sender, System.EventArgs e)
    {
        switch (gameManager.state)
        {
            case GameState.Selection:
                break;
            case GameState.Preperation:
                DOTween.Kill(MOVEID);
                if (gameManager.GetSelectedObject() == this)
                {
                    Destroy(GetComponent<Collider>());
                    transform.parent = Camera.main.transform;
                    transform.DOMove(gameManager.foodCameraLocation.position, 0.2f).OnComplete(() =>
                        {
                            gameManager.OnFoodGrabbed?.Invoke();
                        });
                }
                break;
            case GameState.Playing:
                if (gameManager.GetSelectedObject() == this)
                {
                    transform.parent = gameManager.plateSpinner;
                    transform.DORotate(Vector3.zero, 0.1f).SetEase(Ease.InOutQuad);
                }
                break;
            case GameState.GameFinished:
                break;
            default:
                break;
        }
    }

    private void OnMouseEnter()
    {
        Debug.Log("enter");
        if (gameManager.state != GameState.Selection) return;

        gameManager.SetSelectedObject(this);
        TweenMove(new Vector3(fridgePos.x, fridgePos.y + 0.2f, fridgePos.z), 0.5f);
    }

    private void OnMouseExit()
    {
        if (gameManager.state != GameState.Selection) return;

        gameManager.SetSelectedObject(null);
        TweenMove(fridgePos, 0.5f);
    }

    public void TweenMove(Vector3 pos, float time, Ease ease = Ease.InOutQuad)
    {
        transform.DOMove(pos, time).SetId(MOVEID).SetEase(ease);
    }
}
