using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 500.0f;
    public float maxLifetime = 10.0f;

    private AudioSource audio;
    private TrailRenderer tr;
    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        tr = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody2D>();
        //this.tr.material.color = Random.ColorHSV(); //Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    private void Start()
    {
        Color color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        tr.startColor = color;
        audio.Play();
        tr.endColor = color;// new Color (0.35f, 1f, 0.75f);
    }

    public void Project(Vector2 direction)
    {

        rb.AddForce(direction * speed);
        Destroy(this.gameObject, this.maxLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }


}
