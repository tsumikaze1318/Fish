using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

[RequireComponent(typeof(CanvasGroup))]
public class FadeSceneManager : MonoBehaviour
{
    [SerializeField, Label("フェードする時間")]
    private float fadeSpeed = 1f;

    private CanvasGroup canvasGroup;

    private static FadeSceneManager instance;
    public static FadeSceneManager Inctance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FadeSceneManager>();

                if (instance == null)
                {
                    var obj = new GameObject("FadeSceneManager");
                    instance = obj.AddComponent<FadeSceneManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    //画面が切り替わったらフェードインが流れる
    private async void Start()
    {
        await FadeIn();
    }

    /// <summary>
    /// フェードイン
    /// </summary>
    public async UniTask FadeIn()
    {
        this.gameObject.SetActive(true);
        canvasGroup.alpha = 1;
        float alpha = 1;
        while (canvasGroup.alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            canvasGroup.alpha = Mathf.Max(alpha, 0f);
            await UniTask.Yield();
        }
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// フェードアウト
    /// </summary>
    public async UniTask FadeOut()
    {
        
        this.gameObject.SetActive(true);
        
        canvasGroup.alpha = 0;
        float alpha = 0;
        while (canvasGroup.alpha < 1)
        {
            alpha += Time.deltaTime * fadeSpeed;
            canvasGroup.alpha = Mathf.Min(alpha, 1f);
            await UniTask.Yield();
        }
    }

    /// <summary>
    /// シーン遷移時フェードアウトとシーン移動をする
    /// </summary>
    /// <param name="fadeSceneName">遷移先のシーン名</param>
    /// <returns></returns>
    public async UniTask FadeOutScene(string fadeSceneName)
    {
        this.gameObject.SetActive(true);
        await FadeOut();
        SceneManager.LoadScene(fadeSceneName);
    }
}
