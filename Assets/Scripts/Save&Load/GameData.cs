using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public int currency;

    public Serializable_Dictionary<string, bool> skillTree;
    public Serializable_Dictionary<string, int> inventory;
    public List<string> equipmentID;

    public GameData()
    {
        this.currency = 0;
        skillTree = new Serializable_Dictionary<string, bool>();
        inventory = new Serializable_Dictionary<string, int>();
        equipmentID = new List<string>();
    }
}
