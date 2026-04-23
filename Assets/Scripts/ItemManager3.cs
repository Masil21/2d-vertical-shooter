using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager3 : MonoBehaviour
{
    public static ItemManager3 Instance;

    public GameObject[] items;

    void Awake()
    {
        Instance = this;
    }

    public void CreateItem(Vector3 pos)
    {
        var prefab = items[Random.Range(0, items.Length)];
        var go = Instantiate(prefab, pos, Quaternion.identity);
        var item3 = go.GetComponent<Item3>();
        StartCoroutine(item3.Move());
    }
}