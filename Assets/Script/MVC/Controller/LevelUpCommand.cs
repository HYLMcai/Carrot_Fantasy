using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpCommand : Controller
{
    public override void Excute(MEventArgs args)
    {
        MLevelUpTowerArgs e = args as MLevelUpTowerArgs;

        //数据修改
        GameModel gm = GetModel<GameModel>(MModelName.GameModel);
        gm.Gold -= e.Tower.Price;
        e.Tower.Level += 1;

        //视图刷新
        GetView<MenuView>(MViewName.MenuView).Score = gm.Gold;
    }
}
