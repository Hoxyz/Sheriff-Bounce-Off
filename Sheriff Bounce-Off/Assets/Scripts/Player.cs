using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public KeyCode keyShoot;
    public KeyCode keyMirror;
    public KeyCode keyBomb;

    public int maxBullets = 6;
    public float mirrorDuration = 0.5f;
    public float mirrorCooldown = 2f;
    public GameObject bulletPrefab;
    public GameObject bulletDeathAnimationPrefab;
    public GameObject mirror;

    private bool canUseMirror = true;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private SpriteMask mirrorSpriteMask;
    private PlayerController playerController;
    public List<GameObject> bullets;

    private void Start() {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mirrorSpriteMask = mirror.GetComponent<SpriteMask>();

        bullets = new List<GameObject>();
    }

    private void Update() {
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

    private void Shoot() {
        if (Input.GetKeyDown(keyShoot)) {
            animator.SetTrigger("Shooting");
            float angle = Mathf.Atan2(playerController.getLastMoveDir().y, playerController.getLastMoveDir().x) * Mathf.Rad2Deg;
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
}
