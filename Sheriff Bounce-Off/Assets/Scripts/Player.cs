using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    // Movement
    public KeyCode keyLeft;
    public KeyCode keyUp;
    public KeyCode keyRight;
    public KeyCode keyDown;
    public KeyCode keyShoot;
    public KeyCode keyMirror;
    public KeyCode keyBomb;

    public int maxBullets = 6;
    public float speed = 5f;
    public float mirrorDuration = 0.5f;
    public float mirrorCooldown = 2f;
    public Animator animator;
    public GameObject bulletPrefab;
    public GameObject bulletDeathAnimationPrefab;
    public GameObject mirror;

    private bool canUseMirror = true;
    private Collider2D collider;
    private Vector2 moveDir;
    private Vector2 lastMoveDir;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    private SpriteMask mirrorSpriteMask;
    public List<GameObject> bullets;

    private void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        mirrorSpriteMask = mirror.GetComponent<SpriteMask>();

        bullets = new List<GameObject>();
    }
    // Update is called once per frame
    void FixedUpdate() {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot_Up")       ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot_AngleUp")  ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot_Side")     ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot_AngleDown")||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot_Down")     ||
            gameObject.tag == "Mirror") {

            rb2d.velocity = Vector2.zero;
        }
        else {
            rb2d.velocity = moveDir * speed * Time.deltaTime;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y + collider.offset.y);
    }

    private void Update() {
        Move();
        Shoot();
        if (Input.GetKeyDown(keyMirror)) {
            StartCoroutine(Mirror());
        }
        RemoveOpponentsBullets();
    }
    
    IEnumerator Mirror() {
        string tag = gameObject.tag;
        if (canUseMirror) {
            canUseMirror = false;
            float t = 0f;
            while (t < mirrorDuration) {
                t += Time.deltaTime;
                mirror.SetActive(true);
                gameObject.tag = "Mirror";
                mirrorSpriteMask.sprite = spriteRenderer.sprite;
                if (spriteRenderer.flipX) {
                    mirror.transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                else {
                    mirror.transform.localScale = new Vector3(1f, 1f, 1f);
                }
                yield return null;
            }
            gameObject.tag = tag;
            mirror.SetActive(false);
            StartCoroutine(MirrorCooldown());
        }
    }

    IEnumerator MirrorCooldown() {
        float t = 0f;
        while (t < mirrorCooldown) {
            t += Time.deltaTime;
            yield return null;
        }
        canUseMirror = true;
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

    private void Shoot() {
        if (Input.GetKeyDown(keyShoot)) {
            animator.SetTrigger("Shooting");
            float angle = Mathf.Atan2(lastMoveDir.y, lastMoveDir.x) * Mathf.Rad2Deg;
            GameObject bulletObj;
            if (bullets.Count < maxBullets) {
                bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));
                bullets.Add(bulletObj);
            }
            else {
                bulletObj = bullets[maxBullets - 1];
                GameObject animObj = Instantiate(bulletDeathAnimationPrefab, bulletObj.transform.position, Quaternion.identity);
                Destroy(animObj, 0.25f);
                bullets.RemoveAt(maxBullets - 1);
                bullets.Insert(0, bulletObj);
                bulletObj.transform.position = transform.position;
                bulletObj.transform.eulerAngles = new Vector3(0f, 0f, angle);
            }
            bulletObj.GetComponent<Bullet>().SetFiringVelocity();
        }
    }

    private void RemoveOpponentsBullets() {
        for (int i = bullets.Count - 1; i >= 0; i--) {
            if (gameObject.tag == "PlayerRed" && bullets[i].tag == "BulletBlue" ||
                gameObject.tag == "PlayerBlue" && bullets[i].tag == "BulletRed") {
                bullets.Remove(bullets[i]);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "BulletBlue" || collision.gameObject.tag == "BulletRed") {
            if (gameObject.tag == "PlayerRed" || gameObject.tag == "PlayerBlue") {
                print("DED");
            }
            else {
                collision.gameObject.GetComponent<Bullet>().SwitchBullet();
            }
        }
    }
}
