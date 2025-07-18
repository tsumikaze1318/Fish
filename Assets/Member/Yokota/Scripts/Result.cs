using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using System;

public class Result : MonoBehaviour
{
    private TMP_Text _resultText;
    private TMP_Text[] _texts;

    private void Start()
    {
        _resultText = GetComponent<TMP_Text>();
        _texts = transform.parent.GetComponentsInChildren<TMP_Text>();
        foreach (var text in _texts)
        {
            text.enabled = false;
        }

        transform.parent.gameObject.SetActive(false);
    }

    public async void SetResult(int result)
    {
        transform.parent.gameObject.SetActive(true);
        _resultText.text = $"íﬁâ Å@{result} Ç–Ç´";
        foreach (var text in _texts)
        {
            text.enabled = true;
            SeManager.Instance.PlaySE(5);
            await UniTask.Delay(TimeSpan.FromSeconds(1));
        }
    }
}
