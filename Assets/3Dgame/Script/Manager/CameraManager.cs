using UniRx;
using UnityEngine;

namespace Manager
{

    public class CameraManager : MonoBehaviour
    {
        [SerializeField]
        private Transform _player;

        private Vector3 _angle;

        private bool _cameraDirOn = false;

        private void Awake()
        {
            var diff = gameObject.transform.position - _player.position;

            _angle = this.gameObject.transform.localEulerAngles;

            Observable.EveryUpdate()
              .Subscribe(_ => {
                  gameObject.transform.position = _player.position + diff;

                  if (_cameraDirOn)
                  {
                      _angle.y += Input.GetAxis("Mouse X");

                      _angle.x -= Input.GetAxis("Mouse Y");

                      transform.localEulerAngles = _angle;
                  }
              })
              .AddTo(this);

            UniRxManager.Instance.OnChangeCameraDirMouseEvent
            .Subscribe(down =>
            {
                _cameraDirOn = down;
            })
            .AddTo(this);
        }
    }
}
