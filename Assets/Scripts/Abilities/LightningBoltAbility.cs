using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LightningBoltAbility : Ability {

    [SerializeField] GameObject lineConnector;
    private void Awake() {

        this.uiController = FindObjectOfType<UIController>();
        this.abilityButton = GetComponent<Button>();

        this.abilityButton.onClick.AddListener(ExcuteAbility);
        //this.abilityButton.onClick.AddListener(this.uiController.ExitAbilitiesButton);
    
        this.EnergyUsed = 70;

    }

    private void Start() {
    }
    protected override void ExcuteAbility() {

        if(CanUseAbility()) {
            UpdatePowerScore();
            base.ExcuteAbility();
            GameObject lineConnector = Instantiate(this.lineConnector);
            lineConnector.SetActive(true);

            this.uiController.ExitAbilitiesButton();
        }

    }

}
