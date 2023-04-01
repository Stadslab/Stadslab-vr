using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class easterScript : MonoBehaviour
{
    public List<GameObject> Eggs = new List<GameObject>();
    public TMP_Text textUI;
    public TextMesh textMesh;
    public GameObject prefab;
    public Transform SpawnPosition;
    public int maxEggs;
    // Start is called before the first frame update
    void Start()
    {
        

    }

  

    public void CreateEgg(GameObject newEgg)
    {
        Eggs.Add(newEgg);
        maxEggs ++;
        UpdateUI();
    }

    public void CompleteEgg(GameObject CompletedEgg)
    {
        GameObject eggToRemove = CompletedEgg;
        if (Eggs.Contains(eggToRemove))
        {
            Eggs.Remove(eggToRemove);

            if (Eggs.Count <= 0)
            {
                FoundAllEggs();
            }
            else
            {
                UpdateUI();
            }
        }
    }

    public void UpdateUI()
    {
        textUI = GameObject.Find("Text (TMP)").GetComponent<TextMeshPro>();
        //Debug.Log("updating uit to "+Eggs.Count);
        Debug.Log((maxEggs - Eggs.Count) + "/" + maxEggs);
        textUI.text = maxEggs - Eggs.Count + "/" + maxEggs+" Eggs";
    }

    public void FoundAllEggs()
    {
        textUI = GameObject.Find("Text (TMP)").GetComponent<TextMeshPro>();
        textUI.text = "Congratulations! <br> You have found all eggs";
    }
}
