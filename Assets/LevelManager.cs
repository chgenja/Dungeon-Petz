using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager: MonoBehaviour {

    void Start()
    {
        Invoke("LoadNextLevel", 3);
    }
   
    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
