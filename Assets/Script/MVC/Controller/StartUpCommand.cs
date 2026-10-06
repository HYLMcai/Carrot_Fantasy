using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpCommand : Controller
{
    public override void Excute(MEventArgs args)
    {
        //----处理StartUp事件的逻辑
        //--在StartUp事件里注册Model
        GameModel gameModel = new GameModel();
        RegisterModel(gameModel);
        //只有开始回合会需要加载这个roundmodel因此直接在里面new
        RegisterModel(new RoundModel());

        //--在StartUp事件里注册Controller
        RegisterController(EventType.EnterScene, typeof(EnterSceneCommand));
        RegisterController(EventType.ExitScene, typeof(ExitSceneCommand));
        RegisterController(EventType.StartLevel, typeof(StartLevelCommand));
        RegisterController(EventType.EndLevel, typeof(EndLevelCommand));
        RegisterController(EventType.CountDownComplete, typeof(CountDownComplete));
        RegisterController(EventType.MonsterDead, typeof(MonsterDeadCommand));
        RegisterController(EventType.SpawnTower, typeof(SpawnTowerCommand));
        RegisterController(EventType.SellTower, typeof(SellTowerCommand));
        RegisterController(EventType.Restart, typeof(RestartCommand));
        RegisterController(EventType.LevelUp, typeof(LevelUpCommand));

        //先在开始是加载号游戏配置表，后面要用直接调用
        gameModel.Initialize();
    }
}
