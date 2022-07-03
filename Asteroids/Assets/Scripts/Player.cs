using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Bullet bulletPrefab;
    private bool thrusting;
    private float turnDirection;

    private Rigidbody2D rb;
    public float thrustSpeed = 1.0f;
    public float turnSpeed = 1.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
    
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            turnDirection = 1.0f;
        }
        else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            turnDirection = -1.0f;
        }
        else
        {
            turnDirection  = 0;
        }

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))   //getkeydown becuase i dont want to spawn bullets every single frame
        {
            Shoot();
        }
    }


    private void FixedUpdate()
    {
        if(thrusting)
        {
            rb.AddForce(this.transform.up * thrustSpeed );
        }

        if(turnDirection != 0)
        {
            rb.AddTorque(turnDirection * turnSpeed);
        }
    }


    private void Shoot()
    {
        Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.Project(this.transform.up); // shoot in the direction that i am moving
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Asteroid")
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0.0f;
            this.gameObject.SetActive(false);

            FindObjectOfType<GameManager>().PlayerDied();
        }
    }
}
