using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDMovement : MonoBehaviour
{
    public float speed = 3.0f;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    public bool canMove = true;

    
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }
    void FixedUpdate() 
    {
        if (canMove)
        {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
        }
    }
}
