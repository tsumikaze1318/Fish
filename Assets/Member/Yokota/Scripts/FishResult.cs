using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class FishResult : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _splashParticle;
    [SerializeField]
    private ParticleSystem _haloParticle;
    private Image _overlapImage;
    [SerializeField]
    private FishDictionary _fishDictionary;
    private CancellationTokenSource _cts;

    private GameObject _fish;

    private void Start()
    {
        _overlapImage = GetComponentInChildren<Image>();
        _overlapImage.enabled = false;
    }

    public async void DisplayishResult()
    {
        _splashParticle.Play();
        await UniTask.Delay(TimeSpan.FromSeconds(_splashParticle.main.duration));

        _cts = new CancellationTokenSource();
        CancellationToken ct = _cts.Token;

        try { ct.ThrowIfCancellationRequested(); }
        catch { return; }

        GameManager.Instance.SetGameState(GameState.Show);

        _fish = Instantiate(_fishDictionary.GetRandomFish(), new Vector3(0, 3, -1), Quaternion.Euler(0, -90, -90), transform);
        _overlapImage.enabled = true;
        _haloParticle.Play();

        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(3), cancellationToken: ct); 
        }
        catch 
        {
            HideFishResult();
            return;
        }

        HideFishResult();
    }

    private void HideFishResult()
    {
        _haloParticle.Clear();
        _haloParticle.Pause();
        _overlapImage.enabled = false;
        Destroy(_fish);
        GameManager.Instance.StartFishMoving();
    }

    public void CancelDisplayFishResult()
    {
        _cts.Cancel();
    }
}
