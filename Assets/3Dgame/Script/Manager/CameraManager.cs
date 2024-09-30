using UniRx;
using UnityEngine;

namespace Manager
{

    public class CameraManager : MonoBehaviour
    {
        [SerializeField]
        private Transform _player;

        private void Awake()
        {
            var diff = gameObject.transform.position - _player.position;
           
            Observable.EveryUpdate()
              .Subscribe(_ => {
                  //gameObject.transform.LookAt(_player.position);
                  gameObject.transform.position = _player.position + diff;
              })
              .AddTo(this);
        }
    }
}
