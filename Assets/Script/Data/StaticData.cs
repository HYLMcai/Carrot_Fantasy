using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StaticData : Singleton<StaticData>
{
    //用于存静态数据并提供静态数据接口
    Dictionary<int, MonsterInfo> Monsters = new Dictionary<int, MonsterInfo>();
    Dictionary<int, BulletInfo> Bullets = new Dictionary<int, BulletInfo>();
    Dictionary<int, LuoboInfo> LuoBos = new Dictionary<int, LuoboInfo>();
    Dictionary<int, TowerInfo> Towers = new Dictionary<int, TowerInfo>();
    protected override void Initial()
    {
        InitBullets();
        InitLuoBo();
        InitMonsters();
        InitTowers();
    }
    //初始化数据
    void InitBullets()
    {
        //设置子弹数据
        Bullets.Add(0, new BulletInfo() { ID = 0, PrefabName = "BottleBullet", BaseSpeed = 15f, BaseAttack = 10, IsLaser = false });
        Bullets.Add(1, new BulletInfo() { ID = 1, PrefabName = "FanBullet", BaseSpeed = 12f, BaseAttack = 10, IsLaser = false });
        Bullets.Add(2, new BulletInfo() { ID = 2, PrefabName = "FireBullet", BaseSpeed = 12f, BaseAttack = 3, IsLaser = true });
        Bullets.Add(3, new BulletInfo() { ID = 3, PrefabName = "LightningBullet", BaseSpeed = 12f, BaseAttack = 1, IsLaser = true });
        Bullets.Add(4, new BulletInfo() { ID = 4, PrefabName = "Shit", BaseSpeed = 12f, BaseAttack = 13, IsLaser = false });
    }
    void InitMonsters()
    {
        Monsters.Add(0, new MonsterInfo() { ID = 0, Hp = 50, MoveSpeed = 1f, Price = 50, PrefabName = "Monster0" });
        Monsters.Add(1, new MonsterInfo() { ID = 1, Hp = 50, MoveSpeed = 1f, Price = 50, PrefabName = "Monster1" });
        Monsters.Add(2, new MonsterInfo() { ID = 2, Hp = 50, MoveSpeed = 1f, Price = 50, PrefabName = "Monster2" });
        Monsters.Add(3, new MonsterInfo() { ID = 3, Hp = 500, MoveSpeed = 1f, Price = 500, PrefabName = "Boss0" });
        Monsters.Add(4, new MonsterInfo() { ID = 4, Hp = 500, MoveSpeed = 1f, Price = 500, PrefabName = "Boss1" });
        Monsters.Add(5, new MonsterInfo() { ID = 5, Hp = 500, MoveSpeed = 1f, Price = 500, PrefabName = "Boss2" });
    }
    void InitLuoBo()
    {
        LuoBos.Add(0, new LuoboInfo() { ID = 0, Hp = 7 });
    }
    void InitTowers()
    {
        Towers.Add(0, new TowerInfo() { ID = 0, PrefabName = "Bottle", NormalIcon = "Icon/Bottle/AllowPlaced", DisabledIcon = "Icon/Bottle/CannotPlaced", MaxLevel = 3, BasePrice = 100, ShotRate = 2, GuardRange = 3f, UseBulletID = 0, IsBottle = true, IsChain = false });
        Towers.Add(1, new TowerInfo() { ID = 1, PrefabName = "Fan", NormalIcon = "Icon/Fan/AllowPlaced", DisabledIcon = "Icon/Fan/CannotPlaced", MaxLevel = 3, BasePrice = 160, ShotRate = 2, GuardRange = 3f, UseBulletID = 1, IsBottle = false, IsChain = false });
        Towers.Add(2, new TowerInfo() { ID = 2, PrefabName = "FireBottle", NormalIcon = "Icon/FireBottle/AllowPlaced", DisabledIcon = "Icon/FireBottle/CannotPlaced", MaxLevel = 3, BasePrice = 160, ShotRate = 1f, GuardRange = 3f, UseBulletID = 2, IsBottle = true, IsChain = false });
        Towers.Add(3, new TowerInfo() { ID = 3, PrefabName = "Lightning", NormalIcon = "Icon/Lightning/AllowPlaced", DisabledIcon = "Icon/Lightning/CannotPlaced", MaxLevel = 3, BasePrice = 160, ShotRate = 1f, GuardRange = 3f, UseBulletID = 3, IsBottle = false, IsChain = true });
        Towers.Add(4, new TowerInfo() { ID = 4, PrefabName = "Shit", NormalIcon = "Icon/Shit/AllowPlaced", DisabledIcon = "Icon/Shit/CannotPlaced", MaxLevel = 3, BasePrice = 160, ShotRate = 1f, GuardRange = 3f, UseBulletID = 4, IsBottle = false, IsChain = false });

    }

    public LuoboInfo GetLuoBoInfo()
    {
        return LuoBos[0];
    }
    public MonsterInfo GetMonsterInfo(int monsterId)
    {
        return Monsters[monsterId];
    }
    public BulletInfo GetBulletInfo(int bulletId)
    {
        return Bullets[bulletId];
    }
    public TowerInfo GetTowerInfo(int towerId)
    {
        return Towers[towerId];
    }
    public List<TowerInfo> GetAllTowerInfo()
    {
        return Towers.Values.ToList<TowerInfo>();
    }
}
