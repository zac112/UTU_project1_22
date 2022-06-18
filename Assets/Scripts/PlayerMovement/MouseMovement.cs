using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : PlayerMovement
{

    public float speed = 3.0f;

    Rigidbody2D rigidbody2d;

    Vector2 target;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        target = rigidbody2d.position;
    }

    // Update is called once per frame
    void Update()
    {
        GetMousePos();
    }

    void FixedUpdate() 
    {
        Vector2 position = rigidbody2d.position;
        rigidbody2d.position = Vector2.MoveTowards(position, target, speed * Time.deltaTime);
    }

    void GetMousePos()
    {
        if (Input.GetMouseButton(0)) 
        {
            Vector2 mousePos = Input.mousePosition;
            target = Camera.main.ScreenToWorldPoint(new Vector2(mousePos.x, mousePos.y));
        }
    }
}
