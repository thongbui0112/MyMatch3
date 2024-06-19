 using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AllDotState : MonoBehaviour
{
    private static AllDotState instance;

    public static AllDotState Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<AllDotState>();
            }
            return instance;
        }
    }


    [SerializeField] bool isPause;
    [SerializeField] GameObject pausePanel;

    [SerializeField] private bool isAllDotStatic;
    public bool IsAllDotStatic { get => isAllDotStatic; set => isAllDotStatic = value; }
    public bool IsPause { get => isPause; set => isPause = value; }

    private AllDotController board;
    private void Start() {
        board = FindObjectOfType<AllDotController>();

        this.IsPause = false;
    }

    public void SetCollider(bool state) {
        for(int i=0;i<this.board.Width;i++) {
            for(int j=0;j<this.board.Height;j++) {
                Dot dot = this.board.AllDots[i,j].GetComponent<Dot>();
                dot.Col.enabled = state;
            }
        }
    }

    private void Update() {
        this.isAllDotStatic = GetAllDotStatic();
        this.isPause = pausePanel.activeSelf;
    }
    public bool GetAllDotStatic() {
        for(int i = 0; i < this.board.Width; i++) {
            for(int j=0; j < this.board.Height; j++) {
                if(this.board.AllDots[i,j] == null) 
                    return false; 
                if(!this.board.AllDots[i, j].GetComponent<DotInteraction>().IsStatic)
                    return false;   
            }
        }
        return true;
    }

}
