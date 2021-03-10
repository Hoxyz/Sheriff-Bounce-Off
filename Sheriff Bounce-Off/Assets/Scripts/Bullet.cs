using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed;

    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;

    public Sprite bulletBlueSprite;
    public Sprite bulletRedSprite;

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetFiringVelocity() {
        rb2d.velocity = transform.right * speed * Time.deltaTime;
    }

    void FixedUpdate() {
        Vector2 dir = rb2d.velocity;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rb2d.velocity = transform.right * speed * Time.deltaTime;
        //Debug.DrawRay(transform.position, rb2d.velocity);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }

    public void SwitchBullet() {
        if (gameObject.tag == "BulletBlue") {
            gameObject.tag = "BulletRed";
            gameObject.layer = LayerMask.NameToLayer("BulletRed");
            spriteRenderer.sprite = bulletRedSprite;
        }
        else if (gameObject.tag == "BulletRed") {
            gameObject.tag = "BulletBlue";
            gameObject.layer = LayerMask.NameToLayer("BulletBlue");
            spriteRenderer.sprite = bulletBlueSprite;
        }
    }

}
