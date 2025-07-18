using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishDictionary : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _fishList = new List<GameObject>();

    public GameObject GetRandomFish()
    {
        int i = Random.Range(0, _fishList.Count);
        var fish = _fishList[i];
        return fish;
    }
}
