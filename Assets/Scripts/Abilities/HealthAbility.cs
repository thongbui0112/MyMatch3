using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthAbility : Ability {

    ScoreSystem enemyScore;
    ScoreSystem playerScore;

    private void Awake() {
        this.enemyScore = GameObject.Find("EnemyScore").GetComponent<ScoreSystem>();
        this.playerScore = GameObject.Find("PlayerScore").GetComponent<ScoreSystem>();

        this.uiController = FindObjectOfType<UIController>();
        this.abilityButton = GetComponent<Button>();

        this.abilityButton.onClick.AddListener(ExcuteAbility);
        this.abilityButton.onClick.AddListener(this.uiController.ExitAbilitiesButton);
        this.EnergyUsed = 50;
    }
    private void Start() {
    }
    protected override void ExcuteAbility() {
        //if(GameStateController.Instance.CurrentState != GameStates.Swipe)
        //    return;
        if(CanUseAbility()) {
            UpdatePowerScore();
            base.ExcuteAbility();
            if(TurnController.Instance.CurrentTurn == GameTurn.playerTurn) {
                this.playerScore.CurrentBloodScore += (this.playerScore.MaxBloodScore - this.playerScore.CurrentBloodScore) * 0.5f;
            }
            else {
                this.enemyScore.CurrentBloodScore += (this.enemyScore.MaxBloodScore - this.enemyScore.CurrentBloodScore) * 0.5f;
            }

            GameStateController.Instance.CurrentState = GameStates.Swipe;

            this.uiController.ExitAbilitiesButton();
        }

    }
}
