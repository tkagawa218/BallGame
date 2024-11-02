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

        private Transform _targetObject;// �ڕW�ʒu

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
               // NavMesh�������ł��Ă���Ȃ�
               if (_navMeshAgent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathInvalid)
               {
                   // NavMeshAgent�ɖړI�n���Z�b�g
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
