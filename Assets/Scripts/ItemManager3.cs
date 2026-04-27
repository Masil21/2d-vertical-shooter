using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager3 : MonoBehaviour
{
    public static ItemManager3 Instance;

    void Awake()
    {
        Instance = this;
    }

    public void CreateItem(Vector3 pos)
    {
        int rand = Random.Range(0, 100);
        Item3.ItemType type;

        if (rand < 30) return;
        else if (rand < 60) type = Item3.ItemType.Coin;
        else if (rand < 80) type = Item3.ItemType.Power;
        else type = Item3.ItemType.Boom;

        GameObject go = ObjectPoolManager.Instance.GetItem(type);
        if (go == null) return;

        go.transform.position = pos;
        go.SetActive(true);
    }
}
