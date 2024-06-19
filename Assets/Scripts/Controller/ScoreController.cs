using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour {
    private static ScoreController instance;
    public static ScoreController Instance {
        get {
            if(instance == null) {
                instance = FindObjectOfType<ScoreController>();
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }
    Player player;
    Enemy enemy;

    private void Start() {
        this.player = GameObject.Find("Player").GetComponent<Player>();
        this.enemy = GameObject.Find("Enemy").GetComponent<Enemy>();
    }
    public int BloodScore { get => bloodScore; set => bloodScore = value; }
    public int PowerScore { get => powerScore; set => powerScore = value; }
    public int EnergyScore { get => energyScore; set => energyScore = value; }
    public int DefenseScore { get => defenseScore; set => defenseScore = value; }
    public int AttackScore { get => attackScore; set => attackScore = value; }

    [SerializeField] ScoreSystem playerScore;
    [SerializeField] ScoreSystem enemyScore;

    [SerializeField] int bloodScore;
    [SerializeField] int powerScore;
    [SerializeField] int energyScore;
    [SerializeField] int defenseScore;
    [SerializeField] int attackScore;


    public void ResetScore() {
        this.bloodScore = 0;
        this.powerScore = 0;
        this.energyScore = 0;
        this.defenseScore = 0;
        this.attackScore = 0;
    }

    public void UpdateScore() {
        this.playerScore.IsUpdateUI = true;
        this.enemyScore.IsUpdateUI = true;
        if(TurnController.Instance.CurrentTurn == GameTurn.playerTurn) {
            this.playerScore.UpdateScore();
            this.playerScore.UpdateCurrentScore();
            if(this.playerScore.AttackScore > 0) {

                this.player.MoveToTargetMove = true;
                this.enemy.IsGetDame = true;

                GameStateController.Instance.CurrentState = GameStates.Attacking;
            }
        }
        else {
            this.enemyScore.UpdateScore();
            this.enemyScore.UpdateCurrentScore();
            if(this.enemyScore.AttackScore > 0) {
                this.player.IsGetDame = true;
                this.enemy.Shoot = true;
                this.enemy.Attack = true;
                GameStateController.Instance.CurrentState = GameStates.Attacking;
            }
        }
    }
    public void TakeDameToPlayer() {
        if(this.playerScore.CurrentDefenseScore >= this.enemyScore.AttackScore * 0.7f) {

            this.playerScore.CurrentDefenseScore -= this.enemyScore.AttackScore * 0.7f;
            this.playerScore.CurrentBloodScore -= this.enemyScore.AttackScore * 0.3f;
        }
        else {
            this.playerScore.CurrentBloodScore -= (this.enemyScore.AttackScore - this.playerScore.CurrentDefenseScore);
            this.playerScore.CurrentDefenseScore = 0;
        }

        this.playerScore.IsUpdateUI = true;

    }
    public void TakeDameToEnemy() {
        if(this.enemyScore.CurrentDefenseScore >= this.playerScore.AttackScore * 0.7f) {
            this.enemyScore.CurrentBloodScore -= this.playerScore.AttackScore * 0.3f;
            this.enemyScore.CurrentDefenseScore -= this.playerScore.AttackScore * 0.7f;

        }
        else {
            this.enemyScore.CurrentBloodScore -= (this.playerScore.AttackScore - this.enemyScore.CurrentDefenseScore);
            this.enemyScore.CurrentDefenseScore = 0;
        }
        this.enemyScore.IsUpdateUI = true;
    }
}
