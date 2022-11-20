using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController>
{
    [SerializeField] private Player player;

    public Player Player { get { return player; } }

    private List<Enemy> enemies = new List<Enemy>();

    public void RegisterEnemy(Enemy _enemy) {
        enemies.Add(_enemy);
    }

    public void GameOver() {
        player.CanMove = false;
        foreach (Enemy enemy in enemies)
            enemy.CanMove = false;

        StartCoroutine(resetScene());
            
    }
    private IEnumerator resetScene() {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
