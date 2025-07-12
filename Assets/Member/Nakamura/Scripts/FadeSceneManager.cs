using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

[RequireComponent(typeof(CanvasGroup))]
public class FadeSceneManager : MonoBehaviour
{
    [SerializeField, Label("�t�F�[�h���鎞��")]
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

    //��ʂ��؂�ւ������t�F�[�h�C���������
    private async void Start()
    {
        await FadeIn();
    }

    /// <summary>
    /// �t�F�[�h�C��
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
    /// �t�F�[�h�A�E�g
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
    /// �V�[���J�ڎ��t�F�[�h�A�E�g�ƃV�[���ړ�������
    /// </summary>
    /// <param name="fadeSceneName">�J�ڐ�̃V�[����</param>
    /// <returns></returns>
    public async UniTask FadeOutScene(string fadeSceneName)
    {
        this.gameObject.SetActive(true);
        await FadeOut();
        SceneManager.LoadScene(fadeSceneName);
    }
}
