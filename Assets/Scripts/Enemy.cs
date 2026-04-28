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
