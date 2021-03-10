using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private void Start() {
        GameObject[] environmentObjects = GameObject.FindGameObjectsWithTag("Environment");
        foreach (GameObject item in environmentObjects) {
            Collider2D itemCollider = item.GetComponent<Collider2D>();
            item.transform.position += new Vector3(0f, 0f, item.transform.position.y + itemCollider.offset.y);
        }
    }
}
