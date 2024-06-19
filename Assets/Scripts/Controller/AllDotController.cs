using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllDotController : MonoBehaviour {
    #region declare
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] GameObject[] dotsPrefab = new GameObject[10];
    [SerializeField] Transform dotParent;
    [SerializeField] int offset;

    private GameObject[,] allDots;
    private GameObject[,] allTiles;
    [SerializeField] GameObject tilePrefabs;

    [SerializeField] GameObject[] destroyEffect;

    private bool switchTurn;

    #endregion
    #region public

    public GameObject[,] AllDots { get => allDots; set => allDots = value; }
    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }
    public GameObject[,] AllTiles { get => allTiles; set => allTiles = value; }
    public bool SwitchTurn { get => switchTurn; set => switchTurn = value; }
    public GameObject[] DestroyEffect { get => destroyEffect; set => destroyEffect = value; }

    #endregion

    private void Start() {
        GetDotsPrefabs();
        this.AllDots = new GameObject[this.Width, this.Height];
        this.AllTiles = new GameObject[this.Width, this.Height];
        StartCoroutine(CreatBoard());

    }

    private void GetDotsPrefabs() {
        for(int i = 0; i < this.dotParent.childCount; i++) {
            this.dotsPrefab[i] = this.dotParent.GetChild(i).gameObject;
        }
    }
    private IEnumerator CreatBoard() {
        yield return new WaitForSeconds(1);
        for(int i = 0; i < Width; i++) {
            for(int j = 0; j < Height; j++) {
                Vector2 tilePosition = new Vector2(i, j);
                GameObject tile = Instantiate(this.tilePrefabs, tilePosition, Quaternion.identity);
                tile.transform.parent = this.transform;
                tile.name = "(" + i + "," + j + ")";
                this.AllTiles[i, j] = tile;
            }
        }

        yield return new WaitForSeconds(1);
        for(int i = 0; i < this.width; i++) {
            yield return null;
            for(int j = 0; j < this.height; j++) {
                yield return null;
                for(; ; ) {
                    int dotToUse = DotToUse();

                    string dotTag = this.dotsPrefab[dotToUse].tag;
                    string downDotTag = j - 1 >= 0 ? this.allDots[i, j - 1].tag : string.Empty;
                    string leftDotTag = i - 1 >= 0 ? this.allDots[i - 1, j].tag : string.Empty;

                    if(dotTag == downDotTag || dotTag == leftDotTag)
                        continue;

                    else {
                        Vector2 dotPosition = (Vector2)this.allTiles[i, j].transform.position + Vector2.up * this.offset;
                        GameObject dot = Instantiate(this.dotsPrefab[dotToUse], dotPosition, Quaternion.identity);
                        dot.transform.parent = this.transform;
                        dot.transform.name = "(" + i + "," + j + ")";
                        dot.GetComponent<DotInteraction>().Column = i;
                        dot.GetComponent<DotInteraction>().Row = j;
                        this.AllDots[i, j] = dot.gameObject;
                        dot.SetActive(true);
                        break;
                    }

                }
            }
        }
          StartCoroutine(DestroyMatches());
    }
    private int DotToUse() {
        int dotLocation;
        int randomNumber = UnityEngine.Random.Range(1, 100);
        if(randomNumber >= 1 && randomNumber <= 17)
            dotLocation = 0;
        else if(randomNumber > 17 && randomNumber <= 38)
            dotLocation = 1;
        else if(randomNumber > 38 && randomNumber <= 55)
            dotLocation = 2;
        else if(randomNumber > 55 && randomNumber <= 72)
            dotLocation = 3;
        else if(randomNumber > 72 && randomNumber <= 95)
            dotLocation = 4;
        else if(randomNumber > 95 && randomNumber <= 96)
            dotLocation = 5;
        else if(randomNumber > 96 && randomNumber <= 97)
            dotLocation = 6;
        else if(randomNumber > 97 && randomNumber <= 98)
            dotLocation = 7;
        else if(randomNumber > 98 && randomNumber <= 99)
            dotLocation = 8;
        else
            dotLocation = 9;

        return dotLocation;
    }


    public bool IsDotRepeated(List<string> tags, string tag) {
        foreach(string s in tags) {
            if(s == tag)
                return true;
        }
        return false;
    }


    private void DestroyMatchesAt(int column, int row) {
        if(this.allDots[column, row].GetComponent<DotInteraction>().IsMatched) {
            Destroy(this.allDots[column, row]);
            this.allDots[column, row] = null;
        }
    }

    public void SpawnDestroyEffect(Dot dot) {
        GameObject destroyEffect = Instantiate(this.DestroyEffect[GetDestroyEffectIndex(dot.gameObject)], dot.transform.position, Quaternion.identity);
        destroyEffect.SetActive(true);
        Destroy(destroyEffect, 0.4f);
    }

    public int GetDestroyEffectIndex(GameObject dot) {
        int index = 0;
        switch(dot.tag) {
            case "power":
                index = 0;
                break;
            case "blood":
                index = 1;
                break;
            case "energy":
                index = 2;
                break;
            case "defense":
                index = 3;
                break;
            case "attack":
                index = 4;
                break;
            default:
                break;
        }
        return index;
    }

    public IEnumerator DestroyMatches() {
        yield return new WaitForSeconds(0.05f);
        for(int i = 0; i < this.width; i++) {
            for(int j = 0; j < this.height; j++) {
                if(this.allDots[i, j] != null)
                    DestroyMatchesAt(i, j);
            }
        }
        StartCoroutine(DecreaseRowCo());

    }

    private IEnumerator DecreaseRowCo() {
        yield return new WaitForSeconds(0.2f);
        int nullCount = 0;

        for(int i = 0; i < this.width; i++) {
            for(int j = 0; j < this.height; j++) {
                if(this.allDots[i, j] == null)
                    nullCount++;
                else if(nullCount > 0) {
                    allDots[i, j].GetComponent<DotInteraction>().Row -= nullCount;
                    allDots[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return null;
        StartCoroutine(FillBoardDot());
    }

    private void RefillBoard() {
        for(int i = 0; i < this.width; i++) {
            for(int j = 0; j < this.height; j++) {
                if(this.allDots[i, j] == null) {
                    Vector2 dotPosition = new Vector2(i, j + this.offset);
                    int dotToUse = DotToUse();
                    GameObject dot = Instantiate(this.dotsPrefab[dotToUse], dotPosition, Quaternion.identity);
                    allDots[i, j] = dot;

                    dot.SetActive(true);
                    dot.GetComponent<DotInteraction>().Column = i;
                    dot.GetComponent<DotInteraction>().Row = j;
                    dot.transform.parent = this.transform;
                }
            }
        }
    }

    private bool MatchesOnBoard() {
        for(int i = 0; i < this.width; i++) {
            for(int j = 0; j < this.height; j++) {
                if(this.allDots[i, j] != null) {
                    if(this.allDots[i, j].GetComponent<DotInteraction>().IsMatched)
                        return true;
                }
            }
        }
        return false;
    }

    public IEnumerator FillBoardDot() {
        RefillBoard();
        yield return new WaitForSeconds(0.55f);

        if(MatchesOnBoard()) {
            StartCoroutine(DestroyMatches());
            TurnController.Instance.CountCombos++;
            StartCoroutine(TurnController.Instance.DisplayCountCombos());
            if(TurnController.Instance.CountCombos >= 3 && TurnController.Instance.GetTurnByCombos) {
                TurnController.Instance.CountTurn++;
                TurnController.Instance.GetTurnByCombos = false;
                StartCoroutine(TurnController.Instance.DisplayGetTurn());
            }
        }
        else {
            if(GameStateController.Instance.CurrentState == GameStates.None) {
                GameStateController.Instance.CurrentState = GameStates.Swipe;
                TurnController.Instance.CurrentTurn = GameTurn.playerTurn;
                TurnController.Instance.NewTurn();
            }

            if(GameStateController.Instance.CurrentState == GameStates.FillingDots) {

                ScoreController.Instance.UpdateScore();

                if(GameStateController.Instance.CurrentState == GameStates.FillingDots) {

                    yield return new WaitForSeconds(0.1f);
                    GameStateController.Instance.CurrentState = GameStates.Finish;
                }
            }
        }

    }

}
