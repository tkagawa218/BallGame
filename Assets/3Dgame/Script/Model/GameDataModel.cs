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

        //�G�L�����z��
        private static List<EnemyManager> _enemyS = new List<EnemyManager>();

        //�G�L�������𑝂₷�g���K�[�ƂȂ�p�[�e�B�N��List(�G�L�������G���̂��g���K�[�ƂȂ�)
        private static List<GameObject> _enemyParticleS = new List<GameObject>();

        //�G�L�����������炷�g���K�[�ƂȂ�p�[�e�B�N��List(�����L�������G���̂��g���K�[�ƂȂ�)
        private static List<GameObject> _playerParticleS = new List<GameObject>();

        //�Q�[���̍Œ���
        private static bool _gameOn = false;

        //�����L�����̏����̈ʒu
        private static Vector3 _initPlayerPos;

        //�c�莞��(�b)
        private static int restTime;

        public static void Init(GameData gameData)
        {
            SetGameData(gameData);
        }


        public static bool GetGameOn() { return _gameOn; }

        /// <summary>
        /// �Q�[���̍Œ��̃t���O�����Ă�
        /// </summary>
        public static void SetGameOn()
        {
            _gameOn = true;
        }

        //�c�莞��(�b)
        public static int _restTime;

 
        public static int RestTime
        {
            get { return _restTime; }
            set { _restTime = value; }
        }


        /// <summary>
        /// �Q�[���̍Œ��̃t���O�����Ƃ�
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