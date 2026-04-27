using UnityEngine;

public class EnemyBullit : MonoBehaviour
{
    private Transform firePoint;

    private float delta = 0;
    private int shotCount = 0;
    private bool isResting = false;
    private float restDelta = 0;
    private bool isAiming = true;
    private float aimDelta = 0;
    private Vector3 targetDirection;

    private GameObject player;

    void Awake()
    {
        firePoint = transform.Find("firePoint");
    }

    void OnEnable()
    {
        player = GameObject.Find("Player");
        delta = 0;
        shotCount = 0;
        isResting = false;
        restDelta = 0;
        isAiming = true;
        aimDelta = 0;
        targetDirection = Vector3.down;
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
        if (firePoint == null) return;

        GameObject bulletGo = ObjectPoolManager.Instance.GetEnemyBullet();
        if (bulletGo == null) return;

        bulletGo.transform.position = firePoint.position;

        EnemyBullitController ebc = bulletGo.GetComponent<EnemyBullitController>();
        if (ebc != null)
        {
            ebc.moveDirection = targetDirection;
        }

        bulletGo.SetActive(true);
    }
}
