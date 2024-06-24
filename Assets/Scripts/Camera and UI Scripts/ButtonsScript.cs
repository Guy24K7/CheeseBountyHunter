using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick(string action)
    {
        if (action.Equals("Start"))//start from the start screen
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (action.Equals("ReStart"))//start from the Game over after death or from the lab
        {
            SceneManager.LoadScene("Level 1");
        }
        if (action.Equals("Quit"))//quit the game
        {
            Application.Quit();
        }
    }
}
