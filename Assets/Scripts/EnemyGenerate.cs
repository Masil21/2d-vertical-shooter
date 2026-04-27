using UnityEngine;

public class EnemyGenerate : MonoBehaviour
{
    private float delta = 0;
    private float spawnTime = 0;

    void Start()
    {
        spawnTime = Random.Range(1f, 3f);
    }

    void Update()
    {
        delta += Time.deltaTime;

        if (delta > spawnTime)
        {
            SpawnEnemy();
            delta = 0;
            spawnTime = Random.Range(1f, 3f);
        }
    }

    void SpawnEnemy()
    {
        GameObject enemyGo = ObjectPoolManager.Instance.GetRandomEnemy();
        if (enemyGo == null) return;

        enemyGo.transform.position = new Vector3(Random.Range(-2.4f, 2.4f), 4.5f, 0);
        enemyGo.SetActive(true);
        // moveDirection은 EnemyController.OnEnable에서 Vector3.down으로 리셋됨
    }
}
