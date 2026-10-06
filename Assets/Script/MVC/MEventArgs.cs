using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//不需要写别的东西，作为一个父类参数承载Model传入事件
public class MEventArgs
{

}
//场景相关事件参数
public class MSceneArgs : MEventArgs
{
    public int SceneIdx;
    public string SceneName;
    public MSceneArgs() { }
    public MSceneArgs(int sceneIdx,string sceneName)
    {
        this.SceneIdx = sceneIdx;
        this.SceneName = sceneName;
    }
}

public class MLevelArgs : MEventArgs
{
    public int LevelIndex;
    public bool IsSuccess;
    
    public MLevelArgs(int levelIndex)
    {
        LevelIndex = levelIndex;
    }
    public MLevelArgs(int levelIndex,bool isSuccess)
    {
        LevelIndex = levelIndex;
        IsSuccess = isSuccess;
    }
}

public class MRoundArgs : MEventArgs
{
    public int CurRoundIndex;
    public int TotalRound;

    public MRoundArgs(int curRoundIndex,int totalRound)
    {
        CurRoundIndex = curRoundIndex;
        TotalRound = totalRound;
    }  
}

public class MSpwanMonsterArgs : MEventArgs
{
    public int MonsterID;
    public MSpwanMonsterArgs() { }
    public MSpwanMonsterArgs(int monsterId) { this.MonsterID = monsterId; }

}

public class MMonsterDeadArgs : MEventArgs
{
    public Monster Monster;

    public MMonsterDeadArgs(Monster monster)
    {
        this.Monster = monster;
    }
}

public class MSpawnTowerArgs : MEventArgs
{
    public int TowerID;
    public Vector3 Position = Vector3.zero;
    public MSpawnTowerArgs() { }
    public MSpawnTowerArgs(int towerID)
    {
        this.TowerID = towerID;
    }

    public MSpawnTowerArgs(int towerID,Vector3 position)
    {
        this.TowerID = towerID;
        this.Position = position;
    }
}

public class MSellTowerArgs : MEventArgs
{
    public Tower Tower;
    public MSellTowerArgs(Tower tower)
    {
        this.Tower = tower;
    }
}

public class MLevelUpTowerArgs : MEventArgs
{
    public Tower Tower;
    public MLevelUpTowerArgs(Tower tower)
    {
        this.Tower = tower;
    }
}
