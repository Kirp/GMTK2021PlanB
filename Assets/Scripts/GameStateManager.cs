using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public GameObject MainConsole;
    public Text GameWinText;


    public void GameWin()
    {
        //Debug.Log("Show game win state");
        GameWinText.enabled = true;
        MainConsole.GetComponent<ConsoleControl>().canPlay = false;
    }

    public void ResetStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetToTitle()
    {
        SceneManager.LoadScene(0);

    }

}
