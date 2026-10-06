using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopupMenuType
{
    Create,
    Upgrade,
}

public class Spawner : View
{
    public const int monsterDamge = 1;
    Map map;
    LuoBo luoBo;
    public override MViewName Name => MViewName.Spawner;

    public override void HandleEvent(EventType eventType, MEventArgs mEventArgs)
    {
        switch (eventType)
        {
            case EventType.EnterScene:
                //1.获取地图组件
                GameObject objMap = GameObject.Find("Map").gameObject;
                if (objMap == null)
                {
                    Debug.LogError("找不到对象，请检查!");
                    return;
                }
                map = objMap.GetComponent<Map>();
                //2.加载地图关卡数据
                map.LoadLevel(GetModel<GameModel>(MModelName.GameModel).CurLevel);

                map.OnTileClickEvent += OnTileClick;
                //3.生成萝卜
                OnSpawnLuoBo();
                break;
            case EventType.SpawnMonster:
                //4.刷怪
                MSpwanMonsterArgs e = mEventArgs as MSpwanMonsterArgs;
                OnSpawnMonster(e.MonsterID);
                break;
            default:
                break;
        }
    }

    void OnTileClick(object sender,EventArgs args)
    {
        //处理点击格子的逻辑
        GameModel gm = GetModel<GameModel>(MModelName.GameModel);
        if (!gm.IsPlaying) return;
        //判断view状态
        PopupView popupView = GetView<PopupView>(MViewName.PopupView);
        if (popupView.IsShow)
        {
            //隐藏菜单
            popupView.Hide();
            return;
        }

        TileClickEventArgs eventArgs = args as TileClickEventArgs;
        //if (!eventArgs.tile.CanHold)
        //{
        //    //隐藏菜单
        //    popupView.Hide();
        //    return;
        //}
        //右键关闭炮塔菜单
        if (eventArgs.mouseBotton == 1)
        {
            popupView.Hide();
            return;
        }
        //如果炮塔菜单展示出来则不触发点击展示炮塔菜单界面
        if (popupView.IsCreate == true || !eventArgs.tile.CanHold) return;
        //在菜单没显示的情况下点击到一个可放置塔的格子
        if (eventArgs.tile.data == null)
        {
            //显示创建菜单
            popupView.Show(PopupMenuType.Create, map.GetPosition(eventArgs.tile));
        }
        else
        {
            //显示升级菜单
            popupView.Show(PopupMenuType.Upgrade, map.GetPosition(eventArgs.tile), eventArgs.tile.data as Tower);
        }
    }

    void OnSpawnLuoBo()
    {
        GameObject objLuoBo = PoolManager.GetInstance().Take("LuoBo");
        luoBo = objLuoBo.GetComponent<LuoBo>();
        luoBo.Position = map.Path[map.Path.Length - 1];

        luoBo.HpEvent += OnLuoBoHpEvent;
        luoBo.DeadEvent += OnLuoBoDeadEvent;
    }

    void OnLuoBoHpEvent(int curHp,int maxHp)
    {

    }

    void OnLuoBoDeadEvent(Role role)
    {
        Game.GetInstance().Pool.Back(role.gameObject);
        GameModel gm = GetModel<GameModel>(MModelName.GameModel);
        RoundModel rm = GetModel<RoundModel>(MModelName.RoundModel);
        SendEvent(EventType.EndLevel, new MLevelArgs(gm.CurSelectIdx, false));
        SendEvent(EventType.Lose, new MRoundArgs(rm.CurRoundIdx, rm.RoundCount));
    }

    void OnSpawnMonster(int monsterId)
    {
        MonsterInfo info = Game.GetInstance().StaticData.GetMonsterInfo(monsterId);
        GameObject objMonster = PoolManager.GetInstance().Take("Monster/"+info.PrefabName);
        Monster monster = objMonster.GetComponent<Monster>();
        monster.Load(map.Path, info);

        monster.ReachedEvent += OnMonsterReached;
        monster.DeadEvent += OnMonsterDead;
    }

    //怪物碰到萝卜方法
    protected void OnMonsterReached(Monster monster)
    {
        //用观察者模式
        luoBo.TakeDamge(monsterDamge);
        monster.CurHp = 0;
    }

    void OnMonsterDead(Role monster)
    {
        //怪物回收
        Game.GetInstance().Pool.Back(monster.gameObject);

        //获取怪物(改)
        Monster tempMonster = monster as Monster;

        //发送死亡事件
        MMonsterDeadArgs args = new MMonsterDeadArgs(monster as Monster);
        SendEvent(EventType.MonsterDead, args);

        RoundModel rm = GetModel<RoundModel>(MModelName.RoundModel);
        GameModel gm = GetModel<GameModel>(MModelName.GameModel);
        //检验萝卜是否死亡且回合是否打完
        if (!luoBo.IsDead && rm.IsComplete)
        {
            //查找是否有怪物存活
            Monster[] monsters = GameObject.FindObjectsOfType<Monster>();
            if (monsters.Length == 0)
            {
                //游戏胜利
                SendEvent(EventType.EndLevel, new MLevelArgs(gm.CurSelectIdx, true));
                SendEvent(EventType.Win, new MRoundArgs(rm.CurRoundIdx, rm.RoundCount));
            }
        }
        gm.Gold += tempMonster.Price;//改
        GetView<MenuView>(MViewName.MenuView).Score = gm.Gold;
    }

    public void SpawnTower(TowerInfo info,Vector3 position)
    {
        Tile tile = map.GetTile(position);

        GameObject go = Game.GetInstance().Pool.Take("Tower/" + info.PrefabName);
        Tower tower = go.GetComponent<Tower>();
        tower.transform.position = position;
        tower.Load(tile, info);
        tile.data = tower;
    }

    protected override void Awake()
    {
        base.Awake();
        RegisterEvent(EventType.EnterScene);//获取loadlevel，传数据给map
        RegisterEvent(EventType.SpawnMonster);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnregisterAll();
    }
}
