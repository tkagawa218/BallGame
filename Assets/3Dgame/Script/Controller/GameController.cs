using Common;
using Model;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Controller
{

    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private GameView _gameView;

        [SerializeField]
        private GameData _gameData;

        //�G�L�������𑝂₷�g���K�[�ƂȂ�p�[�e�B�N��List(�G�L�������G���̂��g���K�[�ƂȂ�)
        private List<GameObject> _enemyParticleS = new List<GameObject>();

        //�G�L�����������炷�g���K�[�ƂȂ�p�[�e�B�N��List(�����L�������G���̂��g���K�[�ƂȂ�)
        private List<GameObject> _playerParticleS = new List<GameObject>();
        public void Init()
        {
            GameDataModel.Init(_gameData);
        }


        public void GameOn()
        {
            GameDataModel.SetGameOn();
        }

        public bool GetGameOn()
        {
            return GameDataModel.GetGameOn();
        }

        public void ActionByTimeChanged(int t)
        {
            if (GameDataModel.GetGameOn())
            {
                GameDataModel.RestTime = t - 1;
                if (t == 0)
                {
                    GameDataModel.SetGameOff();
                    UniRxManager.Instance.SendEndEvent(true);
                }
                UniRxManager.Instance.SendSetTimeEvent(GameDataModel.RestTime);
            }

        }

        public void AdjustmentEnemyS(GameObject p, GameObject target, int num)
        {
            int diff = num - GameDataModel.GetEnemyS().Count;

            if (diff == 0)
            {
                return;
            }

            if(diff > 0)
            {
                while (diff > 0)
                {
                    AddEnemyS(p, target);
                    diff--;
                }

                return;
            }

            while (diff < 0)
            {
                DelEnemyS();
                diff++;
            }
        }

        /// <summary>
        /// �G�L�����̒ǉ�
        /// </summary>
        /// <param name="p">�G�L�������i�[����e�I�u�W�F�N�g</param>
        /// <param name="target">�G���炪�ǂ�������I�u�W�F�N�g(�����L����)</param>
        public void AddEnemyS(GameObject p, GameObject target)
        {
            var gamedata = GameDataModel.GetGameData();

            var enemyNum = GameDataModel.GetEnemyS().Count;

            int num = 0;
            if (enemyNum + gamedata.enemyInitNum > gamedata.enemyNumMax)
            {
                num = gamedata.enemyNumMax - GameDataModel.GetEnemyS().Count;
            }
            else
            {
                num = gamedata.enemyInitNum;
            }

            var ememyPosS = CommonTool.Shuffle<Vector3>(gamedata.enemyPos);

            for (int i = 0; i < num; i++)
            {
                // Cube�v���n�u�����ɁA�C���X�^���X�𐶐��A
                var obj = Instantiate(_gameView.EnemyPrefab, ememyPosS[i], Quaternion.identity);
               obj.TargetObject = target.transform;

                obj.transform.parent = p.transform;

                GameDataModel.AddEnemyS(obj);
            }

            UniRxManager.Instance.SendVarEnemyEvent(enemyNum);
            if (num > 0) GameSoundManager.Instance.sendEnemyparticleSeEvent();
        }

        /// <summary>
        /// �G�L��������
        /// </summary>
        public void DelEnemyS()
        {
            var gamedata = GameDataModel.GetGameData();

            var enemyNum = GameDataModel.GetEnemyS().Count;

            int num = 0;
            if (enemyNum - gamedata.enemyInitNum <= 0)
            {
                num = 1;
            }
            else
            {
                num = enemyNum - gamedata.enemyInitNum;
            }

            System.Random r = new System.Random();//���̕��Ő錾


            var enemyS = GameDataModel.GetEnemyS().OrderBy(a => r.Next(enemyNum)).ToList();

            GameDataModel.SetEnemyS(enemyS);

            int num_enayS = enemyNum;

            for (int i = 0; i < num_enayS; i++)
            {
                if (i < num) continue;
                if (i >= enemyS.Count) break;

                Destroy(enemyS[i]);
                GameDataModel.RemoveEnemyS(enemyS[i]);
            }
            UniRxManager.Instance.SendVarEnemyEvent(GameDataModel.GetEnemyS().Count);
        }

        /// <summary>
        /// �G�L�����S�ăN���A
        /// </summary>
        public void AllClearEnemyS()
        {
            var enemyS = GameDataModel.GetEnemyS();
            foreach(var enemy in enemyS)
            {
                Destroy(enemy);
            }
            GameDataModel.ClearEnemyS();
        }

        /// <summary>
        /// �G�L�����S�Ă̈ʒu��u��������
        /// </summary>
        public void allExplaceEnemyS()
        {
            var gamedata = GameDataModel.GetGameData();

            var enemyS = GameDataModel.GetEnemyS();

            foreach (var e in enemyS)
            {
                var ememyPosS = gamedata.enemyPos[UnityEngine.Random.Range(0, gamedata.enemyPos.Length)];

                e.transform.position = ememyPosS;
            }
        }

        /// <summary>
        /// �G�L�������Ԃ���ƓG�L������������p�[�e�B�N���폜
        /// </summary>
        /// <param name="item">�폜����p�[�e�B�N��</param>
        public void DellAnyEnemyParticleS(GameObject item)
        {
            Destroy(item);

            GameDataModel.RemoveEnemyParticle(item);
        }

        /// <summary>
        /// �����L�������Ԃ���ƓG�L����������p�[�e�B�N���폜
        /// </summary>
        /// <param name="item">�폜����p�[�e�B�N��</param>
        public void DellAnyPlayerParticleS(GameObject item)
        {
            Destroy(item);

            GameDataModel.RemovePlayerParticle(item);
        }

        /// <summary>
        /// �c��^�C���̐ݒ�
        /// </summary>
        /// <param name="time">�c��^�C��(�b)</param>
        public void setRestTime(int time)
        {
            GameDataModel.RestTime = time;
            UniRxManager.Instance.SendTimeChanged(GameDataModel.RestTime);
        }

        public void InitView()
        {
            _gameView.Start.SetActive(false);
            _gameView.Help.SetActive(false);
            _gameView.GameOver.SetActive(false);
            _gameView.GameWin.SetActive(false);
        }
        public void EndView(bool result)
        {
            if (result)
            {
                _gameView.GameWin.SetActive(true);
                _gameView.Help.SetActive(true);
            }
            else
            {
                _gameView.GameOver.SetActive(true);
                _gameView.Help.SetActive(true);
            }
        }

        public void SetPlayerPos(Vector3 p)
        {
            GameDataModel.SetInitPlayerPos(p);
        }
    }
}