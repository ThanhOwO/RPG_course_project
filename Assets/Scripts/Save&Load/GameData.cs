using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public int currency;
    public Serializable_Dictionary<string, int> inventory;

    public GameData()
    {
        this.currency = 0;
        inventory = new Serializable_Dictionary<string, int>();
    }
}
