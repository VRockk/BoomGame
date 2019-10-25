using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BombType
{
    Regular = 0,
    Acid = 1
}
public class BombData
{
    private int[] bombUpgradeLevels = new int[4];
    private BombType bombType;
    private bool bombUnlocked;

    public BombType BombType { get => bombType; set => bombType = value; }
    public int[] BombUpgradeLevels { get => bombUpgradeLevels; set => bombUpgradeLevels = value; }
    public bool BombUnlocked { get => bombUnlocked; set => bombUnlocked = value; }

    public BombData(BombType bombType, int[] bombUpgradeLevels, bool bombUnlocked )
    {
        this.bombType = bombType;
        this.bombUpgradeLevels = bombUpgradeLevels;
        this.bombUnlocked = bombUnlocked;
    }

}