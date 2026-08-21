# Carrot Fantasy（保卫萝卜风格 2D 塔防）

一款使用 Unity 开发的 2D 塔防游戏，玩法类似"保卫萝卜"：玩家在网格上放置炮塔，阻止怪物抵达终点并伤害萝卜（`LuoBo`）。

>  **素材声明**：本仓库为**纯代码 + 关卡数据**版本。出于版权虑，所有美术/音效/动画/预制体/场景素材**已从仓库移除**。仓库当前无法直接编译运行，需按下方[「运行说明」](#运行说明)自行补充素材。

---

## 技术栈

- **引擎**：Unity 2022.3.62f1c1（2D）
- **语言**：C#
- **架构**：手写事件驱动 MVC 框架（无第三方 MVC 库）
- **动画补间**：DOTween
- **UI**：UGUI
- **关卡数据**：XML

---

## 项目亮点

1. **手写 MVC 游戏框架** —— 纯 C# 手写事件驱动架构，通过反射 + 事件映射完成控制器动态分发，数据层（Model）/ 逻辑层（Controller）/ 表现层（View）解耦；View 随场景切换自动注册 / 注销。
2. **通用对象池系统** —— `IReusable` 接口 + 泛型对象池管理器，统一接管炮塔、子弹、怪物、萝卜等高频生成 / 销毁对象，避免频繁 `Instantiate` / `Destroy` 的 GC 开销。
3. **可视化关卡编辑器** —— 基于 Unity Editor 扩展（自定义 Inspector + Gizmos），左键绘制怪物路线、右键标记可建造格，一键序列化为 XML，实现"配置驱动"的内容生产。
4. **可扩展实体体系** —— 抽象 `Role` / `Tower` / `Bullet` 三层基类，多态支撑 5 种炮塔、5 种子弹、6 种怪物，新增单位只需实现子类、不改框架核心。
5. **配置与逻辑分离** —— 数值表（`StaticData`）+ XML 关卡数据与游戏逻辑解耦，`PlayerPrefs` 存档实现通关解锁与续玩。

---

## 架构概览

```
Assets/
├── Script/                 # 全部游戏逻辑代码
│   ├── Game/               # 引导（Game.cs）+ 关卡数据类（Level/Tile/Round/Point）
│   ├── MVC/                # 手写 MVC 框架
│   │   ├── Controller/     # 控制器（纯 C#，每次事件新建）
│   │   ├── Model/          # 模型（GameModel / RoundModel）
│   │   └── View/           # 视图（MonoBehaviour，随场景注册/注销）
│   ├── Object/             # 游戏实体（Role/Monster/LuoBo/Tower/Bullet 及子类）
│   ├── Pool/               # 对象池
│   ├── Data/               # 静态数值表（StaticData 等）
│   ├── Common/             # 工具（XML 加载/存储、路径、存档）
│   └── Singletons/         # 单例封装
├── Editor/                 # 关卡编辑器（MapEditor.cs）
├── Resources/Levels/       # 关卡数据（XML，保留）
└── Plugins/                # DOTween 依赖
```

**事件流转**：`Game.Start()` → 发送 `StartUp` 事件 → `StartUpCommand` 注册所有 Model / Controller → 场景切换发送 `ExitScene` / `EnterScene` → Controller 修改 Model、View 处理 UI。

---

## 运行说明

仓库当前**缺少运行所需的素材资源**，需补充以下内容后才能编译运行：

### 需要补充的素材

| 目录 | 内容 | 用途 |
|------|------|------|
| `Assets/Scenes/` | `.unity` 场景文件 | 游戏必须，缺失则无法打开工程 |
| `Assets/Animation/` | `.anim` / `.controller` 动画 | 萝卜、怪物、炮塔动画 |
| `Assets/Resources/Prefab/` | `.prefab` 预制体 | 炮塔 / 子弹 / 怪物 / 萝卜实例化 |
| `Assets/Resources/Icon/`、`TowerImage/` | `.png` 图标 | 炮塔建造面板图标 |
| `Assets/Resources/Cards/`、`Maps/` | `.png` 背景/路径/卡片图 | 关卡背景、路线、选关卡片 |
| `Assets/Gizmos/` | `.png` 图标 | 关卡编辑器 Gizmos 标记 |

### 素材路径约定（供替换参考）

代码通过以下方式引用资源，替换时需保持目录与命名一致：

- **预制体**（对象池）：`Resources/Prefab/{Tower|Bullet|Monster}/{PrefabName}`，其中 `PrefabName` 见 `StaticData.cs`，如 `Tower/Bottle`、`Bullet/BottleBullet`、`Monster/Monster0`。
- **炮塔图标**：`Resources/Icon/{类型}/{AllowPlaced|CannotPlaced}`。
- **关卡图片**：`Resources/Cards/`、`Resources/Maps/`，文件名见各 `level*.xml` 中的 `CardImage` / `Background` / `Road` 字段。

### 运行步骤

1. 用 **Unity 2022.3.62f1c1** 或更高版本打开本项目。
2. 补齐上述素材，或联系仓库所有者获取完整工程。
3. 打开场景 `00_IniScene`，点击 Play 运行。

---

## 操作方式

- **主菜单**：点击"冒险"进入选关。
- **选关界面**：左右切换关卡，点击"开始"进入游戏（后续关卡需通关解锁）。
- **游戏内**：点击可建造格弹出建造面板 → 选择炮塔建造；点击已建造炮塔弹出升级 / 出售面板。
- **倍速 / 暂停**：游戏内右上角按钮控制（基于 `Time.timeScale`）。

---

## 许可证

本项目代码仅供学习与作品集展示使用。素材资源不包含在本仓库内。
