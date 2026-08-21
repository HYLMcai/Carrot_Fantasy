using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    private GameModel gameModel;
    private PopupView view;
    //获取升级，出售方法
    private LevelUpIcon levelUpIcon;
    private SellIcon sellIcon;
    //获取所操作的塔
    private Tower tower;

    private void Awake()
    {
        levelUpIcon = this.GetComponentInChildren<LevelUpIcon>();
        sellIcon = this.GetComponentInChildren<SellIcon>();
    }

    public void Load(GameModel gameModel,PopupView view,Tower tower)
    {
        this.gameModel = gameModel;
        this.view = view;
        this.tower = tower;
        levelUpIcon.Load(tower, gameModel, view);
        sellIcon.Load(tower, view);
    }
    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
