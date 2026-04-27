using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 1f;
    public Vector3 moveDirection = Vector3.down;
    public int hp = 16;
    public int score = 10;

    public Action<Vector3> onDie;

    private SpriteRenderer sr;
    private Sprite originalSprite;
    public Sprite hitSprite;

    private int maxHp;
    private float delta = 0;
    private bool isHit = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalSprite = sr.sprite;
        maxHp = hp;
    }

    void OnEnable()
    {
        hp = maxHp;
        isHit = false;
        delta = 0;
        moveDirection = Vector3.down;
        onDie = null;
        if (sr != null) sr.sprite = originalSprite;
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);

        if (isHit)
        {
            delta += Time.deltaTime;

            if (delta > 0.1f)
            {
                sr.sprite = originalSprite;
                isHit = false;
            }
        }

        if (transform.position.y < -4.5f)
        {
            onDie = null;
            ReturnToPool();
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"{gameObject.name} 데미지피해 : {damage}, 남은 HP: {hp}");

        sr.sprite = hitSprite;
        isHit = true;
        delta = 0;

        if (hp <= 0)
        {
            GameOver gameOver = FindAnyObjectByType<GameOver>();
            if (gameOver != null)
                gameOver.AddScore(score);

            onDie?.Invoke(transform.position);
            onDie = null;
            ReturnToPool();
        }
    }

    void ReturnToPool()
    {
        if (ObjectPoolManager.Instance != null)
            ObjectPoolManager.Instance.ReturnToPool(gameObject);
        else
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        BullitController bullet = other.GetComponent<BullitController>();

        if (bullet != null)
        {
            TakeDamage(bullet.damage);
            bullet.ReturnOrDeactivate();
        }
    }
}
