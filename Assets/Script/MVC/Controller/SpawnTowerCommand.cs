using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTowerCommand : Controller
{
    public override void Excute(MEventArgs args)
    {
        MSpawnTowerArgs e = args as MSpawnTowerArgs;
        TowerInfo towerInfo = Game.GetInstance().StaticData.GetTowerInfo(e.TowerID);

        GameModel gm = GetModel<GameModel>(MModelName.GameModel);
        gm.Gold -= towerInfo.BasePrice;

        GetView<Spawner>(MViewName.Spawner).SpawnTower(towerInfo, e.Position);

        GetView<MenuView>(MViewName.MenuView).Score = gm.Gold;
    }
}
