using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour
{
    [SerializeField]
    private EnemyManager _enemyPrefab;

    [SerializeField]
    private GameObject _gameOver; //GameOver時表示オブジェクト

    [SerializeField]
    private GameObject _gameWin; //Game勝利時表示オブジェクト

    [SerializeField]
    private GameObject _start; //スタートボタン用オブジェクト

    [SerializeField]
    private GameObject _help; //ヘルプ用オブジェクト


    public EnemyManager EnemyPrefab => _enemyPrefab;
    public GameObject Help => _help;
    public GameObject Start => _start;
    public GameObject GameWin => _gameWin;
    public GameObject GameOver => _gameOver;
}
