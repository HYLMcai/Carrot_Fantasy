using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameModel : Model
{
    public override MModelName Name => MModelName.GameModel;

    private List<Level> levels = new List<Level>();

    private int curLevelIdx;//当前解锁的关卡
    private int curSelectIdx;//当前选择的关卡
    private int gold;//关卡金币（分数）
    private bool isPlaying;//游戏是否进行中（是否暂停）

    public List<Level> Levels { get => levels; private set => levels = value; }
    //获取关卡信息
    public int CurLeveldx { get => this.curLevelIdx; }
    public int CurSelectIdx { get => this.curSelectIdx; }
    public int Gold { get => this.gold; set => this.gold = value; }
    public bool IsPlaying { get => this.isPlaying; }
    public int LevelCount { get => levels.Count; }
    public bool IsGamePass { get => curLevelIdx >= levels.Count; }//通关判断
    //当前关卡信息
    public Level CurLevel
    {
        get
        {
            //异常处理
            if (curSelectIdx >= levels.Count || curSelectIdx < 0)
            {
                Debug.LogError("不存在的关卡下标:" + curSelectIdx);
                return null;
            }
            return levels[curSelectIdx];
        }
    }

    public void Initialize()
    {
        //读取关卡列表
        List<FileInfo> levelList = Utils.GetAllLevelFiles();
        //加载关卡列表
        for (int i = 0; i < levelList.Count; i++)
        {
            //内存换速度：注意控制，管理好内存
            //只加载一次存起来
            Level level = new Level();
            Utils.LoadLevel(levelList[i].Name, ref level);
            Levels.Add(level);
        }
        //关卡进度存档
        curLevelIdx = PlayerPrefsHelper.GetCurrentLevelIdx();
    }

    public void StartLevel(int selectIdx)
    {
        this.curSelectIdx = selectIdx;
        isPlaying = true;
        //获取金币（分数）数据
        this.gold = this.CurLevel.InitScore;
    }
    public void EndLevel(bool isSuccess)
    {
        //通过之后要调用这个函数，如果通过解锁了新关卡，则保存
        if (isSuccess && curSelectIdx >= curLevelIdx)
        {
            curLevelIdx++;
            //保存
            PlayerPrefsHelper.SaveCurrentLevelIndex(curLevelIdx);
        }
        isPlaying = false;
    }
    public void ClearGameProgress()
    {
        isPlaying = false;
        curLevelIdx = 0;
        curSelectIdx = -1;
        PlayerPrefsHelper.SaveCurrentLevelIndex(0);
    }
}
