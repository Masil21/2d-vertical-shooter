# SpaceShooters Code Export

## Assets/App.cs

```csharp
using UnityEngine;

public class App
{
    public App()
    {
        Debug.Log("App 클래스 생성자 호출됨");
    }
}
```

## Assets/BullitController.cs

```csharp
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
```

## Assets/Button.cs

```csharp
using UnityEngine;
using UnityEngine.UI;

public class Grammar : MonoBehaviour
{
    public Button testButton;
    
    void Start()
    {
        testButton.onClick.AddListener(() =>
        {
            Debug.Log("Hello World");
        });
    }
}
```

## Assets/DrawArrow.cs

```csharp
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
// adapted from http://wiki.unity3d.com/index.php/DrawArrow 


public enum ArrowType
{
  Default,
	Thin,
	Double,
	Triple,
	Solid,
	Fat,
	ThreeD,
}

public static class DrawArrow
{
	public static void ForGizmo(Vector3 pos, Vector3 direction, Color? color = null,  bool doubled = false, float arrowHeadLength = 0.2f, float arrowHeadAngle = 20.0f)
	{
		Gizmos.color = color ?? Color.white;

		//arrow shaft
		Gizmos.DrawRay(pos, direction);
 
		if (direction != Vector3.zero)
		{
			//arrow head
			Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180+arrowHeadAngle,0) * new Vector3(0,0,1);
			Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180-arrowHeadAngle,0) * new Vector3(0,0,1);
			Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
			Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
		}
	}
 
	public static void ForDebug(Vector3 pos, Vector3 direction, float duration = 0.5f, Color? color = null, ArrowType type = ArrowType.Default, float arrowHeadLength = 0.2f, float arrowHeadAngle = 30.0f, bool sceneCamFollows = false)
	{
		Color actualColor = color ?? Color.white;
		duration = duration/Time.timeScale;
		
		float width = 0.01f;

		Vector3 directlyRight = Vector3.zero;
		Vector3 directlyLeft = Vector3.zero;
		Vector3 directlyBack = Vector3.zero;
		Vector3 headRight = Vector3.zero;
		Vector3 headLeft = Vector3.zero;

		if (direction != Vector3.zero)
		{
			directlyRight = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180+90,0) * new Vector3(0,0,1);
			directlyLeft = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180-90,0) * new Vector3(0,0,1);
			directlyBack = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180,0) * new Vector3(0,0,1);
			headRight = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180+arrowHeadAngle,0) * new Vector3(0,0,1);
			headLeft = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180-arrowHeadAngle,0) * new Vector3(0,0,1);
		}		

		//draw arrow head
		Debug.DrawRay(pos + direction, headRight * arrowHeadLength, actualColor, duration);
		Debug.DrawRay(pos + direction, headLeft * arrowHeadLength, actualColor, duration);
		
		switch (type) {
		case ArrowType.Default:
			Debug.DrawRay(pos, direction, actualColor, duration); //draw center line
			break;
		case ArrowType.Double:
			Debug.DrawRay(pos + directlyRight * width, direction * (1-width), actualColor, duration); //draw line slightly to right
			Debug.DrawRay(pos +  directlyLeft * width, direction * (1-width), actualColor, duration); //draw line slightly to left

			//draw second arrow head
			Debug.DrawRay(pos + directlyBack * width + direction, headRight * arrowHeadLength, actualColor, duration);
			Debug.DrawRay(pos + directlyBack * width + direction, headLeft * arrowHeadLength, actualColor, duration);
			
			break;
		case ArrowType.Triple:
			Debug.DrawRay(pos, direction, actualColor, duration); //draw center line
			Debug.DrawRay(pos + directlyRight * width, direction * (1-width), actualColor, duration); //draw line slightly to right
			Debug.DrawRay(pos +  directlyLeft * width, direction * (1-width), actualColor, duration); //draw line slightly to left
			break;
		case ArrowType.Fat:
			break;
		case ArrowType.Solid:
			int increments = 20;
			for (int i=0;i<increments;i++)
			{
				float displacement = Mathf.Lerp(-width, +width, i/(float)increments);
				//draw arrow body
				Debug.DrawRay(pos + directlyRight * displacement, direction, actualColor, duration); //draw line slightly to right
				Debug.DrawRay(pos +  directlyLeft * displacement, direction, actualColor, duration); //draw line slightly to left
				//draw arrow head
				Debug.DrawRay((pos + direction) + directlyRight * displacement, headRight * arrowHeadLength, actualColor, duration);
				Debug.DrawRay((pos + direction) + directlyRight * displacement, headLeft * arrowHeadLength, actualColor, duration);
			}
			break;
		case ArrowType.Thin:
			Debug.DrawRay(pos, direction, actualColor, duration); //draw center line
			break;
		case ArrowType.ThreeD:
			break;
		}

/*#if UNITY_EDITOR
    //snap the Scene view camera to a spot where it is looking directly at this arrow.
		if (sceneCamFollows)
			SceneViewCameraFollower.activateAt(pos + direction, duration, "_arrow");
#endif*/
	}

	public static void randomStar(Vector3 center, Color color)
	{
		//special: refuse to draw at 0,0.
		if (center == Vector3.zero) return;
			for(int i=0;i<2;i++)
				DrawArrow.ForGizmo(center, UnityEngine.Random.onUnitSphere * 1, color, false, 0.1f, 30.0f);
	}
	
	public static void comparePositions(Transform t1, Transform t2)
	{
		//direct from one to the other:
		ForDebug(t1.position, t2.position - t1.position);

		//direction
		//Vector3 moveDirection = (t2.position-t1.position).normalized;
	}
	
	public static void ForDebug2D(Vector3 pos, Vector3 direction, float duration = 0.5f, Color? color = null, float arrowHeadLength = 0.2f, float arrowHeadAngle = 30.0f)
	{
		Color actualColor = color ?? Color.white;

		Debug.DrawRay(pos, direction, actualColor, duration);

		if (direction != Vector3.zero)
		{
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

			Vector3 right = Quaternion.Euler(0, 0, angle + 180 + arrowHeadAngle) * Vector3.right;
			Vector3 left = Quaternion.Euler(0, 0, angle + 180 - arrowHeadAngle) * Vector3.right;

			Debug.DrawRay(pos + direction, right * arrowHeadLength, actualColor, duration);
			Debug.DrawRay(pos + direction, left * arrowHeadLength, actualColor, duration);
		}
	}
}
```

