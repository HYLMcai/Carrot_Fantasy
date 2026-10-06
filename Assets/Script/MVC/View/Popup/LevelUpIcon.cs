using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpIcon : MonoBehaviour
{
    private Tower tower;
    private GameModel gm;
    private GameObject objImg;
    private PopupView view;
    private Text txtPrice;

    private void Awake()
    {
        objImg = transform.Find("LevelUp").gameObject;
        txtPrice = transform.Find("txtPrice").GetComponent<Text>();

        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void Load(Tower tower,GameModel gm,PopupView view)
    {
        this.tower = tower;
        this.gm = gm;
        this.view = view;

        if (tower.IsTopLevel)
        {
            this.objImg.SetActive(false);
            transform.Find("txtPrice").gameObject.SetActive(false);
            transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Icon/TowerBottom/Btn_ReachHighestLevel");
        }
        else
        {
            this.txtPrice.text = tower.Price.ToString();
            this.objImg.SetActive(gm.Gold >= tower.Price);
        }
    }
    
    private void OnClick()
    {
        this.view.HideAllPanel();
        
        if (tower.IsTopLevel|| gm.Gold <= tower.Price)
        {
            return;
        }
        //如果够钱且能升，则升级
        MLevelUpTowerArgs e = new MLevelUpTowerArgs(tower);
        view.MSendEvent(EventType.LevelUp, e);

        
    }
}
