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
    public event Action RoundChanged = () => { };
    public event Action StartingPlayerChanged = () => { };
    public event Action<Player> CurrentPlayerChanged = (x) => { };

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
        view.UpdatePlayerColorBoard(currentPlayer);
        view.UpdateImpCounts();
        view.UpdateGoldCounts();

        //starting round 1, first phase
        round = 1;
        view.UpdateRound();

        view.UpdateStartingPlayerMarker();

        PhaseChanged += OnPhaseChanged;

        currentPhase = Phase.SETUP;
        PhaseChanged(Phase.SETUP);
    }
    
    public void EndPhase()
    {        
        currentPhase += 1;
        PhaseChanged(currentPhase);
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
            case Phase.SETUP_GROUPING_PLAYER:
                OrganizeDefaultPlayerOrder();
                EndPhase();
                break;
            case Phase.GROUPING_PLAYER:
                //giving turn to a player
                if (playerOrder.Count != 0)
                {                
                    currentPlayer = playerOrder[0];
                    CurrentPlayerChanged(currentPlayer);
                    playerOrder.Remove(currentPlayer);
                    OnGroupingPlayer(currentPlayer);
                }
                break;
            case Phase.CHECK_GROUPING_PLAYERS:
                {
                    OnCheckGroupingPlayers();
                    break;
                }
            case Phase.SETUP_SHOPPING_ACTIONS:
                {
                    //creating list of actions, now using only starting player
                    for (int i = 0; i < players.Length; i++)
                    {
                        if (players[i].isStartingPlayer)
                        {
                            currentPlayer = players[i];
                            CurrentPlayerChanged(currentPlayer);
                            break;
                        }
                    }
                    EndPhase();
                    break;
                }
            case Phase.SHOPPING_ACTION:
                {
                    OnShoppingAction(currentPlayer);
                    break;
                }
            case Phase.EVALUATE_SHOPPING_ACTION:
                {
                    OnEvaluateShoppingAction();
                    break;
                }
            case Phase.CHECK_SHOPPING_ACTIONS:
                {
                    OnCheckShoppingActions();
                    break;
                }
            case Phase.SETUP_ARRANGING_PETS:
                OrganizeDefaultPlayerOrder();
                EndPhase();
                break;
            case Phase.ARRANGING_PETS:
                //giving turn to a player
                if (playerOrder.Count != 0)
                {
                    currentPlayer = playerOrder[0];
                    CurrentPlayerChanged(currentPlayer);
                    playerOrder.Remove(currentPlayer);
                    OnArrangingPets(currentPlayer);
                }
                break;
            case Phase.EVALUATE_ARRANGING_PETS:
                {
                    OnEvaluateArrangingPets();
                    break;
                }
            case Phase.CHECK_ARRANGING_PETS:
                {
                    OnCheckArrangingPets();
                    break;
                }
            case Phase.SETUP_NEED_CARDS:
                {
                    OrganizeDefaultPlayerOrder();
                    EndPhase();
                    break;
                }
            case Phase.NEED_CARDS_DEALT:
                {
                    if (playerOrder.Count != 0)
                    {
                        currentPlayer = playerOrder[0];
                        CurrentPlayerChanged(currentPlayer);
                        playerOrder.Remove(currentPlayer);
                        OnNeedCardsDealt();
                    }
                    break;
                }
            case Phase.EVALUATE_NEED_CARDS:
                {
                    OnEvaluateNeedCards();
                    break;
                }
            case Phase.CHECK_NEED_CARDS:
                {
                    OnCheckNeedCards();
                    break;
                }
            case Phase.SETUP_EXHIBITION:
                {
                    OrganizeDefaultPlayerOrder();
                    EndPhase();
                    break;
                }
            case Phase.EXHIBITION:
                {
                    if (playerOrder.Count != 0)
                    {
                        currentPlayer = playerOrder[0];
                        CurrentPlayerChanged(currentPlayer);
                        playerOrder.Remove(currentPlayer);
                        OnExhibition();
                    }
                    break;
                }
            case Phase.EVALUATE_EXHIBITION:
                {
                    OnEvaluateExhibition();
                    break;
                }
            case Phase.CHECK_EXHIBITION:
                {
                    OnCheckExhibition();
                    break;
                }
            case Phase.SETUP_BUSINESS:
                {
                    OrganizeDefaultPlayerOrder();
                    EndPhase();
                    break;
                }
            case Phase.BUSINESS:
                {
                    if (playerOrder.Count != 0)
                    {
                        currentPlayer = playerOrder[0];
                        CurrentPlayerChanged(currentPlayer);
                        playerOrder.Remove(currentPlayer);
                        OnBusiness();
                    }
                    break;
                }
            case Phase.EVALUATE_BUSINESS:
                {
                    OnEvaluateBusiness();
                    break;
                }
            case Phase.CHECK_BUSINESS:
                {
                    OnCheckBusiness();
                    break;
                }
            case Phase.SETUP_USING_IMPS:
                {
                    OrganizeDefaultPlayerOrder();
                    EndPhase();
                    break;
                }
            case Phase.USING_IMPS:
                {
                    if (playerOrder.Count != 0)
                    {
                        currentPlayer = playerOrder[0];
                        CurrentPlayerChanged(currentPlayer);
                        playerOrder.Remove(currentPlayer);
                        OnUsingImps();
                    }
                    break;
                }
            case Phase.EVALUATE_USING_IMPS:
                {
                    OnEvaluateUsingImps();
                    break;
                }
            case Phase.CHECK_USING_IMPS:
                {
                    OnCheckUsingImps();
                    break;
                }
            case Phase.AGING:
                OnAging();
                break;
            case Phase.STARTING_NEXT_ROUND:
                OnStartingNextRound();
                break;
            case Phase.START_AGAIN:
                OnStartAgain();
                break;
            case Phase.GAME_END:
                OnGameEnd();
                break;
        }
    }

    private void OrganizeDefaultPlayerOrder()
    {
        int index = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].isStartingPlayer)
            {
                index = i;
                break;
            }
        }
        for (int i = 0; i < players.Length; i++)
        {
            if (index + i == players.Length)
            {
                index = -i;
            }
            playerOrder.Add(players[index + i]);
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
        currentPhase = Phase.SETUP_VIEW_ADDING_NEW_STUFF;
        PhaseChanged(Phase.SETUP_VIEW_ADDING_NEW_STUFF);
    }

    private void OnGroupingPlayer(Player player)
    {
        //informing view about popup
        currentPhase = Phase.VIEW_GROUPING_PLAYER;
        PhaseChanged(Phase.VIEW_GROUPING_PLAYER);
    }

    private void OnCheckGroupingPlayers()
    {
        if (playerOrder.Count != 0)
        {
            currentPhase = Phase.GROUPING_PLAYER;
            PhaseChanged(Phase.GROUPING_PLAYER);
        }
        else
        {
            EndPhase();
        }
    }    

    private void OnShoppingAction(Player player)
    {
        currentPhase = Phase.VIEW_SHOPPING_ACTION;
        PhaseChanged(Phase.VIEW_SHOPPING_ACTION);
    }

    private void OnEvaluateShoppingAction()
    {
        currentPhase = Phase.VIEW_EVALUATE_SHOPPING_ACTION;
        PhaseChanged(Phase.VIEW_EVALUATE_SHOPPING_ACTION);

    }
    private void OnCheckShoppingActions()
    {
        EndPhase();
    }

    private void OnArrangingPets(Player player)
    {
        //informing view about popup
        currentPhase = Phase.VIEW_ARRANGING_PETS;
        PhaseChanged(Phase.VIEW_ARRANGING_PETS);
    }

    private void OnEvaluateArrangingPets()
    {
        currentPhase = Phase.VIEW_EVALUATE_ARRANGING_PETS;
        PhaseChanged(Phase.VIEW_EVALUATE_ARRANGING_PETS);
    }

    private void OnCheckArrangingPets()
    {
        if (playerOrder.Count != 0)
        {
            currentPhase = Phase.ARRANGING_PETS;
            PhaseChanged(Phase.ARRANGING_PETS);
        }
        else
        {
            EndPhase();
        }
    }

    private void OnNeedCardsDealt()
    {
        currentPhase = Phase.VIEW_NEED_CARDS_DEALT;
        PhaseChanged(Phase.VIEW_NEED_CARDS_DEALT);
    }

    private void OnEvaluateNeedCards()
    {
        currentPhase = Phase.VIEW_EVALUATE_NEED_CARDS;
        PhaseChanged(Phase.VIEW_EVALUATE_NEED_CARDS);
    }

    private void OnCheckNeedCards()
    {
        if (playerOrder.Count != 0)
        {
            currentPhase = Phase.NEED_CARDS_DEALT;
            PhaseChanged(Phase.NEED_CARDS_DEALT);
        }
        else
        {
            EndPhase();
        }
    }

    private void OnExhibition()
    {
        currentPhase = Phase.VIEW_EXHIBITION;
        PhaseChanged(Phase.VIEW_EXHIBITION);
    }

    private void OnEvaluateExhibition()
    {
        currentPhase = Phase.VIEW_EVALUATE_EXHIBITION;
        PhaseChanged(Phase.VIEW_EVALUATE_EXHIBITION);
    }

    private void OnCheckExhibition()
    {
        if (playerOrder.Count != 0)
        {
            currentPhase = Phase.EXHIBITION;
            PhaseChanged(Phase.EXHIBITION);
        }
        else
        {
            EndPhase();
        }
    }

    private void OnBusiness()
    {
        currentPhase = Phase.VIEW_BUSINESS;
        PhaseChanged(Phase.VIEW_BUSINESS);
    }

    private void OnEvaluateBusiness()
    {
        currentPhase = Phase.VIEW_EVALUATE_BUSINESS;
        PhaseChanged(Phase.VIEW_EVALUATE_BUSINESS);
    }

    private void OnCheckBusiness()
    {
        if (playerOrder.Count != 0)
        {
            currentPhase = Phase.BUSINESS;
            PhaseChanged(Phase.BUSINESS);
        }
        else
        {
            EndPhase();
        }
    }

    private void OnUsingImps()
    {
        currentPhase = Phase.VIEW_USING_IMPS;
        PhaseChanged(Phase.VIEW_USING_IMPS);
    }

    private void OnEvaluateUsingImps()
    {
        currentPhase = Phase.VIEW_EVALUATE_USING_IMPS;
        PhaseChanged(Phase.VIEW_EVALUATE_USING_IMPS);
    }

    private void OnCheckUsingImps()
    {
        if (playerOrder.Count != 0)
        {
            currentPhase = Phase.USING_IMPS;
            PhaseChanged(Phase.USING_IMPS);
        }
        else
        {
            EndPhase();
        }
    }

    private void OnAging()
    {

        //animation of aging
        currentPhase = Phase.VIEW_AGING;
        PhaseChanged(Phase.VIEW_AGING);
    }

    private void OnStartingNextRound()
    {
        round++;
        if (round > 5)
        {
            currentPhase = Phase.GAME_END;
            PhaseChanged(Phase.GAME_END);
            return;
        }
        RoundChanged();

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].isStartingPlayer)
            {
                players[i].isStartingPlayer = false;
                if (i == players.Length-1)
                {
                    players[0].isStartingPlayer = true;
                    CurrentPlayerChanged(players[0]);
                    break;
                } else
                {
                    players[i + 1].isStartingPlayer = true;
                    CurrentPlayerChanged(players[i+1]);
                    break;
                }            
            }
        }
        StartingPlayerChanged();        

        //animation of next round
        currentPhase = Phase.VIEW_STARTING_NEXT_ROUND;
        PhaseChanged(Phase.VIEW_STARTING_NEXT_ROUND);
    }

    private void OnStartAgain()
    {
        currentPhase = Phase.SETUP;
        PhaseChanged(Phase.SETUP);
    }

    private void OnGameEnd()
    {
        currentPhase = Phase.VIEW_GAME_END;
        PhaseChanged(Phase.VIEW_GAME_END);

    }
}