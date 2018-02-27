using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View: MonoBehaviour {
    private GameManager gameManager;
    private Player selectedPlayer;


    public Text[] playerNames;
    public GameObject[] startingPlayerMarkers;
    public GameObject[] roundMarkers;
    public GameObject playerColorBoard;
    public Sprite[] colorBoards;
    
    
    void Start()
    {
        gameManager = this.GetComponent<GameManager>();
    }

 
    public void UpdateNames()
    {
        for (int i = 0; i < gameManager.players.Length; i++)
        {
            playerNames[i].text = gameManager.players[i].GetName();
        }
    }

    public void UpdateStartingPlayerMarker()
    {
        for (int i = 0; i < gameManager.players.Length; i++)
        {
            if (gameManager.players[i].isStartingPlayer)
            {
                startingPlayerMarkers[i].SetActive(true);
            } else
            {
                startingPlayerMarkers[i].SetActive(false);
            }          
        }
    }

    public void UpdateRound()
    {
        for (int i = 0; i < roundMarkers.Length; i++)
        {
            if (gameManager.round-1 == i)
            {
                roundMarkers[i].SetActive(true);
            } else
            {
                roundMarkers[i].SetActive(false);
            }
        }
        
    }

    public void UpdatePlayerColorBoard(Player player)
    {
        switch (player.color)
        {
            case PlayerColor.RED:
                playerColorBoard.GetComponent<Image>().sprite = colorBoards[0];
                break;
            case PlayerColor.YELLOW:
                playerColorBoard.GetComponent<Image>().sprite = colorBoards[1];
                break;
            case PlayerColor.GREEN:
                playerColorBoard.GetComponent<Image>().sprite = colorBoards[2];
                break;
            case PlayerColor.BLUE:
                playerColorBoard.GetComponent<Image>().sprite = colorBoards[3];
                break;
        }
    }

}
