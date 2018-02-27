using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private View view;

    public Player[] players;
    public int round;
    public Phase currentPhase;
    public Player currentPlayer;


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
        currentPhase = Phase.SETUP;
        
    }




}
