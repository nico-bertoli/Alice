using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    //public static MenuController Instance;
    
    public Canvas Menu;

    public Button PLAY, QUIT, RESTART, NEXTLEVEL, EXIT;

    public TMP_Text Text2Display;

    int lastScene, MaxScene;

    bool mc_over, mc_haswon;

    void Awake()
    {
        
        if (Time.timeScale == 0) Time.timeScale = 1;
        if (GameController.Instance == null)
        {
            lastScene = 0;
            mc_haswon = false;
            mc_over = false;
        }
        else
        {
            lastScene = GameController.Instance.SetHighScene();
            mc_over = GameController.Instance.SetIsOverStatus();
            mc_haswon = GameController.Instance.SetWonStatus();
        }

        Text2Display.text = "WELCOME!!";

    }

    // Start is called before the first frame update
    void Start()
    {
        MaxScene = GameController.Instance.MAXSCENENUM;
        if (lastScene == 0)
            GameController.Instance.ResetHighScene(); //clean up 
        GameController.Instance.IsOver = mc_over;
        GameController.Instance.PlayerWon = mc_haswon;

        //Debug.Log("in Start():" + lastScene + " " + mc_over + " " + mc_haswon);

        ActivateUI();
    }

    public void StartGame() //start over from scratch
    {
        if (lastScene == 0)
            GameController.Instance.ResetHighScene(); //clean up

        SceneManager.LoadScene(1);

    }

    public void Quit()	//function to return to the main menu
    {
        GameController.Instance.ResetHighScene();
        SceneManager.LoadScene(0);
    }

    public void Restart() //repeat the level
    {

        if ((mc_over || mc_haswon) && lastScene > 0 && lastScene <= MaxScene)
            SceneManager.LoadScene(lastScene);
    }

    public void NextLevel() //go to the next level
    {
        if (lastScene > 0 && lastScene < MaxScene) //last level here has buildindex == 2
            SceneManager.LoadScene(lastScene + 1);
    }


    public void Exit()	//function to quit the programm
    {
        Application.Quit();

        Debug.Log("Application.Quit()");
    }

    /// <summary>
    /// function to activate the main window
    /// </summary>
    public void ActivateUI() 
    {

        if (lastScene == 0)
        {
            PLAY.gameObject.SetActive(true);
            QUIT.gameObject.SetActive(false);
            RESTART.gameObject.SetActive(false);
            NEXTLEVEL.gameObject.SetActive(false);
            EXIT.gameObject.SetActive(true);
        }
        else
        {
            if (GameController.Instance.IsOver)
            {
                PLAY.gameObject.SetActive(false);
                QUIT.gameObject.SetActive(true);
                RESTART.gameObject.SetActive(true);
                NEXTLEVEL.gameObject.SetActive(false);
                EXIT.gameObject.SetActive(false);

                Text2Display.text = "GOOD JOB! TRY IT AGAIN!";
            }
            else if (GameController.Instance.PlayerWon)
            {
                PLAY.gameObject.SetActive(false);
                QUIT.gameObject.SetActive(true);
                RESTART.gameObject.SetActive(true);
                if (lastScene == MaxScene) //if in the final scene of the game, no next level will be available next
                {
                    RESTART.gameObject.SetActive(false); 
                    NEXTLEVEL.gameObject.SetActive(false);
                    Text2Display.text = "YOU WON!!!";
                }
                else
                { 
                    NEXTLEVEL.gameObject.SetActive(true);
                    Text2Display.text = "THAT WAS GREAT!!!";
                }
                EXIT.gameObject.SetActive(false);

                
            }
        }

        Menu.enabled = true;
    }

}
