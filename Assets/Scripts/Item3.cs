using UnityEngine;

public class Item3 : MonoBehaviour
{
    public enum ItemType
    {
        None = -1, Coin, Boom, Power
    }

    public ItemType itemType = ItemType.None;
    public float speed = 1f;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y <= -5.5f)
        {
            if (ObjectPoolManager.Instance != null)
                ObjectPoolManager.Instance.ReturnToPool(gameObject);
            else
                Destroy(gameObject);
        }
    }
}
