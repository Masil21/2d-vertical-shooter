using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour
{
    public Button attackButton;

    void Start()
    {
        attackButton.onClick.AddListener(() =>
        {
            if (Enemy_01.Instance != null)
            {
                Enemy_01.Instance.TakeDamage(5);
            }
            else
            {
                Debug.Log("적이 이미 제거되었습니다.");
            }
        });
    }
}