using Common;
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

   

    public bool getGameOn() { return _gameOn; }

    public int getMinX() { return _minX; }
    public int getMaxX() { return _maxX; }
    public int getMinZ() { return _minZ; }
    public int getMaxZ() { return _maxZ; }

    /// <summary>
    /// ゲームの最中のフラグをおとす
    /// </summary>
    public void setGameOff(bool result) {
        _gameOn = false;
        UniRxManager.Instance.SendEndEvent(result);
    }

    public void setInitPlayerPos(Vector3 p) { initPlayerPos = p; }

    public List<GameObject> getEnemyS()
    {
        return _enemyS;
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
        UniRxManager.Instance.SendVarEnemyEvent(_enemyS.Count);
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
        UniRxManager.Instance.SendVarEnemyEvent(_enemyS.Count);
    }

    /// <summary>
    /// 敵キャラ全ての位置を置き換える
    /// </summary>
    public void allExplaceEnemyS()
    {
        foreach (GameObject e in _enemyS)
        {
            Vector3 ememyPosS = GameData.Instance.enemyPos[UnityEngine.Random.Range(0, GameData.Instance.enemyPos.Length)];

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

        Vector3[] particlePosS = CommonTool.Shuffle<Vector3>(gamedata.areaPos);
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

  }
