using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePanel : MonoBehaviour
{
    private GameObject objPrefab;
    private bool inited = false;//是否初始化
    private List<TowerInfo> listTowerInfo;
    private List<CreateIcon> listCreateIcons = new List<CreateIcon>();
    private GameModel gameModel;
    private PopupView view;

    public void Load(GameModel gameModel,PopupView view)
    {
        this.gameModel = gameModel;
        this.view = view;
    }

    private void Awake()
    {
        objPrefab = transform.Find("Prefab").gameObject;
        objPrefab.SetActive(false);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
        if (!inited) Init();
        //根据关卡给定的炮塔加载图标
        for(int i = 0; i < listCreateIcons.Count; i++)
        {
            listCreateIcons[i].CheckIsEnough();
        }
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
    public void Init()
    {
        //根据配置表生成炮塔图标
        inited = true;
        listTowerInfo = Game.GetInstance().StaticData.GetAllTowerInfo();
        
        if (listTowerInfo.Count > 0)
        {
            for(int i = 0; i < listTowerInfo.Count; i++)
            {
                CreateIcon icon = CreateIcons();
                icon.Load(listTowerInfo[i], gameModel, view);
                listCreateIcons.Add(icon);
            }
        }
    }
    CreateIcon CreateIcons()
    {
        GameObject go = GameObject.Instantiate(this.objPrefab);
        go.transform.SetParent(this.transform);
        go.SetActive(true);
        return go.GetComponent<CreateIcon>();
    }
}
