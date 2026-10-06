# CLAUDE.md

本文件为 Claude Code（claude.ai/code）在此仓库中工作时提供指导。

## 项目概述

一款使用 **Unity 2022.3.62f1c1**（见 `ProjectSettings/ProjectVersion.txt`）构建的 2D 塔防游戏（"保卫萝卜" / Carrot-Fantasy 风格）。玩家在网格上放置炮塔，阻止怪物抵达并伤害萝卜（`LuoBo`）。游戏逻辑代码全部位于 `Assets/Script/` 目录；美术资源和预制体位于 `Assets/Resources/` 与 `Assets/Animation/` 目录。

本项目没有自定义的构建 / 代码检查 / 测试工具链。项目通过 Unity 编辑器打开并运行；测试通过 Unity 的 Test Framework 运行（已作为包安装，但当前未真正使用——见 `Assets/Script/test/`，其中是练习 / 实验性脚本，而非真正的测试）。

## 源码编码

所有 `.cs` 文件均为 **GBK/ISO-8859 编码，包含中文注释，并使用 CRLF 行尾**。编辑这些文件时，请保留原有编码和行尾，不要重新格式化或改写中文注释。Bash 工具会把这些注释显示为乱码——如需了解其含义，请使用 `Read` 工具读取。

## 架构

代码库是一个纯 C# 手写的 **MVC 框架**（未使用外部 MVC 库）。理解事件流转是理解一切的关键。

### 引导与场景流程

- `Assets/Script/Game/Game.cs` —— 唯一的引导 `MonoBehaviour`，放置在场景 `00_IniScene`（`Assets/Scenes/00_IniScene.unity`）中。它是 `DontDestroyOnLoad` 的根对象，其他所有内容都挂在它下面。在 `Start()` 中它会：
  1. 解析 `PoolManager`、`SoundManager` 和 `StaticData` 三个单例。
  2. 注册 `StartUp` 控制器，然后发送 `EventType.StartUp`（该事件会注册所有 Model 和其余的 Controller）。
  3. 初始化 DOTween 并调用 `LoadScene(1)`。
- 通过 `Game.GetInstance()`（手动实现的静态单例）访问全局服务，例如 `Game.GetInstance().StaticData`、`Game.GetInstance().Pool`、`Game.GetInstance().Sound`。
- 场景顺序（见 `ProjectSettings/EditorBuildSettings.asset`）：`00_IniScene`（引导）→ `01_StartScene`（主菜单）→ `02_SelectLevelScene`（关卡选择）→ `03_LevelScene`（游戏关卡）→ `04_SceneEditor`（关卡制作工具）。`Game.LoadScene(int)` 在加载前发送 `ExitScene`，场景加载完成后发送 `EnterScene`。

### MVC 事件系统

- `Assets/Script/MVC/MVC.cs` —— 静态门面类，包含三个注册表：以 `MModelName` 为键的 Model、以 `MViewName` 为键的 View，以及 `EventType → Controller 类型` 的映射。`MVC.SendEvent(EventType, MEventArgs)` 通过反射（`Activator.CreateInstance`）实例化对应的 Controller 并调用 `Excute(args)`，然后通知所有关注列表中包含该事件类型的已注册 View。
- `EventType`（`Assets/Script/MVC/EventType.cs`）是唯一一个为每个系统事件命名的枚举。`MEventArgs.cs` 包含基类 `MEventArgs` 以及所有类型化的子类（如 `MLevelArgs`、`MRoundArgs`、`MSpawnTowerArgs`、`MMonsterDeadArgs`）。
- **Model**（`Model.cs`、`MVC/Model/`）—— 纯 C# 类，不是 MonoBehaviour。由 `StartUpCommand` 在启动时注册一次。共两个：
  - `GameModel` —— 关卡列表（从 XML 加载）、当前 / 已选关卡索引、金币，以及进度持久化（通过 `PlayerPrefsHelper`）。
  - `RoundModel` —— 波次 / 刷怪状态；运行刷怪协程并发出 `StartRound` / `SpawnMonster` 事件。
- **View**（`View.cs`、`MVC/View/`）—— MonoBehaviour，在 `Awake()` 中自我注册，在 `OnDestroy()` 中自我注销。每个 View 通过 `RegisterEvent(...)` 声明它关注的事件，并在 `HandleEvent(...)` 中处理这些事件。UI 通过硬编码的 `transform.Find("...")` 路径定位，并在 `Initialize()` 中完成连线。注意：View 子类常常跳过 `RegisterEvent`，只重写 `HandleEvent`，因此 View 只有在自己注册过某事件时才会收到该事件。
- **Controller**（`Controller.cs`、`MVC/Controller/`）—— 无持久状态的纯 C# 类，每次事件都会新建一个实例。`EventType → Controller` 的映射集中注册在 `StartUpCommand.Excute()`（`MVC/Controller/StartUpCommand.cs`）中——要了解每个事件做什么，请读这个文件。Controller 修改 Model 并读取 View。

