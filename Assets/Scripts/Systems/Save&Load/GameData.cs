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
    //public string closestSavepointID;
    public string lastActivatedSavepointID;
    public string lastActivatedRoomID;
    public float lastDeathX;
    public float lastDeathY;
    public int lastDeathAmount;
    //public Serializable_Dictionary<string, float> volumeSettings;
    public Vector2 playerLastestPosition;
    public List<string> discoveredRooms;
    public List<Vector3Int> clearedFogTiles;
    //Boss room 4 gate
    public bool isGateOpened;
    //Healing potion storage
    public int flaskStorage;

    public GameData()
    {
        this.flaskStorage = 0;
        this.lastDeathAmount = 0;
        this.lastDeathX = 0;
        this.lastDeathY = 0;
        playerLastestPosition = Vector2.zero;
        
        this.currency = 0;
        skillTree = new Serializable_Dictionary<string, bool>();
        inventory = new Serializable_Dictionary<string, int>();
        equipmentID = new List<string>();

        //closestSavepointID = string.Empty;
        lastActivatedSavepointID = string.Empty;
        lastActivatedRoomID = string.Empty;
        savePoints = new Serializable_Dictionary<string, bool>();
        //volumeSettings = new Serializable_Dictionary<string, float>();

        discoveredRooms = new List<string>();
        clearedFogTiles = new List<Vector3Int>();

        isGateOpened = false;
    }
}
