using UnityEngine;
using UniRx;
using Controller;
using System.Collections.Generic;
using System;
using Model;
using static UnityEngine.Networking.UnityWebRequest;
using UnityEditor;

namespace Manager
{

    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private GameController _gameController;

        [SerializeField]
        private Transform _player;

        [SerializeField]
        private Transform _enemyParent;// �G�L�������i�[����e�I�u�W�F�N�g

        private void Awake()
        {
            _gameController.SetPlayerPos(_player.transform.position);

            UniRxManager.Instance.OnStartEvent
            .Subscribe(_ =>
            {
                _gameController.GameOn();
                _gameController.AllClearEnemyS();
                _gameController.setRestTime(GameData.Instance.gameTime);
                _gameController.InitView();
                _gameController.AddEnemyS(_enemyParent.gameObject, _player.gameObject);
                UniRxManager.Instance.SendSetParticleEvent();
                GameSoundManager.Instance.sendSoundPlayBgmEvent();
                Observable.Interval(TimeSpan.FromSeconds(GameData.Instance.enemyExplaceInterval))
               .Do(x => _gameController.allExplaceEnemyS())
               .Subscribe();
            })
            .AddTo(this);

            UniRxManager.Instance.OnAddEnemyEvent
            .Subscribe(_ =>
            {
                _gameController.AddEnemyS(_enemyParent.gameObject, _player.gameObject);
            })
            .AddTo(this);

            //timer�C�x���g���w��
            UniRxManager.Instance.OnTimeChanged
            .Delay(TimeSpan.FromMilliseconds(1000))
            .Subscribe(t =>
            {
                _gameController.ActionByTimeChanged(t);
            });

            //�Q�[���I���C�x���g���w��
            //result
            //true:����
            //false:�s�k
            UniRxManager.Instance.OnEndEvent
            .Subscribe(result =>
            {
                _gameController.EndView(result);
                if (result)
                {
                    GameSoundManager.Instance.sendSoundWinBgmEvent();
                }
                else
                {
                    GameSoundManager.Instance.sendSoundLoseBgmEvent();
                }
            });

            UniRxManager.Instance.OnDelEnemyParticleEvent
            .Subscribe(item =>
            {
                _gameController.DellAnyEnemyParticleS(item);
            });

            UniRxManager.Instance.OnDelPlayerParticleEvent
            .Subscribe(item =>
            {
                _gameController.DellAnyPlayerParticleS(item);
            });
        }

        private void OnDestroy()
        {
            _gameController.AllClearEnemyS();
        }
    }
}
