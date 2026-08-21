using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevelCommand : Controller
{
    public override void Excute(MEventArgs args)
    {
        MLevelArgs e = args as MLevelArgs;
        GameModel gameModel = GetModel<GameModel>(MModelName.GameModel);
        gameModel.StartLevel(e.LevelIndex);
        GetModel<RoundModel>(MModelName.RoundModel).LoadLevel(gameModel.Levels[e.LevelIndex]);
        Game.GetInstance().LoadScene(3);
    }
}
