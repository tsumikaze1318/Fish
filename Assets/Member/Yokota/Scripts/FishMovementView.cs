using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace FishMovement
{
    public class FishMovementPresenter
    {
        private FishMovementModel _model;
        private FishMovementView _view;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public FishMovementPresenter(FishMovementModel model, FishMovementView view)
        {
            _model = model;
            _view = view;

            _view.IsMoving.Subscribe(x =>
            {
                if (!x)
                {
                    var ct = _cancellationTokenSource.Token;
                    _model.SetMoveTarget(ct);
                }
            });
            _model.MoveTargetProperty.Subscribe(x =>
            {
                _view.MoveFish(x);
            });
        }

        public void CancelFishMoving()
        {
            _cancellationTokenSource?.Cancel();
            _view.FishUp();
        }

        public float GetFishPositionX()
        {
            return _model.MoveTargetProperty.Value;
        }
    }
}

namespace FishMovement
{
    public class FishMovementView : MonoBehaviour
    {
        private Image _fishImage;
        private ReactiveProperty<bool> _isMoving = new ReactiveProperty<bool>();
        private float _preTarget = 0;
        public IReadOnlyReactiveProperty<bool> IsMoving => _isMoving;

        private void Start()
        {
            _fishImage = GetComponent<Image>();
        }

        public void MoveFish(float target)
        {
            _fishImage ??= GetComponent<Image>();

            _isMoving.Value = true;

            Vector3 vector3 = Vector3.one;
            vector3.x
                = _preTarget < target
                ? -1 : 1;
            transform.localScale = vector3;
            _preTarget = target;

            Vector3 transformPosition = transform.localPosition;
            transformPosition.x = target * 700;
            transform.localPosition = transformPosition;
        }

        public void FishUp()
        {
            transform.parent.gameObject.SetActive(false);
        }
    }
}

namespace FishMovement
{
    public class FishMovementModel
    {
        // -1から1の範囲で移動先を決める
        private ReactiveProperty<float> _moveTargetProperty = new ReactiveProperty<float>();
        public IReadOnlyReactiveProperty<float> MoveTargetProperty => _moveTargetProperty;

        private float _preTarget = -1;
        private const float MOVE_SPEED = 0.2f;

        public async void SetMoveTarget(CancellationToken token)
        {
            try { token.ThrowIfCancellationRequested(); }
            catch { return; }

            _moveTargetProperty.Value = _preTarget;

            // 今の自分の位置から±0.1の距離へ移動
            // 真ん中に近づくほど端へ帰る確率が上がる
            float rand = Random.Range(0f, 1f);
            int sign = _preTarget < 0.5f ? 1 : -1;
            float distance = 0;

            if (rand < Mathf.Abs(_preTarget))
            {
                // 真ん中へ移動していく
                while (Mathf.Abs(distance) < 0.1f)
                {
                    distance += Time.deltaTime * sign * MOVE_SPEED;
                    _moveTargetProperty.Value = distance + _preTarget;
                    try { await UniTask.Yield(cancellationToken: token); }
                    catch { return; }
                }
            }
            else
            {
                // 端へ移動していく
                while (Mathf.Abs(distance) < 0.1f)
                {
                    distance += Time.deltaTime * -sign * MOVE_SPEED;
                    _moveTargetProperty.Value = distance + _preTarget;
                    try { await UniTask.Yield(cancellationToken: token); }
                    catch { return; }
                }
            }
            
            _preTarget = _moveTargetProperty.Value;
            SetMoveTarget(token);
        }
    }
}