using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float _timer = 120;
    private TMP_Text _timerText;

    private void Start()
    {
        _timerText = GetComponent<TMP_Text>();
        _timerText.text = _timer.ToString("0");
    }

    private void Update()
    {
        if (GameManager.Instance.State == GameState.Fish)
        {
            _timer -= Time.deltaTime;
            _timerText.text = _timer.ToString("0");
        }

        if (_timer < 0)
        {
            _timer = 0;
            _timerText.text = _timer.ToString("0");
            enabled = false;
        }
    }

    public float GetTime()
    {
        return _timer;
    }
}
