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
    /// �G�����C�x���g�̍w�Ǒ����������J
    /// </summary>
    public IObservable<int> OnVarEnemyEvent
    {
        get { return varEnemySubject; }
    }

    /// <summary>
    /// �G�L���������C�x���g���s
    /// </summary>
    /// <param name="num">�G�L�����̐�</param>
    public void SendVarEnemyEvent(int num)
    {
        varEnemySubject.OnNext(num);
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
}
