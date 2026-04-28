using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigid2D;
    private Animator animator;
    private float speed = 5f;

    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        int dirX = (int)Input.GetAxisRaw("Horizontal");
        int dirY = (int)Input.GetAxisRaw("Vertical");

        animator.SetInteger("dirX", dirX);

        Vector3 dir = new Vector3(dirX, dirY, 0);
        transform.Translate(dir.normalized * speed * Time.deltaTime);

        float clampX = Mathf.Clamp(transform.position.x, -2.22f, 2.22f);
        float clampY = Mathf.Clamp(transform.position.y, -4.41f, 4.41f);
        transform.position = new Vector3(clampX, clampY, 0);
    }
}