using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Description : MonoBehaviour {

    [SerializeField] GameObject descriptionText;
    bool interactable;
    // Start is called before the first frame update
    IEnumerator Start() {
        yield return null;
        this.descriptionText = transform.GetChild(0).gameObject;
        Useable();
    }

    // Update is called once per frame
    void Update() {
        // 
    }

    private void OnEnable() {
        Useable();
        if(this.descriptionText)
            this.descriptionText.SetActive(false);

    }

    private void OnMouseOver() {
        this.descriptionText.SetActive(true);
    }


    private void OnMouseExit() {
        this.descriptionText.SetActive(false);
    }

    public void Useable() {
        if(TurnController.Instance.IsTurnOfPlayer()) {
            ScoreSystem playerScore = GameObject.Find("PlayerScore").GetComponent<ScoreSystem>();
            int eneryUsed = transform.parent.GetComponent<Ability>().EnergyUsed;
            if(playerScore.CurrentPowerScore >= eneryUsed) {
                transform.GetChild(1).gameObject.SetActive(false);
            }
            else if(playerScore.CurrentPowerScore < eneryUsed) {
                transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        else if(TurnController.Instance.IsTurnOfEnemy()) {
            ScoreSystem enemyScore = GameObject.Find("EnemyScore").GetComponent<ScoreSystem>();
            int eneryUsed = transform.parent.GetComponent<Ability>().EnergyUsed;
            if(enemyScore.CurrentPowerScore >= eneryUsed) {
                transform.GetChild(1).gameObject.SetActive(false);
            }
            else if(enemyScore.CurrentPowerScore < eneryUsed) {
                transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

}
