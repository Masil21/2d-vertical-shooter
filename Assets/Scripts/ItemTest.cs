using UnityEngine;
using UnityEngine.UI;

public class ItemTest : MonoBehaviour
{
    public Button spawnButton;
    public GameObject boomPrefab;
    public GameObject coinPrefab;
    public GameObject powerPrefab;
    public Transform spawnCenter;
    public float gap = 1f;

    void Start()
    {
        spawnButton.onClick.AddListener(() =>
        {
            SpawnItems();
        });
    }

    void SpawnItems()
    {
        GameObject boom = Instantiate(boomPrefab);
        boom.transform.position = spawnCenter.position + new Vector3(-gap, 0, 0);

        GameObject coin = Instantiate(coinPrefab);
        coin.transform.position = spawnCenter.position;

        GameObject power = Instantiate(powerPrefab);
        power.transform.position = spawnCenter.position + new Vector3(gap, 0, 0);
    }
}