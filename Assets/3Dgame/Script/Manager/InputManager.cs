using Common;
using Manager;
using UniRx;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private void Awake()
    {
        Observable.EveryUpdate()
          .Where(_ => Input.GetKey(KeyCode.UpArrow)
                   || Input.GetKey(KeyCode.DownArrow)
                   || Input.GetKey(KeyCode.LeftArrow)
                   || Input.GetKey(KeyCode.RightArrow)
                   )
          .Subscribe(_ => {
              if (Input.GetKey(KeyCode.UpArrow))
              {
                  UniRxManager.Instance.SendPlayerDirectionEvent(PlayerDirection.UPARROW);
              }
              else if (Input.GetKey(KeyCode.DownArrow))
              {
                  UniRxManager.Instance.SendPlayerDirectionEvent(PlayerDirection.DOWNARROW);
              }
              else if (Input.GetKey(KeyCode.LeftArrow))
              {
                  UniRxManager.Instance.SendPlayerDirectionEvent(PlayerDirection.LEFTARROW);
              }
              else if (Input.GetKey(KeyCode.RightArrow))
              {
                  UniRxManager.Instance.SendPlayerDirectionEvent(PlayerDirection.RIGHTARROW);
              }
          })
          .AddTo(this);

        Observable.EveryUpdate()
          .Where(_ => !Input.GetKey(KeyCode.UpArrow)
                   && !Input.GetKey(KeyCode.DownArrow)
                   && !Input.GetKey(KeyCode.LeftArrow)
                   && !Input.GetKey(KeyCode.RightArrow)
                   )
          .Subscribe(_ => {
              UniRxManager.Instance.SendPlayerDirectionEvent(PlayerDirection.NONE);
          })
          .AddTo(this);
    }
}
