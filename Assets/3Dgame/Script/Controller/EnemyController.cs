using Manager;
using Model;
using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UniRx.Async.Triggers;
using UnityEngine;


namespace Controller
{

    public class EnemyController : MonoBehaviour
    {
        private AsyncCollisionTrigger _asyncCollisionTrigger;

        private void Awake()
        {
            //AsyncTrigger（extends MonoBehaviour）を取得する
            _asyncCollisionTrigger = this.GetAsyncCollisionTrigger();
        }

        public async UniTask DoAsync()
        {
            // OnCollisionEnterが発生するまで待機する
            // 敵球が、茶色のパーティクルに触れると、敵が増えます。
            var target = await _asyncCollisionTrigger.OnCollisionEnterAsync();

            var enemyParticleS = GameDataModel.GetEnemyParticleS();
            bool flag = false;
            GameObject item = null;
            foreach (var enemyParticle in enemyParticleS)
            {
                if (target.gameObject == enemyParticle)
                {
                    flag = true;
                    item = enemyParticle;
                    break;
                }
            }

            if (flag)
            {
                UniRxManager.Instance.SendDelEnemyParticleEvent(item);
                UniRxManager.Instance.SendVarEnemyEvent(GameDataModel.GetEnemyS().Count + 1);
                UniRxManager.Instance.SendAddEnemyEvent();
            }

            DoAsync().Forget();


            // OnCollisionExitが発生するまで待つ
            //await asyncCollisionTrigger.OnCollisionExitAsync();

            //Debug.Log("Bye!");
        }

    }
}
