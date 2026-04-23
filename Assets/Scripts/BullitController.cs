using UnityEngine;

public class BullitController : MonoBehaviour
{
    public float speed = 3f;
    public int damage = 5;

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (transform.position.y > 4.5f)
        {
            if (transform.parent != null)
            {
                if (transform.parent.childCount <= 1)
                {
                    Destroy(transform.parent.gameObject);
                }
            }

            Destroy(gameObject);
        }
    }
}