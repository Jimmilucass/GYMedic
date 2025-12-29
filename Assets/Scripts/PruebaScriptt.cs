using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaScriptt : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    int value;
    void Update()
    {
        //print("valor " + value);
        if (value >= 10)
            return;
        value++;

        //valores que se ejecutaran 
        print("hola xdd");

        if (value == 10)
        {
            StartCoroutine(Die());
            value = 0;
        }
            
    }
    IEnumerator Die()
    {
        yield return new WaitForSeconds(10);
    }

}
