using UnityEngine;
using System.Collections;

public class gameManger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Camera.main.transform.Translate (0, 0, 0.01f);
	
	}
}
