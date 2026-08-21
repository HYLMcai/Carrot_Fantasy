using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeadCommand : Controller
{
    public override void Excute(MEventArgs args)
    {
        MMonsterDeadArgs e = args as MMonsterDeadArgs;

        GameModel gameModel = GetModel<GameModel>(MModelName.GameModel);
        gameModel.Gold += e.Monster.Score;

        //Ë¢ÐÂ½çÃæ
        //GetView<MenuView>(MViewName.MenuView).Score = gameModel.Gold;
    }
}
