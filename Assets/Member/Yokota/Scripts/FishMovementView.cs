using Cysharp.Threading.Tasks;
using PrimeTween;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace FishMovement
{
    public class FishMovementPresenter
    {
        private FishMovementModel _model;
        private FishMovementView _view;

        public FishMovementPresenter(FishMovementModel model, FishMovementView view)
        {
            _model = model;
            _view = view;

            _view.IsMoving.Subscribe(b =>
            {
                if (!b)
                {
                    _model.SetMoveTarget();
                }
            });
            _model.MoveTargetProperty.Subscribe(x => 
            {
                _view.MoveFish(x);
            });
        }

        //public float GetFishPosition()
        //{
        //    return 
        //}
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
            float duration = Mathf.Abs(_preTarget - target) * 5;
            _preTarget = target;

            Tween.LocalPositionX(transform
                , 700 * target
                , duration
                , Ease.Linear)
                .OnComplete(() =>
                {
                    _isMoving.Value = false;
                });
        }
    }
}

namespace FishMovement
{
    public class FishMovementModel
    {
        // -1‚©‚ç1‚Ì”ÍˆÍ‚ÅˆÚ“®æ‚ğŒˆ‚ß‚é
        private ReactiveProperty<float> _moveTargetProperty = new ReactiveProperty<float>();
        public IReadOnlyReactiveProperty<float> MoveTargetProperty => _moveTargetProperty;

        private float _preTarget = 0;

        public async void SetMoveTarget()
        {
            // -1‚©‚ç1‚Ì”ÍˆÍ‚Åƒ‰ƒ“ƒ_ƒ€‚ÉˆÚ“®æ‚ğŒˆ’è
            float randTarget = Random.Range(-1f, 1f);

            int sign = _preTarget < randTarget ? 1 : -1;

            while ( _moveTargetProperty.Value > randTarget)
            {
                _moveTargetProperty.Value += Time.deltaTime * sign;
                await UniTask.Yield();
            }
        }
    }
}