using System.Collections;
using UnityEditor;
using UnityEngine;

public class HeatElement : MonoBehaviour
{
    const string EMISSION = "_EMISSION";

    [SerializeField] private Light l;
    [SerializeField] Renderer rend;

    private void Start()
    {
        GameManager.Instance.OnStateChanged += OnStateChanged;
        rend.material.DisableKeyword(EMISSION);
        l.enabled = false;
    }

    private void OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.state != GameState.Playing)
        {
            rend.material.DisableKeyword(EMISSION);
            l.enabled = false;
        }
        else
        {
            rend.material.EnableKeyword(EMISSION);
            l.enabled = true;
        }

    }
}
