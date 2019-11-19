using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BombType
{
    Regular = 0,
    Acid = 1,
    Screamer = 2,
    HolyWater = 3,
    Fire = 4,
    Mega = 5,
    Void = 6
}
public class BombData
{
    private int level;
    private BombType bombType;
    private bool locked;
    private string name;
    private string spritePath;


    public BombData(BombType bombType, int level, bool locked, string name, string spritePath)
    {
        this.bombType = bombType;
        this.level = level;
        this.locked = locked;
        this.name = name;
        this.spritePath = spritePath;
    }

    public int Level { get => level; set => level = value; }
    public BombType BombType { get => bombType; }
    public bool Locked { get => locked; set => locked = value; }
    public string Name { get => name; }
    public string SpritePath { get => spritePath;}
}