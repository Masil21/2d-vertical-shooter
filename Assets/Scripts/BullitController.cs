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
            ReturnOrDeactivate();
        }
    }

    public void ReturnOrDeactivate()
    {
        // 부모가 ParentCleaner를 가진 컨테이너면 자식 불릿 → 비활성화만
        // ParentCleaner가 컨테이너 전체 반환을 담당
        if (transform.parent != null && transform.parent.GetComponent<ParentCleaner>() != null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            if (ObjectPoolManager.Instance != null)
                ObjectPoolManager.Instance.ReturnToPool(gameObject);
            else
                Destroy(gameObject);
        }
    }
}
