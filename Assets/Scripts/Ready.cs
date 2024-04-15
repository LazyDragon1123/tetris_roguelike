using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;


public class Ready : MonoBehaviour {
    public TMP_Text txt;
    private float timer = 0f;
    public delegate void OnReadyComplete();
    public event OnReadyComplete onReadyComplete;

    public void Initialize() {
        txt.text = "Ready";
        gameObject.SetActive(true);
    }
    private void Update() {
        timer += Time.deltaTime;

        if (timer >= 0.5f && timer < 1.2f) {
            txt.text = "Ready.";
        } else if (timer >= 1.2f && timer < 2.0f) {
            txt.text = "Ready..";
        } else if (timer >= 2.0f && timer < 3f) {
            txt.text = "Ready...";
        }
        else if (timer >= 3f && timer < 4.5f) {
            txt.text = "Go!";
        } else if (timer >= 4.5f) {
            txt.text = "";
            gameObject.SetActive(false);
            onReadyComplete?.Invoke();
        }
    }
}
