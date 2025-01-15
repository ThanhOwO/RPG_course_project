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
    public Serializable_Dictionary<string, bool> savePoints;
    public string closestSavepointID;
    public float lastDeathX;
    public float lastDeathY;
    public int lastDeathAmount;
    public Serializable_Dictionary<string, float> volumeSettings;

    public GameData()
    {
        this.lastDeathAmount = 0;
        this.lastDeathX = 0;
        this.lastDeathY = 0;
        
        this.currency = 0;
        skillTree = new Serializable_Dictionary<string, bool>();
        inventory = new Serializable_Dictionary<string, int>();
        equipmentID = new List<string>();

        closestSavepointID = string.Empty;
        savePoints = new Serializable_Dictionary<string, bool>();
        volumeSettings = new Serializable_Dictionary<string, float>();
    }
}
