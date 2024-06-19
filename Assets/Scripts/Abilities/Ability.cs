using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Ability : MonoBehaviour {
    protected UIController uiController;
    protected GameObject enemy;
    protected GameObject player;

    protected bool canUseAbility;

    protected int energyUsed;


    [SerializeField] protected Button abilityButton;
    [SerializeField] protected GameObject playerTarget, enemyTarget;


    public int EnergyUsed { get => energyUsed; set => energyUsed = value; }

    private void Awake() {
        this.player = GameObject.Find("Player");
        this.enemy = GameObject.Find("Enemy");

    }


    protected virtual void ExcuteAbility() {
        GameStateController.Instance.CurrentState = GameStates.ExcutingAbility;
    }

    public bool CanUseAbility() {
        if(TurnController.Instance.IsTurnOfPlayer()) {
            ScoreSystem playerScore = GameObject.Find("PlayerScore").GetComponent<ScoreSystem>();

            if(playerScore.CurrentPowerScore < this.energyUsed)
                return false;

            return true;
        }
        else if(TurnController.Instance.IsTurnOfEnemy()) {
            ScoreSystem enemyScore = GameObject.Find("EnemyScore").GetComponent<ScoreSystem>();

            if(enemyScore.CurrentPowerScore < this.energyUsed)
                return false;

            return true;
        }
        return false;
    }



    public void UpdatePowerScore() {
        if(TurnController.Instance.IsTurnOfPlayer()) {
            ScoreSystem playerScore = GameObject.Find("PlayerScore").GetComponent<ScoreSystem>();
            playerScore.CurrentPowerScore -= this.energyUsed;
        }
        else if(TurnController.Instance.IsTurnOfEnemy()) {
            ScoreSystem enemyScore = GameObject.Find("EnemyScore").GetComponent<ScoreSystem>();
            enemyScore.CurrentPowerScore -= this.energyUsed;
        }
    }
}

