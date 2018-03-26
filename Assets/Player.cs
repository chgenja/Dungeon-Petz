using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

    private string name;
    public PlayerColor color;

    public bool isStartingPlayer = false;
    public int impCount;
    public int gold;
	
    public Player(string name, PlayerColor color)
    {
        this.name = name;
        this.color = color;
        impCount = 6;
        gold = 0;
    }

    public string GetName()
    {
        return name;
    }
}
