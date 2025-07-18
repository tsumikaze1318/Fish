using FishMovement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static object _lock = new object();
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance
                        = FindObjectOfType<GameManager>();
                    if (_instance == null)
                    {
                        var singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<GameManager>();
                        singletonObject.name = nameof(GameManager) + "(singleton)";
                    }
                }

                return _instance;
            }
        }
    }

    private GameState _state = GameState.None;
    public GameState State => _state;

    [SerializeField]
    private CatchController _catchController;
    [SerializeField]
    private BaitRemainController _baitRemainController;
    [SerializeField]
    private BaitController _baitController;
    [SerializeField]
    private FishMovementController _fishMovementController;
    [SerializeField]
    private FishModelAnimation _fishModelAnimation;
    [SerializeField]
    private FishResult _fishResult;
    [SerializeField]
    private GaugeSet _gaugeSet;
    [SerializeField]
    private UkiAnimation _ukiAnimation;
    [SerializeField]
    private Timer _timer;
    [SerializeField]
    private Result _result;

    public void StartFishMoving()
    {
        if (_timer.GetTime() <= 0f)
        {
            // ƒQ[ƒ€I—¹
            _result.SetResult(_catchController.GetFishCount());
            return;
        }

        SetGameState(GameState.None);
        _ukiAnimation.ReleaseAnimation();
        _fishModelAnimation.StartAnimationAsync();
        _baitController.ResetBait();
    }

    public void StartFishFeint()
    {
        SetGameState(GameState.Stay);
    }

    public void StartFishing(float value)
    {
        SetGameState(GameState.Fish);
        _gaugeSet.SetGaugeValue(value);
        _fishMovementController.StartFish();
    }

    public void StartFishResult()
    {
        _ukiAnimation.FishAnimation();
        _fishModelAnimation.ResetPosition();
        _catchController.FluctuationValue(1);
        _fishResult.DisplayishResult();
    }

    public void EndFishResult()
    {
        _fishResult.CancelDisplayFishResult();
    }

    public void Bait()
    {
        _baitController.Bait();
        _fishMovementController.Bait();
    }

    public void Fish()
    {
        _fishMovementController.Fish();
    }

    public void ChargeBait()
    {
        _catchController.FluctuationValue(-1);
        _baitRemainController.FluctuationBaitRemain(5);
    }

    public void ResetBait()
    {
        _baitController.ResetBait();
    }

    public void SetGameState(GameState state)
    {
        _state = state;
    }
}

public enum GameState
{
    None,
    Stay,
    Fish,
    Show
}
