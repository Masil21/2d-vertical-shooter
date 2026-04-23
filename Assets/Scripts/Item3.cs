using UnityEngine;
using System.Collections;

public class Item3 : MonoBehaviour
{
    public enum ItemType
    {
        None = -1, Coin, Boom, Power
    }

    public ItemType itemType = ItemType.None;
    public float speed = 1f;

    public IEnumerator Move()
    {
        while (true)
        {
            if (this == null) yield break;

            transform.Translate(Vector3.down * speed * Time.deltaTime);

            yield return null;

            if (this == null) yield break;

            if (transform.position.y <= -5.5f)
                break;
        }

        if (this != null)
            Destroy(gameObject);
    }
}