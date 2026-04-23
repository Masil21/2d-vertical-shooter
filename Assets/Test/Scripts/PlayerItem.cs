using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("Coin"))
        {
            Debug.Log("코인을 획득했습니다!");
            Destroy(other.gameObject);
        }
        else if (other.gameObject.name.Contains("Boom"))
        {
            Debug.Log("붐을 획득했습니다!");
            Destroy(other.gameObject);
        }
        else if (other.gameObject.name.Contains("Power"))
        {
            Debug.Log("파워를 획득했습니다!");
            Destroy(other.gameObject);
        }
    }
}