## Assets/Enemy.cs

```csharp
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int speed;
    
    private SpriteRenderer sr;
    
    public Sprite[] sprites;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("down");
            Hit();
            
        }
    }

    private void Hit()
    {
        sr.sprite = sprites[1];
        Invoke("ReturnDefaultSprite", 0.1f);
    }

    private void ReturnDefaultSprite()
    {
        sr.sprite = sprites[0];
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        
    }
    
    
}
```

## Assets/EnemyBullit.cs

```csharp
using UnityEngine;

public class EnemyBullit : MonoBehaviour
{
    public GameObject enemyBullitPrefab;
    public Transform firePoint;

    private float delta = 0;
    private int shotCount = 0;
    private bool isResting = false;
    private float restDelta = 0;
    private bool isAiming = true;
    private float aimDelta = 0;
    private Vector3 targetDirection;

    private GameObject player;

    void Start()
    {
        firePoint = transform.Find("firePoint");
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (player == null) return;

        if (isAiming)
        {
            aimDelta += Time.deltaTime;
            targetDirection = (player.transform.position - transform.position).normalized;

            if (aimDelta > 1f)
            {
                isAiming = false;
                aimDelta = 0;
                delta = 0;
                shotCount = 0;
            }

            return;
        }

        if (isResting)
        {
            restDelta += Time.deltaTime;

            if (restDelta > 0.5f)
            {
                isResting = false;
                restDelta = 0;
                isAiming = true;
                aimDelta = 0;
            }

            return;
        }

        delta += Time.deltaTime;

        if (delta > 0.25f)
        {
            Shoot();
            delta = 0;
            shotCount++;

            if (shotCount >= 2)
            {
                isResting = true;
            }
        }
    }

    void Shoot()
    {
        if (enemyBullitPrefab == null || firePoint == null) return;

        GameObject bulletGo = Instantiate(enemyBullitPrefab);
        bulletGo.transform.position = firePoint.position;

        EnemyBullitController ebc = bulletGo.GetComponent<EnemyBullitController>();

        if (ebc != null)
        {
            ebc.moveDirection = targetDirection;
        }
    }
}
```

