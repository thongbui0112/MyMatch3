using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    GameObject player;
    public Animator animator;
    private void OnEnable() {
        this.player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
    }
    private void Update() {
        transform.position = Vector2.MoveTowards(transform.position, this.player.transform.position - Vector3.up * 0.6f, 30 * Time.deltaTime);
    }
}
