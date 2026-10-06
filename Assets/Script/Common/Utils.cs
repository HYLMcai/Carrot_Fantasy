using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.Networking;
using System.IO;
using System.Text;
using UnityEngine.UI;

//GameUtils
//
public class Utils
{
    /// <summary>
    /// 加载配置表文件，解析内容，输出对应的Level对象
    /// </summary>
    /// <param name="path">加载的关卡文件名</param>
    /// <param name="level">对应的Level对象</param>
    public static void LoadLevel(string path,ref Level level)
    {
        path = Const.LevelConfigPath + path;
        //加载配置表文件
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        //读取基础信息
        //获得根节点
        XmlElement root = doc.DocumentElement;
        //获取节点信息
        level.Name = root.SelectSingleNode("Name").InnerText;
        level.CardImage = root.SelectSingleNode("CardImage").InnerText;
        level.Background = root.SelectSingleNode("Background").InnerText;
        level.Road = root.SelectSingleNode("Road").InnerText;
        level.InitScore = int.Parse(root.SelectSingleNode("InitScore").InnerText);
        //读取可放置区域
        XmlNodeList holderNodeList = root.SelectNodes("Holder/Point");
        foreach(XmlNode node in holderNodeList)
        {
            Point p = new Point(int.Parse(node.Attributes["X"].Value), int.Parse(node.Attributes["Y"].Value));
            level.Holder.Add(p);
        }
        //读取路径点
        XmlNodeList pathNodeList = root.SelectNodes("Path/Point");
        for(int i = 0; i < pathNodeList.Count; i++)
        {
            Point p = new Point(int.Parse(pathNodeList[i].Attributes["X"].Value), int.Parse(pathNodeList[i].Attributes["Y"].Value));
            level.Path.Add(p);
        }
        //读取怪物刷怪信息
        XmlNodeList roundsNodeList = root.SelectNodes("Rounds/Round");
        foreach(XmlNode node in roundsNodeList)
        {
            Rounds p = new Rounds(int.Parse(node.Attributes["Monster"].Value), int.Parse(node.Attributes["Count"].Value));
            level.Rounds.Add(p);
        }
    } 
    public static IEnumerator LoadImageAsync(string path,SpriteRenderer sr)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(path);
        yield return request.SendWebRequest();
        //执行到这里表示加载结束
        if (request.isDone)
        {
            //isDone==true，资源加载成功
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite sprite = Sprite.Create(texture, new Rect(new Vector2(0, 0), new Vector2(texture.width, texture.height)),new Vector2(0.5f,0.5f));
            sr.sprite = sprite;
        }
    }
    public static IEnumerator LoadSpriteAsync(string path, Image iamge)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(path);
        yield return request.SendWebRequest();
        //执行到这里表示加载结束
        if (request.isDone)
        {
            //isDone==true，资源加载成功
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite sprite = Sprite.Create(texture, new Rect(new Vector2(0, 0), new Vector2(texture.width, texture.height)), new Vector2(0.5f, 0.5f));
            iamge.sprite = sprite;
        }
    }

    public static List<FileInfo> GetAllLevelFiles()
    {
        string[] files = Directory.GetFiles(Const.LevelConfigPath, "*.xml");
        List<FileInfo> list = new List<FileInfo>();
        for(int i = 0; i < files.Length; i++)
        {
            FileInfo fileinfo = new FileInfo(files[i]);
            list.Add(fileinfo);
        }
        return list;
    }

    public static void SaveLevel(string path,Level level)
    {
        //负责处理字符串拼接的一个类
        //字符串内存池,不会因循环拼接导致内存浪费,频繁处理字符串的时候使用
        StringBuilder sb = new StringBuilder();
        sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
        sb.Append("<Level>\n");
        sb.Append(string.Format("\t<Name>{0}</Name>\n", level.Name));
        sb.Append(string.Format("\t<CardImage>{0}</CardImage>\n", level.CardImage));
        sb.Append(string.Format("\t<Background>{0}</Background>\n", level.Background));
        sb.Append(string.Format("\t<Road>{0}</Road>\n", level.Road));
        sb.Append(string.Format("\t<InitScore>{0}</InitScore>\n", level.InitScore));

        sb.Append("\t<Holder>\n");
        foreach(Point point in level.Holder)
        {
            sb.Append(string.Format("\t\t<Point X=\"{0}\" Y=\"{1}\" />\n", point.Y, point.X));
        }
        sb.Append("\t</Holder>\n");

        sb.Append("\t<Path>\n");
        foreach (Point point in level.Path)
        {
            sb.Append(string.Format("\t\t<Point X=\"{0}\" Y=\"{1}\" />\n", point.Y, point.X));
        }
        sb.Append("\t</Path>\n");
        sb.Append("\t<Rounds>\n");
        foreach (Rounds rounds in level.Rounds)
        {
            sb.Append(string.Format("\t\t<Round Monster=\"{0}\" Count=\"{1}\" />\n", rounds.MonsterId, rounds.Count));
        }
        sb.Append("\t</Rounds>\n");

        sb.Append("</Level>");
        File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
    }
}
