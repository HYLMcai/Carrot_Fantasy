using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rounds
{
    public int MonsterId;//怪物ID

    public int Count;//怪物数量
    public Rounds(int x, int y)
    {
        this.MonsterId = x;
        this.Count = y;
    }
}
