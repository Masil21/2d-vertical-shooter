using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    [Header("Player Bullets")]
    public GameObject power1Prefab;
    public int power1Count = 30;
    public GameObject power2Prefab;
    public int power2Count = 20;
    public GameObject power3Prefab;
    public int power3Count = 20;

    [Header("Enemy Bullets")]
    public GameObject enemyBulletPrefab;
    public int enemyBulletCount = 30;

    [Header("Enemies")]
    public GameObject[] enemyPrefabs;
    public int enemyCountEach = 15;

    [Header("Items")]
    public GameObject coinPrefab;
    public int coinCount = 20;
    public GameObject powerItemPrefab;
    public int powerItemCount = 10;
    public GameObject boomItemPrefab;
    public int boomItemCount = 10;

    private List<GameObject> power1Pool = new List<GameObject>();
    private List<GameObject> power2Pool = new List<GameObject>();
    private List<GameObject> power3Pool = new List<GameObject>();
    private List<GameObject> enemyBulletPool = new List<GameObject>();
    private List<List<GameObject>> enemyPools = new List<List<GameObject>>();
    private List<GameObject> coinPool = new List<GameObject>();
    private List<GameObject> powerItemPool = new List<GameObject>();
    private List<GameObject> boomItemPool = new List<GameObject>();

    // 멀티 불릿 컨테이너의 자식 원래 로컬 위치 저장 (재사용 시 위치 리셋용)
    private Dictionary<GameObject, Vector3[]> originalChildPositions = new Dictionary<GameObject, Vector3[]>();

    public int EnemyPrefabCount => enemyPrefabs != null ? enemyPrefabs.Length : 0;

    void Awake()
    {
        Instance = this;
        InitPool(power1Pool, power1Prefab, power1Count);
        InitPool(power2Pool, power2Prefab, power2Count);
        InitPool(power3Pool, power3Prefab, power3Count);
        InitPool(enemyBulletPool, enemyBulletPrefab, enemyBulletCount);

        if (enemyPrefabs != null)
        {
            foreach (var prefab in enemyPrefabs)
            {
                var pool = new List<GameObject>();
                InitPool(pool, prefab, enemyCountEach);
                enemyPools.Add(pool);
            }
        }

        InitPool(coinPool, coinPrefab, coinCount);
        InitPool(powerItemPool, powerItemPrefab, powerItemCount);
        InitPool(boomItemPool, boomItemPrefab, boomItemCount);
    }

    void InitPool(List<GameObject> pool, GameObject prefab, int count)
    {
        if (prefab == null) return;
        for (int i = 0; i < count; i++)
        {
            var go = Instantiate(prefab, transform);
            go.SetActive(false);
            pool.Add(go);

            // 멀티 불릿 컨테이너의 자식 초기 위치 기록
            if (go.transform.childCount > 0)
            {
                var positions = new Vector3[go.transform.childCount];
                for (int j = 0; j < go.transform.childCount; j++)
                    positions[j] = go.transform.GetChild(j).localPosition;
                originalChildPositions[go] = positions;
            }
        }
    }

    GameObject GetFromPool(List<GameObject> pool)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
                return pool[i];
        }
        return null;
    }

    public void ReturnToPool(GameObject go)
    {
        go.SetActive(false);
        go.transform.SetParent(transform);
    }

    public GameObject GetPlayerBullet(int powerLevel)
    {
        List<GameObject> pool = powerLevel == 2 ? power2Pool : (powerLevel >= 3 ? power3Pool : power1Pool);
        var go = GetFromPool(pool);
        if (go != null && originalChildPositions.TryGetValue(go, out var positions))
        {
            // 멀티 불릿 자식 위치 리셋
            for (int i = 0; i < go.transform.childCount && i < positions.Length; i++)
                go.transform.GetChild(i).localPosition = positions[i];
        }
        return go;
    }

    public GameObject GetEnemyBullet() => GetFromPool(enemyBulletPool);

    public GameObject GetEnemy(int prefabIndex)
    {
        if (prefabIndex < 0 || prefabIndex >= enemyPools.Count) return null;
        return GetFromPool(enemyPools[prefabIndex]);
    }

    public GameObject GetRandomEnemy()
    {
        if (enemyPools.Count == 0) return null;
        int idx = Random.Range(0, enemyPools.Count);
        return GetFromPool(enemyPools[idx]);
    }

    public GameObject GetItem(Item3.ItemType type)
    {
        switch (type)
        {
            case Item3.ItemType.Coin: return GetFromPool(coinPool);
            case Item3.ItemType.Power: return GetFromPool(powerItemPool);
            case Item3.ItemType.Boom: return GetFromPool(boomItemPool);
            default: return null;
        }
    }
}
