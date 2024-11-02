using Model;
using UnityEngine;
using UniRx;
using Controller;
using System;

namespace Manager
{

    public class ParticleManager : MonoBehaviour
    {
        [SerializeField]
        private ParticleController _particleController;

        private IDisposable _disposable;

        void Awake()
        {
            //パーティクル配置イベントを購読
            UniRxManager.Instance.OnSetParticleEvent
            .Subscribe(_ =>
            {
                var gamedata = GameDataModel.GetGameData();
                _particleController.SetParticleS(gameObject, gameObject);
                _disposable = Observable.Interval(TimeSpan.FromSeconds(gamedata.particleInterval))
                          .Do(x => _particleController.SetParticleS(gameObject, gameObject))
                          .Subscribe();
            });

            UniRxManager.Instance.OnUnSetParticleEvent
            .Subscribe(_ =>
            {
                _disposable?.Dispose();
            });
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}