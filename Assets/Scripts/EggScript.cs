using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScript : MonoBehaviour
{
    private GameObject egg;
    private easterScript easter;
    
    // Start is called before the first frame update
    void Start()
    {
        egg = this.gameObject;
        easter  = GameObject.Find("EasterUI").GetComponent<easterScript>();
        easter.CreateEgg(egg);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Egg Trigger")
        {
            easter.CompleteEgg(this.gameObject);
            Destroy(this.gameObject);
        }
    }
    
}
