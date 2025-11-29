using UnityEngine;

public class LeanAnimation : MonoBehaviour
{
    public AnimationType leanAnimationType;
    [SerializeField] Transform initialPos, finalPos;

    [SerializeField] bool playScaleAnimationLoopAfterSlide;

    [Range(1, 2)]
    [SerializeField] float scaleAnimationFactor;
    private void OnEnable()
    {
        switch (leanAnimationType)
        {
            case AnimationType.UptoDownSlide:
                UpDownAnimation();
                break;

            case AnimationType.LeftRightSlider:
                LeftRightAnimation();
                break;

            case AnimationType.ScaleAnimation:
                ScaleLoopAnimation();
                break;

            default:
                break;
        }

    }

    void LeftRightAnimation()
    {
        this.gameObject.transform.position = initialPos.position;
        LeanTween.moveX(this.gameObject, finalPos.position.x, 0.5f).setEaseInOutExpo().setOnComplete(() =>
        {
            if (playScaleAnimationLoopAfterSlide)
            {
                ScaleLoopAnimation();
            }
        });
    }

    void ScaleLoopAnimation()
    {
        LeanTween.scale(this.gameObject, Vector3.one * scaleAnimationFactor, 4f).setLoopPingPong();
    }
    void UpDownAnimation()
    {
        this.gameObject.transform.position = initialPos.position;
        LeanTween.moveY(this.gameObject, finalPos.position.y, 0.5f).setEaseInOutExpo().setOnComplete(() =>
        {
            if (playScaleAnimationLoopAfterSlide)
            {
                ScaleLoopAnimation();
            }
        });
    }

    private void OnDisable()
    {
        LeanTween.cancel(this.gameObject);
    }
}

public enum AnimationType
{
    LeftRightSlider,
    UptoDownSlide,
    ScaleAnimation
}
