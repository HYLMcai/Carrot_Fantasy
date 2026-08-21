using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    private AudioSource musicSource;//音乐
    private AudioSource effectSource;//音效

    //放音频文件的路径
    private string resourcePath = "Sound";

   //音量设置
    private float musicVolume = 1f;
    private float effectVolume = 1f;

    protected override void Initial()
    {
        //生成播放音频的对象
        GameObject go = new GameObject("SoundManager");

        //插入音乐插件并设置
        musicSource = go.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;

        effectSource = go.AddComponent<AudioSource>();
        effectSource.playOnAwake = false;
        effectSource.loop = false;

        Object.DontDestroyOnLoad(go);
    }
    
    //音量调节
    public float MusicVolume
    {
        get { return musicVolume; }
        set { musicVolume = Mathf.Clamp(0, 1, value); }
    }
    public float EffectVolume
    {
        get { return effectVolume; }
        set { effectVolume = Mathf.Clamp(0, 1, value); }
    }

    ////功能接口
    /// <summary>
    /// 播放音乐
    /// </summary>
    /// <param name="path">设置音频地址，又或者是音频文件名称</param>
    /// <param name="isLoop">控制是否循环，一般为true</param>
    public void PlayMusic(string path,bool isLoop = true)
    {
        //检验是否播放同一段音乐
        if (musicSource.clip.name == path)
        {
            return;
        }
        //如果不是同一段则加载新一段音频
        path = resourcePath + "/" + path;
        AudioClip clip = Resources.Load<AudioClip>(path);//新的音频文件返回，用load的音频泛型传回
        //实行播放操作
        musicSource.clip = clip;
        musicSource.Play();
        musicSource.loop = isLoop;
    }
    /// <summary>
    /// 音效控制(会根据点击频率重新播放)
    /// </summary>
    /// <param name="path">设置音频地址，又或者是音频文件名称</param>
    /// <param name="isLoop">控制是否循环，一般为true</param>
    public void PlayEffect(string path,bool isLoop = false)
    {
        if (effectSource.clip.name == path)
        {
            effectSource.Play();
            return;
        }
        //如果不是同一段则加载新一段音频
        path = resourcePath + "/" + path;
        AudioClip clip = Resources.Load<AudioClip>(path);//新的音频文件返回，用load的音频泛型传回
        //实行播放操作
        effectSource.clip = clip;
        effectSource.Play();
        effectSource.loop = isLoop;
    }
    /// <summary>
    /// 音效控制(不清除原本的音效，同时播放)
    /// </summary>
    /// <param name="path">设置音频地址，又或者是音频文件名称</param>
    public void PlayOneShot(string path)
    {
        path = resourcePath + "/" + path;
        AudioClip clip = Resources.Load<AudioClip>(path);
        effectSource.PlayOneShot(clip);
        //思考：多个同时触发，可以通过缓存池节省开销，但是什么时候释放？
    }
}
