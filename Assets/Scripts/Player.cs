using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private Transform transStartPos;
    private Coroutine respawnCoroutine;
    private bool bResapawn;
    private bool bRestarted;
    private Rigidbody rb;

    [SerializeField]
    float fUpForce = 10;

    private bool bTouched = false;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }


    private void Update() {
        if (bResapawn) {
            return;
        }
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0)) {
            rb.AddForce(Vector3.up * fUpForce, ForceMode.Impulse);
        }

#else
        if (Input.touchCount > 0) {
            if (bTouched) {
                return;
            } else {
                rb.AddForce(Vector3.up * fUpForce, ForceMode.Impulse);
                bTouched = true;
            }
        } else {
            bTouched = false;
        }
#endif
    }

    private void OnCollisionEnter(Collision other) {
        if (other.collider.CompareTag("Pipe")) {
            //Kill player.
            TryKillPlayer();
        }
    }

    public void ResetPlayer() {
        bRestarted = true;
    }

    public void TryKillPlayer() {
        GameManager.instance.GameOver();
        rb.isKinematic = true;
        respawnCoroutine = StartCoroutine(IERespawn());
    }
    private readonly WaitForSeconds respawnDuration = new WaitForSeconds(.1f);

    private IEnumerator IERespawn() {
        bResapawn = true;
        GameManager.instance.bRespawning = true;
        Pooled.bReset = true;
        while (!bRestarted)
            yield return respawnDuration;

        bRestarted = false; ;
        transform.position = transStartPos.position;
        Pooled.bReset = false;
        GameManager.instance.bRespawning = false;
        bResapawn = false;
        rb.isKinematic = false;
    }
}
