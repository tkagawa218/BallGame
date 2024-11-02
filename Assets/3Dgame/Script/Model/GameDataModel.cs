using Manager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

namespace Model
{
    public class GameDataModel
    {
        private static GameData _gameData;

        //敵キャラ配列
        private static List<EnemyManager> _enemyS = new List<EnemyManager>();

        //敵キャラ数を増やすトリガーとなるパーティクルList(敵キャラが触れるのがトリガーとなる)
        private static List<GameObject> _enemyParticleS = new List<GameObject>();

        //敵キャラ数を減らすトリガーとなるパーティクルList(味方キャラが触れるのがトリガーとなる)
        private static List<GameObject> _playerParticleS = new List<GameObject>();

        //ゲームの最中か
        private static bool _gameOn = false;

        //味方キャラの初期の位置
        private static Vector3 _initPlayerPos;

        //残り時間(秒)
        private static int restTime;

        public static void Init(GameData gameData)
        {
            SetGameData(gameData);
        }


        public static bool GetGameOn() { return _gameOn; }

        /// <summary>
        /// ゲームの最中のフラグをたてる
        /// </summary>
        public static void SetGameOn()
        {
            _gameOn = true;
        }

        //残り時間(秒)
        public static int _restTime;

 
        public static int RestTime
        {
            get { return _restTime; }
            set { _restTime = value; }
        }


        /// <summary>
        /// ゲームの最中のフラグをおとす
        /// </summary>
        public static void SetGameOff()
        {
            _gameOn = false;
        }

        public static Vector3 GetInitPlayerPos() { return _initPlayerPos; }

        public static void SetInitPlayerPos(Vector3 p) { _initPlayerPos = p; }

        public static GameData GetGameData() { return _gameData; }

        public static void SetGameData(GameData g) { _gameData = g; }

        public static List<EnemyManager> GetEnemyS()
        {
            return _enemyS;
        }

        public static void AddEnemyS(EnemyManager e)
        {
            _enemyS.Add(e);
        }

        public static void RemoveEnemyS(EnemyManager e)
        {
            _enemyS.Remove(e);
        }

        public static void ClearEnemyS()
        {
            _enemyS.Clear();
        }

        public static void SetEnemyS(List<EnemyManager> elist)
        {
            _enemyS.Clear();
            _enemyS = elist;
        }

        public static IEnumerable<GameObject> GetPlayerParticleS()
        {
            return _playerParticleS;
        }

        public static void AddPlayerParticleS(GameObject e)
        {
            _playerParticleS.Add(e);
        }

        public static IEnumerable<GameObject> GetEnemyParticleS()
        {
            return _enemyParticleS;
        }

        public static void AddEnemyParticleS(GameObject e)
        {
            _enemyParticleS.Add(e);
        }

        public static void ClearEnemyParticleS()
        {
            _enemyParticleS.Clear();
        }

        public static void ClearPlayerParticleS()
        {
            _playerParticleS.Clear();
        }

        public static void RemovePlayerParticle(GameObject item)
        {
            _playerParticleS.Remove(item);
        }

        public static void RemoveEnemyParticle(GameObject item)
        {
            _enemyParticleS.Remove(item);
        }
    }
}