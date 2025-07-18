using UnityEngine;

namespace FishMovement
{
    public class FishMovementController : MonoBehaviour
    {
        private FishMovementPresenter _presenter;

        private void Start()
        {
            FishMovementModel model = new FishMovementModel();
            FishMovementView view = GetComponent<FishMovementView>();

            _presenter = new FishMovementPresenter(model, view);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && Mathf.Abs(_presenter.GetFishPositionX()) < 0.5f)
            {
                _presenter.CancelFishMoving();
            }
        }
    }
}

