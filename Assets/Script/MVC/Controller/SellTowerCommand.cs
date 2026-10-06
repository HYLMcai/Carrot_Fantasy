using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellTowerCommand : Controller
{
    public override void Excute(MEventArgs args)
    {
        MSellTowerArgs e = args as MSellTowerArgs;
        //炮塔置空
        e.Tower.Tile.data = null;
        //数据修改
        GameModel gm = GetModel<GameModel>(MModelName.GameModel);
        gm.Gold += e.Tower.SellPrice;
        //视图刷新
        GetView<MenuView>(MViewName.MenuView).Score = gm.Gold;
        //回收炮塔
        Game.GetInstance().Pool.Back(e.Tower.gameObject);
    }
}
