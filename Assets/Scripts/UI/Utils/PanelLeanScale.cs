using UnityEngine;
using UnityEngine.UI;

public class PanelLeanScale : MonoBehaviour
{
    [SerializeField] GameObject childPanelComponents;
    [SerializeField] Button collectButton;
    private void OnEnable()
    {
        childPanelComponents.transform.localScale = Vector3.zero;
        collectButton.interactable = false;
        LeanTween.scale(childPanelComponents, Vector3.one, 0.35f).setEaseInOutBounce().setOnComplete(() =>
        {
            collectButton.interactable = true;
        });
    }
    private void OnDisable()
    {
        LeanTween.cancel(this.gameObject);
    }
}
