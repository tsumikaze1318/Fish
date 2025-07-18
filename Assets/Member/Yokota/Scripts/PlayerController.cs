using FishMovement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void Update()
    {
        // T‚«‰a‚Ì•â[
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.ChargeBait();
        }

        switch (GameManager.Instance.State)
        {
            case GameState.None:
                break;
            case GameState.Stay:
                break;
            case GameState.Fish:
                // T‚«‰a‚ğ‚·‚é
                if (Input.GetMouseButtonDown(1))
                {
                    GameManager.Instance.Bait();
                }
                // ’Ş‚èã‚°‚é
                if (Input.GetMouseButtonDown(0))
                {
                    GameManager.Instance.Fish();
                }
                break;
            case GameState.Show:
                if (Input.GetMouseButtonDown(0))
                {
                    GameManager.Instance.EndFishResult();
                }
                break;
            default:
                break;
        }
    }
}
