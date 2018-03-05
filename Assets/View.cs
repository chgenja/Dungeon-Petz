using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class View: MonoBehaviour {
    public GameManager gameManager;
    private Player selectedPlayer;


    public Text[] playerNames;
    public GameObject[] startingPlayerMarkers;
    public GameObject[] roundMarkers;
    public GameObject playerColorBoard;
    public Sprite[] colorBoards;
    public Text logText;
    public GameObject groupingPopup;

    void Start()
    {
        gameManager.PhaseChanged += OnPhaseChanged;

    }

    void Update()
    {
        
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

    public void OnPhaseChanged(Phase phase)
    {
        switch (phase) {
            case Phase.SETUP_VIEW_RECEIVE_GOLD:
                {
                    StartCoroutine(SkippingAnimation(phase));
                    break;
                }
            case Phase.SETUP_VIEW_REVEALING_INFO:
                {
                    StartCoroutine(SkippingAnimation(phase));
                    break;
                }
            case Phase.SETUP_VIEW_ADDING_NEW_STUFF:
                {
                    StartCoroutine(SkippingAnimation(phase));
                    break;
                }
            case Phase.VIEW_GROUPING_PLAYER:
                {
                    PrintTextToLog("Player " + gameManager.currentPlayer.GetName() + " grouping imps");
                    groupingPopup.SetActive(true);
                    gameManager.EndPhase();
                    break;
                }
            case Phase.EVERYONE_GROUPED:
                {
                    PrintTextToLog("All players grouped imps");
                    groupingPopup.SetActive(false);
                    gameManager.EndPhase();
                    break;
                }
            case Phase.VIEW_SHOPPING_ACTION:
                {
                    PrintTextToLog("Player " + gameManager.currentPlayer.GetName() + " shopping action");                    
                    groupingPopup.SetActive(true);
                    gameManager.EndPhase();
                    break;
                }
            case Phase.EVERYONE_SHOPPED:
                {
                    PrintTextToLog("All players shopped");
                    groupingPopup.SetActive(false);
                    gameManager.EndPhase();
                    break;
                }
            case Phase.VIEW_ARRANGING_PETS:
                {
                    PrintTextToLog("Player " + gameManager.currentPlayer.GetName() + " arranging pets");
                    groupingPopup.SetActive(true);
                    gameManager.EndPhase();
                    break;
                }
            case Phase.EVERYONE_ARRANGED:
                {
                    PrintTextToLog("All players arranged pets");
                    groupingPopup.SetActive(false);
                    gameManager.EndPhase();
                    break;
                }
        } 
    }

    public void PrintTextToLog(string text)
    {
        logText.text = text;
    }

    private IEnumerator SkippingAnimation(Phase phase)
    {        
        switch (phase)
        {
            case Phase.SETUP_VIEW_RECEIVE_GOLD:
                PrintTextToLog("Giving gold to players");
                yield return new WaitForSeconds(3);
                gameManager.EndPhase();
                break;
            case Phase.SETUP_VIEW_REVEALING_INFO:
                PrintTextToLog("Revealing new information");
                yield return new WaitForSeconds(3);
                gameManager.EndPhase();
                break;
            case Phase.SETUP_VIEW_ADDING_NEW_STUFF:
                PrintTextToLog("Adding new stuff");
                yield return new WaitForSeconds(3);
                gameManager.EndPhase();
                break;
        }  
    }

}
