using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour {

    private static GameStateController instance;
    public static GameStateController Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<GameStateController>();
            }
            return instance;
        }
    }


    [SerializeField] private GameStates currentState;
    public GameStates CurrentState { get => currentState; set => currentState = value; }

    private void Start() {
        this.currentState = GameStates.None;
    }

    private void Update() {
        if(GameStateController.Instance.currentState== GameStates.Finish) {
            this.currentState = GameStates.Swipe;
            ScoreController.Instance.ResetScore();
            TurnController.Instance.SetNewTurn();
        }   
    }


    public bool IsAttacking() {
        if(this.currentState == GameStates.Attacking)
            return true;
        return false;
    }
}

public enum GameStates {
    None,
    Swipe,
    CheckingDots,
    ExcutingAbility,  
    FillingDots,
    Attacking,
    Finish
}
