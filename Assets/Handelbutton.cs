using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using Newtonsoft.Json;

public class HandleButton : MonoBehaviour {
    public Text readText;
    public Text testText;
    public Text authText;

    static readonly string raspBerryURL = "http://145.24.238.10:8000/";
    static readonly string ambinodesURL = "https://dashboard.cphsense.com/api/v2/";

    static string authURL = ambinodesURL + "auth/new";
    static string device1LatestURL = ambinodesURL + "devices/0123A8032A74EA24FF/latest";

    public string access_token, refresh_token;

    void Start() {
        StartCoroutine(authenticateMethod());
    }

    IEnumerator authenticateMethod() {
        Hashtable headers = new Hashtable() {
            {"accept", "application/json"},
            {"Content-Type", "application/json"}
        };
        // headers.Add("accept", "application/json");
        // headers.Add("Content-Type", "application/json");

        string payld = "{\"username\": \"1032591@hr.nl\", \"password\": \"I0y9Hmpgf6C5\"}";

        byte[] payldBytes = System.Text.Encoding.UTF8.GetBytes(payld);

        WWW www = new WWW(authURL, payldBytes, headers);

        yield return www;

        if (www.error != null) {
            Debug.Log("request error: " + www.error);
        } else {
            Debug.Log("request success");
            Debug.Log("returned data" + www.text);
        }

        Debug.Log(www.text.GetType());

        Dictionary<string, string> resultDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(www.text);

        foreach(KeyValuePair<string, string> ele in resultDict) {
            Debug.Log("key: " + ele.Key);
            Debug.Log("Value: " + ele.Value);
        }

        access_token = resultDict["access_token"];
        refresh_token = resultDict["refresh_token"];


        authText.text = access_token;
    }

    public void test() {
        StartCoroutine(device1Latest());
    }

    IEnumerator device1Latest() {
        testText.text = "request send ";

        Hashtable headers = new Hashtable() {
            {"accept", "application/json"},
            {"Authorization", "Bearer " + access_token}
        };

        WWW www = new WWW(device1LatestURL, null, headers);

        yield return www;

        if (www.error != null) {
            Debug.Log("request error: " + www.error);
        } else {
            Debug.Log("request success");
            Debug.Log("returned data" + www.text);
        }

        Dictionary<string, dynamic> resultDict = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(www.text);

        testText.text = www.text;
    }

    public void ledChangeMethod() {
        StartCoroutine(ledChange());
    }

    IEnumerator ledChange() {
        string ledURL = raspBerryURL + "led";
        WWW www = new WWW(ledURL);
        yield return www;

        readText.text = www.text;
    }
}

