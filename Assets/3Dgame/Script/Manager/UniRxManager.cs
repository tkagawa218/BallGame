using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class UniRxManager : SingletonMonoBehaviour<UniRxManager>
{
    //タイマーイベントを発行する核となるインスタンス
    private Subject<int> timerSubject = new Subject<int>();

    /// <summary>
    /// タイマーイベントの購読側だけを公開
    /// </summary>
    public IObservable<int> OnTimeChanged
    {
        get { return timerSubject; }
    }

    /// <summary>
    /// タイマーイベント発行
    /// </summary>
    /// <param name="time">設定する秒</param>
    public void SendTimeChanged(int time)
    {
        timerSubject.OnNext(time);
    }

    //ゲームスタートイベントを発行する核となるインスタンス
    private Subject<Unit> startSubject = new Subject<Unit>();

    /// <summary>
    /// ゲームスタートイベントの購読側だけを公開
    /// </summary>
    public IObservable<Unit> OnStartEvent
    {
        get { return startSubject; }
    }

    /// <summary>
    /// ゲームスタートイベント発行
    /// </summary>
    public void SendStartEvent()
    {
        startSubject.OnNext(Unit.Default);
    }

    //ゲーム終了イベントを発行する核となるインスタンス (true:勝利 false:敗北)
    private Subject<bool> endSubject = new Subject<bool>();

    /// <summary>
    /// ゲーム終了イベントの購読側だけを公開
    ///true:勝利
    ///false:敗北
    /// </summary>
    public IObservable<bool> OnEndEvent
    {
        get { return endSubject; }
    }

    /// <summary>
    /// ゲーム終了イベント発行
    /// </summary>
    /// <param name="result">true:勝利 false:敗北</param>
    public void SendEndEvent(bool result)
    {
        endSubject.OnNext(result);
    }

    //敵数変化イベントを発行する核となるインスタンス
    private Subject<int> varEnemySubject = new Subject<int>();

    /// <summary>
    /// 敵増加イベントの購読側だけを公開
    /// </summary>
    public IObservable<int> OnVarEnemyEvent
    {
        get { return varEnemySubject; }
    }

    /// <summary>
    /// 敵キャラ増加イベント発行
    /// </summary>
    /// <param name="num">敵キャラの数</param>
    public void SendVarEnemyEvent(int num)
    {
        varEnemySubject.OnNext(num);
    }

    //パーティクル配置イベントを発行する核となるインスタンス
    private Subject<Unit> setParticleSubject = new Subject<Unit>();

    /// <summary>
    /// パーティクル配置イベントの購読側だけを公開
    /// </summary>
    public IObservable<Unit> OnSetParticleEvent
    {
        get { return setParticleSubject; }
    }


    //パーティクル配置イベント発行
    public void SendSetParticleEvent()
    {
        setParticleSubject.OnNext(Unit.Default);
    }
}
