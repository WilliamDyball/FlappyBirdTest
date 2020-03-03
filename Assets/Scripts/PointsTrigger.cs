using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsTrigger : Pooled {
    private float fSpeed = 3f;

    //Do stuff when spawned.
    override public void OnObjectSpawn() {

    }
    private void Update() {
        if (bReset) {
            gameObject.SetActive(false);
        }
        transform.Translate((-transform.forward * fSpeed) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (GameManager.instance) {
                GameManager.instance.IncrementScore();
            }
        }
    }
}
