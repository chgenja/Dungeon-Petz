using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    private View view;

    public Player[] players;
    public int round;
    public Phase currentPhase;
    public Player currentPlayer;

    public event Action<Phase> PhaseChanged = (x) => { };

    private List<Player> playerOrder = new List<Player>();

    void Start()
    {
        view = this.GetComponent<View>();

        //default pack of players
        players = new Player[4];
        players[0] = new Player("Vasya", PlayerColor.RED);
        players[1] = new Player("Sergey", PlayerColor.YELLOW);
        players[2] = new Player("Katya", PlayerColor.GREEN);
        players[3] = new Player("Kostya", PlayerColor.BLUE);
        view.UpdateNames();

        //selecting starting player
        int startingPlayer = Random.Range(0, players.Length);
        players[startingPlayer].isStartingPlayer = true;
        currentPlayer = players[startingPlayer];
        view.UpdateStartingPlayerMarker();
        view.UpdatePlayerColorBoard(currentPlayer);

        //starting round 1, first phase
        round = 1;
        view.UpdateRound();

        //player order for grouping phase
        int index = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].isStartingPlayer)
            {
                index = i;
                break;
            }
        }
        Debug.Log(index);
        for (int i = 0; i < players.Length; i++)
        {
            if (index + i == players.Length)
            {
                index = -i;
            }
            playerOrder.Add(players[index + i]);
        }


        PhaseChanged += OnPhaseChanged;

        currentPhase = Phase.SETUP;
        PhaseChanged(Phase.SETUP);
    }
    
    public void EndPhase()
    {        
        currentPhase += 1;
        OnPhaseChanged(currentPhase);
    }


    private void OnPhaseChanged(Phase phase)
    {
        switch (phase)
        {
            case Phase.SETUP:
                OnSetup();
                break;
            case Phase.SETUP_REVEALING_INFO:
                OnRevealInfo();
                break;
            case Phase.SETUP_ADDING_NEW_STUFF:
                OnAddingNewStuff();
                break;
            case Phase.GROUPING_PLAYER:
                //giving turn to 1st player
                if (playerOrder.Count != 0)
                {
                    currentPlayer = playerOrder[0];
                    playerOrder.Remove(currentPlayer);
                    OnGroupingPlayer(currentPlayer);
                }
                break;
        }
    }

    private void OnSetup()
    {
        //giving players gold

        //animation of receiving gold
        currentPhase = Phase.SETUP_VIEW_RECEIVE_GOLD;
        PhaseChanged(Phase.SETUP_VIEW_RECEIVE_GOLD);
    }

    private void OnRevealInfo()
    {
        //revealing info

        //animation of revealing info
        currentPhase = Phase.SETUP_VIEW_REVEALING_INFO;
        PhaseChanged(Phase.SETUP_VIEW_REVEALING_INFO);
    }

    private void OnAddingNewStuff()
    {
        //adding new stuff

        //animation of adding new stuff
        currentPhase = Phase.SETUP_ADDING_NEW_STUFF;
        PhaseChanged(Phase.SETUP_ADDING_NEW_STUFF);
    }

    private void OnGroupingPlayer(Player player)
    {
        //informing view about popup
        currentPhase = Phase.VIEW_GROUPING_PLAYER;
        PhaseChanged(Phase.VIEW_GROUPING_PLAYER);
    }
}