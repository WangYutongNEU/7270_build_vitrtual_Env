using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool vertical;
    public float changeTime;

    private float timer;
    private int direction = 1;

    public float speed = 1.0f;
    private Rigidbody2D rb;

    private Animator animator;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
    
        Vector2 position = rb.position;
        if (vertical)
        {
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);

            position.y = position.y + Time.deltaTime * speed * direction;;
        }
        else
        {
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);

            position.x = position.x + Time.deltaTime * speed * direction;;
        }
        
        rb.MovePosition(position);
    }



}

