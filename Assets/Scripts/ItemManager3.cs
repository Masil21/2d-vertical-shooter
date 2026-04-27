using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager3 : MonoBehaviour
{
    public static ItemManager3 Instance;

    public GameObject coinPrefab;
    public GameObject powerPrefab;
    public GameObject boomPrefab;

    void Awake()
    {
        Instance = this;
    }

    public void CreateItem(Vector3 pos)
    {
        int rand = Random.Range(0, 100);
        GameObject prefab = null;

        if (rand < 30)
        {
            return;
        }
        else if (rand < 60)
        {
            prefab = coinPrefab;
        }
        else if (rand < 80)
        {
            prefab = powerPrefab;
        }
        else
        {
            prefab = boomPrefab;
        }

        var go = Instantiate(prefab, pos, Quaternion.identity);
        var item3 = go.GetComponent<Item3>();
        StartCoroutine(item3.Move());
    }
}