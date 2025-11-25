using UnityEngine;

public class PanelLean : MonoBehaviour
{

    [SerializeField] GameObject mainObject;
    [SerializeField] Transform initialPos, finalPos;
    private void OnEnable()
    {
        mainObject.transform.position = initialPos.position;
        LeanTween.moveLocal(mainObject, finalPos.position, 0.5f).setEaseInOutExpo();
    }
    private void OnDisable()
    {
        LeanTween.cancel(this.gameObject);
    }
}
