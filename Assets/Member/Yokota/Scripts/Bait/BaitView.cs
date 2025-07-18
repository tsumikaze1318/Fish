using TMPro;
using UnityEngine;

public class BaitView : MonoBehaviour
{
    private TMP_Text _baitCountText;

    private void Start()
    {
        _baitCountText = GetComponent<TMP_Text>();
    }

    public void UpdateText(int count)
    {
        if (count >= 5) 
        {
            _baitCountText.text = "MAX";
            return;
        }
        _baitCountText.text = $"x {count}";
    }
}
