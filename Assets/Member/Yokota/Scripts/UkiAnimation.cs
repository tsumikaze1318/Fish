using PrimeTween;
using UnityEngine;

public class UkiAnimation : MonoBehaviour
{
    private const float ANIMATION_TIME = 0.2f;

    public async void DoAnimation(bool isBite)
    {
        await Tween.PositionY(transform, -0.45f, ANIMATION_TIME);
        if (isBite) return;
        await Tween.PositionY(transform, -0.2f, ANIMATION_TIME);
    }

    public void FishAnimation()
    {
        SeManager.Instance.PlaySE(3);
        Tween.PositionY(transform, 10f, ANIMATION_TIME);
    }

    public void ReleaseAnimation()
    {
        SeManager.Instance.PlaySE(4);
        Tween.PositionY(transform, -0.2f, ANIMATION_TIME * 5);
    }
}
