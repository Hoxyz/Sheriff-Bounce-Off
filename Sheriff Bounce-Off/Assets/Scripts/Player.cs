using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed = 5f;
    public Animator animator;
    public GameObject bulletPrefab;

    private Collider2D collider;
    private Vector2 moveDir;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;

    private void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }
    // Update is called once per frame
    void FixedUpdate() {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot_Up")       ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot_AngleUp")  ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot_Side")     ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot_AngleDown")||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot_Down")) {

            //rb2d.velocity = Vector2.zero;
        }
        else {
            rb2d.velocity = moveDir * speed * Time.deltaTime;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y + collider.offset.y);
    }

    private void Update() {
        Move();
        Shoot();
    }

    private void Move() {
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

    private void Shoot() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            animator.SetTrigger("Shooting");
        }
    }

}