## Assets/EnemyBullitController.cs

```csharp
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
            player.TakeDamage();
            Destroy(gameObject);
        }
    }
}
```

## Assets/EnemyController.cs

```csharp
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 1f;
    public Vector3 moveDirection = Vector3.down;
    public int hp = 16;
    public int score = 10;

    private SpriteRenderer sr;
    private Sprite originalSprite;
    public Sprite hitSprite;

    private float delta = 0;
    private bool isHit = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalSprite = sr.sprite;
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
            Destroy(gameObject);
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
            {
                gameOver.AddScore(score);
            }

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        BullitController bullet = other.GetComponent<BullitController>();

        if (bullet != null)
        {
            TakeDamage(bullet.damage);

            if (other.transform.parent != null)
            {
                if (other.transform.parent.childCount <= 1)
                {
                    Destroy(other.transform.parent.gameObject);
                }
            }

            Destroy(other.gameObject);
        }
    }
}
```

## Assets/EnemyGenerate.cs

```csharp
using UnityEngine;

public class EnemyGenerate : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    private float delta = 0;
    private float spawnTime = 0;

    void Start()
    {
        spawnTime = Random.Range(1f, 3f);
    }

    void Update()
    {
        delta += Time.deltaTime;

        if (delta > spawnTime)
        {
            int idx = Random.Range(0, enemyPrefabs.Length);
            GameObject enemyGo = Instantiate(enemyPrefabs[idx]);
            enemyGo.transform.position = new Vector3(Random.Range(-2.4f, 2.4f), 4.5f, 0);
            delta = 0;
            spawnTime = Random.Range(1f, 3f);
        }
    }
}
```

## Assets/GameOver.cs

```csharp
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject player;
    public GameObject spawnPointGroup;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverScoreText;
    private int totalScore = 0;
    public Button retryButton;
    private bool isGameOver = false;

    public Image life_0;
    public Image life_1;
    public Image life_2;

    private int lifeCount = 3;

    void Start()
    {
        gameOverPanel.SetActive(false);
        lifeCount = 3;
        totalScore = 0;
        scoreText.text = "0";

        retryButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }

    public void AddScore(int score)
    {
        totalScore += score;
        scoreText.text = $"{totalScore}";
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void PlayerDead()
    {
        lifeCount--;

        if (lifeCount == 2)
        {
            life_2.gameObject.SetActive(false);
        }
        else if (lifeCount == 1)
        {
            life_1.gameObject.SetActive(false);
        }
        else if (lifeCount == 0)
        {
            life_0.gameObject.SetActive(false);
            CancelInvoke("Respawn");
            CancelInvoke("EndPlayerInvincible");
            GameOverSequence();
            return;
        }

        Invoke("Respawn", 1f);
    }

    void GameOverSequence()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
        gameOverScoreText.text = $"Score : {totalScore}";
        player.SetActive(false);
        spawnPointGroup.SetActive(false);

        GameObject[] enemyClones = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemyClones.Length; i++)
        {
            Destroy(enemyClones[i]);
        }

        GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for (int i = 0; i < enemyBullets.Length; i++)
        {
            Destroy(enemyBullets[i]);
        }

        GameObject[] playerBullets = GameObject.FindGameObjectsWithTag("PlayerBullet");
        for (int i = 0; i < playerBullets.Length; i++)
        {
            Destroy(playerBullets[i]);
        }
    }

    void Respawn()
    {
        player.transform.position = new Vector3(0, -3f, 0);
        player.SetActive(true);

        Player playerScript = player.GetComponent<Player>();
        playerScript.SetInvincible(true);

        Invoke("EndPlayerInvincible", 2f);
    }

    void EndPlayerInvincible()
    {
        Player playerScript = player.GetComponent<Player>();
        playerScript.SetInvincible(false);
    }
}
```

## Assets/ParentCleaner.cs

```csharp
using UnityEngine;

public class ParentCleaner : MonoBehaviour
{
    void Update()
    {
        if (transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }
}
```

## Assets/Player.cs

