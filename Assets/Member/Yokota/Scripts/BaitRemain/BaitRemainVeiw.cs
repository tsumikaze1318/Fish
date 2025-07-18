using TMPro;
using UnityEngine;

public class BaitRemainVeiw : MonoBehaviour
{
    private TMP_Text _remainText;

    private void Awake()
    {
        _remainText = GetComponent<TMP_Text>();
    }

    public void UpdateRemainText(int remain)
    {
        _remainText.text = remain.ToString();
    }
}
