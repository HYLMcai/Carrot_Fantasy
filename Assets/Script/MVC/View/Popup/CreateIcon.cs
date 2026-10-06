using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//建造者模式
public class CreateIcon : MonoBehaviour
{
    Image image;
    TowerInfo info;
    bool isEnough;
    GameModel gameModel;
    PopupView view;

    private void Awake()
    {
        image = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
    public void Load(TowerInfo towerInfo,GameModel gameModel,PopupView view)
    {
        this.info = towerInfo;
        this.gameModel = gameModel;
        this.view = view;

        CheckIsEnough();
    }

    //刷新函数
    public void CheckIsEnough()
    {
        isEnough = gameModel.Gold >= info.BasePrice;

        string path = isEnough ? info.NormalIcon : info.DisabledIcon;
        image.sprite = Resources.Load<Sprite>(path);
        transform.localScale = new Vector3(1, 1, 1);
    }

    void OnClick()
    {
        if (isEnough)
        {
            MSpawnTowerArgs e = new MSpawnTowerArgs();
            e.TowerID = info.ID;
            e.Position = this.view.Point;
            this.view.MSendEvent(EventType.SpawnTower, e);
        }
        this.view.HideAllPanel();
    }
}
