using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    private const float BgImageAlphaOn = 0.4f;

    private const float BgImageAlphaOff = 0.0f;

    [SerializeField]
    private EnemyManager _enemyPrefab;

    [SerializeField]
    private GameObject _gameOver; //GameOver���\���I�u�W�F�N�g

    [SerializeField]
    private GameObject _gameWin; //Game�������\���I�u�W�F�N�g

    [SerializeField]
    private GameObject _start; //�X�^�[�g�{�^���p�I�u�W�F�N�g

    [SerializeField]
    private GameObject _help; //�w���v�p�I�u�W�F�N�g

    [SerializeField]
    private Image _bgImage; //�w�i�p�I�u�W�F�N�g

    public EnemyManager EnemyPrefab => _enemyPrefab;
    public GameObject Help => _help;
    public GameObject Start => _start;
    public GameObject GameWin => _gameWin;
    public GameObject GameOver => _gameOver;
    public Image BgImage => _bgImage;

    public void SetBgImageAlpha(bool on)
    {
        Color color = _bgImage.color;
        color.r = 0.8f;
        color.g = 0.3f;
        color.b = 0.1f;
        color.a = on ? BgImageAlphaOn : BgImageAlphaOff;
        _bgImage.color = color;
    }
}
