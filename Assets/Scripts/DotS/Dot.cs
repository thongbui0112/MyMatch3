using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour {
    [SerializeField] int score;
    [SerializeField] string dotName;
    [SerializeField] string dotType;
    [SerializeField] AllDotController allDot;

    [SerializeField] BoxCollider2D col;
    private void Start() {
        this.Col = GetComponent<BoxCollider2D>();
        this.allDot = FindObjectOfType<AllDotController>();
    }

    public int Score { get => score; set => score = value; }
    public string Name { get => name; set => name = value; }
    public string DotType { get => dotType; set => dotType = value; }
    public BoxCollider2D Col { get => col; set => col = value; }

    private void OnDestroy() {
        if(this) {
            if(GameStateController.Instance.CurrentState != GameStates.None)
                UpdateScore();
            if(this.dotName.Contains("big")) {
                DestroyRelatedDots();
            }
            this.allDot.SpawnDestroyEffect(this);
        }
        TurnController.Instance.CountDotsDestroyedInTurn++;

    }



    private void UpdateScore() {
        if(this.dotType == "blood")
            ScoreController.Instance.BloodScore += this.score;
        if(this.dotType == "power")
            ScoreController.Instance.PowerScore += this.score;
        if(this.dotType == "energy")
            ScoreController.Instance.EnergyScore += this.score;
        if(this.dotType == "defense")
            ScoreController.Instance.DefenseScore += this.score;
        if(this.dotType == "attack")
            ScoreController.Instance.AttackScore += this.score;
    }

    private void DestroyRelatedDots() {
        DotInteraction dotIn = GetComponent<DotInteraction>();

        int col = dotIn.Column;
        int row = dotIn.Row;

        int width = this.allDot.Width;
        int height = this.allDot.Height;
        for(int i = -1; i <= 1; i++) {
            for(int j = -1; j <= 1; j++) {
                if(col + i >= 0 && col + i < width && row + j >= 0 && row + j < height) {
                    if(allDot.AllDots[col + i, row + j]) {
                        Destroy(this.allDot.AllDots[col + i, row + j]);
                    }
                }
            }
        }
    }
}
