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

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"Player HP : {hp}");

        if (hp <= 0)
        {
            Debug.Log("사망..");
            gameObject.SetActive(false);

            GameOver gameOver = FindAnyObjectByType<GameOver>();
            if (gameOver != null)
            {
                gameOver.PlayerDead();
            }
        }
    }
}