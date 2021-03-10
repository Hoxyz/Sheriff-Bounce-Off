using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject sandTile;
    public GameObject tileParent;
    public GameObject mirror;
    public GameObject mirrorParent;

    private void Start() {
        GameObject[] environmentObjects = GameObject.FindGameObjectsWithTag("Environment");
        foreach (GameObject item in environmentObjects) {
            Collider2D itemCollider = item.GetComponent<Collider2D>();
            item.transform.position += new Vector3(0f, 0f, item.transform.position.y + itemCollider.offset.y);
        }
        float camX = Camera.main.aspect * Camera.main.orthographicSize;
        float camY = Camera.main.orthographicSize;
        for (float x = -camX; ; x += sandTile.transform.localScale.x) {
            for (float y = -camY; ; y += sandTile.transform.localScale.y) {
                Instantiate(sandTile, new Vector3(x, y, 100f), Quaternion.identity, tileParent.transform);
                if (y > camY) break;
            }
            if (x > camX) break;
        }
        GameObject mirrorObj = Instantiate(mirror, new Vector3(0f, camY, -100f), Quaternion.identity, mirrorParent.transform);
        mirrorObj.transform.localScale = new Vector3(camX * 2 + 1, mirrorObj.transform.localScale.y, 1f);
        mirrorObj = Instantiate(mirror, new Vector3(0f, -camY, -100f), Quaternion.identity, mirrorParent.transform);
        mirrorObj.transform.localScale = new Vector3(camX * 2 + 1, mirrorObj.transform.localScale.y, 1f);
        mirrorObj = Instantiate(mirror, new Vector3(camX, 0f, -100f), Quaternion.Euler(0f, 0f, 90f), mirrorParent.transform);
        mirrorObj.transform.localScale = new Vector3(camY * 2 + 1, mirrorObj.transform.localScale.y, 1f);
        mirrorObj = Instantiate(mirror, new Vector3(-camX, 0f, -100f), Quaternion.Euler(0f, 0f, 90f), mirrorParent.transform);
        mirrorObj.transform.localScale = new Vector3(camY * 2 + 1, mirrorObj.transform.localScale.y, 1f);
    }
}
