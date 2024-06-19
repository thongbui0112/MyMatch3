using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;                                                                                               

public class EnergyShieldAbility :  Ability
{

    [SerializeField] GameObject shieldPref;

    private void Awake() {
        this.uiController = FindObjectOfType<UIController>();
        this.abilityButton = GetComponent<Button>();

        this.abilityButton.onClick.AddListener(ExcuteAbility);
        this.abilityButton.onClick.AddListener(this.uiController.ExitAbilitiesButton);
    }


    protected virtual void ExeuteAbility() {
        GameObject shield = Instantiate(shieldPref);
        if(TurnController.Instance.CurrentTurn == GameTurn.playerTurn) {
            shield.transform.position = this.player.transform.position;
        }
        else
            shield.transform.position = this.enemy.transform.position;
    }



}
