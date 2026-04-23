using UnityEngine;

public class EnemyGenerate : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
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
            int idx = Random.Range(0, enemyPrefabs.Length);
            GameObject enemyGo = Instantiate(enemyPrefabs[idx]);
            enemyGo.transform.position = new Vector3(Random.Range(-2.4f, 2.4f), 4.5f, 0);
            delta = 0;
            spawnTime = Random.Range(1f, 3f);
        }
    }
}