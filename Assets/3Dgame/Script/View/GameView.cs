using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour
{
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


    public EnemyManager EnemyPrefab => _enemyPrefab;
    public GameObject Help => _help;
    public GameObject Start => _start;
    public GameObject GameWin => _gameWin;
    public GameObject GameOver => _gameOver;
}
