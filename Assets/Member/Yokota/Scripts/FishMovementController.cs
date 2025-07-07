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
    }
}

