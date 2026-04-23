using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;

    void Start()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (i == 10 || i == 12) continue;

            float delay = Random.Range(4f, 8f);
            Invoke($"Spawn_{i}", delay);
        }
    }

    void Spawn_0() { SpawnEnemy(0, Vector3.down); }
    void Spawn_1() { SpawnEnemy(1, Vector3.down); }
    void Spawn_2() { SpawnEnemy(2, Vector3.down); }
    void Spawn_3() { SpawnEnemy(3, Vector3.down); }
    void Spawn_4() { SpawnEnemy(4, Vector3.down); }
    void Spawn_5() { SpawnEnemy(5, Vector3.left); }
    void Spawn_6() { SpawnEnemy(6, Vector3.left); }
    void Spawn_7() { SpawnEnemy(7, Vector3.right); }
    void Spawn_8() { SpawnEnemy(8, Vector3.right); }
    void Spawn_9() { SpawnEnemyToward(9, 10); }
    void Spawn_11() { SpawnEnemyToward(11, 12); }

    void SpawnEnemy(int pointIndex, Vector3 moveDir)
    {
        int idx = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyGo = Instantiate(enemyPrefabs[idx]);
        enemyGo.transform.position = spawnPoints[pointIndex].position;

        EnemyController ec = enemyGo.GetComponent<EnemyController>();
        ec.moveDirection = moveDir;

        ec.onDie = (pos) =>
        {
            ItemManager3.Instance.CreateItem(pos);
        };

        float delay = Random.Range(4f, 8f);
        Invoke($"Spawn_{pointIndex}", delay);
    }

    void SpawnEnemyToward(int fromIndex, int toIndex)
    {
        int idx = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyGo = Instantiate(enemyPrefabs[idx]);
        enemyGo.transform.position = spawnPoints[fromIndex].position;

        Vector3 direction = spawnPoints[toIndex].position - spawnPoints[fromIndex].position;

        EnemyController ec = enemyGo.GetComponent<EnemyController>();
        ec.moveDirection = direction.normalized;

        ec.onDie = (pos) =>
        {
            ItemManager3.Instance.CreateItem(pos);
        };

        float delay = Random.Range(4f, 8f);
        Invoke($"Spawn_{fromIndex}", delay);
    }

    void OnDrawGizmos()
    {
        if (spawnPoints == null) return;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i] == null) continue;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(spawnPoints[i].position, 0.2f);
        }

        if (spawnPoints.Length > 10 && spawnPoints[9] != null && spawnPoints[10] != null)
        {
            Vector3 dir9 = spawnPoints[10].position - spawnPoints[9].position;
            DrawArrow.ForGizmo(spawnPoints[9].position, dir9, Color.green);
        }

        if (spawnPoints.Length > 12 && spawnPoints[11] != null && spawnPoints[12] != null)
        {
            Vector3 dir11 = spawnPoints[12].position - spawnPoints[11].position;
            DrawArrow.ForGizmo(spawnPoints[11].position, dir11, Color.blue);
        }
    }
}