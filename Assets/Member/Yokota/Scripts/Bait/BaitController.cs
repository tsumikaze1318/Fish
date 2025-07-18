using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class BaitController : MonoBehaviour
{
    private ParticleSystem _baitParticle;
    private bool _isBait = false;
    private int _baitCount = 0;

    [SerializeField]
    private BaitView _view;
    [SerializeField]
    private BaitRemainController _remainController;

    private void Start()
    {
        _baitParticle = GetComponentInChildren<ParticleSystem>();
    }

    public async void Bait()
    {
        // Å‘å‚Ü‚ÅT‚«‰a‚ğ‚µ‚Ä‚¢‚é
        if (_baitCount >= 5) return;
        // è‚¿‚ÌT‚«‰a‚ª‚È‚­‚È‚Á‚Ä‚¢‚é
        if (_remainController.GetBaitRemain() <= 0) return;
        // ¡ƒGƒT‚ğ‚â‚Á‚Ä‚¢‚é
        if (_isBait) return;

        _isBait = true;

        _baitCount++;
        _view.UpdateText(_baitCount);
        _remainController.FluctuationBaitRemain(-1);

        SeManager.Instance.PlaySE(0);
        _baitParticle.Play();
        await UniTask.Delay(TimeSpan.FromSeconds(_baitParticle.main.duration));

        _isBait = false;
    }

    public void ResetBait()
    {
        _baitCount = 0;
        _view.UpdateText(_baitCount);
    }
}
