using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class GameDataManager : SingletonMonoBehaviour<GameDataManager>
{
    private const string enemyPrefabPath = "3Dgame/Prefab/Enemy";
    private const string scritablePath = "3Dgame/GameData";
    private const string enemyParticlePrefabPath = "3Dgame/Prefab/EnemyParticle";
    private const string playerParticlePrefabPath = "3Dgame/Prefab/PlayerParticle";

    //敵キャラ配列
    private List<GameObject> _enemyS = new List<GameObject>();

    //敵キャラ数を増やすトリガーとなるパーティクルList(敵キャラが触れるのがトリガーとなる)
    private List<GameObject> _enemyParticleS = new List<GameObject>();

    //敵キャラ数を減らすトリガーとなるパーティクルList(味方キャラが触れるのがトリガーとなる)
    private List<GameObject> _playerParticleS = new List<GameObject>();

    //ゲームの最中か
    private bool _gameOn = false;

    //味方キャラの初期の位置
    private Vector3 initPlayerPos;

    //残り時間(秒)
    private int restTime;

    //プレイヤー移動範囲
    private int _minX = 0;
    private int _maxX = 0;
    private int _minZ = 0;
    private int _maxZ = 0;

    //タイマーイベントを発行する核となるインスタンス
    private Subject<int> timerSubject = new Subject<int>();

    //ゲームスタートイベントを発行する核となるインスタンス
    private Subject<Unit> startSubject = new Subject<Unit>();
    //ゲーム終了イベントを発行する核となるインスタンス (true:勝利 false:敗北)
    private Subject<bool> endSubject = new Subject<bool>();

    //敵数変化イベントを発行する核となるインスタンス
    private Subject<int> varEnemySubject = new Subject<int>();

    //パーティクル配置イベントを発行する核となるインスタンス
    private Subject<Unit> setParticleSubject = new Subject<Unit>();

    public bool getGameOn() { return _gameOn; }

    public int getMinX() { return _minX; }
    public int getMaxX() { return _maxX; }
    public int getMinZ() { return _minZ; }
    public int getMaxZ() { return _maxZ; }

    /// <summary>
    /// ゲームの最中のフラグをたてる
    /// </summary>
    public void setGameOn()
    {
        _gameOn = true;
        GameData gamedata = Resources.Load<GameData>(scritablePath);
        _minX = gamedata.minX;
        _maxX = gamedata.maxX;
        _minZ = gamedata.minZ;
        _maxZ = gamedata.maxZ;
        sendStartEvent();
    }

    /// <summary>
    /// ゲームの最中のフラグをおとす
    /// </summary>
    public void setGameOff(bool result) {
        _gameOn = false;
        sendEndEvent(result);
    }

    public Vector3 getInitPlayerPos() { return initPlayerPos; }
    public void setInitPlayerPos(Vector3 p) { initPlayerPos = p; }

    public List<GameObject> getEnemyS()
    {
        return _enemyS;
    }

    /// <summary>
    /// 敵キャラの追加
    /// </summary>
    /// <param name="p">敵キャラを格納する親オブジェクト</param>
    /// <param name="target">敵からが追いかけるオブジェクト(味方キャラ)</param>
    public void addEnemyS(GameObject p, GameObject target)
    {
        GameData gamedata = Resources.Load<GameData>(scritablePath);

        int num = 0;
        if (_enemyS.Count + gamedata.enemyInitNum > gamedata.enemyNumMax)
        {
            num = gamedata.enemyNumMax - _enemyS.Count;
        }
        else
        {
            num = gamedata.enemyInitNum;
        }

        Vector3[] ememyPosS = Common.Shuffle<Vector3>(gamedata.enemyPos);

        for (int i = 0; i < num; i++)
        {
            // EnemyプレハブをGameObject型で取得
            GameObject prefabObj = (GameObject)Resources.Load(enemyPrefabPath);
            // Cubeプレハブを元に、インスタンスを生成、
            GameObject obj = Instantiate(prefabObj, ememyPosS[i], Quaternion.identity);
            EnemyController e = obj.GetComponent<EnemyController>();
            e.TargetObject = target;

            obj.transform.parent = p.transform;

            _enemyS.Add(obj);
        }
        sendVarEnemyEvent(_enemyS.Count);
        if(num > 0) GameSoundManager.Instance.sendEnemyparticleSeEvent();
    }

    /// <summary>
    /// 敵キャラ減少
    /// </summary>
    public void DelEnemyS()
    {
        GameData gamedata = Resources.Load<GameData>(scritablePath);

        int num = 0;
        if (_enemyS.Count - gamedata.enemyInitNum <= 0)
        {
            num = 1;
        }
        else
        {
            num = _enemyS.Count - gamedata.enemyInitNum;
        }

        System.Random r = new System.Random();//頭の方で宣言


        _enemyS = _enemyS.OrderBy(a => r.Next(_enemyS.Count)).ToList();

        int num_enayS = _enemyS.Count;

        for (int i = 0; i < num_enayS; i++)
        {
            if (i < num) continue;
            if( i >= _enemyS.Count) break;

            Destroy(_enemyS[i]);
            _enemyS.Remove(_enemyS[i]);
        }
        sendVarEnemyEvent(_enemyS.Count);
    }

    /// <summary>
    /// 敵キャラを全て削除
    /// </summary>
    public void allDellEnemyS()
    {
        foreach (GameObject e in _enemyS)
        {
            Destroy(e);
        }

        _enemyS.Clear();
        sendVarEnemyEvent(_enemyS.Count);
    }

    /// <summary>
    /// 敵キャラ全ての位置を置き換える
    /// </summary>
    public void allExplaceEnemyS()
    {
        GameData gamedata = Resources.Load<GameData>(scritablePath);

        foreach (GameObject e in _enemyS)
        {
            Vector3 ememyPosS = gamedata.enemyPos[UnityEngine.Random.Range(0, gamedata.enemyPos.Length)];

            e.transform.position = ememyPosS;
        }
    }

    public List<GameObject> getPlayerParticleS()
    {
        return _playerParticleS;
    }

    public List<GameObject> getEnemyParticleS()
    {
        return _enemyParticleS;
    }

    /// <summary>
    /// 敵キャラの増減のトリガーとなるパーティクル設定
    /// </summary>
    /// <param name="enemyP">敵キャラがぶつかると敵キャラが増えるパーティクル格納用親オブジェクト</param>
    /// <param name="playerP">味方キャラがぶつかると敵キャラが減るパーティクル格納用親オブジェクト</param>
    public void SetParticleS(GameObject enemyP, GameObject playerP)
    {
        DellAllParticleS();

        GameData gamedata = Resources.Load<GameData>(scritablePath);

        int enemyParticleNum = gamedata.enemyAreaNum;
        int playerParticleNum = gamedata.playerAreaNum;

        Vector3[] particlePosS = Common.Shuffle<Vector3>(gamedata.areaPos);
        Quaternion rote = Quaternion.Euler(-90.0f, 0.0f, 0.0f);

        for (int i = 0; i < enemyParticleNum; i++)
        {
            // EnemyパーティクルプレハブをGameObject型で取得
            GameObject prefabObj = (GameObject)Resources.Load(enemyParticlePrefabPath);

            // Cubeプレハブを元に、インスタンスを生成、
            GameObject obj = Instantiate(prefabObj, particlePosS[i], rote);

            obj.transform.parent = enemyP.transform;
            obj.name = "EnemyParticle" + i.ToString();

            _enemyParticleS.Add(obj);
        }

        for (int i = enemyParticleNum; i < enemyParticleNum + playerParticleNum; i++)
        {
            // PlayerパーティクルプレハブをGameObject型で取得
            GameObject prefabObj = (GameObject)Resources.Load(playerParticlePrefabPath);
            // Cubeプレハブを元に、インスタンスを生成、
            GameObject obj = Instantiate(prefabObj, particlePosS[i], rote);

            obj.transform.parent = playerP.transform;
            obj.name = "PlayerParticle" + i.ToString();
            _playerParticleS.Add(obj);
        }
    }

    /// <summary>
    /// 敵キャラの増減のトリガーとなるパーティクルを全て削除
    /// </summary>
    public void DellAllParticleS()
    {
        foreach (GameObject e in _enemyParticleS)
        {
            Destroy(e);
        }

        _enemyParticleS.Clear();

        foreach (GameObject e in _playerParticleS)
        {
            Destroy(e);
        }

        _playerParticleS.Clear();
    }

    /// <summary>
    /// 敵キャラがぶつかると敵キャラが増えるパーティクル削除
    /// </summary>
    /// <param name="item">削除するパーティクル</param>
    public void DellAnyEnemyParticleS(GameObject item)
    {
        Destroy(item);

        _enemyParticleS.Remove(item);

    }

    /// <summary>
    /// 味方キャラがぶつかると敵キャラが減るパーティクル削除
    /// </summary>
    /// <param name="item">削除するパーティクル</param>
    public void DellAnyPlayerParticleS(GameObject item)
    {
        Destroy(item);

        _playerParticleS.Remove(item);
    }

    public string GetScritablePath()
    {
        return scritablePath;
    }

    /// <summary>
    /// 残りタイムの設定
    /// </summary>
    /// <param name="time">残りタイム(秒)</param>
    public void setRestTime(int time)
    {
        restTime = time;
        sendTimeChanged(restTime);
    }

    /// <summary>
    /// タイマーイベントの購読側だけを公開
    /// </summary>
    public IObservable<int> OnTimeChanged
    {
        get { return timerSubject; }
    }

    /// <summary>
    /// ゲームスタートイベントの購読側だけを公開
    /// </summary>
    public IObservable<Unit> OnStartEvent
    {
        get { return startSubject; }
    }

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
    /// 敵増加イベントの購読側だけを公開
    /// </summary>
    public IObservable<int> OnVarEnemyEvent
    {
        get { return varEnemySubject; }
    }

    /// <summary>
    /// パーティクル配置イベントの購読側だけを公開
    /// </summary>
    public IObservable<Unit> OnSetParticleEvent
    {
        get { return setParticleSubject; }
    }

    public List<GameObject> EnemyS { get => _enemyS; set => _enemyS = value; }

    /// <summary>
    /// タイマーイベント発行
    /// </summary>
    /// <param name="time">設定する秒</param>
    public void sendTimeChanged(int time)
    {
        timerSubject.OnNext(time);
    }

    /// <summary>
    /// ゲームスタートイベント発行
    /// </summary>
    public void sendStartEvent()
    {
        startSubject.OnNext(Unit.Default);
    }

    /// <summary>
    /// ゲーム終了イベント発行
    /// </summary>
    /// <param name="result">true:勝利 false:敗北</param>
    public void sendEndEvent(bool result)
    {
        endSubject.OnNext(result);
    }

    /// <summary>
    /// 敵キャラ増加イベント発行
    /// </summary>
    /// <param name="num">敵キャラの数</param>
    public void sendVarEnemyEvent(int num)
    {
        varEnemySubject.OnNext(num);
    }

    //パーティクル配置イベント発行
    public void sendSetParticleEvent()
    {
        setParticleSubject.OnNext(Unit.Default);
    }
}
