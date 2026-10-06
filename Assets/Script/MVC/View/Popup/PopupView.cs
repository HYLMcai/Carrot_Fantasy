using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupView : View
{
    private CreatePanel createPanel;
    private UpgradePanel upgradePanel;
    private Transform root;
    private bool isShow = false;
    private Vector3 point;

    public Vector3 Point { get => this.point; }

    public bool IsShow { get => isShow; }

    public bool IsCreate { get; set; }

    public override MViewName Name => MViewName.PopupView;

    public override void HandleEvent(EventType eventType, MEventArgs mEventArgs)
    {
        
    }
    protected override void Awake()
    {
        base.Awake();
        createPanel = GetComponentInChildren<CreatePanel>();
        createPanel.Load(GetModel<GameModel>(MModelName.GameModel),this);

        upgradePanel = GetComponentInChildren<UpgradePanel>();

        root = transform.Find("Root");
    }
    protected override void Start()
    {
        base.Start();
        HideAllPanel();
    }

    public void Hide()
    {
        HideAllPanel();
    }
    public void Show(PopupMenuType type,Vector3 worldPoint,Tower tower = null)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPoint);
        root.position = screenPosition;
        this.point = worldPoint;

        switch (type)
        {
            case PopupMenuType.Create:
                IsCreate = true;
                ShowCreatePanel();
                break;
            case PopupMenuType.Upgrade:
                upgradePanel.Load(GetModel<GameModel>(MModelName.GameModel), this, tower);
                IsCreate = true;
                ShowUpgradePanel();
                break;
            default:
                break;
        }
    }

    public void HideAllPanel()
    {
        this.createPanel.Hide();
        this.upgradePanel.Hide();
        IsCreate = false;
    }

    public void ShowCreatePanel()
    {
        this.createPanel.Show();
    }
    public void ShowUpgradePanel()
    {
        this.upgradePanel.Show();
    }

    public void MSendEvent(EventType type,MEventArgs args)
    {
        SendEvent(type, args);
    }
}
