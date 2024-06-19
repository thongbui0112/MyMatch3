using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyAi : MonoBehaviour {
    AllDotController allDotController;
    string[,] allTags;

    List<MoveDot> allMoves;

    bool isFind1;

    private void Awake() {
        this.allDotController = FindObjectOfType<AllDotController>();
        this.allTags = new string[this.allDotController.Width, this.allDotController.Height];
        this.allMoves = new List<MoveDot>();
        this.isFind1 = true;
    }

    private void Update() {
        if(TurnController.Instance.IsTurnOfEnemy() && this.isFind1) {
            this.allMoves.Clear();
            GetAllDots();
            FindAllMatchesByWidth();
            FindAllMatchesByHeight();

            StartCoroutine(FindRandomMove());

            this.isFind1 = false;
        }

    }

    public IEnumerator FindRandomMove() {
        yield return new WaitForSeconds(1.5f);

        int randomMove = Random.Range(0,this.allMoves.Count);

        MoveDot move = this.allMoves[randomMove];
        GameObject originDot = this.allDotController.AllDots[move.OriginCol,move.OriginRow];
        GameObject targetDot = this.allDotController.AllDots[move.TargetCol, move.TargetRow];

        originDot.GetComponent<DotInteraction>().SetDot(move.TargetCol, move.TargetRow);
        targetDot.GetComponent<DotInteraction>().SetDot(move.OriginCol, move.OriginRow);

        GameStateController.Instance.CurrentState = GameStates.FillingDots;
        StartCoroutine(StartDestroyMatches());
    }
    public IEnumerator StartDestroyMatches() {
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(this.allDotController.DestroyMatches());
        
    }

    public void GetAllDots() {
        for(int i = 0; i < this.allDotController.Width; i++) {
            for(int j = 0; j < this.allDotController.Height; j++) {
                this.allTags[i, j] = this.allDotController.AllDots[i, j].tag;
            }
        }
    }

    public void FindAllMatchesByHeight() {
        for(int i = 0; i < this.allDotController.Width; i++) {
            for(int j = 0; j < this.allDotController.Height - 1; j++) {
                string originDot = this.allTags[i, j];
                string targetDot = this.allTags[i, j + 1];

                bool isOriginMatched = OriginDotFindMatchesByHeight(i, j);
                bool isTargetMatched = TargetDotFindMatchesByHeight(i, j + 1);

                if(isOriginMatched || isTargetMatched) {
                    Debug.Log("Up " +i + " , " + j);
                    MoveDot newMove = new MoveDot(i, j, i, j + 1);
                    this.allMoves.Add(newMove);
                }
            }
        }
    }

    public void FindAllMatchesByWidth() {
        for(int i = 0; i < this.allDotController.Width - 1; i++) {
            for(int j = 0; j < this.allDotController.Width; j++) {
                string originDot = this.allTags[i, j];
                string targetDot = this.allTags[i + 1, j];

                bool isOriginMatched = OriginDotFindMatchesByWidth(i, j);
                bool isTargetMatched = TargetDotFindMatchesByWidth(i + 1, j);

                if(isOriginMatched || isTargetMatched) {
                    Debug.Log("Right "+i + " , " + j);
                    MoveDot newMove = new MoveDot(i, j, i + 1, j);
                    this.allMoves.Add(newMove);
                }
            }
        }
    }
    #region Find Matches by Width
    public bool OriginDotFindMatchesByWidth(int i, int j) {

        bool a = FindMatchesByWidthRight(i, j);
        bool b = FindMatchesByWidthVertical(i, j, 1);

        return IsMatched(a, b);
    }
    public bool TargetDotFindMatchesByWidth(int i, int j) {

        bool a = FindMatchesByWidthLeft(i, j);
        bool b = FindMatchesByWidthVertical(i, j, -1);

        return IsMatched(a, b);
    }

    public bool FindMatchesByWidthRight(int i, int j) {
        int count = 1;
        string dot = this.allTags[i, j];

        string dotRight1 = i + 2 < this.allDotController.Width ? this.allTags[i + 2, j] : string.Empty;
        string dotRight2 = i + 3 < this.allDotController.Width ? this.allTags[i + 3, j] : string.Empty;

        if(dot == dotRight1) {
            count++;
            if(dot == dotRight2)
                count++;
        }

        return IsMatch3(count);
    }

    public bool FindMatchesByWidthLeft(int i, int j) {
        int count = 1;
        string dot = this.allTags[i, j];

        string dotLeft1 = i - 2 >= 0 ? this.allTags[i - 2, j] : string.Empty;
        string dotLeft2 = i - 3 >= 0 ? this.allTags[i - 3, j] : string.Empty;

        if(dot == dotLeft1) {
            count++;
            if(dot == dotLeft2)
                count++;
        }

        return IsMatch3(count);
    }

    public bool FindMatchesByWidthVertical(int i, int j, int k) {
        int count = 1;

        string dot = this.allTags[i, j];
        string dotDown1 = j - 1 >= 0 ? this.allTags[i + k, j - 1] : string.Empty;
        string dotDown2 = j - 2 >= 0 ? this.allTags[i + k, j - 2] : string.Empty;
        string dotUp1 = j + 1 < this.allDotController.Height ? this.allTags[i + k, j + 1] : string.Empty;
        string dotUp2 = j + 2 < this.allDotController.Height ? this.allTags[i + k, j + 2] : string.Empty;

        if(dot == dotDown1) {
            count++;
            if(dot == dotDown2)
                count++;
        }
        if(dot == dotUp1) {
            count++;
            if(dot == dotUp2)
                count++;
        }

        return IsMatch3(count);
    }
    #endregion

    #region Find Matches by Height 
    public bool OriginDotFindMatchesByHeight(int i, int j) {

        bool a = FindMatchesByHeightUp(i, j);
        bool b = FindMatchesByHeightHorizontal(i, j, 1);
      
        return IsMatched(a, b);
    }

    public bool TargetDotFindMatchesByHeight(int i, int j) {

        bool a = FindMatchesByHeightDown(i, j);
        bool b = FindMatchesByHeightHorizontal(i, j, -1);

        return IsMatched(a, b);
    }

    // use only for origin Dot by Height

    public bool FindMatchesByHeightUp(int i, int j) {
        int count = 1;
        string dot = this.allTags[i, j];
        string dotUp1 = j + 2 < this.allDotController.Height ? this.allTags[i, j + 2] : string.Empty;
        string dotUp2 = j + 3 < this.allDotController.Height ? this.allTags[i, j + 3] : string.Empty;

        if(dot == dotUp1) {
            count++;
            if(dot == dotUp2)
                count++;
        }

        return IsMatch3(count);
    }

    // use only for targetDot by Height
    public bool FindMatchesByHeightDown(int i, int j) {
        int count = 1;
        string dot = this.allTags[i, j];
        string dotDown1 = j - 2 >= 0 ? this.allTags[i, j - 2] : string.Empty;
        string dotDown2 = j - 3 >= 0 ? this.allTags[i, j - 3] : string.Empty;

        if(dot == dotDown1) {
            count++;
            if(dot == dotDown2)
                count++;
        }

        return IsMatch3(count);
    }


    // k : originDot : k = 1, targetDot : k=-1;
    public bool FindMatchesByHeightHorizontal(int i, int j, int k) {
        int count = 1;
        string dot = this.allTags[i, j];
        string dotLeft1 = i - 1 >= 0 ? this.allTags[i - 1, j + k] : string.Empty;
        string dotLeft2 = i - 2 >= 0 ? this.allTags[i - 2, j + k] : string.Empty;
        string dotLeft3 = i + 1 < this.allDotController.Width ? this.allTags[i + 1, j + k] : string.Empty;
        string dotLeft4 = i + 2 < this.allDotController.Width ? this.allTags[i + 2, j + k] : string.Empty;

        if(dot == dotLeft1) {
            count++;
            if(dot == dotLeft2)
                count++;
        }
        if(dot == dotLeft3) {
            count++;
            if(dot == dotLeft4)
                count++;
        }
        return IsMatch3(count);

    }
    #endregion

    public bool IsMatch3(int count) {
        if(count >= 3)
            return true;
        return false;
    }

    public bool IsMatched(bool a, bool b) {
        if(a || b)
            return  true;
        else
            return false;
    }
    public void TurnOnAutoFind() {
        this.isFind1 = true;
    }
}





