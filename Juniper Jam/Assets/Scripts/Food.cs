using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.Networking;

public class Food : MonoBehaviour
{
    private const string MOVEID = "moveid";

    [SerializeField] private FoodData data;
    private Vector3 fridgePos;
    private Quaternion rotation;

    private struct Col
    {
        public Vector3 centre;
        public Vector3 size;
    }

    private Col startCol;

    private GameManager gameManager;


    private void Start()
    {
        fridgePos = transform.position;
        rotation = transform.rotation;

        gameManager = GameManager.Instance;
        gameManager.OnStateChanged += GameStateChanged;
        gameManager.OnAtMicrowave += Food_OnAtMicrowave;
        gameManager.OnStartEvaluation += Food_OnStartEvaluation;

        startCol.centre = GetComponent<BoxCollider>().center;
        startCol.size = GetComponent<BoxCollider>().size;
    }

    private void OnDestroy()
    {
        gameManager.OnStateChanged -= GameStateChanged;
        gameManager.OnAtMicrowave -= Food_OnAtMicrowave;
        gameManager.OnStartEvaluation -= Food_OnStartEvaluation;
    }

    private void Food_OnStartEvaluation()
    {
        if (gameManager.GetSelectedObject() != this) return;

        bool isCooked = true;
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<FoodItem>(out FoodItem item))
            {
                if (item.state != CookedState.Cooked)
                {
                    isCooked = false;
                    break;
                }
            }
        }
        if (isCooked) gameManager.UpdateCookedState(data);

        transform.DOMove(gameManager.foodCameraLocation.position, 0.4f).OnComplete(() =>
        {
            BoxCollider col = transform.AddComponent<BoxCollider>();
            col.center = startCol.centre;
            col.size = startCol.size;

            Instantiate(data.Prefab, fridgePos, rotation);

            Rigidbody rb = transform.AddComponent<Rigidbody>();
            rb.AddForce((gameManager.cameraTriggers.transform.position - transform.position).normalized * 5, ForceMode.Impulse);
            gameManager.SetGameState(GameState.Resetting);
            Destroy(this.gameObject, 5f);
        });
    }

    private void Food_OnAtMicrowave()
    {
        if (gameManager.GetSelectedObject() != this) return;

        transform.DOMove(gameManager.foodMicrowaveLocation.position, 0.4f).OnComplete(() =>
        {
            transform.DOLookAt(transform.position + Vector3.forward, 0.5f);
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
                    transform.parent = gameManager.foodCameraLocation.transform;
                    transform.DOMove(gameManager.foodCameraLocation.position, 0.2f).SetEase(Ease.InOutQuad).OnComplete(() =>
                        {
                            gameManager.OnFoodGrabbed?.Invoke();
                        });
                }
                break;
            case GameState.Playing:
                if (gameManager.GetSelectedObject() == this)
                {
                    //transform.LookAt(transform.position + Vector3.forward, Vector3.up);
                    transform.parent = gameManager.plateSpinner;
                }
                break;
            case GameState.CookFinished:
                break;
            default:
                break;
        }
    }

    public FoodData GetData()
    {
        return data;
    }

    private void OnMouseEnter()
    {
        Debug.Log("enter");
        if (gameManager.state != GameState.Selection) return;

        gameManager.SetSelectedObject(this);
        transform.DOMove(new Vector3(fridgePos.x, fridgePos.y + 0.2f, fridgePos.z), 0.5f).SetId(MOVEID).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            gameManager.OnShowDescription?.Invoke(data);
        });
    }

    private void OnMouseExit()
    {
        if (gameManager.state != GameState.Selection) return;

        DOTween.Kill(MOVEID);
        gameManager.SetSelectedObject(null);
        gameManager.OnHideDescription?.Invoke();
        transform.DOMove(fridgePos, 0.5f).SetId(MOVEID).SetEase(Ease.InOutQuad);
    }
}
