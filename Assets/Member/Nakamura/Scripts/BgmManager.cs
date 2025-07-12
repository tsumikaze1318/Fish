using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    private static BgmManager _instance = null;
    public static BgmManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<BgmManager>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject("BgmManager");
                    _instance = obj.AddComponent<BgmManager>();
                }
            }
            return _instance;
        }
    }

    [SerializeField]
    private AudioSource asBGM;

    [SerializeField]
    private List<AudioClip> bgmList = new List<AudioClip>();

    void Awake()
    {
        asBGM = this.gameObject.AddComponent<AudioSource>();
    }

    public void PlayBGM(int num, float volume = 0.5f,bool loop = true)
    {
        asBGM.volume = volume;
        asBGM.clip = bgmList[num];
        asBGM.loop = loop;
        asBGM.Play();
    }
}
