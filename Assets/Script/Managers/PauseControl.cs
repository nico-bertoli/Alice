using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseControl : MonoBehaviour
{
    //GameObject PausePanel;
    public Canvas PauseMenu;

    int lastScene;

    bool pp_over, pp_haswon;

    void Awake()
    {
        if (PauseMenu) {
            PauseMenu.gameObject.SetActive(false);
        }

        if (GameController.Instance == null)
        {
            lastScene = 0;
            pp_haswon = false;
            pp_over = false;
        }
        else
        {
            lastScene = GameController.Instance.SetHighScene();
            pp_over = GameController.Instance.SetIsOverStatus();
            pp_haswon = GameController.Instance.SetWonStatus();
        }

    }

    public void Continue()
    {
        if (PauseMenu)
        {
            PauseMenu.gameObject.SetActive(false);
            Time.timeScale = 1;
            InputManager.Instance.IsPaused = false;
        }
    }

    public void Quit()	//function to return to the main menu
    {
        Time.timeScale = 1;
        GameController.Instance.ResetHighScene();
        SceneManager.LoadScene(0);
    }

    public void Restart() //repeat the level
    {
        Continue();
        if (lastScene > 0 && lastScene <= GameController.Instance.MAXSCENENUM)
        {
            SceneManager.LoadScene(lastScene);
        }

    }

    public void ShowPause()
    {
        if (GameController.Instance == null)
        {
            lastScene = 0;
            pp_haswon = false;
            pp_over = false;
        }
        else
        {
            lastScene = GameController.Instance.SetHighScene();
            pp_over = GameController.Instance.SetIsOverStatus();
            pp_haswon = GameController.Instance.SetWonStatus();
        }

        Debug.Log("in ShowPause():" + lastScene + " " + pp_over + " " + pp_haswon + " ispaused=" + InputManager.Instance.IsPaused + " pausemenu=" + PauseMenu);
        if (PauseMenu)
        { 
            PauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0;
            //InputManager.Instance.IsPaused = true;
        }
    }

}
