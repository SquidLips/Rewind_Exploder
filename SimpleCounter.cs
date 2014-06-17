using UnityEngine;
using System.Collections;

public class SimpleCounter : MonoBehaviour {

    int counter = 0;
	// Use this for initialization
	void Start () 
    {
        counter = 0;
	}
	
	// Update is called once per frame
	void Update () 
    {
        Debug.Log(counter);
        ++counter;
	}
}
