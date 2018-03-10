using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class View: MonoBehaviour {
    public GameManager gameManager;
    private Player selectedPlayer;


    public Text[] playerNames;
    public GameObject startingPlayerMarker;
    public GameObject[] startingPlayerMarkerPositions;
    public GameObject roundMarker;
    public GameObject[] roundMarkerPositions;
    public GameObject playerColorBoard;
    public Sprite[] colorBoards;
    public Text logText;
    public GameObject groupingPopup;
    public GameObject endGamePopup;

    void Start()
    {
        gameManager.PhaseChanged += OnPhaseChanged;
        gameManager.StartingPlayerChanged += UpdateStartingPlayerMarker;
        gameManager.RoundChanged += UpdateRound;
        gameManager.CurrentPlayerChanged += UpdatePlayerColorBoard;

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
        if (gameManager.round == 1)
        {
            for (int i = 0; i < gameManager.players.Length; i++)
            {
                if (gameManager.players[i].isStartingPlayer)
                {
                    startingPlayerMarker.transform.position = startingPlayerMarkerPositions[i].transform.position;
                    startingPlayerMarker.SetActive(true);
                }
            }
        } else
        {
            //finding index of current starting player
            int index = 0;
            for (int i = 0; i < gameManager.players.Length; i++)
            {
                if (gameManager.players[i].isStartingPlayer)
                {
                    index = i;
                    break;
                }
            }
            StartCoroutine(MovingMarker(startingPlayerMarker, startingPlayerMarkerPositions[index].transform.position, 1));
        }
    }

    public void UpdateRound()
    {
        if (gameManager.round == 1)
        {
            roundMarker.transform.position = roundMarkerPositions[0].transform.position;
            roundMarker.SetActive(true);
        } else
        {
            StartCoroutine(MovingMarker(roundMarker,roundMarkerPositions[gameManager.round - 1].transform.position,1));
        }
        
    }

    private IEnumerator MovingMarker(GameObject gameObject, Vector3 newPosition, float seconds)
    {
        float elapsedTime = 0f;
        Vector3 startingPos = gameObject.transform.position;
        while (elapsedTime < seconds)
        {
            gameObject.transform.position = Vector3.Lerp(startingPos, newPosition, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();            
        }
        gameObject.transform.position = newPosition;
        yield return new WaitForSeconds(seconds);
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
            case Phase.VIEW_EVALUATE_SHOPPING_ACTION:
                {
                    StartCoroutine(SkippingAnimation(phase));
                    break;
                }
            case Phase.EVERYONE_SHOPPED:
                {
                    PrintTextToLog("All players shopped");
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
            case Phase.VIEW_EVALUATE_ARRANGING_PETS:
                {
                    StartCoroutine(SkippingAnimation(phase));
                    break;
                }
            case Phase.EVERYONE_ARRANGED:
                {
                    PrintTextToLog("All players arranged pets");
                    gameManager.EndPhase();
                    break;
                }
            case Phase.VIEW_NEED_CARDS_DEALT:
                {
                    PrintTextToLog("Player " + gameManager.currentPlayer.GetName() + " taking care about pets");
                    groupingPopup.SetActive(true);
                    gameManager.EndPhase();
                    break;
                }
            case Phase.VIEW_EVALUATE_NEED_CARDS:
                {                 
                    StartCoroutine(SkippingAnimation(phase));
                    break;
                }
            case Phase.EVERYONE_NEEDED:
                {
                    PrintTextToLog("All players evaluated pet needs");
                    gameManager.EndPhase();
                    break;
                }
            case Phase.VIEW_EXHIBITION:
                {
                    PrintTextToLog("Player " + gameManager.currentPlayer.GetName() + " taking part in exhibition");
                    groupingPopup.SetActive(true);
                    gameManager.EndPhase();
                    break;
                }
            case Phase.VIEW_EVALUATE_EXHIBITION:
                {
                    StartCoroutine(SkippingAnimation(phase));
                    break;
                }
            case Phase.EVERYONE_EXHIBITION:
                {
                    PrintTextToLog("All players took part in exhibition");
                    gameManager.EndPhase();
                    break;
                }
            case Phase.VIEW_BUSINESS:
                {
                    PrintTextToLog("Player " + gameManager.currentPlayer.GetName() + " making business");
                    groupingPopup.SetActive(true);
                    gameManager.EndPhase();
                    break;
                }
            case Phase.VIEW_EVALUATE_BUSINESS:
                {
                    StartCoroutine(SkippingAnimation(phase));
                    break;
                }
            case Phase.EVERYONE_BUSINESS:
                {
                    PrintTextToLog("All players took part in exhibition");
                    gameManager.EndPhase();
                    break;
                }
            case Phase.VIEW_USING_IMPS:
                {
                    PrintTextToLog("Player " + gameManager.currentPlayer.GetName() + " using imps lefted");
                    groupingPopup.SetActive(true);
                    gameManager.EndPhase();
                    break;
                }
            case Phase.VIEW_EVALUATE_USING_IMPS:
                {
                    StartCoroutine(SkippingAnimation(phase));
                    break;
                }
            case Phase.EVERYONE_USING_IMPS:
                {
                    PrintTextToLog("All players used all their imps");
                    gameManager.EndPhase();
                    break;
                }
            case Phase.VIEW_AGING:
                {
                    StartCoroutine(SkippingAnimation(phase));
                    break;
                }
            case Phase.VIEW_STARTING_NEXT_ROUND:
                {
                    StartCoroutine(SkippingAnimation(phase));
                    break;
                }
            case Phase.VIEW_GAME_END:
                {
                    endGamePopup.SetActive(true);
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
                yield return new WaitForSeconds(1);
                gameManager.EndPhase();
                break;
            case Phase.SETUP_VIEW_REVEALING_INFO:
                PrintTextToLog("Revealing new information");
                yield return new WaitForSeconds(1);
                gameManager.EndPhase();
                break;
            case Phase.SETUP_VIEW_ADDING_NEW_STUFF:
                PrintTextToLog("Adding new stuff");
                yield return new WaitForSeconds(1);
                gameManager.EndPhase();
                break;
            case Phase.VIEW_EVALUATE_SHOPPING_ACTION:
                groupingPopup.SetActive(false);
                PrintTextToLog("Player " + gameManager.currentPlayer.GetName() + " making an action");
                yield return new WaitForSeconds(1);
                gameManager.EndPhase();
                break;
            case Phase.VIEW_EVALUATE_ARRANGING_PETS:
                groupingPopup.SetActive(false);
                PrintTextToLog("Player " + gameManager.currentPlayer.GetName() + " arranged pets");
                yield return new WaitForSeconds(1);
                gameManager.EndPhase();
                break;
            case Phase.VIEW_EVALUATE_NEED_CARDS:
                groupingPopup.SetActive(false);
                PrintTextToLog("Player " + gameManager.currentPlayer.GetName() + " evaluating needs");
                yield return new WaitForSeconds(1);
                gameManager.EndPhase();
                break;
            case Phase.VIEW_EVALUATE_EXHIBITION:
                groupingPopup.SetActive(false);
                PrintTextToLog("Player " + gameManager.currentPlayer.GetName() + " evaluating exhibition");
                yield return new WaitForSeconds(1);
                gameManager.EndPhase();
                break;
            case Phase.VIEW_EVALUATE_BUSINESS:
                groupingPopup.SetActive(false);
                PrintTextToLog("Player " + gameManager.currentPlayer.GetName() + " evaluating business");
                yield return new WaitForSeconds(1);
                gameManager.EndPhase();
                break;
            case Phase.VIEW_EVALUATE_USING_IMPS:
                groupingPopup.SetActive(false);
                PrintTextToLog("Player " + gameManager.currentPlayer.GetName() + " evaluating unused imps");
                yield return new WaitForSeconds(1);
                gameManager.EndPhase();
                break;
            case Phase.VIEW_AGING:
                groupingPopup.SetActive(false);
                PrintTextToLog("Aging pets");
                yield return new WaitForSeconds(1);
                gameManager.EndPhase();
                break;
            case Phase.VIEW_STARTING_NEXT_ROUND:
                PrintTextToLog("Starting next round");
                yield return new WaitForSeconds(1);
                gameManager.EndPhase();
                break;
        }  
    }

}
