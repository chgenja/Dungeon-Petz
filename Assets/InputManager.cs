﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public GameManager gameManager;
    public View view;
    
    public void MovingForward()
    {
        gameManager.EndPhase();
    }

}
