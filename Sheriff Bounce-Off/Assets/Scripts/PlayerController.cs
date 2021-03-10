using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public KeyCode keyLeft;
    public KeyCode keyUp;
    public KeyCode keyRight;
    public KeyCode keyDown;

    private Vector2 moveDir;
    private Vector2 lastMoveDir;
    private Rigidbody2D rb2d;
    private Collider2D collider;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public float speed = 5f;
    public GameObject playerDeathAnimationPrefab;

    public Vector2 getLastMoveDir() {
        return lastMoveDir;
    }

    private void Start() {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        Move();
    }

    void FixedUpdate() {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot_Up") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot_AngleUp") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot_Side") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot_AngleDown") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot_Down") ||
            gameObject.tag == "Mirror") {

            rb2d.velocity = Vector2.zero;
        }
        else {
            rb2d.velocity = moveDir * speed * Time.deltaTime;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y + collider.offset.y);
    }

    private void Move() {
        float inputX = Input.GetKey(keyLeft) ? -1f : Input.GetKey(keyRight) ? 1f : 0f;
        float inputY = Input.GetKey(keyDown) ? -1f : Input.GetKey(keyUp) ? 1f : 0f;

        moveDir = new Vector2(inputX, inputY).normalized;
        animator.SetFloat("Horizontal", inputX);
        animator.SetFloat("Vertical", inputY);
        animator.SetFloat("Speed", moveDir.SqrMagnitude());

        if (inputX != 0f || inputY != 0f) {
            lastMoveDir = new Vector2(inputX, inputY);
            animator.SetInteger("LastHorizontal", (int)lastMoveDir.x);
            animator.SetInteger("LastVertical", (int)lastMoveDir.y);
        }
        if (animator.GetInteger("LastHorizontal") == -1) {
            spriteRenderer.flipX = true;
        }
        else {
            spriteRenderer.flipX = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "BulletBlue" || collision.gameObject.tag == "BulletRed") {
            if (gameObject.tag == "PlayerRed" || gameObject.tag == "PlayerBlue") {
                GameObject animObj = Instantiate(playerDeathAnimationPrefab, transform.position, Quaternion.identity);
                animObj.GetComponent<SpriteRenderer>().color = spriteRenderer.color;
                Destroy(gameObject);
            }
            else {
                collision.gameObject.GetComponent<Bullet>().SwitchBullet();
            }
        }
    }

}
