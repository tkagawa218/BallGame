using UniRx.Async;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using Controller;
using Common;
using System.Threading;

namespace Manager
{

    public class EnemyManager : MonoBehaviour
    {
        [SerializeField]
        private NavMeshAgent _navMeshAgent;// NavMeshAgent

        [SerializeField]
        private EnemyController _enemyController;

        private Transform _targetObject;// �ڕW�ʒu
        private CancellationTokenSource _cancellationTokenSource = null;
        private CancellationToken _cancellationToken;

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

        private void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
        }

        private void Awake()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;

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
            _enemyController.DoAsync(_cancellationTokenSource).Forget();
        }
    }
}
