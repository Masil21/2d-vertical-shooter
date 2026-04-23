# 스페이스 슈터 2D 비행기 게임 - 레퍼런스 코드

## 프로젝트 개요
- Unity 2D 세로형 슈팅 게임 (1920x1080 Portrait)
- 플레이어 비행기가 좌우상하 이동하며 3연발 총알 발사
- 적이 위에서 랜덤으로 내려옴
- 총알이 적에 맞으면 개별 파괴, 적 HP 깎임
- 적이 플레이어에 부딪히면 플레이어 HP 깎임

## 강사님 코드 스타일 규칙
- GetAxisRaw + (int) 캐스팅으로 입력 처리
- dir.normalized로 대각선 이동 속도 보정
- Mathf.Clamp로 화면 밖 이동 방지
- delta += Time.deltaTime 타이머 패턴
- GameObject.Find()로 오브젝트 참조
- List<GameObject>로 오브젝트 관리
- Instantiate / Destroy로 생명주기 관리
- AddComponent로 런타임 컴포넌트 부착
- Physics2D.IgnoreCollision으로 아군 충돌 방지
- animator.SetInteger("State", value)로 애니메이션 전환

---

## 1. PlayerController.cs
**부착 위치:** Player 오브젝트에 직접 부착
**필요 컴포넌트:** SpriteRenderer, Animator, Collider2D (Is Trigger)

```csharp
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 2f;
    private Animator animator;
    public int hp = 100;

    void Start()
    {
        Application.targetFrameRate = 60;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        int dirX = (int)Input.GetAxisRaw("Horizontal");
        int dirY = (int)Input.GetAxisRaw("Vertical");

        animator.SetInteger("State", dirX);

        Vector3 dir = new Vector3(dirX, dirY, 0);
        transform.Translate(dir.normalized * speed * Time.deltaTime);

        float clampX = Mathf.Clamp(transform.position.x, -2.27f, 2.27f);
        float clampY = Mathf.Clamp(transform.position.y, -4.39f, 4.39f);
        transform.position = new Vector3(clampX, clampY, 0);
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"Player HP : {hp}");

        if (hp <= 0)
        {
            Debug.Log("사망..");
            Destroy(gameObject);
        }
    }
}
```

**Animator 세팅:**
- Parameters: State (Int 타입)
- MyIdle → MyLeft: State Equals -1
- MyIdle → MyRight: State Equals 1
- MyLeft → MyIdle: State Equals 0
- MyRight → MyIdle: State Equals 0
- MyLeft → MyRight: State Equals 1
- MyRight → MyLeft: State Equals -1
- 모든 전환: Has Exit Time 해제, Transition Duration 0

---

## 2. BulletGenerator.cs
**부착 위치:** BulletGenerator 빈 게임오브젝트에 부착
**Inspector:** midBulletSprite, smallBulletSprite에 스프라이트 할당
**방식:** 프리팹이 아닌 스프라이트 방식으로 총알 생성

```csharp
using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    public Sprite midBulletSprite;
    public Sprite smallBulletSprite;

    private float delta = 0;
    public float fireRate = 0.2f;

    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
            return;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            delta += Time.deltaTime;

            if (delta > fireRate)
            {
                CreateBulletSet();
                delta = 0;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            delta = 0;
        }
    }

    private void CreateBulletSet()
    {
        GameObject bulletParent = new GameObject("BulletParent");
        bulletParent.transform.position = player.transform.position + new Vector3(0, 0.5f, 0);

        GameObject midBullet = new GameObject("midBullet");
        midBullet.transform.parent = bulletParent.transform;
        midBullet.transform.localPosition = Vector3.zero;
        SpriteRenderer midSr = midBullet.AddComponent<SpriteRenderer>();
        midSr.sprite = midBulletSprite;
        CapsuleCollider2D midCol = midBullet.AddComponent<CapsuleCollider2D>();
        midCol.isTrigger = true;

        GameObject leftBullet = new GameObject("leftBullet");
        leftBullet.transform.parent = bulletParent.transform;
        SpriteRenderer leftSr = leftBullet.AddComponent<SpriteRenderer>();
        leftSr.sprite = smallBulletSprite;
        CapsuleCollider2D leftCol = leftBullet.AddComponent<CapsuleCollider2D>();
        leftCol.isTrigger = true;

        GameObject rightBullet = new GameObject("rightBullet");
        rightBullet.transform.parent = bulletParent.transform;
        SpriteRenderer rightSr = rightBullet.AddComponent<SpriteRenderer>();
        rightSr.sprite = smallBulletSprite;
        CapsuleCollider2D rightCol = rightBullet.AddComponent<CapsuleCollider2D>();
        rightCol.isTrigger = true;

        Physics2D.IgnoreCollision(midCol, leftCol);
        Physics2D.IgnoreCollision(midCol, rightCol);
        Physics2D.IgnoreCollision(leftCol, rightCol);

        float midHalfWidth = midSr.sprite.bounds.size.x * 0.5f;
        float leftHalfWidth = leftSr.sprite.bounds.size.x * 0.5f;
        float rightHalfWidth = rightSr.sprite.bounds.size.x * 0.5f;

        leftBullet.transform.localPosition = new Vector3(
            -midHalfWidth - leftHalfWidth,
            0f,
            0f
        );

        rightBullet.transform.localPosition = new Vector3(
            midHalfWidth + rightHalfWidth,
            0f,
            0f
        );

        Rigidbody2D midRb = midBullet.AddComponent<Rigidbody2D>();
        midRb.gravityScale = 0;
        midBullet.AddComponent<BulletController>();

        Rigidbody2D leftRb = leftBullet.AddComponent<Rigidbody2D>();
        leftRb.gravityScale = 0;
        leftBullet.AddComponent<BulletController>();

        Rigidbody2D rightRb = rightBullet.AddComponent<Rigidbody2D>();
        rightRb.gravityScale = 0;
        rightBullet.AddComponent<BulletController>();
    }
}
```

