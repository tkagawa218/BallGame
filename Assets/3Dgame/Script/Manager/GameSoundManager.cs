using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class GameSoundManager : SingletonMonoBehaviour<GameSoundManager>
{
    private GameObject _soundBgm = null;

    //初期BGMサウンド再生イベントを発行する核となるインスタンス
    private Subject<Unit> soundInitBgmSubject = new Subject<Unit>();

    /// <summary>
    /// 初期BGMサウンド再生イベントの購読側だけを公開
    /// </summary>
    public IObservable<Unit> OnSoundInitBgmChanged
    {
        get { return soundInitBgmSubject; }
    }

    /// <summary>
    /// 初期BGMサウンド再生イベント発行
    /// </summary>
    public void sendSoundInitBgmEvent()
    {
        soundInitBgmSubject.OnNext(Unit.Default);
    }



    //play中BGMサウンド再生イベントを発行する核となるインスタンス
    private Subject<Unit> soundPlayBgmSubject = new Subject<Unit>();

    /// <summary>
    /// play中BGMサウンド再生イベントの購読側だけを公開
    /// </summary>
    public IObservable<Unit> OnSoundPlayBgmChanged
    {
        get { return soundPlayBgmSubject; }
    }

    /// <summary>
    /// Play中BGMサウンド再生イベント発行
    /// </summary>
    public void sendSoundPlayBgmEvent()
    {
        soundPlayBgmSubject.OnNext(Unit.Default);
    }

    //WinBGMサウンド再生イベントを発行する核となるインスタンス
    private Subject<Unit> soundWinBgmSubject = new Subject<Unit>();

    /// <summary>
    /// WinBGMサウンド再生イベントの購読側だけを公開
    /// </summary>
    public IObservable<Unit> OnSoundWinBgmChanged
    {
        get { return soundWinBgmSubject; }
    }

    /// <summary>
    /// WinBGMサウンド再生イベント発行
    /// </summary>
    public void sendSoundWinBgmEvent()
    {
        soundWinBgmSubject.OnNext(Unit.Default);
    }

    //LoseBGMサウンド再生イベントを発行する核となるインスタンス
    private Subject<Unit> soundLoseBgmSubject = new Subject<Unit>();

    /// <summary>
    /// LoseBGMサウンド再生イベントの購読側だけを公開
    /// </summary>
    public IObservable<Unit> OnSoundLoseBgmChanged
    {
        get { return soundLoseBgmSubject; }
    }

    /// <summary>
    /// LoseBGMサウンド再生イベント発行
    /// </summary>
    public void sendSoundLoseBgmEvent()
    {
        soundLoseBgmSubject.OnNext(Unit.Default);
    }

    //StartBtnSEサウンド再生イベントを発行する核となるインスタンス
    private Subject<Unit> soundStartBtnSeSubject = new Subject<Unit>();

    /// <summary>
    /// StartBtnSEサウンド再生イベントの購読側だけを公開
    /// </summary>
    public IObservable<Unit> OnSoundStartBtnSeChanged
    {
        get { return soundStartBtnSeSubject; }
    }

    /// <summary>
    /// StartBtnSEサウンド再生イベント発行
    /// </summary>
    public void sendStartBtnSeEvent()
    {
        soundStartBtnSeSubject.OnNext(Unit.Default);
    }

    // 敵がパーティクルに接触SEサウンド再生イベントを発行する核となるインスタンス
    private Subject<Unit> soundEnemyparticleSeSubject = new Subject<Unit>();

    /// <summary>
    ///  敵がパーティクルに接触SEサウンド再生イベントの購読側だけを公開
    /// </summary>
    public IObservable<Unit> OnSoundEnemyparticleSeChanged
    {
        get { return soundEnemyparticleSeSubject; }
    }

    /// <summary>
    /// 敵がパーティクルに接触SEサウンド再生イベント発行
    /// </summary>
    public void sendEnemyparticleSeEvent()
    {
        soundEnemyparticleSeSubject.OnNext(Unit.Default);
    }

    // 味方がパーティクルに接触SEサウンド再生イベントを発行する核となるインスタンス
    private Subject<Unit> soundPlayerparticleSeSubject = new Subject<Unit>();

    /// <summary>
    ///  味方がパーティクルに接触SEサウンド再生イベントの購読側だけを公開
    /// </summary>
    public IObservable<Unit> OnSoundPlayerparticleSeChanged
    {
        get { return soundPlayerparticleSeSubject; }
    }

    /// <summary>
    /// 味方がパーティクルに接触SEサウンド再生イベント発行
    /// </summary>
    public void sendPlayerparticleSeEvent()
    {
        soundPlayerparticleSeSubject.OnNext(Unit.Default);
    }

    /// <summary>
    /// BGMサウンド再生
    /// </summary>
    public void PlaySoundBgm(GameObject g)
    {
        if (_soundBgm) _soundBgm.SetActive(false);
        g.SetActive(true);
        _soundBgm = g;
    }

    public void setSoundBgm(GameObject g)
    {
        _soundBgm = g;
    }

    public void setSoundSe(AudioSource a, AudioClip c)
    {
        a.PlayOneShot(c);
    }
}
