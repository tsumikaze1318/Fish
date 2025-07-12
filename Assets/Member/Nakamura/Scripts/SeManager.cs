using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeManager : MonoBehaviour
{
    private static SeManager _instance = null;
    public static SeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SeManager>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject("SeManager");
                    _instance = obj.AddComponent<SeManager>();
                }
            }
            return _instance;
        }
    }

    [SerializeField]
    private AudioSource asSE;

    [SerializeField]
    private List<AudioClip> seList = new List<AudioClip>();

    void Awake()
    {
        asSE = this.gameObject.AddComponent<AudioSource>();
    }

    public void PlaySE(int num, float volume = 0.5f,bool loop = false)
    {
        asSE.volume = volume;
        asSE.clip = seList[num];
        asSE.loop = loop;
        asSE.Play();
    }
}
