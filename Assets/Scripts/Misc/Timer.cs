using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Timer : MonoBehaviour
{
    private List<TimerEvent> timerDelegates = new List<TimerEvent>();

    //public TextMeshProUGUI timeDisplayText;

    private DateTime lastMinimize;
    private DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    #region singleton
    private static Timer _instance;
    public static Timer instance
    {
        get
        {
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private long currentTime;
    public long CurrentTime
    {
        get
        {
            return currentTime;
        }
    }

    public void init()
    {
        currentTime = (long)(DateTime.Now - epochStart).TotalSeconds;
        StartCoroutine(startTimer());
    }

    public void addTimerDelegate(TimerEvent eventObj)
    {
        timerDelegates.Add(eventObj);
    }

    private IEnumerator startTimer()
    {
        currentTime += 1;
        yield return new WaitForSeconds(1.0f);

        StartCoroutine(startTimer());

        long completedTime;
        for (int i = 0; i < timerDelegates.Count; i++)
        {
            completedTime = currentTime - timerDelegates[i].started_time;
            if(completedTime >= timerDelegates[i].duration)
            {
                timerDelegates[i].callback.Invoke(timerDelegates[i].callbackParam);
                timerDelegates.Remove(timerDelegates[i]);
            }
        }
    }

    void OnApplicationPause(bool isGamePause)
    {
        if (isGamePause)
        {
            lastMinimize = DateTime.Now;
        }
    }
    void OnApplicationFocus(bool isGameFocus)
    {
        if (isGameFocus)
        {
            double minimizedSeconds = (DateTime.Now - lastMinimize).TotalSeconds;
            currentTime += (Int32)minimizedSeconds;
        }
    }

    public string getTimeInMins(int time)
    {
        string timeStr = "";
        int mins = time / 60;
        int secs = time % 60;

        timeStr = checkLengthAndAddZero(mins) + ":" + checkLengthAndAddZero(secs);

        return timeStr;
    }
    private string checkLengthAndAddZero(int time)
    {
        if (time < 10)
            return "0" + time;
        else
            return time.ToString();
    }
}

public class TimerEvent
{
    public long started_time;
    public long duration;
    public UnityAction<object> callback;
    public object callbackParam;
}