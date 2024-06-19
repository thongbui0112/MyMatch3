using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TurnController : MonoBehaviour {
    private static TurnController instance;
    public static TurnController Instance {
        get {
            if(instance == null) {
                instance = FindObjectOfType<TurnController>();
            }
            return instance;
        }
    }

    #region
    private GameTurn currentTurn;
    [SerializeField] private int countTurn;
    [SerializeField] private int countCombos;
    [SerializeField] private int countDotsDestroyedInTurn;

    bool canGetTurnByCombos;
    bool canGetTurnByCountDotsDestroyed;
    bool canGetTurnByFiveMatches;



    [SerializeField] TMP_Text turnTxt;

    [SerializeField] TMP_Text countTurnTxt;
    [SerializeField] TMP_Text countCombosTxt;
    [SerializeField] TMP_Text getTurnTxt;

    [SerializeField] Transform playerGetTurnPos;
    [SerializeField] Transform playerCountCombosPos;

    [SerializeField] Transform enemyGetTurnPos;
    [SerializeField] Transform enemyCountCombosPos;
    #endregion

    #region
    public GameTurn CurrentTurn { get => currentTurn; set => currentTurn = value; }
    public int CountTurn { get => countTurn; set => countTurn = value; }
    public int CountCombos { get => countCombos; set => countCombos = value; }
    public bool GetTurnByCombos { get => canGetTurnByCombos; set => canGetTurnByCombos = value; }
    public bool GetTurnByCountDotsDestroyed { get => canGetTurnByCountDotsDestroyed; set => canGetTurnByCountDotsDestroyed = value; }
    public bool GetTurnByFiveMatches { get => canGetTurnByFiveMatches; set => canGetTurnByFiveMatches = value; }
    public int CountDotsDestroyedInTurn { get => countDotsDestroyedInTurn; set => countDotsDestroyedInTurn = value; }

    #endregion
    private void Start() {
        this.currentTurn = GameTurn.none;
        this.countTurn = 1;
        this.countCombos = 0;
        this.countDotsDestroyedInTurn = 0;
    }

    private void Update() {
        this.turnTxt.text = this.currentTurn.ToString();
        if(TurnController.Instance.GetTurnByCountDotsDestroyed && TurnController.Instance.CountDotsDestroyedInTurn >= 40) {
            TurnController.Instance.CountTurn++;
            TurnController.Instance.GetTurnByCountDotsDestroyed = false;
            StartCoroutine(TurnController.Instance.DisplayGetTurn());
        }
        SetTextPosition();
    }


    public void SetTextPosition() {
        if(this.currentTurn == GameTurn.playerTurn) {
            this.countCombosTxt.transform.position = this.playerCountCombosPos.position;
            this.getTurnTxt.transform.position = this.playerGetTurnPos.position;
        }
        else if(this.currentTurn == GameTurn.enemyTurn) {
            this.countCombosTxt.transform.position = this.enemyCountCombosPos.position;
            this.getTurnTxt.transform.position = this.enemyGetTurnPos.position;
        }
    }

    public IEnumerator DisplayNextTurnTxt() {
        this.countTurnTxt.transform.parent.gameObject.SetActive(true);

        if(this.countTurn <= 0) {
            if(this.currentTurn == GameTurn.playerTurn) {
                this.countTurnTxt.text = "Lượt đối thủ";
            }
            else
                this.countTurnTxt.text = "Lượt của bạn";
        }
        else {

            this.countTurnTxt.text = "Còn " + this.CountTurn.ToString() + " lượt";
        }

        yield return new WaitForSeconds(0.7f);
        this.countTurnTxt.transform.parent.gameObject.SetActive(false);
    }

    public IEnumerator DisplayCountCombos() {
        this.countCombosTxt.gameObject.SetActive(true);
        this.countCombosTxt.text = "X" + this.countCombos.ToString();

        yield return new WaitForSeconds(0.5f);
        this.countCombosTxt.gameObject.SetActive(false);
    }

    public IEnumerator DisplayGetTurn() {
        if(this.getTurnTxt.gameObject.activeSelf)
            yield return new WaitForSeconds(0.6f);

        this.getTurnTxt.gameObject.SetActive(true);
        this.getTurnTxt.text = "+1";

        yield return new WaitForSeconds(0.5f);
        this.getTurnTxt.gameObject.SetActive(false);

    }


    public void SetNewTurn() {
        this.countTurn--;
        if(this.countTurn <= 0) {
            StartCoroutine(DisplayNextTurnTxt());
            this.countTurn = 1;
            SwitchTurn();
            NewTurn();
        }
        else {
            StartCoroutine(DisplayNextTurnTxt());
            NewTurn();
        }
    }

    public void SwitchTurn() {
        if(this.currentTurn == GameTurn.playerTurn)
            this.currentTurn = GameTurn.enemyTurn;
        else
            this.currentTurn = GameTurn.playerTurn;
    }
    public void NewTurn() {
        this.countCombos = 0;
        this.countDotsDestroyedInTurn = 0;
        this.canGetTurnByCombos = true;
        this.canGetTurnByCountDotsDestroyed = true;
        this.canGetTurnByFiveMatches = true;
        FindObjectOfType<EnemyAi>().TurnOnAutoFind();
    }
    public bool IsTurnOfPlayer() {
        if(this.currentTurn == GameTurn.playerTurn)
            return true;
        return false;
    }
    public bool IsTurnOfEnemy() {
        if(this.currentTurn == GameTurn.enemyTurn)
            return true;
        return false;
    }


}

public enum GameTurn {
    none,
    playerTurn,
    enemyTurn
}