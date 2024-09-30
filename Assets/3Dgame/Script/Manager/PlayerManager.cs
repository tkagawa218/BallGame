using UnityEngine;
using UniRx.Async;
using UniRx.Async.Triggers;
using UniRx;
using System;
using Model;
using UnityEditor;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController _playerController;

    [SerializeField]
    private float _force = 1.0f; // 移動速度 m/s
                                 // Update is called once per frame


    private void Awake()
    {
        UniRxManager.Instance.OnEndEvent
        .Subscribe(result =>
        {
            _playerController.InitPlayerPos();
        });

        UniRxManager.Instance.OnPlayerDirectionEvent
        .Subscribe(playerDirection =>
        {
            _playerController.Move(_force, playerDirection);
        });
    }

    private void Start()
    {
        
        _playerController.DoAsync().Forget();
    }
}