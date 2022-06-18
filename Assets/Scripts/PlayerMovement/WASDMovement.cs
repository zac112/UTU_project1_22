using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDMovement : PlayerMovement
{
    protected float speed = 3.0f;

    protected Rigidbody2D rigidbody2d;
    public bool canMove = true;

    
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        Camera.main.GetComponent<MainCameraController>().target = gameObject.transform;
    }

    void FixedUpdate() 
    {
        if (canMove)
        {
            Move();
        }
    }

    public virtual void Move() {
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);        
    } 

    public void setSpeed(float speed) {
        this.speed = speed;
    }
}
