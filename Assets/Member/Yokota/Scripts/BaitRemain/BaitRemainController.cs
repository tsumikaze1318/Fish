using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitRemainController : MonoBehaviour
{
    private int _baitRemain = 5;

    private BaitRemainVeiw _baitRemainView;

    private void Start()
    {
        _baitRemainView = GetComponent<BaitRemainVeiw>();
        _baitRemainView?.UpdateRemainText(_baitRemain);
    }

    public void FluctuationBaitRemain(int value)
    {
        _baitRemain += value;
        _baitRemainView?.UpdateRemainText(_baitRemain);
    }

    public int GetBaitRemain()
    {
        return _baitRemain;
    }
}
