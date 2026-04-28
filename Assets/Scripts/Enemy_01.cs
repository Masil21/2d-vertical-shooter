using UnityEngine;

public class Enemy_01 : MonoBehaviour
{
    public static Enemy_01 Instance;

    public int hp = 10;
    public GameObject coinPrefab;
    public GameObject boomPrefab;
    public GameObject powerPrefab;

    void Awake()
    {
        Instance = this;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"적 HP : {hp}");

        if (hp <= 0)
        {
            DropItem();
            Destroy(gameObject);
        }
    }

    void DropItem()
    {
        int rand = Random.Range(0, 3);
        GameObject item = null;

        if (rand == 0)
        {
            item = Instantiate(coinPrefab);
        }
        else if (rand == 1)
        {
            item = Instantiate(boomPrefab);
        }
        else if (rand == 2)
        {
            item = Instantiate(powerPrefab);
        }

        item.transform.position = transform.position;
    }
}