using UnityEngine;

namespace FishMovement
{
    public class FishMovementController : MonoBehaviour
    {
        private FishMovementPresenter _presenter;
        private float _fishRange;

        private void Start()
        {
            FishMovementModel model = new FishMovementModel();
            FishMovementView view = GetComponent<FishMovementView>();

            _presenter = new FishMovementPresenter(model, view);
            _presenter.CancelFishMoving();
            transform.parent.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _presenter.Init();
        }

        public void Fish()
        {
            if (Mathf.Abs(_presenter.GetFishPositionX()) < _fishRange)
            {
                // 魚ゲージの動きをキャンセルする
                _presenter.CancelFishMoving();
                // 釣り上げた演出を入れる
                GameManager.Instance.StartFishResult();
            }
        }

        public void StartFish()
        {
            _presenter.StartFishMoving();
        }

        public void SetFishRange(float range)
        {
            _fishRange = range;
        }

        public void Bait()
        {
            _presenter.IncreasePercentCorrection();
        }
    }
}

