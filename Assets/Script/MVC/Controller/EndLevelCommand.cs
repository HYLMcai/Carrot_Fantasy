using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelCommand : Controller
{
    public override void Excute(MEventArgs args)
    {
        MLevelArgs e = args as MLevelArgs;
        //停止刷怪
        RoundModel rm = GetModel<RoundModel>(MModelName.RoundModel);
        rm.StopRound();
        //执行结束，新关卡管理
        GameModel gm = GetModel<GameModel>(MModelName.GameModel);
        gm.EndLevel(e.IsSuccess);
        //展示UI
        if (e.IsSuccess)
        {
            GetView<WinView>(MViewName.WinView).Show();
        }
        else
        {
            GetView<LoseView>(MViewName.LoseView).Show();
        }
    }
}
