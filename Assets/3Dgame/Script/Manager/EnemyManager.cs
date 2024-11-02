using UniRx.Async;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using Controller;
using Common;

namespace Manager
{

    public class EnemyManager : MonoBehaviour
    {
        [SerializeField]
        private NavMeshAgent _navMeshAgent;// NavMeshAgent

        [SerializeField]
        private EnemyController _enemyController;

        private Transform _targetObject;// 目標位置

        public Transform TargetObject
        {
            get
            {
                return _targetObject;
            }
            set
            {
                _targetObject = value;
            }
        }

        private void Awake()
        {
            Observable.EveryUpdate()
           .Subscribe(_ => {
               // NavMeshが準備できているなら
               if (_navMeshAgent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathInvalid)
               {
                   // NavMeshAgentに目的地をセット
                   _navMeshAgent.SetDestination(TargetObject.transform.position);
               }
           })
            .AddTo(this);
        }

        private void Start()
        {
            _enemyController.DoAsync().Forget();
        }
    }
}
