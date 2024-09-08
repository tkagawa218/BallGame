using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ButtonController : MonoBehaviour
{
    [SerializeField]
    private ButtonView _buttonView;

    private void Awake()
    {
        _buttonView.StartButton.onClick.AsObservable()
        .Subscribe(_ =>
        {
            UniRxManager.Instance.SendStartEvent();
        });
    }

    public void SetStartActive(bool b)
    {
        _buttonView.StartButton.gameObject.SetActive(b);
    }
}
