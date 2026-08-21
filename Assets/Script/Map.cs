using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

//加入一个负责接收鼠标输入的类，继承EventArgs方便传递数据
public class TileClickEventArgs : EventArgs
{
    public int mouseBotton;//左0右1
    public Tile tile;

    public TileClickEventArgs(int mouseBottom,Tile tile)
    {
        this.mouseBotton = mouseBottom;
        this.tile = tile;
    }
}

public class Map : MonoBehaviour
{

    public const int RowCount = 8;
    public const int ColumnCount = 12;

    float MapWidth;
    float MapHeight;

    float TileWidth;
    float TileHeight;

    public bool ShowGizmos = true;

#if UNITY_EDITOR//只在编辑器阶段存在
    [HideInInspector]
    public Texture2D CardImage;
    [HideInInspector]
    public Texture2D Background;
    [HideInInspector]
    public Texture2D TempRoad;
#endif

    //定义点击屏幕格子事件(功能前瞻性)(用委托)
    public event EventHandler<TileClickEventArgs> OnTileClickEvent;

    Level level;

    private List<Tile> grid;
    private List<Tile> road = new List<Tile>();

    //初始化地图格子
    void InitGrid()
    {
        grid = new List<Tile>();
        for(int i = 0; i < RowCount; i++)
        {
            for(int j = 0; j < ColumnCount; j++)
            {
                Tile tile = new Tile(i, j);
                
                grid.Add(tile);
                
            }
        }
    }

    public List<Tile> Grid
    {
        get { return grid; }
    }

    public List<Tile> Road
    {
        get { return this.road; }
    }

    public Vector3[] Path
    {
        get
        {
            List<Vector3> temp = new List<Vector3>();
            for(int i = 0; i < road.Count; i++)
            {
                temp.Add(GetPosition(road[i]));
            }
            return temp.ToArray();
        }
    }

    public Vector3 GetPosition(Tile tile)
    {
        float x = tile.Y * TileWidth + TileWidth / 2 - MapWidth / 2;
        float y = tile.X * TileHeight + TileHeight / 2 - MapHeight / 2;
        return new Vector3(x,y,0);
    }

    public Tile GetTile(Vector3 position)
    {
        int x = GetColByPositionX(position.x);
        int y = GetRowByPositionY(position.y);

        return GetTile(x, y);
    }

    public Tile GetTile(Point p)
    {
        return GetTile(p.X, p.Y);
    }

    public Tile GetTile(int x,int y)
    {
        int index = GetIndex(x, y);
        if (index < grid.Count)
        {
            return grid[index];
        }
        return null;
    }

    int GetIndex(int x,int y)
    {
        return y * ColumnCount + x;
    }
    //背景加载
    public void SetBackground(string fileName)
    {
        string path = "file://" + Const.MapPath + fileName;
        //动态加载背景
        GameObject go = GameObject.Find("Background");
        if (go == null)
        {
            Debug.LogError("找不到背景对象");
            return;
        }
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        StartCoroutine(Utils.LoadImageAsync(path, sr));
    }
    //图片加载
    public void SetRoad(string fileName)
    {
        string path = "file://" + Const.MapPath + fileName;

        GameObject go = GameObject.Find("Road");
        if (go == null)
        {
            Debug.LogError("找不到背景对象");
            return;
        }
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        StartCoroutine(Utils.LoadImageAsync(path, sr));
    }

    //绘制格子
    void CaculateSize()
    {
        Vector3 leftBottom = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightTop = Camera.main.ViewportToWorldPoint(Vector3.one);

        MapHeight = rightTop.y - leftBottom.y;
        MapWidth = MapHeight * 1.5f;

        TileWidth = MapWidth / ColumnCount;
        TileHeight = MapHeight / RowCount;
    }

    private void Awake()
    {
        CaculateSize();
        InitGrid();

        OnTileClickEvent += OnTileClick;
    }

