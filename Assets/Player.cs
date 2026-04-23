using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform firePoint;
    public GameObject PlayerBulletPrefab;

    public float moveSpeed = 5f;
    public float fireRate = 0.2f;
    public int hp = 100;

    private float _nextFireTime = 0f;
    private Animator _anim;

    void Start()
    {
        Application.targetFrameRate = 60;
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Shoot();
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 dir = new Vector2(h, v).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime);

        float clampX = Mathf.Clamp(transform.position.x, -2.27f, 2.27f);
        float clampY = Mathf.Clamp(transform.position.y, -4.39f, 4.39f);
        transform.position = new Vector3(clampX, clampY, 0);

        if (h < 0)
            _anim.SetInteger("State", 1);
        else if (h > 0)
            _anim.SetInteger("State", 2);
        else
            _anim.SetInteger("State", 0);
    }

    void Shoot()
    {
        if (Input.GetMouseButton(0) && Time.time >= _nextFireTime)
        {
            _nextFireTime = Time.time + fireRate;

            GameObject bullet = Instantiate(PlayerBulletPrefab);
            bullet.transform.position = firePoint.position;

            if (bullet.transform.childCount == 0)
            {
                bullet.AddComponent<BullitController>();
            }
            else
            {
                bullet.AddComponent<ParentCleaner>();
            }
        }
    }

    public void TakeDamage()
    {
        Debug.Log("사망..");
        gameObject.SetActive(false);

        GameOver gameOver = FindAnyObjectByType<GameOver>();
        if (gameOver != null)
        {
            gameOver.PlayerDead();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();

        if (enemy != null)
        {
            TakeDamage();
        }

        EnemyBullitController enemyBullet = other.GetComponent<EnemyBullitController>();

        if (enemyBullet != null)
        {
            TakeDamage();
        }
    }
}