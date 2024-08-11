using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    void Awake()
    {
        GameData gamedata = Resources.Load<GameData>(GameDataManager.Instance.GetScritablePath());
        //パーティクル配置イベントを購読
        GameDataManager.Instance.OnSetParticleEvent
            .Subscribe(_=>
            {
                GameDataManager.Instance.SetParticleS(gameObject, gameObject);
                Observable.Interval(TimeSpan.FromSeconds(gamedata.particleInterval))
                           .Do(x => GameDataManager.Instance.SetParticleS(gameObject, gameObject))
                           .Subscribe();
            });

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
