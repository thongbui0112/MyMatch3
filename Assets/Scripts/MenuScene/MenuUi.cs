using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUi : MonoBehaviour {
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject helpButton;
    [SerializeField] GameObject settingButton;
    [SerializeField] GameObject quitButton;

    private void Awake() {
        StartCoroutine(OnMainMenuAppear());

        this.startButton.GetComponent<Button>().onClick.AddListener(StartButton);
    }

    private void StartButton() {
        StartCoroutine(OnMainMenuDisAppear());
    }

    public IEnumerator OnMainMenuAppear() {
        yield return new WaitForSeconds(1f);
        this.startButton.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        this.helpButton.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        this.settingButton.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        this.quitButton.SetActive(true);
    }

    public IEnumerator OnMainMenuDisAppear() {
        yield return new WaitForSeconds(0.05f);
        this.quitButton.GetComponent<Animator>().SetBool("disappear", true);
        yield return new WaitForSeconds(0.05f);
        this.settingButton.GetComponent<Animator>().SetBool("disappear", true);
        yield return new WaitForSeconds(0.05f);
        this.helpButton.GetComponent<Animator>().SetBool("disappear", true);
        yield return new WaitForSeconds(0.05f);
        this.startButton.GetComponent<Animator>().SetBool("disappear", true);
    }

}
