using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using NaughtyAttributes;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;


//©SekkirTech

/*
 * 作成　
 * 東京デザインテクノロジーセンター専門学校
 * スーパーIT科スーパーゲームクリエイター専攻
 * 関海斗
 */

//60フレーム固定




public class MusicManager : MonoBehaviour
{

    [SerializeField] AudioClip[] _BGMList;
    [SerializeField] AudioClip[] _SEList;

    [SerializeField] AudioClip[] _playerMList;
    [SerializeField] AudioClip[] _enemyMList;


    /// <summary>
    /// BGM用コンポーネント格納用
    /// </summary>
    private AudioSource _BGMAudio;

    /// <summary>
    /// Player用
    /// </summary>
    private AudioSource _playerAudio;

    /// <summary>
    /// Enemy用
    /// </summary>
    private AudioSource _enemyAudio;

    //インスタンス定義
    public static MusicManager Instance;

    [SerializeField] private AudioClip _testClip;



    void Start()
    {
        //破棄不能化
        DontDestroyOnLoad(this);

        //AudioSource格納
        _BGMAudio = gameObject.AddComponent<AudioSource>();
        _playerAudio = gameObject.AddComponent<AudioSource>();
        _enemyAudio = gameObject.AddComponent<AudioSource>();

        //ゲーム開始時0番を再生
        //_BGMAudio.clip = _BGMList[0];
        
        //Loopがfalseだった時用
        _BGMAudio.loop = true;
        _BGMAudio.Play();

        //セリフ類Loopしないよう
        _playerAudio.loop = false;
        _enemyAudio.loop = false;
    }

    //シングルトン
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// Track番号を元にBGMを切り替え
    /// </summary>
    /// <param name="track">トラック番号</param>
    /// <param name="MusicVolume">音量1-100</param>
    /// 
    [Button]
    public void SetTrackBGM(int track=0,float MusicVolume=100)
    {
        Debug.Log("SetTrackBGM");
        _BGMAudio.volume = 0.5f;
        _BGMAudio.volume = 0;
        _BGMAudio.clip = _BGMList[track];
        _BGMAudio.volume = MusicVolume*0.01f;
        _BGMAudio.Play();
        Debug.Log("BGMの" + track + " 番 " + _BGMList[track].name + " を " + MusicVolume + " で再生");
    }

    //SEの後処理用コルーチン
    IEnumerator SEBreaker(AudioSource audioSource,float SEwait)
    {
        yield return new WaitForSeconds(SEwait);
        Destroy(audioSource);
    }

/*    /// <summary>
    /// 名前を元にSEを切り替えたいときに呼び出してもらう
    /// </summary>
    /// <param name="name">SE名</param>
    /// <param name="MusicVolume">音量1-100</param>
    public void CallNameSE(string name, float MusicVolume) 
    {
        AudioSource Ss = this.gameObject.AddComponent<AudioSource>();
        for (int i = 0; i < SEList.Length; i++)
        {
            if (name == SEList[i].name)
            {
                Ss.clip = SEList[i];
                Ss.volume = MusicVolume * 0.01f;
                Ss.Play();
                Debug.Log("SEの"+SEList[i].name +" を " + MusicVolume + " で再生");
                StartCoroutine(SEBreaker(Ss, SEList[i].length));
            }
        }
        Debug.LogError("SE名を確認してください");
    }*/
   
    /// <summary>
    /// Track番号を元にSEを再生
    /// </summary>
    /// <param name="track">トラック番号</param>
    /// <param name="MusicVolume">音量1-100</param>
    public void CallTrackSE(int track, float MusicVolume)
    {
        AudioSource Se=this.gameObject.AddComponent<AudioSource>();
            Se.clip = _SEList[track];
            Se.volume = MusicVolume * 0.01f;
            Se.Play();
            Se.loop = false;
            Debug.Log("SEの"+　track + " 番 " + _SEList[track].name + " を "+MusicVolume+" で再生");
            StartCoroutine(SEBreaker(Se, _SEList[track].length));
    }

    /// <summary>
    /// BGMのフェードアウト
    /// </summary>
    /// <param name="fadetime">何秒かけて消えるか</param>
    public void FadeOut(float fadetime=1.0f)
    {
        Fadetime = fadetime;
        Counttime = 0;
        Isfadeout = true;
    }
    bool Isfadeout=false;
    bool Isfadein = false;
    float Counttime = 0.0f;
    float Fadetime = 1f;
    void Update()
    {
        if (Isfadeout)
        {
            Counttime += Time.deltaTime;
            if (Counttime >= Fadetime)
            {
                Counttime = Fadetime;
                Isfadeout = false;
            }
            _BGMAudio.volume =1.0f - Counttime / Fadetime;
        }
        if (Isfadein)
        {
            Counttime += Time.deltaTime;
            if (Counttime >= Fadetime)
            {
                Counttime = Fadetime;
                Isfadein = false;
            }
            _BGMAudio.volume = Counttime / Fadetime;
        }
    }

