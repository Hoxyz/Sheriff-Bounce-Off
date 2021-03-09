﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed;

    private Rigidbody2D rb2d;

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void SetFiringVelocity() {
        rb2d.velocity = transform.right * speed * Time.deltaTime;
    }

    void FixedUpdate() {
        Vector2 dir = rb2d.velocity;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rb2d.velocity = transform.right * speed * Time.deltaTime;
        Debug.DrawRay(transform.position, rb2d.velocity);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }
}