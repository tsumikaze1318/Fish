using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [SerializeField, Scene, Label("�V�[���J�ڐ�")]
    private string fadeScene;
    [SerializeField, Label("�t�F�[�h�p�l��")]
    private GameObject fadeScenePanel;

    [SerializeField, Label("�X�^�[�g�{�^��")]
    private TextMeshProUGUI startButtonText;
    [SerializeField, Label("�_�ő��x")]
    private float speed = 1.0f;
    private float time;


    void Start()
    {
        BgmManager.Instance.PlayBGM(0);
    }

    void Update()
    {
        startButtonText.color = GetColorAlpha(startButtonText.color);
    }

    Color GetColorAlpha(Color color)
    {
        time += Time.deltaTime * speed;
        color.a = Mathf.Sin(time);

        return color;
    }

    /// <summary>
    /// �X�^�[�g�{�^��
    /// </summary>
    public async void OnStartButton()
    {
        SeManager.Instance.PlaySE(0, 1f);
        fadeScenePanel.SetActive(true);
        await FadeSceneManager.Inctance.FadeOutScene(fadeScene);
    }
}