    void OnTileClick(object sender, EventArgs args)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        TileClickEventArgs eventArgs = args as TileClickEventArgs;
        //左键设置路径点
        if (eventArgs.mouseBotton == 0 && !eventArgs.tile.CanHold)
        {
            if (road.Contains(eventArgs.tile))
            {
                //如果是寻路点则取消
                road.Remove(eventArgs.tile);
            }
            else
            {
                //不是则增加
                road.Add(eventArgs.tile);
            }
        }
        //右键设置放置点
        if (eventArgs.mouseBotton == 1 && !road.Contains(eventArgs.tile))
        {
            //直接取反
            eventArgs.tile.CanHold = !eventArgs.tile.CanHold;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TriggerClickTileEvent(0);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            TriggerClickTileEvent(1);
        }
    }

    void TriggerClickTileEvent(int mouseBottom)
    {
        Tile tile = GetUnderMouseButtom();
        //派发事件
        //1.构建时间参数
        TileClickEventArgs args = new TileClickEventArgs(mouseBottom, tile);

        //2.触发事件
        if (OnTileClickEvent != null)
        {
            OnTileClickEvent.Invoke(this, args);
        }
    }

    /// <summary>
    /// 返回鼠标点击的格子
    /// </summary>
    /// <returns></returns>
    Tile GetUnderMouseButtom()
    {
        //屏幕坐标转世界坐标
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int row = GetRowByPositionY(worldPoint.y);
        int col = GetColByPositionX(worldPoint.x);
        //转换成行列数
        return GetTile(col, row);
    }

    int GetColByPositionX(float x)
    {
        return (int)((x + this.MapWidth / 2) / TileWidth);
    }
    int GetRowByPositionY(float y)
    {
        return (int)((y + this.MapHeight / 2) / TileHeight);
    }

    public void LoadLevel(Level level)
    {
        Clear();
        this.level = level;

        this.SetBackground(level.Background);
        this.SetRoad(level.Road);

        //填充Holder
        for(int i = 0; i < level.Holder.Count; i++)
        {
            Point p = level.Holder[i];
            
            Tile tile = GetTile(p);
            tile.CanHold = true;
        }
        //填充路径点
        for (int i = 0; i < level.Path.Count; i++)
        {
            Point p = level.Path[i];
            Tile tile = GetTile(p);
            road.Add(tile);
        }
    }

    public void ClearHolder()
    {
        for(int i = 0; i < this.grid.Count; i++)
        {
            this.grid[i].CanHold = false;
        }
    }

    public void ClearRoad()
    {
        road.Clear();
    }

    public void Clear()
    {
        ClearHolder();
        ClearRoad();
    }

    private void OnDrawGizmos()
    {
        if (!ShowGizmos) return;
        CaculateSize();
        Gizmos.color = Color.green;
        //绘制行
        for(int i = 0; i <= RowCount; i++)
        {
            Gizmos.DrawLine(
                new Vector3(-MapWidth / 2, i * TileHeight - MapHeight / 2),
                new Vector3(MapWidth / 2, i * TileHeight - MapHeight / 2));
        }
        //绘制列
        for (int i = 0; i <= ColumnCount; i++)
        {
            Gizmos.DrawLine(
                new Vector3(i * TileHeight - MapWidth / 2, -MapHeight / 2),
                new Vector3(i * TileHeight - MapWidth / 2, MapHeight / 2));
        }
        //绘制寻路点+可放置点
        //绘制起点+终点
        if (grid == null) return;
        Gizmos.color = Color.red;
        if (road.Count > 0)
        {
            for(int i = 0; i < road.Count; i++)
            {
                if (i == 0)
                {
                    Gizmos.DrawIcon(GetPosition(road[i]), "start");
                }
                if (i == road.Count - 1 && i >= 1)
                {
                    Gizmos.DrawIcon(GetPosition(road[i]), "end");
                }
                if (i < road.Count - 1)
                {
                    Gizmos.DrawLine(GetPosition(road[i]), GetPosition(road[i + 1]));
                }
            }
        }
        //绘制可放置点
        for(int i = 0; i < grid.Count; i++)
        {
            if (grid[i].CanHold)
            {
                Gizmos.DrawIcon(GetPosition(grid[i]), "holder");
            }
        }

    }
}
