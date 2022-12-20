using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameController : Singleton<GameController>
{
    [SerializeField] private Player player;

    public Player Player { get { return player; } }

    private List<Enemy> enemies = new List<Enemy>();

    public void RegisterEnemy(Enemy _enemy) {
        enemies.Add(_enemy);
    }

    /// <summary>
    /// the last scene buildindex, setup in WorldGrid
    /// </summary>
    [HideInInspector] public int SceneBuildIndex;

    /// <summary>
    /// tracking if game is over or player has won
    /// </summary>
    [HideInInspector] public bool IsOver { get; set; }
    [HideInInspector] public bool PlayerWon { get; set; }

    [HideInInspector] public bool KeyisTaken { get; set; }
    [HideInInspector] public bool DoorisOpen { get; set; }

    /// <summary>
    /// Setup here the maximum number of levels/scene in the game
    /// </summary>
    public int MAXSCENENUM = 5;

    public void GameOver() {
        player.CanMove = false;
        foreach (Enemy enemy in enemies)
            enemy.CanMove = false;
        /// <summary>
        /// setting global variables when player wons
        /// </summary
        IsOver = true;
        InputManager.Instance.IsPaused = false;
        PlayerWon = false;
        StartCoroutine(resetScene());
            
    }
    //private IEnumerator resetScene() {
    //    yield return new WaitForSeconds(2f);
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //}


    public void GameWon()
    {
        player.CanMove = false;
        foreach (Enemy enemy in enemies)
            enemy.CanMove = false;
        /// <summary>
        /// setting global variables when player wons
        /// </summary
        IsOver = false;
        InputManager.Instance.IsPaused = false;
        PlayerWon = true;

        StartCoroutine(resetScene());
    }

    private IEnumerator resetScene()
    {
        yield return new WaitForSeconds(2f);
        /// <summary>
        /// Calling the menu scene (buildindex == 0) 
        /// </summary> 
        Debug.Log("index=" + SceneManager.GetActiveScene().buildIndex + ",SceneBuildIndex=" + SceneBuildIndex + " isOver=" + IsOver + " hasWon=" + PlayerWon);

        SaveHighScene(SceneBuildIndex, IsOver, PlayerWon);

        SceneManager.LoadScene(0); //new line

    }

    /// <summary>
    /// SetHighScene, SaveHighScene,ResetHighScene needed to go to next level
    /// </summary>
    public int SetHighScene()
    {
        int _lastscene;

        if (PlayerPrefs.HasKey("HighScene"))
        {
            _lastscene = PlayerPrefs.GetInt("HighScene");
        }
        else
        {
            _lastscene = 0;
        }
        return _lastscene;
    }

    public bool SetWonStatus()
    {
        string _haswon;
        bool returnvalue;

        if (PlayerPrefs.HasKey("HasWon"))
        {
            _haswon = PlayerPrefs.GetString("HasWon");
        }
        else
        {
            _haswon = "false";
        }
        if (String.IsNullOrEmpty(_haswon)) _haswon = "false";
        returnvalue = bool.Parse(_haswon);

        return returnvalue;
    }

    public bool SetIsOverStatus()
    {
        string _over;
        bool returnvalue;

        if (PlayerPrefs.HasKey("IsOver"))
        {
            _over = PlayerPrefs.GetString("IsOver");
        }
        else
        {
            _over = "false";
        }
        if (String.IsNullOrEmpty(_over)) _over = "false";
        returnvalue = bool.Parse(_over);

        return returnvalue;
    }

    public void SaveHighScene(int HighScene, bool _over, bool _haswon)
    {
        string over = _over.ToString();
        string haswon = _haswon.ToString();
        PlayerPrefs.SetInt("HighScene", HighScene);
        PlayerPrefs.Save();
        PlayerPrefs.SetString("HasWon", haswon);
        PlayerPrefs.Save();
        PlayerPrefs.SetString("IsOver", over);
        PlayerPrefs.Save();
    }

    public void ResetHighScene()
    {
        PlayerPrefs.DeleteKey("HighScene");
        PlayerPrefs.DeleteKey("Paused");
        PlayerPrefs.DeleteKey("IsOver");
    }

}
