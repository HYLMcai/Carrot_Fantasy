using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownView : View
{
    public override MViewName Name => MViewName.CountDownView;

    private GameObject[] numbers;

    private int time = 3;
    private int realTime = 0;

    public override void HandleEvent(EventType eventType, MEventArgs mEventArgs)
    {
        
    }

    protected override void Initialize()
    {
        base.Initialize();
        numbers = new GameObject[3];
        Transform count = transform.Find("Count");
        numbers[0] = count.Find("1").gameObject;
        numbers[1] = count.Find("2").gameObject;
        numbers[2] = count.Find("3").gameObject;
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(DisplayCount());
    }

    IEnumerator DisplayCount()
    {
        realTime = time;
        while (true)
        {
            RefreshNumbers(realTime);
            yield return new WaitForSeconds(1f);
            realTime--;
            if (realTime <= 0)
            {
                break;
            }
        }
        //倒计时结束
        SetActive(false);
        //派发倒计时结束事件
        SendEvent(EventType.CountDownComplete, null);
    }

    private void RefreshNumbers(int realtime)
    {
        for(int i = 0; i < numbers.Length; i++)
        {
            numbers[i].SetActive(i == (realTime - 1));
        }
    }
}
