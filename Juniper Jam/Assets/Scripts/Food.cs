using UnityEngine;
using DG.Tweening;

public class Food : MonoBehaviour
{
    const string MOVEID = "moveid";

    [SerializeField] private Vector3 fridgePos;

    private GameManager gameManager;

    private void Start()
    {
        fridgePos = transform.position;
        gameManager = GameManager.Instance;
        gameManager.OnStateChanged += GameStateChanged;
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
                }
                break;
            case GameState.Playing:
                if (gameManager.GetSelectedObject() == this)
                {
                    transform.parent = PlateSpinner.Instance.transform;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
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
        TweenMove(new Vector3(fridgePos.x, fridgePos.y + 0.3f, fridgePos.z), 0.5f);
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
