using UnityEngine;

public class ParentCleaner : MonoBehaviour
{
    void Update()
    {
        if (transform.childCount == 0)
        {
            ReturnToPool();
            return;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
                return;
        }
        ReturnToPool();
    }

    void ReturnToPool()
    {
        if (ObjectPoolManager.Instance != null)
            ObjectPoolManager.Instance.ReturnToPool(gameObject);
        else
            Destroy(gameObject);
    }
}
