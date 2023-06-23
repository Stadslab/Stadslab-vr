// #include <windows.h>;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Ambinodes : MonoBehaviour {
    static readonly string ambinodesURL = "https://dashboard.cphsense.com/api/v2/";

    static string authURL = ambinodesURL + "auth/new";
    static string device1LatestURL = ambinodesURL + "devices/0123A8032A74EA58FF/latest";
    static string device2LatestURL = ambinodesURL + "devices/0123A8032A74EA24FF/latest";

    public string access_token, refresh_token;

    public static int pm10Device1, pm2_5Device1;
    public static int pm10Device2, pm2_5Device2;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(authenticateMethod());
    }

    // Update is called once per frame
    void Update() {
        // StartCoroutine(device1Latest());
        // StartCoroutine(device2Latest());
        // Sleep(10000);
    }

    IEnumerator authenticateMethod() {
        Hashtable headers = new Hashtable() {
            {"accept", "application/json"},
            {"Content-Type", "application/json"}
        };

        string payld = "{\"username\": \"1032591@hr.nl\", \"password\": \"I0y9Hmpgf6C5\"}";

        byte[] payldBytes = System.Text.Encoding.UTF8.GetBytes(payld);

        WWW auth = new WWW(authURL, payldBytes, headers);

        yield return auth;

        if (auth.error != null) {
            Debug.Log("request error: " + auth.error);
        } else {
            Debug.Log("request success");
            Debug.Log("returned data" + auth.text);
        }

        Dictionary<string, string> resultDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(auth.text);

        // foreach(KeyValuePair<string, string> ele in resultDict) {
        //     Debug.Log("Key: " + ele.Key);
        //     Debug.Log("Value: " + ele.Value);
        // }

        access_token = resultDict["access_token"];
        refresh_token = resultDict["refresh_token"];
    }

    IEnumerator device1Latest() {
        Hashtable headers = new Hashtable() {
            {"accept", "application/json"},
            {"Authorization", "Bearer " + access_token}
        };

        WWW www = new WWW(device1LatestURL, null, headers);

        yield return www;

        if (www.error != null) {
            Debug.Log("request error: " + www.error);
        } else {
            // Debug.Log("request success");
            // Debug.Log("returned data" + www.text);
        }

        Dictionary<string, Dictionary<string, object>> dict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(www.text);
        Dictionary<string, object> dataDict = dict["data"];

        // Debug.Log(dataDict["sensors"].GetType());

        JArray sensors = (JArray) dataDict["sensors"];

        for(int i = 0; i < sensors.Count; i++) {
            if(sensors[i].Value<string>("key") == "pm10") {
                pm10Device1 = sensors[i].Value<int>("value");
                Debug.Log($"pm10 {pm10Device1}");
            } else if(sensors[i].Value<string>("key") == "pm2_5") {
                pm2_5Device1 = sensors[i].Value<int>("value");
                Debug.Log($"pm2_5 {pm2_5Device1}");
            }
        }
    }

    IEnumerator device2Latest() {
        Hashtable headers = new Hashtable() {
            {"accept", "application/json"},
            {"Authorization", "Bearer " + access_token}
        };

        WWW www = new WWW(device2LatestURL, null, headers);

        yield return www;

        if (www.error != null) {
            Debug.Log("request error: " + www.error);
        } else {
            Debug.Log("request success");
            Debug.Log("returned data" + www.text);
        }

        Dictionary<string, Dictionary<string, object>> dict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(www.text);
        Dictionary<string, object> dataDict = dict["data"];

        Debug.Log(dataDict["sensors"].GetType());

        JArray sensors = (JArray) dataDict["sensors"];

        for(int i = 0; i < sensors.Count; i++) {
            if(sensors[i].Value<string>("key") == "pm10") {
                pm10Device2 = sensors[i].Value<int>("value");
                Debug.Log($"pm10 {pm10Device2}");
            } else if(sensors[i].Value<string>("key") == "pm2_5") {
                pm2_5Device2 = sensors[i].Value<int>("value");
                Debug.Log($"pm2_5 {pm2_5Device2}");
            }
        }
    }
}
