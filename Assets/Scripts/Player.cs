using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform firePoint;
    public GameObject Power1Prefab;
    public GameObject Power2Prefab;
    public GameObject Power3Prefab;
    public GameObject boomAnimationPrefab;

    public float moveSpeed = 5f;
    public float fireRate = 0.2f;

    private float _nextFireTime = 0f;
    private Animator _anim;
    private bool isInvincible = false;
    private int powerLevel = 1;
    public int boomCount = 0;

    void Start()
    {
        Application.targetFrameRate = 60;
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        GameOver gameOver = FindAnyObjectByType<GameOver>();
        if (gameOver != null && gameOver.IsGameOver())
        {
            return;
        }

        Move();
        Shoot();
        UseBoom();
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

            GameObject bulletPrefab = Power1Prefab;

            if (powerLevel == 2)
            {
                bulletPrefab = Power2Prefab;
            }
            else if (powerLevel >= 3)
            {
                bulletPrefab = Power3Prefab;
            }

            GameObject bullet = Instantiate(bulletPrefab);
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

    void UseBoom()
    {
        if (Input.GetMouseButtonDown(1) && boomCount > 0)
        {
            boomCount--;

            GameOver gameOver = FindAnyObjectByType<GameOver>();
            if (gameOver != null)
            {
                gameOver.UseBoom();
            }

            GameObject boomEffect = Instantiate(boomAnimationPrefab);
            boomEffect.transform.position = Vector3.zero;
            Destroy(boomEffect, 2f);

            StartCoroutine(BoomDuration());

            Debug.Log($"Boom 사용! 남은 Boom: {boomCount}");
        }
    }

    System.Collections.IEnumerator BoomDuration()
    {
        float elapsed = 0f;

        while (elapsed < 2f)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < enemies.Length; i++)
            {
                EnemyController ec = enemies[i].GetComponent<EnemyController>();
                if (ec != null)
                {
                    ec.TakeDamage(9999);
                }
            }

            GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
            for (int i = 0; i < enemyBullets.Length; i++)
            {
                Destroy(enemyBullets[i]);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void AddBoom()
    {
        if (boomCount < 3)
        {
            boomCount++;
            Debug.Log($"Boom 획득! 현재 Boom: {boomCount}");

            GameOver gameOver = FindAnyObjectByType<GameOver>();
            if (gameOver != null)
            {
                gameOver.UpdateBoomUI(boomCount);
            }
        }
    }

    public void TakeDamage()
    {
        if (isInvincible) return;

        Debug.Log("사망..");
        powerLevel = 1;
        gameObject.SetActive(false);

        GameOver gameOver = FindAnyObjectByType<GameOver>();
        if (gameOver != null)
        {
            gameOver.PlayerDead();
        }
    }

    public void SetInvincible(bool value)
    {
        isInvincible = value;
    }

    public void PowerUp()
    {
        if (powerLevel < 3)
        {
            powerLevel++;
            Debug.Log($"파워 레벨 : {powerLevel}");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var item3 = other.gameObject.GetComponent<Item3>();

        if (item3 != null)
        {
            GameOver gameOver = FindAnyObjectByType<GameOver>();

            if (item3.itemType == Item3.ItemType.Coin)
            {
                Debug.Log("Coin 획득!");
                if (gameOver != null) gameOver.AddScore(100);
            }
            else if (item3.itemType == Item3.ItemType.Power)
            {
                Debug.Log("Power 획득!");
                if (gameOver != null) gameOver.AddScore(500);
                PowerUp();
            }
            else if (item3.itemType == Item3.ItemType.Boom)
            {
                Debug.Log("Boom 획득!");
                if (gameOver != null) gameOver.AddScore(200);
                AddBoom();
            }

            Destroy(other.gameObject);
            return;
        }

        if (isInvincible) return;

        EnemyController enemy = other.GetComponent<EnemyController>();

        if (enemy != null)
        {
            TakeDamage();
            return;
        }

        EnemyBullitController enemyBullet = other.GetComponent<EnemyBullitController>();

        if (enemyBullet != null)
        {
            TakeDamage();
            return;
        }
    }
}