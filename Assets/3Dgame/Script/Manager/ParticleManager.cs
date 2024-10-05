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

        void Awake()
        {
            //パーティクル配置イベントを購読
            UniRxManager.Instance.OnSetParticleEvent
            .Subscribe(_ =>
            {
                var gamedata = GameDataModel.GetGameData();
                _particleController.SetParticleS(gameObject, gameObject);
                Observable.Interval(TimeSpan.FromSeconds(gamedata.particleInterval))
                          .Do(x => _particleController.SetParticleS(gameObject, gameObject))
                          .Subscribe();
            });
        }
    }
}