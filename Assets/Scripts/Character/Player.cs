using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character {
    Vector2 basePosition;
    Quaternion baseRotation;
    [SerializeField] Transform targetMove;

    bool idle;
    [SerializeField] bool moveToTargetMove;
    bool attacking;
    bool backToBase;
    bool isGetDame;
    [SerializeField] float timeAttacking;
    GameObject attackZone;
    public bool MoveToTargetMove { get => moveToTargetMove; set => moveToTargetMove = value; }
    public bool IsGetDame { get => isGetDame; set => isGetDame = value; }

    private void Start() {
        this.Animator = GetComponent<Animator>();
        this.targetAttack = GameObject.Find("Enemy");
        this.isGetDame = true;
        this.basePosition = transform.position;
        this.baseRotation = transform.rotation;
        attackZone = transform.GetChild(0).gameObject;
        SetNewState();
    }
    private void Update() {
        Attacking();
    }
    public override void Attacking() {

        base.Attacking();
        if(GameStateController.Instance.IsAttacking() && TurnController.Instance.IsTurnOfPlayer()) {
            this.idle = false;
            if(this.moveToTargetMove && !this.attacking && !this.backToBase) {
                transform.position = Vector2.Lerp(transform.position, targetMove.position, 10 * Time.deltaTime);
                if(Vector2.Distance(transform.position, this.targetMove.position) <= 0.2f) {
                    transform.position = this.targetMove.position;
                    this.MoveToTargetMove = false;
                    this.attacking = true;
                }

            }

            else if(this.attacking && !this.moveToTargetMove && !this.backToBase) {
                this.attackZone.SetActive(true);
                StartCoroutine(FinishAttacking());
            }

            else if(this.backToBase && !this.attacking && !this.moveToTargetMove) {
                transform.rotation = Quaternion.Euler(new Vector2(this.baseRotation.x, this.baseRotation.y + 180));
                transform.position = Vector2.Lerp(transform.position, this.basePosition, 10 * Time.deltaTime);

                if(Vector2.Distance(transform.position, basePosition) <= 0.2f) {
                    this.backToBase = false;
                    transform.position = this.basePosition;
                    transform.rotation = this.baseRotation;

                    GameStateController.Instance.CurrentState = GameStates.Finish;
                }
            }
        }
        else {
            SetNewState();
        }
        this.Animator.SetBool("backToBase", this.backToBase);
        this.Animator.SetBool("attacking", this.attacking);
        this.Animator.SetBool("moveToTargetMove", this.MoveToTargetMove);
        this.Animator.SetBool("idle", this.idle);
    }

    private void SetNewState() {
        this.MoveToTargetMove = false;
        this.idle = true;
        this.attacking = false;
        this.backToBase = false;
    }

    IEnumerator FinishAttacking() {
        yield return new WaitForSeconds(0.6f);
        this.attackZone.SetActive(false);
        this.attacking = false;
        this.backToBase = true;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(GameStateController.Instance.CurrentState == GameStates.Attacking && this.IsGetDame) {
            if(collision.gameObject.CompareTag("Bullet")) {
                this.IsGetDame = false;
                StartCoroutine(GetDame());
            }
        }
        if(collision.gameObject.CompareTag("Bullet")) {
            Destroy(collision.gameObject, 0.2f);
            this.Animator.SetTrigger("hurt");
            collision.GetComponent<Bullet>().animator.SetBool("exploision", true);
            collision.transform.localScale *= 0.08f;
        }

    }

    private IEnumerator GetDame() {
        yield return new WaitForSeconds(0.2f);
        ScoreController.Instance.TakeDameToPlayer();
    }


}
