using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public GameManager gameManager;

    void Start()
    {
        gameManager.PhaseChanged += OnPhaseChanged;

    }

    void OnPhaseChanged(Phase phase)
    {
        switch (phase)
        {
            case Phase.INPUT_GROUPING_PLAYER:
                {
                    //receiving input
                    break;
                }
        }

    }
}