```csharp
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform firePoint;
    public GameObject PlayerBulletPrefab;

    public float moveSpeed = 5f;
    public float fireRate = 0.2f;

    private float _nextFireTime = 0f;
    private Animator _anim;
    private bool isInvincible = false;

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
        if (isInvincible) return;

        Debug.Log("사망..");
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

    void OnTriggerEnter2D(Collider2D other)
    {
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
```

## Assets/PlayerController.cs

```csharp
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
```

## Assets/SpawnPoint.cs

```csharp
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;

    void Start()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (i == 10 || i == 12) continue;

            float delay = Random.Range(4f, 8f);
            Invoke($"Spawn_{i}", delay);
        }
    }

    void Spawn_0() { SpawnEnemy(0, Vector3.down); }
    void Spawn_1() { SpawnEnemy(1, Vector3.down); }
    void Spawn_2() { SpawnEnemy(2, Vector3.down); }
    void Spawn_3() { SpawnEnemy(3, Vector3.down); }
    void Spawn_4() { SpawnEnemy(4, Vector3.down); }
    void Spawn_5() { SpawnEnemy(5, Vector3.left); }
    void Spawn_6() { SpawnEnemy(6, Vector3.left); }
    void Spawn_7() { SpawnEnemy(7, Vector3.right); }
    void Spawn_8() { SpawnEnemy(8, Vector3.right); }
    void Spawn_9() { SpawnEnemyToward(9, 10); }
    void Spawn_11() { SpawnEnemyToward(11, 12); }

    void SpawnEnemy(int pointIndex, Vector3 moveDir)
    {
        int idx = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyGo = Instantiate(enemyPrefabs[idx]);
        enemyGo.transform.position = spawnPoints[pointIndex].position;

        EnemyController ec = enemyGo.GetComponent<EnemyController>();
        ec.moveDirection = moveDir;

        float delay = Random.Range(4f, 8f);
        Invoke($"Spawn_{pointIndex}", delay);
    }

    void SpawnEnemyToward(int fromIndex, int toIndex)
    {
        int idx = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyGo = Instantiate(enemyPrefabs[idx]);
        enemyGo.transform.position = spawnPoints[fromIndex].position;

        Vector3 direction = spawnPoints[toIndex].position - spawnPoints[fromIndex].position;

        EnemyController ec = enemyGo.GetComponent<EnemyController>();
        ec.moveDirection = direction.normalized;

        float delay = Random.Range(4f, 8f);
        Invoke($"Spawn_{fromIndex}", delay);
    }

    void OnDrawGizmos()
    {
        if (spawnPoints == null) return;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i] == null) continue;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(spawnPoints[i].position, 0.2f);
        }

        if (spawnPoints.Length > 10 && spawnPoints[9] != null && spawnPoints[10] != null)
        {
            Vector3 dir9 = spawnPoints[10].position - spawnPoints[9].position;
            DrawArrow.ForGizmo(spawnPoints[9].position, dir9, Color.green);
        }

        if (spawnPoints.Length > 12 && spawnPoints[11] != null && spawnPoints[12] != null)
        {
            Vector3 dir11 = spawnPoints[12].position - spawnPoints[11].position;
            DrawArrow.ForGizmo(spawnPoints[11].position, dir11, Color.blue);
        }
    }
}
```

## Assets/Vertical 2D Shooting BE4/ReadMe/Scripts/ReadmeBE4.cs

```csharp
using System;
using UnityEngine;

public class ReadmeBE4 : ScriptableObject {
	public Texture2D icon;
	public string title;
	public Section[] sections;
	public bool loadedLayout;
	
	[Serializable]
	public class Section {
		public string heading, text, linkText, url;
	}
}
```

## Assets/Vertical 2D Shooting BE4/ReadMe/Scripts/Editor/ReadmeEditorBE4.cs

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Reflection;

[CustomEditor(typeof(ReadmeBE4))]
[InitializeOnLoad]
public class ReadmeEditorBE4 : Editor {
	
	static string kShowedReadmeSessionStateName = "ReadmeEditor.showedReadme";
	
	static float kSpace = 16f;
	
	static ReadmeEditorBE4()
	{
		EditorApplication.delayCall += SelectReadmeAutomatically;
	}
	
	static void SelectReadmeAutomatically()
	{
		if (!SessionState.GetBool(kShowedReadmeSessionStateName, false ))
		{
			var readme = SelectReadme();
			SessionState.SetBool(kShowedReadmeSessionStateName, true);
			
			if (readme && !readme.loadedLayout)
			{
				LoadLayout();
				readme.loadedLayout = true;
			}
		} 
	}
	
