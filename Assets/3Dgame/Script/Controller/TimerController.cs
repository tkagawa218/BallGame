using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public Text timeText;

    void Awake()
    {
        //timerイベントを購読
        GameDataManager.Instance.OnTimeChanged
            .Delay(TimeSpan.FromMilliseconds(1000))
            .Subscribe(t =>
            {
                if(GameDataManager.Instance.getGameOn())
                {
                    timeText.text = timeFormat(t);
                    if(t ==0)
                    {
                        GameDataManager.Instance.setGameOff(true);
                    }
                    else
                    {
                        GameDataManager.Instance.
                        setRestTime(t - 1);
                    }
                }
            });
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private string timeFormat(int time)
    {
        int minutes = 0;
        int seconds = time;
        while (seconds >= 60)
        {
            minutes++;
            seconds = seconds - 60;
        }

        return minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
