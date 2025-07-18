using TMPro;
using UnityEngine;

public class CatchView : MonoBehaviour
{
    private TMP_Text _cacthText;

    private void Awake()
    {
        _cacthText = GetComponent<TMP_Text>();
    }

    public void UpdateCatch(int value)
    {
        _cacthText.text = value.ToString();
    }
}
