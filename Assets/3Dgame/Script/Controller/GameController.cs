using Common;
using Model;
using System.Collections.Generic;
using System.Linq;
using UniRx.Async;
using UnityEngine;
using View;

namespace Controller
{

    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private GameView _gameView;

        [SerializeField]
        private GameData _gameData;

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

        public void AdjustmentEnemyS(GameObject p, GameObject target, int enemyNum)
        {
            var num = GameDataModel.GetEnemyS().Count;

            if (enemyNum == num)
            {
                return;
            }

            if (enemyNum > num)
            {
                AddEnemyS(p, target, enemyNum);

                return;
            }

            DelEnemyS();
        }

        /// <summary>
        /// 敵キャラの追加
        /// </summary>
        /// <param name="p">敵キャラを格納する親オブジェクト</param>
        /// <param name="target">敵からが追いかけるオブジェクト(味方キャラ)</param>
        /// <param name="enemyNum">敵の数</param>
        public void AddEnemyS(GameObject p, GameObject target, int enemyNum)
        {
            var gamedata = GameDataModel.GetGameData();

            var increaceNum = enemyNum - GameDataModel.GetEnemyS().Count;

            if(increaceNum < gamedata.enemyNumIncreaseRate)
            {
                increaceNum = gamedata.enemyNumIncreaseRate;
            }

            var num = 0;
            if (enemyNum + increaceNum > gamedata.enemyNumMax)
            {
                num = gamedata.enemyNumMax - GameDataModel.GetEnemyS().Count;
            }
            else
            {
                num = increaceNum;
            }

            var ememyPosS = CommonTool.Shuffle<Vector3>(gamedata.enemyPos);

            for (var i = 0; i < num; i++)
            {
                // Cubeプレハブを元に、インスタンスを生成、
                var obj = Instantiate(_gameView.EnemyPrefab, ememyPosS[i], Quaternion.identity);
               obj.TargetObject = target.transform;

                obj.transform.parent = p.transform;

                GameDataModel.AddEnemyS(obj);
            }

            UniRxManager.Instance.SendChangeEnemyNumEvent(GameDataModel.GetEnemyS().Count);
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

            int num_enamyS = enemyNum;

            for (int i = 0; i < num_enamyS; i++)
            {
                if (i < num) continue;
                if (i >= enemyS.Count) break;

                Destroy(enemyS[i].gameObject);
                GameDataModel.RemoveEnemyS(enemyS[i]);
            }

            UniRxManager.Instance.SendChangeEnemyNumEvent(GameDataModel.GetEnemyS().Count);
        }

        /// <summary>
        /// 敵キャラ全てクリア
        /// </summary>
        public void AllClearEnemyS()
        {
            var enemyS = GameDataModel.GetEnemyS();
            foreach(var enemy in enemyS)
            {
                if (enemy != null)
                {
                    Destroy(enemy.gameObject);
                }
            }
            GameDataModel.ClearEnemyS();
        }

        /// <summary>
        /// 敵キャラ全ての位置を置き換える
        /// </summary>
        public void AllExplaceEnemyS()
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
        /// 敵キャラがぶつかると敵キャラが増えるパーティクル全て削除
        /// </summary>
        public void AllClearEnemyParticleS()
        {
            var allEnemyParticleS = GameDataModel.GetEnemyParticleS().ToArray();

            foreach (var enemyParticle in allEnemyParticleS)
            {
                DellAnyEnemyParticleS(enemyParticle);
            }
        }

        /// <summary>
        /// 敵キャラがぶつかると敵キャラが増えるパーティクル削除
        /// </summary>
        /// <param name="item">削除するパーティクル</param>
        public void DellAnyEnemyParticleS(GameObject item)
        {
            GameDataModel.RemoveEnemyParticle(item);
            Destroy(item);
        }

        /// <summary>
        /// 味方キャラがぶつかると敵キャラが減るパーティクル全て削除
        /// </summary>
        public void AllClearPlayerParticleS()
        {
            var allPlayerParticleS = GameDataModel.GetPlayerParticleS();
            foreach (var playerParticle in allPlayerParticleS)
            {
                DellAnyEnemyParticleS(playerParticle);
            }
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
            _gameView.SetBgImageAlpha(false);
        }

        public void EndView()
        {
            AllClearEnemyS();
            AllClearEnemyParticleS();
            AllClearPlayerParticleS();

            //_gameView.SetBgImageAlpha(true);
        }

        public void SetPlayerPos(Vector3 p)
        {
            GameDataModel.SetInitPlayerPos(p);
        }
    }
}