---

## 3. BulletController.cs
**부착 위치:** 직접 붙이지 않음 (BulletGenerator에서 AddComponent로 자동 부착)
**역할:** 각 총알 개별 이동 + 화면 밖 파괴

```csharp
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (transform.position.y > 4.5f)
        {
            Destroy(gameObject);
        }
    }
}
```

---

## 4. EnemyGenerator.cs
**부착 위치:** EnemyGenerator 빈 게임오브젝트에 부착
**Inspector:** enemyPrefab에 Enemy 프리팹 할당

```csharp
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject enemyPrefab;
    private float delta = 0;
    public List<GameObject> enemies = new List<GameObject>();

    void Update()
    {
        delta += Time.deltaTime;

        if (delta > 1.5f)
        {
            GameObject enemyGo = Instantiate(enemyPrefab);
            enemyGo.name = $"enemy_{enemies.Count}";
            enemyGo.transform.position = new Vector3(Random.Range(-2.4f, 2.4f), 4.5f, 0);
            enemies.Add(enemyGo);
            delta = 0;
        }
    }
}
```

---

## 5. EnemyController.cs
**부착 위치:** Enemy 프리팹에 부착
**필요 컴포넌트:** SpriteRenderer, Circle Collider 2D (Is Trigger), Rigidbody2D (Gravity Scale 0, Body Type Kinematic)
**Animator:** 미구현 (나중에 추가 예정, null 체크로 안전 처리됨)

```csharp
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 0.2f;
    public int hp = 8;
    public int attackDamage = 8;

    private Animator animator;
    private float delta = 0;
    private bool isHit = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (isHit && animator != null)
        {
            delta += Time.deltaTime;

            if (delta > 0.1f)
            {
                animator.SetInteger("State", 0);
                isHit = false;
            }
        }

        if (transform.position.y < -4.5f)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"{gameObject.name} 데미지피해 : {damage}, 남은 HP: {hp}");

        if (animator != null)
        {
            animator.SetInteger("State", 1);
            isHit = true;
            delta = 0;
        }

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        BulletController bullet = other.GetComponent<BulletController>();

        if (bullet != null)
        {
            int damage = 0;

            if (other.gameObject.name == "midBullet")
            {
                damage = 2;
            }
            else
            {
                damage = 1;
            }

            TakeDamage(damage);
            Destroy(other.gameObject);
            return;
        }

        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            player.TakeDamage(attackDamage);
            Destroy(gameObject);
        }
    }
}
```

---

## 프로젝트 구조 정리

### Hierarchy 구성
```
GameScene
 ├─ Main Camera
 ├─ Player (PlayerController.cs, Animator, Collider2D)
 ├─ BulletGenerator (BulletGenerator.cs)
 ├─ EnemyGenerator (EnemyGenerator.cs)
 └─ BackgroundScroller (배경 스크롤용, 별도)
```

### 수치 정리
| 항목 | 값 |
|------|-----|
| Player 이동 범위 x | -2.27 ~ 2.27 |
| Player 이동 범위 y | -4.39 ~ 4.39 |
| Player HP | 100 |
| Player speed | 2f |
| 총알 speed | 2f |
| 총알 발사 간격 | 0.2초 |
| 총알 발사 위치 오프셋 | y + 0.5f |
| 큰 총알(mid) 데미지 | 2 |
| 작은 총알(small) 데미지 | 1 |
| 적 생성 간격 | 1.5초 |
| 적 생성 x 범위 | -2.4 ~ 2.4 |
| 적 생성 y | 4.5 |
| 적 HP | 8 |
| 적 speed | 0.2f |
| 적 attackDamage | 8 |
| 총알 파괴 y | > 4.5 |
| 적 파괴 y | < -4.5 |

### 핵심 시행착오 메모
1. Player를 Hierarchy에 두고 Instantiate하면 2개 생성됨 → 직접 배치 방식 사용
2. GameObject.Find("Player(Clone)") vs "Player" 이름 주의
3. 총알 3개가 한번에 깨지는 문제 → BulletController를 부모가 아닌 각 자식에 개별 부착
4. GetComponentInParent → GetComponent로 변경 (개별 총알 감지)
5. Destroy(bullet.gameObject) → Destroy(other.gameObject) (맞은 총알만 파괴)
6. Enemy Body Type을 Kinematic으로 설정해야 Player와 충돌 시 튕기지 않음
7. Animator 없을 때 null 체크 필수

### 미구현 (추후 추가 예정)
- 적 hit 애니메이션 (State 파라미터, 0.1초 후 Idle 복귀)
- GameOver / Clear 씬 전환
- 배경 무한 스크롤 (BackgroundScroller - 별도 구현 완료)
