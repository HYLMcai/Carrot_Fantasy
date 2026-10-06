using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellIcon : MonoBehaviour
{
    Tower tower;
    PopupView view;
    private Text txtPrice;

    private void Awake()
    {
        txtPrice = transform.Find("txtPrice").GetComponent<Text>();
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void Load(Tower tower,PopupView view)
    {
        this.tower = tower;
        this.view = view;

        this.txtPrice.text = tower.SellPrice.ToString();
    }

    private void OnClick()
    {
        this.view.HideAllPanel();

        this.view.MSendEvent(EventType.SellTower, new MSellTowerArgs(this.tower));
    }
}
