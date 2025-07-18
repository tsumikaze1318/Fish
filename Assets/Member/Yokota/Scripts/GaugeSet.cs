using FishMovement;
using UnityEngine;
using UnityEngine.UI;

public class GaugeSet : MonoBehaviour
{
    [SerializeField]
    private Image _leftGauge;
    [SerializeField]
    private Image _rightGauge;

    private FishMovementController _controller;

    private const float PERFECT_FILL = 0.5f;
    private const float GREAT_FILL = 0.3f;
    private const float GOOD_FILL = 0.2f;

    private void Start()
    {
        _controller ??= GetComponentInChildren<FishMovementController>();
    }

    public void SetGaugeValue(float value)
    {
        _controller ??= GetComponentInChildren<FishMovementController>();
        float fill = 0;

        switch (value)
        {
            case float v when v < 0.3f:
                fill = PERFECT_FILL;
                break;
            case float v when v < 0.5f:
                fill = GREAT_FILL;
                break;
            case float v when v < 1.0f:
                fill = GOOD_FILL;
                break;
            default:
                // Ž¸”s
                return;
        }

        SetFillAmount(fill);
        _controller.SetFishRange(fill);
        gameObject.SetActive(true);
        return;
    }

    private void SetFillAmount(float fill)
    {
        Vector3 vector = _leftGauge.transform.localPosition;

        switch (fill)
        {
            case PERFECT_FILL:
                vector.x = 350;
                _leftGauge.rectTransform.localPosition = -vector;
                _rightGauge.rectTransform.localPosition = vector;
                break;
            case GREAT_FILL:
                vector.x = 210;
                _leftGauge.rectTransform.localPosition = -vector;
                _rightGauge.rectTransform.localPosition = vector;
                break;
            case GOOD_FILL:
                vector.x = 140;
                _leftGauge.rectTransform.localPosition = -vector;
                _rightGauge.rectTransform.localPosition = vector;
                break;
        }

        _leftGauge.fillAmount = _rightGauge.fillAmount = fill;
    }
}