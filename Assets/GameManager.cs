using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Diagnostics;

public class GameManager : MonoBehaviour
{
    private View view;

    public Player[] players;
    public int round;
    public Phase currentPhase;
    public Player currentPlayer;

    public event Action<Phase> PhaseChanged = (x) => { };

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

        PhaseChanged += OnPhaseChanged;

        currentPhase = Phase.SETUP;
        OnPhaseChanged(Phase.SETUP);
    }

    private void OnPhaseChanged(Phase phase)
    {
        switch (phase)
        {
            case Phase.SETUP:
                //OnSetup();
                break;
        }
    }

    private void OnSetup()
    {
        //getting income
        view.PrintTextToLog("Receiving gold");
        WaitTime(3000);
        //revealing new information
        view.PrintTextToLog("Revealing new information");
        WaitTime(3000);
        //adding new stuff
        view.PrintTextToLog("Adding new stuff");
        WaitTime(3000);
        //moving to next phase
        currentPhase = Phase.GROUPING;
        OnPhaseChanged(Phase.GROUPING);
    }

    private void WaitTime(int milisecs)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        for (int i = 0; ; i++)
        {
            if (i % 1000 == 0)
            {
                sw.Stop();
                if (sw.ElapsedMilliseconds > milisecs)
                {
                    break;                   
                } else
                {
                    sw.Start();
                }
            }
        }
    }
}