using Common;
using Model;
using System;
using UniRx;
using UnityEngine;
using View;

namespace Controller
{
    public class ParticleController : MonoBehaviour
    {
        [SerializeField]
        private ParticleView _particleView;
        

        /// <summary>
        /// 敵キャラの増減のトリガーとなるパーティクル設定
        /// </summary>
        /// <param name="enemyP">敵キャラがぶつかると敵キャラが増えるパーティクル格納用親オブジェクト</param>
        /// <param name="playerP">味方キャラがぶつかると敵キャラが減るパーティクル格納用親オブジェクト</param>
        public void SetParticleS(GameObject enemyP, GameObject playerP)
        {
            DellAllParticleS();

            var gamedata = GameDataModel.GetGameData();

            int enemyParticleNum = gamedata.enemyAreaNum;
            int playerParticleNum = gamedata.playerAreaNum;

            Vector3[] particlePosS = CommonTool.Shuffle<Vector3>(gamedata.areaPos);
            Quaternion rote = Quaternion.Euler(-90.0f, 0.0f, 0.0f);

            for (int i = 0; i < enemyParticleNum; i++)
            {
                // Cubeプレハブを元に、インスタンスを生成、
                var obj = Instantiate(_particleView.EnemyParticlePrefab, particlePosS[i], rote);

                obj.transform.parent = enemyP.transform;
                obj.name = Constant.EnemyParticleNamePre + i.ToString();

                GameDataModel.AddEnemyParticleS(obj);
            }

            for (int i = enemyParticleNum; i < enemyParticleNum + playerParticleNum; i++)
            {
                // Cubeプレハブを元に、インスタンスを生成、
                var obj = Instantiate(_particleView.PlayerParticlePrefab, particlePosS[i], rote);

                obj.transform.parent = playerP.transform;
                obj.name = Constant.PlayerParticleNamePre + i.ToString();

                GameDataModel.AddPlayerParticleS(obj);
            }
        }

        /// <summary>
        /// 敵キャラの増減のトリガーとなるパーティクルを全て削除
        /// </summary>
        public void DellAllParticleS()
        {
            foreach (GameObject e in GameDataModel.GetEnemyParticleS())
            {
                Destroy(e);
            }

            GameDataModel.ClearEnemyParticleS();

            foreach (GameObject e in GameDataModel.GetPlayerParticleS())
            {
                Destroy(e);
            }

            GameDataModel.ClearPlayerParticleS();
        }
    }
}