	static void LoadLayout()
	{
		var assembly = typeof(EditorApplication).Assembly; 
		var windowLayoutType = assembly.GetType("UnityEditor.WindowLayout", true);
		var method = windowLayoutType.GetMethod("LoadWindowLayout", BindingFlags.Public | BindingFlags.Static);
		method.Invoke(null, new object[]{Path.Combine(Application.dataPath, "TutorialInfo/Layout.wlt"), false});
	}
	
	static ReadmeBE4 SelectReadme() 
	{
		var ids = AssetDatabase.FindAssets("Readme t:ReadmeBE4");
		if (ids.Length == 1)
		{
			var readmeObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(ids[0]));
			
			Selection.objects = new UnityEngine.Object[]{readmeObject};
			
			return (ReadmeBE4)readmeObject;
		}
		else
		{
			Debug.Log("Couldn't find a readme");
			return null;
		}
	}
	
	protected override void OnHeaderGUI()
	{
		var readme = (ReadmeBE4)target;
		Init();
		
		var iconWidth = Mathf.Min(EditorGUIUtility.currentViewWidth/3f - 20f, 128f);
		
		GUILayout.BeginHorizontal("In BigTitle");
		{
			GUILayout.Label(readme.icon, GUILayout.Width(iconWidth), GUILayout.Height(iconWidth));
			GUILayout.Label(readme.title, TitleStyle);
		}
		GUILayout.EndHorizontal();
	}
	
	public override void OnInspectorGUI()
	{
		var readme = (ReadmeBE4)target;
		Init();
		
		foreach (var section in readme.sections)
		{
			if (!string.IsNullOrEmpty(section.heading))
			{
				GUILayout.Label(section.heading, HeadingStyle);
			}
			if (!string.IsNullOrEmpty(section.text))
			{
				GUILayout.Label(section.text, BodyStyle);
			}
			if (!string.IsNullOrEmpty(section.linkText))
			{
				if (LinkLabel(new GUIContent(section.linkText)))
				{
					Application.OpenURL(section.url);
				}
			}
			GUILayout.Space(kSpace);
		}
	}
	
	
	bool m_Initialized;
	
	GUIStyle LinkStyle { get { return m_LinkStyle; } }
	[SerializeField] GUIStyle m_LinkStyle;
	
	GUIStyle TitleStyle { get { return m_TitleStyle; } }
	[SerializeField] GUIStyle m_TitleStyle;
	
	GUIStyle HeadingStyle { get { return m_HeadingStyle; } }
	[SerializeField] GUIStyle m_HeadingStyle;
	
	GUIStyle BodyStyle { get { return m_BodyStyle; } }
	[SerializeField] GUIStyle m_BodyStyle;
	
	void Init()
	{
		if (m_Initialized)
			return;
		m_BodyStyle = new GUIStyle(EditorStyles.label);
		m_BodyStyle.wordWrap = true;
		m_BodyStyle.fontSize = 14;
		
		m_TitleStyle = new GUIStyle(m_BodyStyle);
		m_TitleStyle.fontSize = 26;
		
		m_HeadingStyle = new GUIStyle(m_BodyStyle);
		m_HeadingStyle.fontSize = 18 ;
		
		m_LinkStyle = new GUIStyle(m_BodyStyle);
		m_LinkStyle.wordWrap = false;
		// Match selection color which works nicely for both light and dark skins
		m_LinkStyle.normal.textColor = new Color (0x00/255f, 0x78/255f, 0xDA/255f, 1f);
		m_LinkStyle.stretchWidth = false;
		
		m_Initialized = true;
	}
	
	bool LinkLabel (GUIContent label, params GUILayoutOption[] options)
	{
		var position = GUILayoutUtility.GetRect(label, LinkStyle, options);

		Handles.BeginGUI ();
		Handles.color = LinkStyle.normal.textColor;
		Handles.DrawLine (new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
		Handles.color = Color.white;
		Handles.EndGUI ();

		EditorGUIUtility.AddCursorRect (position, MouseCursor.Link);

		return GUI.Button (position, label, LinkStyle);
	}
}
```