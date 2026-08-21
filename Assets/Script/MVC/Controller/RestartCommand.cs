using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartCommand : Controller
{
    public override void Excute(MEventArgs args)
    {
        //Í£Ö¹Ë¢¹Ö
        RoundModel rm = GetModel<RoundModel>(MModelName.RoundModel);
        rm.StopRound();
    }
}
