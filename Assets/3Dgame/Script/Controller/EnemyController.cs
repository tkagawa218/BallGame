using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UniRx.Async.Triggers;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject TargetObject; /// 目標位置
    
    private UnityEngine.AI.NavMeshAgent _navMeshAgent; /// NavMeshAgent
    private AsyncCollisionTrigger _asyncCollisionTrigger;

    // Use this for initialization
    void Start()
    {
        // NavMeshAgentコンポーネントを取得
        _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        //AsyncTrigger（extends MonoBehaviour）を取得する
        _asyncCollisionTrigger = this.GetAsyncCollisionTrigger();

        DoAsync().Forget();
    }
    
    // Update is called once per frame
    void Update()
    {
        // NavMeshが準備できているなら
        if (_navMeshAgent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            // NavMeshAgentに目的地をセット
            _navMeshAgent.SetDestination(TargetObject.transform.position);
        }
    }

    async UniTask DoAsync()
    {
        // OnCollisionEnterが発生するまで待機する
        var target = await _asyncCollisionTrigger.OnCollisionEnterAsync();
        
        int n = GameDataManager.Instance.getEnemyParticleS().Count;
        bool flag = false;
        GameObject item = null;
        for (int i = 0; i < n; i++)
        {
            if (target.gameObject.name == GameDataManager.Instance.getEnemyParticleS()[i].name)
            {
                flag = true;
                item = GameDataManager.Instance.getEnemyParticleS()[i];
                break;
            }

        }

        if (flag)
        {
            GameDataManager.Instance.DellAnyEnemyParticleS(item);
            GameDataManager.Instance.addEnemyS(gameObject.transform.parent.gameObject, TargetObject);
        }

        DoAsync().Forget();


        // OnCollisionExitが発生するまで待つ
        //await asyncCollisionTrigger.OnCollisionExitAsync();

        //Debug.Log("Bye!");
    }
}