    /// <summary>
    /// 途中から再生
    /// </summary>
    /// <param name="fadetime">かける秒数</param>
    /// <param name="Track">トラック番号</param>
    /// <param name="DuringTime">再生開始時間（秒）</param>
    public void DuringStart(float fadetime,int Track,float DuringTime)
    {
        _BGMAudio.volume = 0f;
        _BGMAudio.clip = _BGMList[Track];
        _BGMAudio.time = DuringTime;
        Counttime = 0.0f;
        Isfadein = true;
    } 

    /// <summary>
    /// 引数型SE
    /// </summary>
    /// <param name="clip">再生したいSEClip</param>
    /// <param name="MusicVolume">音量1-100</param>
    public void ArgumentSE(AudioClip clip,float MusicVolume)
    {
        Debug.Log("ArgumentSE");
        AudioSource Se=this.AddComponent<AudioSource>();
        Se.clip = clip;
        Se.volume = MusicVolume*0.01f;
        Se.Play();
        StartCoroutine(SEBreaker(Se,clip.length));
    }


    public void SayPlayerAudio(int _num,float _volume)
    {
        _playerAudio.clip=_playerMList[_num];
        _playerAudio.volume = _volume*0.01f;
        _playerAudio.Play();
    }


    public void StopPlayerAudio()
    {
        _playerAudio.Stop();
    }

    public void ChangePlayerAudio(int _num,float _volume)
    {
        _playerAudio.Stop();
        _playerAudio.clip = _playerMList[_num];
        _playerAudio.volume = _volume * 0.01f;
        _playerAudio.Play();
    }

    public async void FadeOutPlayerAudio(float _fadeTime, float _volume)
    {
        float _time = 0;
        float _defaultVolume = _enemyAudio.volume;
        if (_fadeTime == 0)
        {
            _enemyAudio.volume = _volume * 0.01f;
        }
        while (true)
        {
            await UniTask.Yield();
            _time += Time.deltaTime;
            _enemyAudio.volume = _defaultVolume - ((_time / _fadeTime) * _defaultVolume);
            if (_enemyAudio.volume <= _volume * 0.01f) _enemyAudio.volume = _volume * 0.01f; break;
        }
        Debug.Log("終了");
    }

    public void SayEnemy(int _num,float _volume)
    {
        _enemyAudio.clip=_enemyMList[_num];
        _enemyAudio.volume = _volume * 0.01f;
        _enemyAudio.Play();
    }

    public void StopEnemyAudio()
    {
        _enemyAudio.Stop();
    }

    public void ReStartEnemyAudio()
    {
        _enemyAudio.Play();
    }

    public void SetVolumeEnemyAudio(float _volume)
    {
        _enemyAudio.volume= _volume*0.01f;
    }

    [Button]
    public void DebugStart()
    {
        _enemyAudio.clip = _testClip;
        _enemyAudio.volume = 0.0f;
        _enemyAudio.Play();
    }

    /// <summary>
    /// Enemy用FadeOut
    /// </summary>
    /// <param name="_fadeTime">fadeにかける時間</param>
    /// <param name="_volume">どこまで下げるか</param>
    [Button]
    public async void FadeOutEnemyAudio(float _fadeTime,float _volume)
    {
        float _time = 0;
        float _defaultVolume=_enemyAudio.volume;
        if (_fadeTime == 0)
        {
            _enemyAudio.volume = _volume*0.01f;
        }
        while (true)
        {
            await UniTask.Yield();
            _time += Time.deltaTime;
            _enemyAudio.volume =_defaultVolume - ((_time / _fadeTime)*_defaultVolume);
            if (_enemyAudio.volume <=_volume*0.01f) _enemyAudio.volume = _volume * 0.01f; break;
        }
        Debug.Log("終了");
    }

    [Button]
    public async void FadeInEnemyAudio(float _fadeTime=5,float _volume=80)
    {
        float _time = 0;

        if (_fadeTime == 0)
        {
            _enemyAudio.volume = _volume * 0.01f;
        }

        while (true)
        {
            await UniTask.Yield();
            _time += Time.deltaTime;
            _enemyAudio.volume = (_time / _fadeTime)*(_volume*0.01f);

            if (_enemyAudio.volume >= (_volume * 0.01))
            {
                _enemyAudio.volume = _volume * 0.01f; Debug.Log("break"); break;
            }
        }
        Debug.Log("終了");
    }

    public void ChangeEnemyAudio(int _num, float _volume)
    {
        _enemyAudio.Stop();
        _enemyAudio.clip = _playerMList[_num];
        _enemyAudio.volume = _volume * 0.01f;
        _enemyAudio.Play();
    }
}