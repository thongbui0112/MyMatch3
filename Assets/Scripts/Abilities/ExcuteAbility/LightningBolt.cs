using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : MonoBehaviour {
    AllDotController board;
    [SerializeField] List<GameObject> dots = new List<GameObject>();
    List<GameObject> lightningBolt = new List<GameObject>();
    [SerializeField] GameObject boltPref;
    int countRandomDots = 10;

    bool isGetDots;
    Transform targetDot;
    Transform currentDot;
    int countCurrentDot;
    float timeCounter;

    bool isConnected;
    private void Awake() {
        board = FindObjectOfType<AllDotController>();
        this.isGetDots = true;
        this.countCurrentDot = 0;
        this.isConnected = false;
    }


    private void Update() {
        if(this.isGetDots) {
            this.dots = GetRandomDots();
            this.isGetDots = false;
        }
        else {
            LineConnect();
        }
    }

    public List<GameObject> GetRandomDots() {
        List<GameObject> randomDots = new List<GameObject>();
        for(; ; ) {
            int ranCol = Random.Range(0, board.Width);
            int ranRow = Random.Range(0, board.Height);

            GameObject dot = this.board.AllDots[ranCol, ranRow];

            int count = 0;
            foreach(GameObject obj in randomDots) {
                if(obj == dot) {
                    count++;
                    break;
                }
            }

            if(count == 0) {
                randomDots.Add(this.board.AllDots[ranCol, ranRow]);
            }

            if(randomDots.Count == this.countRandomDots)
                break;
        }
        return randomDots;
    }

    public void LineConnect() {
        if(this.targetDot == null&& this.currentDot ==null  && !this.isConnected) {
            this.countCurrentDot = 0;
            this.currentDot = this.dots[this.countCurrentDot].transform;
            this.targetDot = this.dots[countCurrentDot+1].transform;
        }
        else if(!this.isConnected && this.countCurrentDot < this.countRandomDots-1) {
            this.timeCounter += Time.deltaTime;
            if(this.timeCounter >= 0.3f) {
                GameObject lightningBolt = Instantiate(this.boltPref);
                lightningBolt.SetActive(true);
                lightningBolt.transform.GetChild(0).position = this.dots[this.countCurrentDot].transform.position;
                lightningBolt.transform.GetChild(1).position = this.dots[this.countCurrentDot+1].transform.position;
                this.dots[this.countCurrentDot].GetComponent<DotInteraction>().DisplayDotMatched();
                this.dots[this.countCurrentDot +1].GetComponent<DotInteraction>().DisplayDotMatched();
                
                this.lightningBolt.Add(lightningBolt);
                
                this.countCurrentDot++;
                this.timeCounter = 0;
            }
        }
        else if(!this.isConnected && this.countCurrentDot == this.countRandomDots - 1) {
            this.isConnected = true;
            Debug.Log("finishhhh");
            StartCoroutine(ExcuteDestroy());

        }
    }

    private void DestroyRandomDots() {
        foreach(GameObject  obj in this.dots) {
            Destroy(obj);
        }
    }
    private void DestroyLightningBolts() {
        foreach(GameObject obj in this.lightningBolt) {
            Destroy(obj);
        }
    }
    IEnumerator ExcuteDestroy() {
        yield return new WaitForSeconds(0.6f);
        DestroyLightningBolts ();
        DestroyRandomDots();

        yield return null;
        GameStateController.Instance.CurrentState = GameStates.FillingDots;
        StartCoroutine(this.board.DestroyMatches());
        Debug.Log("Destroy");

        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
