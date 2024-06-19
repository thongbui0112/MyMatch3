using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Character {

    [SerializeField] GameObject bullet;

    bool idle;
    bool shoot;
    bool attack;
    bool isGetDame;

    public bool Attack { get => attack; set => attack = value; }
    public bool Shoot { get => shoot; set => shoot = value; }
    public bool IsGetDame { get => isGetDame; set => isGetDame = value; }
    private void Start() {
        this.Animator = GetComponent<Animator>();

        this.idle = true;
        this.attack = false;
        this.Shoot = true;
    }

    private void Update() {
        Attacking();
    }

    public override void Attacking() {
        base.Attacking();
        if(GameStateController.Instance.CurrentState == GameStates.Attacking && TurnController.Instance.CurrentTurn == GameTurn.enemyTurn) {
            this.idle = false;
            if(this.attack) {
                StartCoroutine(ExcuteAttacking());
            }
        }
        else {
            this.idle = true;
        }
        this.Animator.SetBool("attack", this.attack);
        this.Animator.SetBool("idle", this.idle);
    }

    public IEnumerator ExcuteAttacking() {
        if(this.Shoot) {
            this.Shoot = false;
            yield return new WaitForSeconds(0.2f);
            SpawnBullet();
            yield return new WaitForSeconds(0.3f);
            SpawnBullet();
            yield return new WaitForSeconds(0.2f);
            this.attack = false;
            yield return new WaitForSeconds(0.4f);
            GameStateController.Instance.CurrentState = GameStates.Finish;
        }
    }

    private void SpawnBullet() {
        GameObject bullet = Instantiate(this.bullet,transform.position,Quaternion.identity);
        bullet.transform.localScale = Vector3.one * 10;
        bullet.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(GameStateController.Instance.CurrentState == GameStates.Attacking && this.IsGetDame) {
            if(collision.gameObject.CompareTag("AttackZone")) {
                FindObjectOfType<CameraShake>().ShakeCam = true;
                this.IsGetDame = false;
                StartCoroutine(GetDame());
                this.Animator.SetTrigger("hurt");
            }
        }
    }
    
    private IEnumerator GetDame() {
        yield return new WaitForSeconds(0.2f);
        ScoreController.Instance.TakeDameToEnemy();
    }

}
