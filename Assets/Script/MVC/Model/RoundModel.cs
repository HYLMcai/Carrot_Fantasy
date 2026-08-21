using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundModel : Model
{
    public override MModelName Name => MModelName.RoundModel;

    //--数据与数据逻辑
    public const float ROUND_INTERVAL = 3f;//回合间隔
    public const float SPAWN_INTERVAL = 1f;//出怪间隔

    //数据
    List<Rounds> rounds;//回合信息
    int curRoundIdx = -1;//当前回合数
    bool isComplete = false;//是否所有回合都结束
    Coroutine runner;//迭代器

    public int CurRoundIdx { get => this.curRoundIdx; }//当前回合数
    public int RoundCount { get => rounds.Count; }//总回合数
    public bool IsComplete { get => this.isComplete; }//回合是否结束
    public Rounds CurRound { get => rounds[curRoundIdx]; }//设置当前回合数

    public void LoadLevel(Level level)
    {

        rounds = level.Rounds;
    }
    public void StartRound()
    {
        runner = Game.GetInstance().StartCoroutine(RunRound());
        
    }
    public void StopRound()
    {
        if (runner != null)
        {
            Game.GetInstance().StopCoroutine(runner);
            runner = null;
        }
    }

    //数据逻辑
    IEnumerator RunRound()
    {
        curRoundIdx = -1;
        isComplete=false;

        for(int i = 0; i < RoundCount; i++)
        {
            curRoundIdx = i;

            //发送回合开始事件
            MRoundArgs argsRound = new MRoundArgs(curRoundIdx, RoundCount);
            SendEvent(EventType.StartRound, argsRound);

            //刷新这回合的怪物
            for (int j = 0; j < CurRound.Count; j++)
            {
                //刷怪间隔
                yield return new WaitForSeconds(SPAWN_INTERVAL);
                //发送刷怪事件
                MSpwanMonsterArgs argsMonster = new MSpwanMonsterArgs(CurRound.MonsterId);
                SendEvent(EventType.SpawnMonster, argsMonster);
                //判断是否刷完
                if (i == RoundCount - 1 && j == CurRound.Count - 1)
                {
                    isComplete = true;
                }
            }
            //如果回合未结束，但这一轮怪刷完了，则等待三秒加载下一回合
            if (!isComplete)
            {
                yield return new WaitForSeconds(ROUND_INTERVAL);
            }
            
        }
    }
}
