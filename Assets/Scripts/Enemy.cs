using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float movingSpeed = 1f;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), 1f);
    }

    // Update is called once per frame
    void Update()
    {
        Flipping();

    }

    private void Flipping()
    {
        if (transform.localScale.x > 0)
        {
            rb.velocity = new Vector2(movingSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-movingSpeed, rb.velocity.y);
        }
    }
}
