using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class DotInteraction : MonoBehaviour {

    #region  Variable
    [Header("Board Variables ")]
    AllDotController allDot;
    [SerializeField] int column, row;
    [SerializeField] int targetX, targetY;
    [SerializeField] bool isMatched;
    private int preColumn, preRow;
    private bool isStatic;
    private Color colorOfDot;


    [SerializeField] GameObject targetDot;
    Vector2 mouseDownPos, mouseUpPos;
    float swipeLengthCanMove;
    Vector2 tempPosition;
    SpriteRenderer spriteRenderer;

    GamePlay gamePlay;
    #endregion

    #region     public
    public int Column { get => column; set => column = value; }
    public int Row { get => row; set => row = value; }
    public bool IsMatched { get => isMatched; set => isMatched = value; }
    public bool IsStatic { get => isStatic; set => isStatic = value; }
    public Color ColorOfDot { get => colorOfDot; set => colorOfDot = value; }
    #endregion


    private void Awake() {
        this.gamePlay = FindObjectOfType<GamePlay>();
        allDot = FindObjectOfType<AllDotController>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();

    }
    private void Start() {
        this.swipeLengthCanMove = 80f;
        this.ColorOfDot = ColorController();
    }

    private void Update() {
        this.targetX = this.column;
        this.targetY = this.row;

        if(AllDotState.Instance.IsAllDotStatic)
            FindMatches();

        MoveDot();
    }
    private void LateUpdate() {
        if(this.isMatched)
            DisplayDotMatched();

    }
    public void DisplayDotMatched() {
        // yield return new WaitForSeconds(0);
        this.spriteRenderer.color = this.ColorOfDot;

    }
    private IEnumerator CheckTargetDot() {
        if(targetDot == null)
            GameStateController.Instance.CurrentState = GameStates.Swipe;
        yield return new WaitForSeconds(0.3f);
        if(targetDot != null) {
            if(!this.isMatched && !this.targetDot.GetComponent<DotInteraction>().IsMatched) {
                this.targetDot.GetComponent<DotInteraction>().Row = this.row;
                this.targetDot.GetComponent<DotInteraction>().Column = this.column;

                this.column = this.preColumn;
                this.row = this.preRow;
                GameStateController.Instance.CurrentState = GameStates.Swipe;
            }
            else {
                GameStateController.Instance.CurrentState = GameStates.FillingDots;
                StartCoroutine(this.allDot.DestroyMatches());
            }

            targetDot = null;
        }

    }

    private void MoveDot() {
        if(Mathf.Abs(this.targetX - transform.position.x) > 0.1f) {
            this.tempPosition = new Vector2(this.targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, this.tempPosition, 9 * Time.deltaTime);

            if(this.allDot.AllDots[this.column, this.row] != this.gameObject)
                this.allDot.AllDots[this.column, this.row] = this.gameObject;
        }
        else {
            this.tempPosition = new Vector2(this.targetX, transform.position.y);
            transform.position = this.tempPosition;
        }


        if(Mathf.Abs(this.targetY - transform.position.y) > 0.1f) {
            this.tempPosition = new Vector2(transform.position.x, this.targetY);
            transform.position = Vector2.Lerp(transform.position, this.tempPosition, 9 * Time.deltaTime);

            if(this.allDot.AllDots[this.column, this.row] != this.gameObject)
                this.allDot.AllDots[this.column, this.row] = this.gameObject;
        }
        else {
            this.tempPosition = new Vector2(transform.position.x, this.targetY);
            transform.position = this.tempPosition;
        }

        Vector2 targetPosition = new Vector2(this.targetX, this.targetY);
        if(Vector2.Distance(targetPosition, transform.position) == 0f) {
            this.isStatic = true;
        }
        else
            this.isStatic = false;
    }


    private void FindMatches() {
        if(this.column > 0 && this.column < this.allDot.Width - 1) {
            GameObject leftDot = this.allDot.AllDots[this.column - 1, this.row];
            GameObject rightDot = this.allDot.AllDots[this.column + 1, this.row];
            if(leftDot && rightDot && leftDot != rightDot && leftDot != this.gameObject && rightDot != gameObject) {
                if(leftDot.tag == rightDot.tag && gameObject.tag == leftDot.tag) {
                    leftDot.GetComponent<DotInteraction>().IsMatched = true;
                    rightDot.GetComponent<DotInteraction>().IsMatched = true;
                    this.isMatched = true;
                }

            }
        }
        if(this.row > 0 && this.row < this.allDot.Height - 1) {
            GameObject downDot = this.allDot.AllDots[this.column, this.row - 1];
            GameObject upDot = this.allDot.AllDots[this.column, this.row + 1];
            if(upDot && downDot && upDot != downDot && upDot != this.gameObject && downDot != gameObject) {
                if(downDot.tag == upDot.tag && gameObject.tag == downDot.tag) {
                    downDot.GetComponent<DotInteraction>().IsMatched = true;
                    upDot.GetComponent<DotInteraction>().IsMatched = true;
                    this.isMatched = true;
                }

            }
        }
        this.gamePlay.Find5Matches(this.gameObject);
    }

    #region 
    private void DetectTargetDot(string swipeDirection) {
        if(swipeDirection == "right" && this.column < this.allDot.Width - 1) {
            this.targetDot = this.allDot.AllDots[this.column + 1, this.row];
            this.targetDot.GetComponent<DotInteraction>().column -= 1;
            SetPreviousDot();
            this.column += 1;
        }
        else if(swipeDirection == "left" && this.column > 0) {
            this.targetDot = this.allDot.AllDots[this.column - 1, this.row];
            this.targetDot.GetComponent<DotInteraction>().column += 1;
            SetPreviousDot();
            this.column -= 1;
        }
        else if(swipeDirection == "up" && this.row < this.allDot.Height - 1) {
            this.targetDot = this.allDot.AllDots[this.column, this.row + 1];
            this.targetDot.GetComponent<DotInteraction>().row -= 1;
            SetPreviousDot();
            this.row += 1;
        }
        else if(swipeDirection == "down" && this.row > 0) {
            this.targetDot = this.allDot.AllDots[this.column, this.row - 1];
            this.targetDot.GetComponent<DotInteraction>().row += 1;
            SetPreviousDot();
            this.row -= 1;
        }

        StartCoroutine(CheckTargetDot());
    }
    private string SwipeDirection() {
        string swipeDirection = "invalid";
        if(Vector2.Distance(this.mouseUpPos, this.mouseDownPos) < this.swipeLengthCanMove) {
            GameStateController.Instance.CurrentState = GameStates.Swipe;
            return swipeDirection;
        }

        Vector2 mouseVector = mouseUpPos - mouseDownPos;
        float angleMouseVector = Vector2.Angle(mouseVector, Vector2.right);

        if(angleMouseVector <= 45)
            swipeDirection = "right";
        else if(angleMouseVector >= 135)
            swipeDirection = "left";
        else {
            if(mouseVector.y >= 0)
                swipeDirection = "up";
            else
                swipeDirection = "down";
        }
        return swipeDirection;
    }
    public Vector2 GetMousePosition() {
        Vector2 direction = Input.mousePosition;
        return direction;
    }

    private void SetPreviousDot() {
        this.preColumn = this.column;
        this.preRow = this.row;
    }
    private Color ColorController() {
        if(gameObject.CompareTag("energy"))
            return Color.green;
        if(gameObject.CompareTag("blood"))
            return Color.red;
        if(gameObject.CompareTag("power"))
            return Color.blue;
        if(gameObject.CompareTag("defense"))
            return Color.grey;
        if(gameObject.CompareTag("attack"))
            return Color.cyan;
        return Color.clear;

    }
    #endregion 

    #region Input Mouse 
    private void OnMouseEnter() {
        if(!TurnController.Instance.IsTurnOfPlayer())
            return;

        if(!Input.GetMouseButton(0) && GameStateController.Instance.CurrentState == GameStates.Swipe && !AllDotState.Instance.IsPause)
            transform.localScale = Vector3.one * 1.2f;
    }

    private void OnMouseOver() {
        if(!TurnController.Instance.IsTurnOfPlayer())
            return;

        if(!Input.GetMouseButton(0) && GameStateController.Instance.CurrentState == GameStates.Swipe && !AllDotState.Instance.IsPause)
            transform.localScale = Vector3.one * 1.2f;
    }

    private void OnMouseExit() {
        if(!TurnController.Instance.IsTurnOfPlayer())
            return;

        if(!Input.GetMouseButton(0) && GameStateController.Instance.CurrentState == GameStates.Swipe && !AllDotState.Instance.IsPause)
            transform.localScale = Vector3.one;
    }
    private void OnMouseDown() {
        if(!TurnController.Instance.IsTurnOfPlayer())
            return;
        if(GameStateController.Instance.CurrentState != GameStates.Swipe || AllDotState.Instance.IsPause)
            return;

        this.mouseDownPos = GetMousePosition();
        transform.localScale = Vector3.one * 1.4f;
    }

    private void OnMouseUp() {
        if(!TurnController.Instance.IsTurnOfPlayer())
            return;
        if(GameStateController.Instance.CurrentState != GameStates.Swipe || AllDotState.Instance.IsPause)
            return;

        GameStateController.Instance.CurrentState = GameStates.CheckingDots;
        this.mouseUpPos = GetMousePosition();
        this.transform.localScale = Vector3.one * 1f;

        string swipeDirection = SwipeDirection();
        DetectTargetDot(swipeDirection);
    }
    #endregion

    public void SetDot(int col, int row) {
        this.column = col;
        this.row = row;
    }
}
