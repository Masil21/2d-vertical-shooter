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
    private bool isGameOver = false;

    public Image life_0;
    public Image life_1;
    public Image life_2;

    public Image boom_0;
    public Image boom_1;
    public Image boom_2;

    private int lifeCount = 3;

    void Start()
    {
        gameOverPanel.SetActive(false);
        lifeCount = 3;
        totalScore = 0;
        scoreText.text = "0";

        boom_0.gameObject.SetActive(false);
        boom_1.gameObject.SetActive(false);
        boom_2.gameObject.SetActive(false);

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

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void UpdateBoomUI(int boomCount)
    {
        boom_0.gameObject.SetActive(boomCount >= 1);
        boom_1.gameObject.SetActive(boomCount >= 2);
        boom_2.gameObject.SetActive(boomCount >= 3);
    }

    public void UseBoom()
    {
        Player playerScript = player.GetComponent<Player>();
        UpdateBoomUI(playerScript.boomCount);
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
            CancelInvoke("Respawn");
            CancelInvoke("EndPlayerInvincible");
            GameOverSequence();
            return;
        }

        Invoke("Respawn", 1f);
    }

    void GameOverSequence()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
        gameOverScoreText.text = $"Score : {totalScore}";
        player.SetActive(false);
        spawnPointGroup.SetActive(false);

        GameObject[] enemyClones = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemyClones.Length; i++)
        {
            ObjectPoolManager.Instance.ReturnToPool(enemyClones[i]);
        }

        GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for (int i = 0; i < enemyBullets.Length; i++)
        {
            ObjectPoolManager.Instance.ReturnToPool(enemyBullets[i]);
        }

        GameObject[] playerBullets = GameObject.FindGameObjectsWithTag("PlayerBullet");
        for (int i = 0; i < playerBullets.Length; i++)
        {
            ObjectPoolManager.Instance.ReturnToPool(playerBullets[i]);
        }
    }

    void Respawn()
    {
        player.transform.position = new Vector3(0, -3f, 0);
        player.SetActive(true);

        Player playerScript = player.GetComponent<Player>();
        playerScript.SetInvincible(true);

        Invoke("EndPlayerInvincible", 2f);
    }

    void EndPlayerInvincible()
    {
        Player playerScript = player.GetComponent<Player>();
        playerScript.SetInvincible(false);
    }
}