using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public Animator animator;

    private Vector2 moveDir;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;

    private void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void FixedUpdate() {
        rb2d.velocity = moveDir * speed * Time.deltaTime;
    }

    private void Update() {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(inputX, inputY).normalized;
        animator.SetFloat("Horizontal", inputX);
        animator.SetFloat("Vertical", inputY);
        animator.SetFloat("Speed", moveDir.SqrMagnitude());

        if (inputX != 0f || inputY != 0f) {
            animator.SetInteger("LastHorizontal", (int)inputX);
            animator.SetInteger("LastVertical", (int)inputY);
        }
        if (animator.GetInteger("LastHorizontal") == -1) {
            spriteRenderer.flipX = true;
        }
        else {
            spriteRenderer.flipX = false;
        }
    }
}
