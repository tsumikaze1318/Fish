using PrimeTween;
using TMPro;
using UnityEngine;

public class FluctuationEffect : MonoBehaviour
{
    private TMP_Text _text;
    private Vector3 _position;

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
        _position = transform.position;
    }

    public void AnimationUi(int value)
    {
        _text.text = value.ToString();
        if (value > 0)
        {
            _text.color = Color.green;
            Tween.LocalPositionY(transform, _position.y, _position.y + 100);
        }
        if (value < 0)
        {
            _text.color = Color.red;
            Tween.LocalPositionY(transform, _position.y, _position.y - 100);
        }
    }
}
