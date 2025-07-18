using Cysharp.Threading.Tasks;
using PrimeTween;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TimingCatcher))]
public class FishModelAnimation : MonoBehaviour
{
    private const float EDGE = 15f;
    private const float STAY = 4f;
    private const float CENTER = 3f;

    private const float MAX_FEINT = 3;
    private float _feintCount = 0;

    private bool _isBiting = false;

    private TimingCatcher _timingCatcher;
    [SerializeField]
    private Image _exclamationMark;
    [SerializeField]
    private UkiAnimation _ukiAnimation;

    private CancellationTokenSource _cancellationTokenSource;

    private void Start()
    {
        _timingCatcher = GetComponent<TimingCatcher>();
        
        StartAnimationAsync();
    }

    private void Update()
    {
        if (GameManager.Instance.State == GameState.Stay)
        {
            if (Input.GetMouseButtonDown(0) && !_isBiting)
            {
                _cancellationTokenSource?.Cancel();
            }
        }
    }

    private async UniTask FeintAsync(CancellationToken token)
    {
        try { token.ThrowIfCancellationRequested(); }
        catch { return; }

        // 食いつきのフェイント
        await Tween.PositionX(transform, -CENTER, 0.3f, Ease.Linear)
            .OnUpdate(target: transform, (target, tween) =>
            {
                try { token.ThrowIfCancellationRequested(); }
                catch
                {
                    tween.Stop();
                    EscapeAsync();
                    return;
                }
            });

        // 食いつくか判定
        if (UnityEngine.Random.Range(0f, 1f) < _feintCount / MAX_FEINT)
        {
            _exclamationMark.enabled = true;
            SeManager.Instance.PlaySE(2);
            _ukiAnimation.DoAnimation(true);
            float time = await _timingCatcher.MeasureTime();
            Debug.Log(time);
            _exclamationMark.enabled = false;
            // 時間切れだった場合
            if (time > 1.0f)
            {
                // 逃げて終了
                EscapeAsync();
                _ukiAnimation.DoAnimation(false);
                return;
            }
            
            SeManager.Instance.PlaySE(3);
            GameManager.Instance.StartFishing(time);
            return;
        }

        SeManager.Instance.PlaySE(1);
        _ukiAnimation.DoAnimation(false);

        try { token.ThrowIfCancellationRequested(); }
        catch { return; }

        // カウントアップ
        _feintCount++;
        // 元の位置に戻る
        await Tween.PositionX(transform, -STAY, 1f)
            .OnUpdate(target: transform, (target, tween) =>
            {
                try { token.ThrowIfCancellationRequested(); }
                catch 
                {
                    tween.Stop();
                    EscapeAsync();
                    return;
                }
            });
        // もう一度
        await FeintAsync(token);
    }

    private async void EscapeAsync()
    {
        // ゲームの状態をNoneに戻す
        GameManager.Instance.SetGameState(GameState.None);
        // 魚を逆向きにする
        transform.Rotate(new Vector3(0, 180, 0));
        // 端まで逃げる
        await Tween.PositionX(transform, -EDGE, 0.3f, Ease.Linear);
        // 時間を置く
        await UniTask.Delay(TimeSpan.FromSeconds(5));
        ResetPosition();
        // また魚を生成する
        StartAnimationAsync();
    }

    public void ResetPosition()
    {
        _feintCount = 0;
        transform.rotation = Quaternion.Euler(0, 90, 0);
        transform.position = new Vector3(-EDGE, -3, 0);
    }

    public async void StartAnimationAsync()
    {
        await Tween.PositionX(transform, -STAY, 5f);
        GameManager.Instance.StartFishFeint();
        _cancellationTokenSource = new CancellationTokenSource();
        var ct = _cancellationTokenSource.Token;
        await FeintAsync(ct);
    }
}
