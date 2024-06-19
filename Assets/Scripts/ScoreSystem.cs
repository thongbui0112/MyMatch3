using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    private float bloodScore;
    private float energyScore;
    private float powerScore;
    private float defenseScore;
    private float attackScore;

    [SerializeField] private float maxBloodScore;
    [SerializeField] private float maxEnergyScore;
    [SerializeField] private float maxPowerScore;
    [SerializeField] private float maxDefenseScore;
    [SerializeField] private float maxAttackScore;

    [SerializeField] private float currentBloodScore;
    [SerializeField] private float currentEnergyScore;
    [SerializeField] private float currentPowerScore;
    [SerializeField] private float currentDefenseScore;
    [SerializeField] private float currentAttackScore;

    [SerializeField] Image currentBloodBar;
    [SerializeField] Image currentEnergyBar;
    [SerializeField] Image currentPowerBar;
    [SerializeField] Image currentDefenseBar;

    private bool isUpdateUI;
    #region public
    public float BloodScore { get => bloodScore; set => bloodScore = value; }
    public float EnergyScore { get => energyScore; set => energyScore = value; }
    public float PowerScore { get => powerScore; set => powerScore = value; }
    public float DefenseScore { get => defenseScore; set => defenseScore = value; }
    public float AttackScore { get => attackScore; set => attackScore = value; }
    public bool IsUpdateUI { get => isUpdateUI; set => isUpdateUI = value; }
    public float CurrentBloodScore { get => currentBloodScore; set => currentBloodScore = value; }
    public float CurrentEnergyScore { get => currentEnergyScore; set => currentEnergyScore = value; }
    public float CurrentPowerScore { get => currentPowerScore; set => currentPowerScore = value; }
    public float CurrentDefenseScore { get => currentDefenseScore; set => currentDefenseScore = value; }
    public float CurrentAttackScore { get => currentAttackScore; set => currentAttackScore = value; }
    public float MaxBloodScore { get => maxBloodScore; set => maxBloodScore = value; }
    public float MaxEnergyScore { get => maxEnergyScore; set => maxEnergyScore = value; }
    public float MaxPowerScore { get => maxPowerScore; set => maxPowerScore = value; }
    public float MaxDefenseScore { get => maxDefenseScore; set => maxDefenseScore = value; }
    public float MaxAttackScore { get => maxAttackScore; set => maxAttackScore = value; }

    #endregion

    private void Start() {

        this.CurrentBloodScore = this.MaxBloodScore;
        this.CurrentPowerScore = 50;
        this.CurrentEnergyScore = 50;
        this.CurrentDefenseScore = 0;
        this.CurrentAttackScore = 0;
        this.isUpdateUI = true;
    }
    private void Update() {
        StartCoroutine(UpdateUI());
    }

    IEnumerator UpdateUI() {
        
        if( GameStateController.Instance.CurrentState!=GameStates.None) {

            this.currentBloodBar.fillAmount = Mathf.Lerp(this.currentBloodBar.fillAmount , this.CurrentBloodScore / this.MaxBloodScore, 15* Time.deltaTime);
            this.currentPowerBar.fillAmount = Mathf.Lerp(this.currentPowerBar.fillAmount , this.CurrentPowerScore / this.MaxPowerScore, 15*Time.deltaTime);
            this.currentEnergyBar.fillAmount = Mathf.Lerp(this.currentEnergyBar.fillAmount , this.CurrentEnergyScore / this.MaxEnergyScore, 15*Time.deltaTime);
            this.currentDefenseBar.fillAmount = Mathf.Lerp(this.currentDefenseBar.fillAmount , this.CurrentDefenseScore / this.MaxDefenseScore, 15*Time.deltaTime);
            
            this.currentBloodScore = this.currentBloodScore <=0 ? 0 : this.currentBloodScore;
            this.currentPowerScore = this.currentPowerScore<=0 ? 0 : this.CurrentPowerScore;
            this.CurrentEnergyScore = this.currentEnergyScore <= 0 ? 0 : this.currentEnergyScore;
            
            yield return new WaitForSeconds(1f);
            //ResetScore();
            this.IsUpdateUI = false;
        }

    }



    public void ResetScore() {
        this.bloodScore = 0;
        this.energyScore = 0;
        this.powerScore = 0;
        this.defenseScore = 0;
        this.attackScore = 0;
    }

    public void UpdateCurrentScore() {
        this.currentBloodScore += this.bloodScore;
        this.currentPowerScore+= this.powerScore;
        this.currentEnergyScore += this.energyScore;
        this.currentDefenseScore+= this.defenseScore;

        this.currentBloodScore = this.currentBloodScore >=this.MaxBloodScore ? this.MaxBloodScore : this.currentBloodScore;
        this.currentPowerScore = this.currentPowerScore >= this.MaxPowerScore ? this.MaxPowerScore : this.currentPowerScore;
        this.currentEnergyScore = this.CurrentEnergyScore >= this.MaxEnergyScore ? this.MaxEnergyScore : this.currentEnergyScore;
        this.currentDefenseScore = this.currentDefenseScore >= this.currentBloodScore ? this.currentBloodScore : this.currentDefenseScore;
    }

    public void UpdateScore() {
        this.bloodScore = ScoreController.Instance.BloodScore;
        this.powerScore = ScoreController.Instance.PowerScore;
        this.energyScore= ScoreController.Instance.EnergyScore;
        this.defenseScore= ScoreController.Instance.DefenseScore;
        this.attackScore = ScoreController.Instance.AttackScore;
    }


}
