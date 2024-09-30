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

        //敵キャラ数を増やすトリガーとなるパーティクルList(敵キャラが触れるのがトリガーとなる)
        private List<GameObject> _enemyParticleS = new List<GameObject>();

        //敵キャラ数を減らすトリガーとなるパーティクルList(味方キャラが触れるのがトリガーとなる)
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
        /// 敵キャラの追加
        /// </summary>
        /// <param name="p">敵キャラを格納する親オブジェクト</param>
        /// <param name="target">敵からが追いかけるオブジェクト(味方キャラ)</param>
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
                // Cubeプレハブを元に、インスタンスを生成、
                var obj = Instantiate(_gameView.EnemyPrefab, ememyPosS[i], Quaternion.identity);
               obj.TargetObject = target.transform;

                obj.transform.parent = p.transform;

                GameDataModel.AddEnemyS(obj);
            }

            UniRxManager.Instance.SendVarEnemyEvent(enemyNum);
            if (num > 0) GameSoundManager.Instance.sendEnemyparticleSeEvent();
        }

        /// <summary>
        /// 敵キャラ減少
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

            System.Random r = new System.Random();//頭の方で宣言


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
        /// 敵キャラ全てクリア
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
        /// 敵キャラ全ての位置を置き換える
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
        /// 敵キャラがぶつかると敵キャラが増えるパーティクル削除
        /// </summary>
        /// <param name="item">削除するパーティクル</param>
        public void DellAnyEnemyParticleS(GameObject item)
        {
            Destroy(item);

            GameDataModel.RemoveEnemyParticle(item);
        }

        /// <summary>
        /// 味方キャラがぶつかると敵キャラが減るパーティクル削除
        /// </summary>
        /// <param name="item">削除するパーティクル</param>
        public void DellAnyPlayerParticleS(GameObject item)
        {
            Destroy(item);

            GameDataModel.RemovePlayerParticle(item);
        }

        /// <summary>
        /// 残りタイムの設定
        /// </summary>
        /// <param name="time">残りタイム(秒)</param>
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