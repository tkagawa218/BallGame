using UniRx.Async.Triggers;
using UniRx.Async;
using UnityEngine;
using UnityEngine.AI;
using Model;
using UniRx;
using System;
using Controller;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Manager
{

    public class EnemyManager : MonoBehaviour
    {
        [SerializeField]
        private NavMeshAgent _navMeshAgent;// NavMeshAgent

        [SerializeField]
        private EnemyController _enemyController;

        private Transform _targetObject;// –Ú•WˆÊ’u

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
            UniRxManager.Instance.OnStartEvent
            .Subscribe(_ =>
            {
                _enemyController.DoAsync().Forget();
            })
            .AddTo(this);
        }
    }
}
