using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;

    [Header("Abilities")]
    [SerializeField] Button abilitiesButton;
    [SerializeField] Button exitAbilitiesButton;
    [SerializeField] GameObject abilitiesObj;



    private void Awake() {
        this.abilitiesButton.onClick.AddListener(AbilitiesButton);
        this.exitAbilitiesButton.onClick.AddListener(ExitAbilitiesButton);
    }

    private void Start() {
        this.abilitiesObj.SetActive(false);
    }

    public void AbilitiesButton() {
        if(GameStateController.Instance.CurrentState != GameStates.Swipe )
            return;

        this.abilitiesObj.SetActive(true);
        this.pausePanel.SetActive(true);
        AllDotState.Instance.IsPause = true;
        AllDotState.Instance.SetCollider(false);
    }

    public void ExitAbilitiesButton() {
        this.abilitiesObj.SetActive(false);
        this.pausePanel.SetActive(false);
        AllDotState.Instance.IsPause = false;
        AllDotState.Instance.SetCollider(true);
    }
}
