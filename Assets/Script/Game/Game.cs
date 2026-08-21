using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Game : MonoBehaviour
{
    private static Game instance;
    public static Game GetInstance()
    {
        return instance;
    }
    //--组合模式
    //因为这个类绑定的对象dontdestory贯串这个游戏,因此在start里获取号单例对象,之后直接Game.xxx直接调用
    public PoolManager Pool;
    public SoundManager Sound;
    public StaticData StaticData;
    private void Awake()
    {
        instance = this;
        //这里会读取后台配置信息（json格式，内容为头像资源地址，渠道，平台，资源服务器地址）
        //对比资源版本号
    }

    private void Start()
    {
        //--初始化
        //dontdestory
        DontDestroyOnLoad(this.gameObject);
        //获取单例
        Pool = PoolManager.GetInstance();
        Sound = SoundManager.GetInstance();
        StaticData = StaticData.GetInstance();
        //--启动完转跳到start场景
        //页面转跳时运行
        SceneManager.sceneLoaded += OnSceneLoaded;
        //启动游戏时通过StartUp事件注册Model和Controller,View在打开和关闭时动态注册和取消
        //1.定义处理StartUp事件的Controller
        //2.注册这个Controller
        MVC.RegisterController(EventType.StartUp, typeof(StartUpCommand));
        MVC.SendEvent(EventType.StartUp, null);
        //初始化dotween
        DOTween.Init(false/*运行完是否自动销毁*/, true/*是否安全模式*/, LogBehaviour.Default);
        //进入游戏
        LoadScene(1);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MSceneArgs args = new MSceneArgs(scene.buildIndex, scene.name);
        MVC.SendEvent(EventType.EnterScene, args);
    }

    public void LoadScene(int level)
    {
        //--进入新场景时退出旧场景
        //构建事件参数
        Scene activeScene = SceneManager.GetActiveScene();
        MSceneArgs argsExit = new MSceneArgs(activeScene.buildIndex, activeScene.name);
        //发送事件
        MVC.SendEvent(EventType.ExitScene, argsExit);

        SceneManager.LoadScene(level);
    }
}
