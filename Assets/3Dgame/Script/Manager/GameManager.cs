using UnityEngine;
using UniRx;
using Controller;
using System;
using Model;

namespace Manager
{

    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private GameController _gameController;

        [SerializeField]
        private Transform _player;

        [SerializeField]
        private Transform _enemyParent;// 敵キャラを格納する親オブジェクト

        private void Awake()
        {
            _gameController.Init();

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

            UniRxManager.Instance.OnVarEnemyEvent
            .Subscribe(num =>
            {
                _gameController.AdjustmentEnemyS(_enemyParent.gameObject, _player.gameObject, num);
            })
            .AddTo(this);

            //timerイベントを購読
            UniRxManager.Instance.OnTimeChanged
            .Delay(TimeSpan.FromMilliseconds(1000))
            .Subscribe(t =>
            {
                _gameController.ActionByTimeChanged(t);
            })
            .AddTo(this);

            //ゲーム終了イベントを購読
            //result
            //true:勝利
            //false:敗北
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
            })
            .AddTo(this);

            UniRxManager.Instance.OnDelEnemyParticleEvent
            .Subscribe(item =>
            {
                _gameController.DellAnyEnemyParticleS(item);
            })
            .AddTo(this);

            UniRxManager.Instance.OnDelPlayerParticleEvent
            .Subscribe(item =>
            {
                _gameController.DellAnyPlayerParticleS(item);
            })
            .AddTo(this);

            Observable
            .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1)) //0秒後から1秒間隔で実行
            .Subscribe(x => //xは起動してからの秒数
            {
                if (!_gameController.GetGameOn())
                {
                    return;
                }
                _gameController.setRestTime(GameDataModel.RestTime);
            })
            .AddTo(this);
        }

        private void OnDestroy()
        {
            _gameController.AllClearEnemyS();
            _gameController.AllClearEnemyParticleS();
            _gameController.AllClearPlayerParticleS();
        }
    }
}
