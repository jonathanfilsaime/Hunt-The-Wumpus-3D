using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {

	public GameObject GameManager;

	// Use this for initialization
	void Start () 
	{
		transform.position.Set (20, 20, 10);

	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		transform.position = GameManager.transform.position;
	}
}