### 游戏对象

- `Role`（`Object/Role.cs`）—— 所有带血量的池化对象的抽象基类，实现了 `IReusable`。暴露 `CurHp` / `MaxHp`、`HpEvent`、`DeadEvent` 和 `TakeDamge`。两个子类：
  - `Monster`（`Object/Monster.cs`）—— 沿 `Vector3[]` 路径点移动（`Load(path, MonsterInfo)`），到达终点时触发 `ReachedEvent`。
  - `LuoBo`（`Object/LuoBo.cs`）—— 需要保卫的萝卜；它的 `TakeDamge` 会驱动 Animator 的 HP 参数。
- `Tower`（`Object/Tower/Tower.cs`）—— 抽象的池化炮塔。`Load(Tile, TowerInfo)` 完成配置。基类 `Update()` 负责所有索敌逻辑（通过 `FindObjectsOfType<Monster>` 找到射程内最近的怪物）、瓶子类炮塔的旋转，以及开火间隔计时；子类重写 `Shot(target)` / `Shot(target, targetChain)`。在 `Object/Tower/*/` 下有五种实现：`Bottle`、`Fan`、`FireBottle`、`Lightning`（链式 / `IsChain`）和 `Shit`，每种都对应一个 `Bullet` 子类（基类在 `Object/Tower/Bullet.cs`，具体实现在各类型文件夹下）。`IsLaser` 子弹持续造成伤害；其他为单次命中。
- `Map`（`Map.cs`）—— 8×12 的 `Tile` 网格（`Game/Data/Tile.cs`）。`Tile` 包含 `X`、`Y`、`CanHold`（能否放置炮塔），以及一个存放已放置 `Tower` 的 `data` 字段。Map 会加载一个 `Level` 的可放置点 / 路径点，暴露 `Road`（怪物的路径点路径），并在格子被点击（左键 / 右键）时触发 `OnTileClickEvent`。

### 对象池

- `Pool` / `PoolManager` / `IReusable`（`Script/Pool/`）。`PoolManager` 是一个 `MonoSingleton`，从 `Resources/Prefab/` 加载预制体，并通过 `Take(path)` 返回。**所有**游戏实体（炮塔、子弹、怪物、萝卜）都是池化的——始终用 `Game.GetInstance().Pool.Back(gameObject)` 回收，而不是 `Destroy`，并在 `IReusable.Back()` 重写中重置对象状态。

### 数据与配置

- `StaticData`（`Data/StaticData.cs`）—— 一个 `Singleton`，用整数 ID 硬编码所有炮塔 / 子弹 / 怪物 / 萝卜的属性（无外部数据表）。这是添加或调整炮塔、怪物的地方。
- 关卡定义以 **XML** 存放在 `Assets/Resources/Levels/level*.xml`，由 `Utils.LoadLevel` / `Utils.SaveLevel`（`Common/Utils.cs`）解析 / 序列化。每个关卡指定名称、背景 / 路径 / 卡片图片、初始金币、可放置点、路径点，以及波次（怪物 id + 数量）。
- `Const.cs` 存放基于 `Application.dataPath` 构建的文件系统路径（用于 XML 关卡和图片加载）。

### 单例

- `Singleton<T>`（`Script/Singletons/Singleton.cs`）—— 基于反射的纯 C# 单例；被 `StaticData` 和 `SoundManager` 使用。
- `MonoSingleton<T>` —— 创建一个 `DontDestroyOnLoad` 的 GameObject；被 `PoolManager` 使用。
- `Game` 使用自己的手动 `instance` 模式，而不是以上两种。

## 关卡编辑器

`Assets/Editor/MapEditor.cs` 是 `Map` 的自定义 Inspector，在 `04_SceneEditor` 场景中使用（需要处于 Play 模式）。它用于创作 / 编辑 XML 关卡文件：左键切换路径格子，右键切换可放置（`CanHold`）格子，按钮用于新建关卡或将修改保存回 XML。添加或编辑关卡时，请通过这个编辑器（或直接编辑 XML），而不是改动场景对象。

## 关键注意事项

- `GameModel.CurLevel` 会校验 `curSelectIdx`，越界时记录错误——访问关卡数据前请先调用 `StartLevel`。
- View 之间以及 View 与 Model 之间通过 `GetView<T>(MViewName...)` / `GetModel<T>(MModelName...)` 相互解析；枚举 `MViewName` 和 `MModelName` 必须与启动时注册的类保持同步。
- 金币 / 分数 UI 挂在 `MenuView.Score` 上，Controller 在修改 `GameModel.Gold` 后直接更新它。
- `Time.timeScale` 用于暂停 / 倍速控制（`MenuView`），因此任何依赖缩放时间的协程或移动都会受到这些按钮的影响。
