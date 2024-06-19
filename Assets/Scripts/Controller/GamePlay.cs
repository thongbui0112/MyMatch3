using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour {
    private AllDotController board;

    private bool is5Matches;
    private int countCombos;
    private void Start() {
        this.board = FindObjectOfType<AllDotController>();
        this.is5Matches = false;
    }

    private void Update() {

    }


    public void Find5Matches(GameObject dot) {
        //Vertical : row change
        DotInteraction _dot = dot.GetComponent<DotInteraction>();
        int column = _dot.Column;
        int row = _dot.Row;
        if(column > 1 && column < this.board.Width - 2) {

            GameObject dotUp = this.board.AllDots[column + 1, row];
            GameObject dotDown = this.board.AllDots[column - 1, row];
            GameObject dotUpUp = this.board.AllDots[column + 2, row];
            GameObject dotDownDown = this.board.AllDots[column - 2, row];

            if(dotUp.tag == dotDown.tag && dotDown.tag == dot.tag && dotUp != dot && dotDown != dot) {
                if(dotUpUp.tag == dotDownDown.tag && dotDownDown.tag == dot.tag) {
                    this.is5Matches = true;
                    AddTurnBy5Matches();
                }
            }

        }

        if(row > 1 && row < this.board.Height - 2) {

            GameObject dotRight = this.board.AllDots[column, row + 1];
            GameObject dotLeft = this.board.AllDots[column, row - 1];
            GameObject dotRightRight = this.board.AllDots[column, row + 2];
            GameObject dotLeftLeft = this.board.AllDots[column, row - 2];

            if(dotRight.tag == dotLeft.tag && dotRight.tag == dot.tag && dotRight != dot && dotLeft != dot) {
                if(dotLeftLeft.tag == dotRightRight.tag && dotLeftLeft.tag == dot.tag) {
                    this.is5Matches = true;
                    AddTurnBy5Matches();
                }
            }

        }
    }

    public void AddTurnBy5Matches() {
        if(TurnController.Instance.GetTurnByFiveMatches) {
            TurnController.Instance.CountTurn++;
            TurnController.Instance.GetTurnByFiveMatches = false;
            StartCoroutine(TurnController.Instance.DisplayGetTurn());
        }
    }
}
