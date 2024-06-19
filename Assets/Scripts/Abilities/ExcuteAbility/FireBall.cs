using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour {
    Rigidbody2D rb;
    GameObject enemy;
    GameObject player;

    bool isAttackDot;

    bool isAttackEnemy;
    bool isAttackPlayer;
    
    bool isTakeDameToEnemy;
    bool isTakeDameToPlayer;

    Animator animator;

    float fireDamage;


    Vector3 moveTarget;
    [SerializeField] AllDotController board;
    [SerializeField] GameObject dotTarget;

    int randomCol, randomRow;
    bool destroyDots;
    public bool IsAttackEnemy { get => isAttackEnemy; set => isAttackEnemy = value; }
    public bool IsAttackDot { get => isAttackDot; set => isAttackDot = value; }
    public bool IsTakeDameToPlayer { get => isTakeDameToPlayer; set => isTakeDameToPlayer = value; }
    public bool IsAttackPlayer { get => isAttackPlayer; set => isAttackPlayer = value; }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();

        this.animator = GetComponent<Animator>();

        this.board = FindObjectOfType<AllDotController>();
    }
    private void Start() {

        this.fireDamage = 300f;//   rb.AddForce(transform.right * 2f,ForceMode2D.Impulse);
        this.isTakeDameToEnemy = true;
        this.IsTakeDameToPlayer = true;
        this.destroyDots = true;
    }
    private void Update() {
        if(TurnController.Instance.CurrentTurn == GameTurn.playerTurn) {
            AttackEnemy();
            AttackDot();
        }
        if(TurnController.Instance.CurrentTurn == GameTurn.enemyTurn) {
            AttackPlayer();
            AttackDot();
        }
    }


    public void AttackPlayer() {
        if(this.IsAttackPlayer) {

            if(!this.player) {
                this.player = GameObject.Find("Player");
                transform.rotation = SetFireBallRotationToTarget(this.player.transform.position);
            }

            if(Vector2.Distance(transform.position, this.player.transform.position-Vector3.up*0.6f) <= 0.3f) {
                Destroy(gameObject, 0.4f);
                this.animator.SetBool("exploision", true);
                if(this.IsTakeDameToPlayer) {
                    this.IsTakeDameToPlayer = false;
                    TakeDameToPlayer();
                    this.player.GetComponent<Player>().Animator.SetTrigger("hurt");
                }
            }
            else
                transform.position = Vector2.MoveTowards(transform.position, this.player.transform.position-Vector3.up*0.6f, 20 * Time.deltaTime);
        }
    }
    public void AttackEnemy() {

        if(this.IsAttackEnemy) {

            if(!this.enemy) {
                this.enemy = GameObject.Find("Enemy");
                transform.rotation = SetFireBallRotationToTarget(this.enemy.transform.position);    
            }

            if(Vector2.Distance(transform.position, enemy.transform.position) <= 0.3f) {
                Destroy(gameObject, 0.4f);
                this.animator.SetBool("exploision", true);
                if(this.isTakeDameToEnemy) {
                    this.isTakeDameToEnemy = false;
                    TakeDameToEnemy();
                    this.enemy.GetComponent<Enemy>().Animator.SetTrigger("hurt");
                }

            }
            else
                transform.position = Vector2.MoveTowards(transform.position, enemy.transform.position, 20 * Time.deltaTime);
        }
    }

    public void AttackDot() {
        if(this.isAttackDot && this.destroyDots ) {
            if(!this.dotTarget) {
                this.dotTarget = GetRandomDot();
                
                transform.rotation = SetFireBallRotationToTarget(this.dotTarget.transform.position);
                return;
            }

            if(Vector2.Distance(transform.position, this.dotTarget.transform.position) <= 0.1f ) {
                Destroy(gameObject, 0.4f);
                if(this.destroyDots) {
                    this.destroyDots = false;
                    transform.localScale *= 3f;
                    StartCoroutine(DestroyDots());
                    Debug.Log(this.randomRow);
                }
                this.animator.SetBool("exploision", true);
            }
            else
                transform.position = Vector2.MoveTowards(transform.position, this.dotTarget.transform.position, 20 * Time.deltaTime);
        }

    }

    public void TakeDameToEnemy() {
        ScoreSystem enemyScore = GameObject.Find("EnemyScore").GetComponent<ScoreSystem>();

        if(enemyScore.CurrentDefenseScore >= this.fireDamage * 0.7f) {
            enemyScore.CurrentBloodScore -= this.fireDamage * 0.3f;
            enemyScore.CurrentDefenseScore -= this.fireDamage * 0.7f;

        }
        else {
            enemyScore.CurrentBloodScore -= (this.fireDamage - enemyScore.CurrentDefenseScore);
            enemyScore.CurrentDefenseScore = 0;
        }
        enemyScore.IsUpdateUI = true;
    }
    public void TakeDameToPlayer() {
        ScoreSystem playerScore = GameObject.Find("PlayerScore").GetComponent<ScoreSystem>();

        if(playerScore.CurrentDefenseScore >= this.fireDamage * 0.7f) {
            playerScore.CurrentBloodScore -= this.fireDamage * 0.3f;
            playerScore.CurrentDefenseScore -= this.fireDamage * 0.7f;

        }
        else {
            playerScore.CurrentBloodScore -= (this.fireDamage - playerScore.CurrentDefenseScore);
            playerScore.CurrentDefenseScore = 0;
        }
        playerScore.IsUpdateUI = true;
    }

    public IEnumerator DestroyDots() {
        yield return null;
        for(int i = -2; i <= 2; i++) {
            for(int j = -2; j <= 2; j++) {
                if(this.board.AllDots[this.randomCol + i, this.randomRow + j]) {
                    Destroy(this.board.AllDots[this.randomCol + i, this.randomRow + j]);
                }
            }
        }
        yield return null;
        GameStateController.Instance.CurrentState = GameStates.FillingDots;
        StartCoroutine(this.board.DestroyMatches());
    }

    private Quaternion SetFireBallRotationToTarget(Vector3 target) {
        Vector3 targetDirection = target - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion ro = Quaternion.Euler(0, 0, angle);
        return ro;
    }
    private GameObject GetRandomDot() {
        this.randomCol = Random.Range(2, this.board.Width - 2);
        this.randomRow = Random.Range(2, this.board.Height - 2);

        GameObject dotTarget = this.board.AllDots[this.randomCol, this.randomRow];
        return dotTarget;
    }
}
