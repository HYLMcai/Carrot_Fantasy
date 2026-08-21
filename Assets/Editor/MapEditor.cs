using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

[CustomEditor(typeof(Map))]
//Editor代码可以访问运行时的代码（Runtime）
//运行时的代码（Runtime）无法访问Editor代码
public class MapEditor : Editor
{
    //定义游戏对象
    Map map;
    //保存关卡信息
    Level level;

    List<FileInfo> levelFiles;

    private int curSelectIndex = -1;

    private string[] modeToolBar = new string[] { "新建关卡", "编辑关卡" };

    int curMode = 0;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //判断游戏是否运行,若不运行则不显示
        if (!Application.isPlaying) return;

        map = target as Map;

        EditorGUILayout.BeginHorizontal();
        curMode = GUILayout.Toolbar(curMode, modeToolBar);
        EditorGUILayout.EndHorizontal();
        switch (curMode)
        {
            case 0:
                OnCreateMode();
                break;
            case 1:
                OnEditMode();
                break;
            default:
                break;
        }
        
    }

    string levelName;
    string initScore;
    string newLevelFileName = "";
    SerializedProperty CardImage;
    SerializedProperty Background;
    SerializedProperty TempRoad;

    private void OnEnable()
    {
        CardImage = serializedObject.FindProperty("CardImage");//获取当前指向对象的属性
        Background = serializedObject.FindProperty("Background");//获取当前指向对象的属性
        TempRoad = serializedObject.FindProperty("TempRoad");//获取当前指向对象的属性
    }

    int roundCout = 0;
    List<int> MonsterIDList = new List<int>();
    List<int> MonsterCountList = new List<int>();
    private void OnCreateMode()
    {
        if (levelFiles == null)
        {
            LoadAllLevelFiles();
            newLevelFileName = "level" + levelFiles.Count + ".xml";
        }

        #region 第一层 关卡文件名
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("关卡文件名:");
        newLevelFileName = GUILayout.TextField(newLevelFileName);
        EditorGUILayout.EndHorizontal();
        #endregion

        #region 第二层 关卡名字
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("关卡名字:");
        levelName = GUILayout.TextField(levelName);
        EditorGUILayout.EndHorizontal();
        #endregion

        #region 第三层 初始分数
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("初始分数:");
        initScore = GUILayout.TextField(initScore);
        EditorGUILayout.EndHorizontal();
        #endregion

        #region 第四层 图片设置

        EditorGUILayout.PropertyField(CardImage);//显示编辑属性框
        serializedObject.ApplyModifiedProperties();//允许编辑属性

        EditorGUILayout.PropertyField(Background);//显示编辑属性框
        serializedObject.ApplyModifiedProperties();//允许编辑属性

        EditorGUILayout.PropertyField(TempRoad);//显示编辑属性框
        serializedObject.ApplyModifiedProperties();//允许编辑属性

        #endregion

        #region 第五层 怪物总波数
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("回合信息");
        GUILayout.Label("怪物总波数:");
        int newRoundCount = EditorGUILayout.IntField(roundCout);
        EditorGUILayout.EndHorizontal();
        #endregion

        #region 第六层 怪物属性
        if (newRoundCount > roundCout)
        {
            for (int i = roundCout; i < newRoundCount; i++)
            {
                MonsterIDList.Add(0);
                MonsterCountList.Add(0);
            }
        }
        roundCout = newRoundCount;
        if (roundCout > 0)
        {
            for (int i = 0; i < roundCout; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("怪物ID:");
                MonsterIDList[i] = EditorGUILayout.IntField(MonsterIDList[i]);

                GUILayout.Label("怪物数量:");
                MonsterCountList[i] = EditorGUILayout.IntField(MonsterCountList[i]);
                EditorGUILayout.EndHorizontal();
            }
        }
        #endregion

        #region 第七层 新建关卡
        EditorGUILayout.BeginHorizontal();
        //新建关卡
        if (GUILayout.Button("新建关卡"))
        {
            level = new Level();

            //保存关卡数据
            LevelSettingSave(level);
            if (level == null) return;

            string path = Const.LevelConfigPath + newLevelFileName;
            if (File.Exists(path))
            {
                EditorUtility.DisplayDialog("新建错误", "关卡文件已经存在,请检查你的关卡文件名!", "确定");
                return;
            }
            Utils.SaveLevel(path, level);
            EditorUtility.DisplayDialog("新建成功", "新建关卡成功!", "确定");

            //AssetDatabase：编辑器加载资源，就是非运行时加载资源
            AssetDatabase.Refresh();
        }
        EditorGUILayout.EndHorizontal();
        #endregion
    }

    int newRoundCount = -1;//修改后的回合数
    int levelIdx = -1;//暂存选择的关卡
    private void OnEditMode()
    {
        #region 第一层
        EditorGUILayout.BeginHorizontal();
        int roundCount = -1;//原本回合数
        int selectIndex = EditorGUILayout.Popup(curSelectIndex, GetLevelFiles());//下拉框
        if (selectIndex != curSelectIndex)
        {
            //保存选项
            curSelectIndex = selectIndex;
            //生成
            LoadLevel();
        }
        if (GUILayout.Button("读取关卡"))
        {
            LoadAllLevelFiles();
        }
        EditorGUILayout.EndHorizontal();
        #endregion

        #region 第二层
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("清除所有路径点"))
        {
            if (map != null && level != null)
            {
                map.ClearRoad();
            }
        }
        if (GUILayout.Button("清除所有放置点"))
        {
            if (map != null && level != null)
            {
                map.ClearHolder();
            }
        }
        if (GUILayout.Button("恢复关卡设置"))
        {
            if (map != null && level != null)
            {
                map.LoadLevel(level);
            }
        }
        EditorGUILayout.EndHorizontal();
        #endregion

        #region 第三层 关卡名字
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("关卡名字:");
        if (levelIdx != curSelectIndex)
        {
            levelName = level.Name;
        }
        levelName = GUILayout.TextField(levelName);
        EditorGUILayout.EndHorizontal();
        #endregion

        #region 第三层 金币（分数）初始设置
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("金币（分数）信息:");
        if (levelIdx != curSelectIndex)
        {
            initScore = level.InitScore.ToString();
        }
        initScore = GUILayout.TextField(initScore);
        EditorGUILayout.EndHorizontal();
        #endregion

        #region 第三层 波次信息及设置
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("波次信息:");
        if (levelIdx != curSelectIndex)
        {
            newRoundCount = level.Rounds.Count;
        }
        roundCount = EditorGUILayout.IntField(newRoundCount);
        EditorGUILayout.EndHorizontal();
        #endregion

        #region 第四层 怪物信息及设置
        if (roundCount != -1)
        {
            for (int i = 0; i < newRoundCount; i++)
            {
                MonsterIDList.Add(0);
                MonsterCountList.Add(0);
            }
        }
        newRoundCount = roundCount;
        if (roundCount > 0)
        {
            //能优化,应该能合并到一个for循环里
            for (int i = 0; i < level.Rounds.Count; i++)
            {
                //获取当前回合怪物ID以及获取当前回合出怪数
                //关卡切换判断
                if (levelIdx != curSelectIndex)
                {
                    //关卡切换后初始化
                    MonsterIDList[i] = 0;
                    MonsterCountList[i] = 0;

                    //初始化后将原本设置好的关卡显示
                    MonsterIDList[i] = level.Rounds[i].MonsterId;
                    MonsterCountList[i] = level.Rounds[i].Count;
                }
                //怪物ID显示与修改
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("怪物ID:");
                MonsterIDList[i] = EditorGUILayout.IntField(MonsterIDList[i]);

                //回合怪物数量显示与修改
                GUILayout.Label("怪物数量:");
                MonsterCountList[i] = EditorGUILayout.IntField(MonsterCountList[i]);
                EditorGUILayout.EndHorizontal();
            }
            for (int i = level.Rounds.Count; i < newRoundCount; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("怪物ID:");
                MonsterIDList[i] = EditorGUILayout.IntField(MonsterIDList[i]);

                GUILayout.Label("怪物数量:");
                MonsterCountList[i] = EditorGUILayout.IntField(MonsterCountList[i]);
                EditorGUILayout.EndHorizontal();
            }
            //判断切换关卡
            levelIdx = curSelectIndex;
        }
        #endregion

        #region 第五层 保存信息
        if (GUILayout.Button("保存关卡"))
        {
            //保存数据
            SaveLevel();
        }
        #endregion
    }
    private void LoadLevel()
    {
        //加载选中的关卡
        string fileName = levelFiles[curSelectIndex].Name;
        //Level设置
        level = new Level();
        Utils.LoadLevel(fileName, ref level);
        //设置给map对象的LoadLevel函数生成游戏信息,将level传入Map
        //该对象要在游戏运行时才存在
        map.LoadLevel(level);
    }

    private void LoadAllLevelFiles()
    {
        levelFiles = Utils.GetAllLevelFiles();
    }
    private string[] GetLevelFiles()
    {
        if (levelFiles == null || levelFiles.Count <= 0)
        {
            return null;
        }
        string[] result = new string[levelFiles.Count];
        for(int i = 0; i < result.Length; i++)
        {
            result[i] = levelFiles[i].Name;
        }
        return result;
    }

    //保存
    private void SaveLevel()
    {
        Level saveLevel = new Level();

        saveLevel.Name = level.Name;
        saveLevel.CardImage = level.CardImage;
        saveLevel.Road = level.Road;
        saveLevel.Background = level.Background;
        //saveLevel.InitScore = level.InitScore;
        if (!int.TryParse(initScore, out saveLevel.InitScore))//尝试去返回一个int，成功则true，否则false
        {
            EditorUtility.DisplayDialog("新建错误", "初始输入分数不合法!新建失败", "确定");
            return;
        }

        //saveLevel.Rounds = level.Rounds;
        for (int i = 0; i < newRoundCount; i++)
        {
            Rounds r = new Rounds(MonsterIDList[i], MonsterCountList[i]);
            saveLevel.Rounds.Add(r);
        }

        foreach (Tile tile in map.Road)
        {
            Point point = new Point(tile.X, tile.Y);
            saveLevel.Path.Add(point);
        }
        foreach (Tile tile in map.Grid)
        {
            if (tile.CanHold)
            {
                Point point = new Point(tile.X, tile.Y);
                saveLevel.Holder.Add(point);
            }
        }




        Utils.SaveLevel(levelFiles[curSelectIndex].FullName, saveLevel);
    }
    
    private Level LevelSettingSave(Level level)
    {
        level.Name = levelName;

        if (!int.TryParse(initScore, out level.InitScore))//尝试去返回一个int，成功则true，否则false
        {
            EditorUtility.DisplayDialog("新建错误", "初始输入分数不合法!新建失败", "确定");
            return null;
        }
        if (CardImage.objectReferenceValue != null)
        {
            level.CardImage = (CardImage.objectReferenceValue as Texture2D)?.name + ".png";
        }
        if (Background.objectReferenceValue != null)
        {
            level.Background = (Background.objectReferenceValue as Texture2D)?.name + ".png";
        }
        if (TempRoad.objectReferenceValue != null)
        {
            level.Road = (TempRoad.objectReferenceValue as Texture2D)?.name + ".png";
        }
        if (roundCout > 0)
        {
            for (int i = 0; i < roundCout; i++)
            {
                Rounds r = new Rounds(MonsterIDList[i], MonsterCountList[i]);
                level.Rounds.Add(r);
            }
        }
        return level;
    }
    
}
