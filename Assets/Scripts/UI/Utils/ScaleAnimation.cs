using UnityEngine;

public class ScaleAnimation : MonoBehaviour
{
    [SerializeField] float duration = 0.5f;
    private void OnEnable()
    {
        LeanTween.scale(this.gameObject, Vector3.one*1.1f, duration).setLoopPingPong();
    }
    private void OnDisable()
    {
        LeanTween.cancel(this.gameObject);
    }
}
