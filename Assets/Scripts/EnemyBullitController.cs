using UnityEngine;

public class EnemyBullitController : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 10;
    public Vector3 moveDirection = Vector3.down;

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);

        Vector3 pos = transform.position;

        if (pos.y < -5f || pos.y > 5f || pos.x < -3f || pos.x > 3f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            Destroy(gameObject);
        }
    }
}