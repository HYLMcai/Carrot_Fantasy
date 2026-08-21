using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartView : View
{
    public override MViewName Name => MViewName.StartView;

    private GameObject btnAdventure;
    private GameObject btnQuit;

    public override void HandleEvent(EventType eventType, MEventArgs mEventArgs)
    {

    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Initialize()
    {
        base.Initialize();
        btnAdventure = transform.Find("BtnAdventure").gameObject;
        btnQuit = transform.Find("BtnQuit").gameObject;
        btnAdventure.GetComponent<Button>().onClick.AddListener(OnAdventureBtnClick);
        btnQuit.GetComponent<Button>().onClick.AddListener(OnQuitBtnClick);
    }

    private void OnAdventureBtnClick()
    {
        Game.GetInstance().LoadScene(2); 
    }

    private void OnQuitBtnClick()
    {
        Application.Quit();
    }
}
