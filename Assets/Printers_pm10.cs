using System.Collections;
// using System.Collections.IEnumerator;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Networking;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Printers_pm10 : MonoBehaviour
{
    public ParticleSystem ps;

    static readonly string ambinodesURL = "https://dashboard.cphsense.com/api/v2/";

    static string authURL = ambinodesURL + "auth/new";
    static string device2LatestURL = ambinodesURL + "devices/0123A8032A74EA24FF/latest";

    private static double sensorValuepm10;

    private float time = 0.0f;
    private float interpolationPeriod = 5.0f;

    public string access_token, refresh_token;
    public static int pm10Device2, pm2_5Device2;

    Dictionary<string, object> dataDict;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        StartCoroutine(authenticateMethod());
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if(time >= interpolationPeriod) {
            time = 0.0f;
            StartCoroutine(device2Latest());
        }
        
        var col = ps.colorOverLifetime;
        col.enabled = true;
        Gradient grad = new Gradient();

        sensorValuepm10 = pm10Device2;

        Debug.Log("Printers_pm10 " + sensorValuepm10);

        if(sensorValuepm10 <= 4) {  //verander deze waarde als je de grenzen voor de kleurveranderingen wilt aanpassen
            grad.SetKeys(new GradientColorKey[] {new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.green, 1.0f)}, 
                new GradientAlphaKey[] {new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 0.0f)});
        } else if(sensorValuepm10 > 4 && sensorValuepm10 < 6) {      //verander deze waarde als je de grenzen voor de kleurveranderingen wilt aanpassen
            grad.SetKeys(new GradientColorKey[] {new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.yellow, 1.0f)}, 
                new GradientAlphaKey[] {new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 0.0f)});

        } else if(sensorValuepm10 > 6) {  //verander deze waarde als je de grenzen voor de kleurveranderingen wilt aanpassen
            grad.SetKeys(new GradientColorKey[] {new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.red, 1.0f)}, 
                new GradientAlphaKey[] {new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 0.0f)});
        }
        
        col.color = grad;
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
        dataDict = dict["data"];

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
