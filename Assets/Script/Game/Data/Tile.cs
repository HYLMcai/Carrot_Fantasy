using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//格子设置
public class Tile
{
    //格子的纵横向索引
    public int X;
    public int Y;
    //格子是否能放
    public bool CanHold;
    //炮塔实例
    public object data;
    
    public Tile(int x,int y)
    {
        this.X = x;
        this.Y = y;
    }

    public override string ToString()
    {
        return string.Format("{X:{0};Y={1},CanHold={2}}", this.X, this.Y, this.CanHold);
    }
}
