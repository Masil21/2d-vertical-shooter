using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject player;
    public GameObject spawnPointGroup;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverScoreText;
    private int totalScore = 0;
    public Button retryButton;
    

    public Image life_0;
    public Image life_1;
    public Image life_2;

    private int lifeCount = 3;

    void Start()
    {
        gameOverPanel.SetActive(false);
        lifeCount = 3;
        totalScore = 0;
        scoreText.text = "0";

        retryButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }
    public void AddScore(int score)
    {
        totalScore += score;
        scoreText.text = $"{totalScore}";
    }

    public void PlayerDead()
    {
        lifeCount--;

        if (lifeCount == 2)
        {
            life_2.gameObject.SetActive(false);
        }
        else if (lifeCount == 1)
        {
            life_1.gameObject.SetActive(false);
        }
        else if (lifeCount == 0)
        {
            life_0.gameObject.SetActive(false);
            GameOverSequence();
            return;
        }

        Invoke("Respawn", 1f);
    }

    void GameOverSequence()
    {
        gameOverPanel.SetActive(true);
        player.SetActive(false);
        spawnPointGroup.SetActive(false);
        gameOverScoreText.text = $"Score : {totalScore}";

        GameObject[] enemyClones = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemyClones.Length; i++)
        {
            Destroy(enemyClones[i]);
        }

        GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for (int i = 0; i < enemyBullets.Length; i++)
        {
            Destroy(enemyBullets[i]);
        }

        GameObject[] playerBullets = GameObject.FindGameObjectsWithTag("PlayerBullet");
        for (int i = 0; i < playerBullets.Length; i++)
        {
            Destroy(playerBullets[i]);
        }
    }

    void Respawn()
    {
        Player playerScript = player.GetComponent<Player>();
        playerScript.hp = 100;
        player.transform.position = new Vector3(0, -3f, 0);
        player.SetActive(true);
    }

    public void OnRetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}