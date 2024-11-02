using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameSoundController : MonoBehaviour
{
    [SerializeField]
    private GameObject _playBGM; // Game中BGM

    [SerializeField]
    private GameObject _initBGM; // Game開始前BGM

    [SerializeField]
    private GameObject _winBGM; // Game勝利BGM

    [SerializeField]
    private GameObject _loseBGM; // Game敗北BGM

    [SerializeField]
    private AudioClip _startBtnSE;

    [SerializeField]
    private AudioClip _enemyParticleSE;

    [SerializeField]
    private AudioClip _playerParticleSE;

    private AudioSource audioSource;

    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        GameSoundManager.Instance.setSoundBgm(_initBGM);

        //ゲームスタートイベントを購読
        GameSoundManager.Instance.OnSoundPlayBgmChanged
        .Subscribe(_ =>
        {
            GameSoundManager.Instance.PlaySoundBgm(_playBGM);
        });

        //ゲーム勝利イベントを購読
        GameSoundManager.Instance.OnSoundWinBgmChanged
        .Subscribe(_ =>
        {
            GameSoundManager.Instance.PlaySoundBgm(_winBGM);
        });

        //ゲーム敗北イベントを購読
        GameSoundManager.Instance.OnSoundLoseBgmChanged
        .Subscribe(_ =>
        {
            GameSoundManager.Instance.PlaySoundBgm(_loseBGM);
        });

        //スタートボタン押下イベントを購読
        GameSoundManager.Instance.OnSoundStartBtnSeChanged
        .Subscribe(_ =>
        {
            GameSoundManager.Instance.setSoundSe(audioSource, _startBtnSE);
        });

        //敵がパーティクルに接触イベントを購読
        GameSoundManager.Instance.OnSoundEnemyparticleSeChanged
        .Subscribe(_ =>
        {
            GameSoundManager.Instance.setSoundSe(audioSource, _enemyParticleSE);
        });

        //味方がパーティクルに接触イベントを購読
        GameSoundManager.Instance.OnSoundPlayerparticleSeChanged
        .Subscribe(_ =>
        {
            GameSoundManager.Instance.setSoundSe(audioSource, _playerParticleSE);
        });

        //初期化イベントを購読
        GameSoundManager.Instance.OnSoundInitBgmChanged
        .Subscribe(_ =>
        {
            GameSoundManager.Instance.PlaySoundBgm(_initBGM);
        });
    }
}
