using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FireBallAbility : Ability {
    [SerializeField] GameObject fireBallObj;
    [SerializeField] Transform fireBallPosEne;
    [SerializeField] Transform fireBallPosPla;

    private void Awake() {
        this.uiController = FindObjectOfType<UIController>();
        this.abilityButton = GetComponent<Button>();

        this.abilityButton.onClick.AddListener(ExcuteAbility);
        this.energyUsed = 100;
    }
    private void Start() {
        this.playerTarget = GameObject.Find("Player");
        this.enemyTarget = GameObject.Find("Enemy");
    }



    protected override void ExcuteAbility() {

        if(CanUseAbility()) {
            UpdatePowerScore();
            base.ExcuteAbility();
            Debug.Log("tai sao lai vao day duoc");
            AttackEnemyOrPlayer();
            Invoke(nameof(AttackDot), 0.6f);

            this.uiController.ExitAbilitiesButton();
        }
    }

    public void AttackEnemyOrPlayer() {
        GameObject fireball = Instantiate(this.fireBallObj);
        if(TurnController.Instance.CurrentTurn == GameTurn.playerTurn)
            fireball.transform.position = this.fireBallPosEne.position;
        else
            fireball.transform.position = this.fireBallPosPla.position;

        fireball.GetComponent<FireBall>().IsAttackPlayer = true;
        fireball.GetComponent<FireBall>().IsAttackEnemy = true;
        fireball.GetComponent<FireBall>().IsAttackDot = false;
    }

    public void AttackDot() {
        GameObject fireball = Instantiate(this.fireBallObj);
        if(TurnController.Instance.CurrentTurn == GameTurn.playerTurn)
            fireball.transform.position = this.fireBallPosEne.position;
        else
            fireball.transform.position = this.fireBallPosPla.position;

        fireball.GetComponent<FireBall>().IsAttackPlayer = false;
        fireball.GetComponent<FireBall>().IsAttackEnemy = false;
        fireball.GetComponent<FireBall>().IsAttackDot = true;

    }





}
