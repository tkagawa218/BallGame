using Common;
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

    // 初期状態イベントを発行する核となるインスタンス
    private Subject<Unit> initSubject = new Subject<Unit>();

    /// <summary>
    /// 初期状態イベントの購読側だけを公開
    /// </summary>
    public IObservable<Unit> OnInitEvent
    {
        get { return initSubject; }
    }

    /// <summary>
    /// 初期状態イベント発行
    /// </summary>
    public void SendInitEvent()
    {
        initSubject.OnNext(Unit.Default);
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
    /// 敵数変化イベントの購読側だけを公開
    /// </summary>
    public IObservable<int> OnVarEnemyEvent
    {
        get { return varEnemySubject; }
    }

    /// <summary>
    /// 敵数変化イベント発行
    /// </summary>
    /// <param name="enemyNum">敵キャラの数</param>
    public void SendVarEnemyEvent(int enemyNum)
    {
        varEnemySubject.OnNext(enemyNum);
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

    //パーティクル配置取り消しイベントを発行する核となるインスタンス
    private Subject<Unit> unSetParticleSubject = new Subject<Unit>();

    /// <summary>
    /// パーティクル配置取り消しイベントの購読側だけを公開
    /// </summary>
    public IObservable<Unit> OnUnSetParticleEvent
    {
        get { return unSetParticleSubject; }
    }


    //パーティクル配置取り消しイベント発行
    public void UnSendSetParticleEvent()
    {
        unSetParticleSubject.OnNext(Unit.Default);
    }

    //敵パーティクル削除イベントを発行する核となるインスタンス
    private Subject<GameObject> _delEnemyParticleSubject = new Subject<GameObject>();

    /// <summary>
    /// 敵パーティクル削除イベントの購読側だけを公開
    /// </summary>
    public IObservable<GameObject> OnDelEnemyParticleEvent
    {
        get { return _delEnemyParticleSubject; }
    }


    //敵パーティクル削除イベント発行
    public void SendDelEnemyParticleEvent(GameObject item)
    {
        _delEnemyParticleSubject.OnNext(item);
    }

    //味方パーティクル削除イベントを発行する核となるインスタンス
    private Subject<GameObject> _delPlayerParticleSubject = new Subject<GameObject>();

    /// <summary>
    /// 味方パーティクル削除イベントの購読側だけを公開
    /// </summary>
    public IObservable<GameObject> OnDelPlayerParticleEvent
    {
        get { return _delPlayerParticleSubject; }
    }


    //味方パーティクル削除イベント発行
    public void SendDelPlayerParticleEvent(GameObject item)
    {
        _delPlayerParticleSubject.OnNext(item);
    }

    // Time設定イベントを発行する核となるインスタンス
    private Subject<int> setTimeSubject = new Subject<int>();

    /// <summary>
    /// Time設定イベントの購読側だけを公開
    /// </summary>
    public IObservable<int> OnSetTimeEvent
    {
        get { return setTimeSubject; }
    }

    // Time設定イベント発行
    public void SendSetTimeEvent(int time)
    {
        setTimeSubject.OnNext(time);
    }

    // startボタン表示非表示イベントを発行する核となるインスタンス
    private Subject<bool> _startButtonSubject = new Subject<bool>();

    /// <summary>
    /// startボタン表示非表示イベントの購読側だけを公開
    /// </summary>
    public IObservable<bool> OnStartButtonEvent
    {
        get { return _startButtonSubject; }
    }

    // startボタン表示非表示イベント発行
    public void SendStartButtonEvent(bool b)
    {
        _startButtonSubject.OnNext(b);
    }

    //player方向指示イベントを発行する核となるインスタンス
    private Subject<PlayerDirection> _playerDirectionSubject = new Subject<PlayerDirection>();

    /// <summary>
    /// player方向指示イベントの購読側だけを公開
    /// </summary>
    public IObservable<PlayerDirection> OnPlayerDirectionEvent
    {
        get { return _playerDirectionSubject; }
    }

    // player方向指示イベント発行
    public void SendPlayerDirectionEvent(PlayerDirection p)
    {
        _playerDirectionSubject.OnNext(p);
    }

    //敵数変更イベントを発行する核となるインスタンス
    private Subject<int> changeEnemyNumSubject = new Subject<int>();

    /// <summary>
    /// 敵数変更イベントの購読側だけを公開
    /// </summary>
    public IObservable<int> OnChangeEnemyNumEvent
    {
        get { return changeEnemyNumSubject; }
    }

    /// <summary>
    /// 敵数変更イベント発行
    /// </summary>
    /// <param name="num">敵キャラの数</param>
    public void SendChangeEnemyNumEvent(int num)
    {
        changeEnemyNumSubject.OnNext(num);
    }
}
