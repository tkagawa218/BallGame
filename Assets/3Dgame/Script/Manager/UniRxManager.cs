using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class UniRxManager : SingletonMonoBehaviour<UniRxManager>
{
    //�^�C�}�[�C�x���g�𔭍s����j�ƂȂ�C���X�^���X
    private Subject<int> timerSubject = new Subject<int>();

    /// <summary>
    /// �^�C�}�[�C�x���g�̍w�Ǒ����������J
    /// </summary>
    public IObservable<int> OnTimeChanged
    {
        get { return timerSubject; }
    }

    /// <summary>
    /// �^�C�}�[�C�x���g���s
    /// </summary>
    /// <param name="time">�ݒ肷��b</param>
    public void SendTimeChanged(int time)
    {
        timerSubject.OnNext(time);
    }

    // ������ԃC�x���g�𔭍s����j�ƂȂ�C���X�^���X
    private Subject<Unit> initSubject = new Subject<Unit>();

    /// <summary>
    /// ������ԃC�x���g�̍w�Ǒ����������J
    /// </summary>
    public IObservable<Unit> OnInitEvent
    {
        get { return initSubject; }
    }

    /// <summary>
    /// ������ԃC�x���g���s
    /// </summary>
    public void SendInitEvent()
    {
        initSubject.OnNext(Unit.Default);
    }

    //�Q�[���X�^�[�g�C�x���g�𔭍s����j�ƂȂ�C���X�^���X
    private Subject<Unit> startSubject = new Subject<Unit>();

    /// <summary>
    /// �Q�[���X�^�[�g�C�x���g�̍w�Ǒ����������J
    /// </summary>
    public IObservable<Unit> OnStartEvent
    {
        get { return startSubject; }
    }

    /// <summary>
    /// �Q�[���X�^�[�g�C�x���g���s
    /// </summary>
    public void SendStartEvent()
    {
        startSubject.OnNext(Unit.Default);
    }

    //�Q�[���I���C�x���g�𔭍s����j�ƂȂ�C���X�^���X (true:���� false:�s�k)
    private Subject<bool> endSubject = new Subject<bool>();

    /// <summary>
    /// �Q�[���I���C�x���g�̍w�Ǒ����������J
    ///true:����
    ///false:�s�k
    /// </summary>
    public IObservable<bool> OnEndEvent
    {
        get { return endSubject; }
    }

    /// <summary>
    /// �Q�[���I���C�x���g���s
    /// </summary>
    /// <param name="result">true:���� false:�s�k</param>
    public void SendEndEvent(bool result)
    {
        endSubject.OnNext(result);
    }

    //�G���ω��C�x���g�𔭍s����j�ƂȂ�C���X�^���X
    private Subject<int> varEnemySubject = new Subject<int>();

    /// <summary>
    /// �G���ω��C�x���g�̍w�Ǒ����������J
    /// </summary>
    public IObservable<int> OnVarEnemyEvent
    {
        get { return varEnemySubject; }
    }

    /// <summary>
    /// �G���ω��C�x���g���s
    /// </summary>
    /// <param name="enemyNum">�G�L�����̐�</param>
    public void SendVarEnemyEvent(int enemyNum)
    {
        varEnemySubject.OnNext(enemyNum);
    }

    //�p�[�e�B�N���z�u�C�x���g�𔭍s����j�ƂȂ�C���X�^���X
    private Subject<Unit> setParticleSubject = new Subject<Unit>();

    /// <summary>
    /// �p�[�e�B�N���z�u�C�x���g�̍w�Ǒ����������J
    /// </summary>
    public IObservable<Unit> OnSetParticleEvent
    {
        get { return setParticleSubject; }
    }


    //�p�[�e�B�N���z�u�C�x���g���s
    public void SendSetParticleEvent()
    {
        setParticleSubject.OnNext(Unit.Default);
    }

    //�p�[�e�B�N���z�u�������C�x���g�𔭍s����j�ƂȂ�C���X�^���X
    private Subject<Unit> unSetParticleSubject = new Subject<Unit>();

    /// <summary>
    /// �p�[�e�B�N���z�u�������C�x���g�̍w�Ǒ����������J
    /// </summary>
    public IObservable<Unit> OnUnSetParticleEvent
    {
        get { return unSetParticleSubject; }
    }


    //�p�[�e�B�N���z�u�������C�x���g���s
    public void UnSendSetParticleEvent()
    {
        unSetParticleSubject.OnNext(Unit.Default);
    }

    //�G�p�[�e�B�N���폜�C�x���g�𔭍s����j�ƂȂ�C���X�^���X
    private Subject<GameObject> _delEnemyParticleSubject = new Subject<GameObject>();

    /// <summary>
    /// �G�p�[�e�B�N���폜�C�x���g�̍w�Ǒ����������J
    /// </summary>
    public IObservable<GameObject> OnDelEnemyParticleEvent
    {
        get { return _delEnemyParticleSubject; }
    }


    //�G�p�[�e�B�N���폜�C�x���g���s
    public void SendDelEnemyParticleEvent(GameObject item)
    {
        _delEnemyParticleSubject.OnNext(item);
    }

    //�����p�[�e�B�N���폜�C�x���g�𔭍s����j�ƂȂ�C���X�^���X
    private Subject<GameObject> _delPlayerParticleSubject = new Subject<GameObject>();

    /// <summary>
    /// �����p�[�e�B�N���폜�C�x���g�̍w�Ǒ����������J
    /// </summary>
    public IObservable<GameObject> OnDelPlayerParticleEvent
    {
        get { return _delPlayerParticleSubject; }
    }


    //�����p�[�e�B�N���폜�C�x���g���s
    public void SendDelPlayerParticleEvent(GameObject item)
    {
        _delPlayerParticleSubject.OnNext(item);
    }

    // Time�ݒ�C�x���g�𔭍s����j�ƂȂ�C���X�^���X
    private Subject<int> setTimeSubject = new Subject<int>();

    /// <summary>
    /// Time�ݒ�C�x���g�̍w�Ǒ����������J
    /// </summary>
    public IObservable<int> OnSetTimeEvent
    {
        get { return setTimeSubject; }
    }

    // Time�ݒ�C�x���g���s
    public void SendSetTimeEvent(int time)
    {
        setTimeSubject.OnNext(time);
    }

    // start�{�^���\����\���C�x���g�𔭍s����j�ƂȂ�C���X�^���X
    private Subject<bool> _startButtonSubject = new Subject<bool>();

    /// <summary>
    /// start�{�^���\����\���C�x���g�̍w�Ǒ����������J
    /// </summary>
    public IObservable<bool> OnStartButtonEvent
    {
        get { return _startButtonSubject; }
    }

    // start�{�^���\����\���C�x���g���s
    public void SendStartButtonEvent(bool b)
    {
        _startButtonSubject.OnNext(b);
    }

    //player�����w���C�x���g�𔭍s����j�ƂȂ�C���X�^���X
    private Subject<PlayerDirection> _playerDirectionSubject = new Subject<PlayerDirection>();

    /// <summary>
    /// player�����w���C�x���g�̍w�Ǒ����������J
    /// </summary>
    public IObservable<PlayerDirection> OnPlayerDirectionEvent
    {
        get { return _playerDirectionSubject; }
    }

    // player�����w���C�x���g���s
    public void SendPlayerDirectionEvent(PlayerDirection p)
    {
        _playerDirectionSubject.OnNext(p);
    }

    //�G���ύX�C�x���g�𔭍s����j�ƂȂ�C���X�^���X
    private Subject<int> changeEnemyNumSubject = new Subject<int>();

    /// <summary>
    /// �G���ύX�C�x���g�̍w�Ǒ����������J
    /// </summary>
    public IObservable<int> OnChangeEnemyNumEvent
    {
        get { return changeEnemyNumSubject; }
    }

    /// <summary>
    /// �G���ύX�C�x���g���s
    /// </summary>
    /// <param name="num">�G�L�����̐�</param>
    public void SendChangeEnemyNumEvent(int num)
    {
        changeEnemyNumSubject.OnNext(num);
    }
}
