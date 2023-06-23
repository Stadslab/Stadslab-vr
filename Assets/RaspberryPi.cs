using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using Newtonsoft.Json;

public class RaspberryPi : MonoBehaviour {
    public Text readText;

    static readonly string raspBerryURL = "http://145.24.238.10:8000/";
    
    public void ledChangeMethod() {
        StartCoroutine(ledChange());
    }

    IEnumerator ledChange() {
        string ledURL = raspBerryURL;
        WWW www = new WWW(ledURL);
        yield return www;

        readText.text = www.text;
    }
}

