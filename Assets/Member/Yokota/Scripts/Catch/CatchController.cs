using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchController : MonoBehaviour
{
    private CatchView _catchView;

    private int _count = 0;

    private void Start()
    {
        _catchView = GetComponent<CatchView>();
        _catchView.UpdateCatch(_count);
    }

    public bool FluctuationValue(int value)
    {
        _count += value;

        if (_count < 0)
        {
            _count = 0;
            return false;
        }

        _catchView.UpdateCatch(_count);
        return true;
    }

    public int GetFishCount()
    {
        return _count;
    }
}
