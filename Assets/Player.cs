using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

    private string name;
    public PlayerColor color;

    public bool isStartingPlayer = false;
	
    public Player(string name, PlayerColor color)
    {
        this.name = name;
        this.color = color;
    }

    public string GetName()
    {
        return name;
    }
}
