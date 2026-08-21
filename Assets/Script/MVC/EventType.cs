using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//事件类型,枚举
public enum EventType
{
    EnterScene,
    ExitScene,
    StartUp,
    StartLevel,
    EndLevel,
    StartRound,
    SpawnMonster,
    CountDownComplete,
    MonsterDead,
    SpawnTower,
    SellTower,
    Win,
    Lose,
    Restart,
    LevelUp,
}
