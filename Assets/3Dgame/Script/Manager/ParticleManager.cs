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
            var gamedata = GameDataModel.GetGameData();
            //パーティクル配置イベントを購読
            UniRxManager.Instance.OnSetParticleEvent
            .Subscribe(_ =>
            {
                _particleController.SetParticleS(gameObject, gameObject);
                Observable.Interval(TimeSpan.FromSeconds(gamedata.particleInterval))
                          .Do(x => _particleController.SetParticleS(gameObject, gameObject))
                          .Subscribe();
            });
        }
    }
}