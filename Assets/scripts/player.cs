using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class player : MonoBehaviour {

	public bool moveMode = false;
	public bool shootMode = false;
	 
	public int wumpusLocation;
	public int superBats1Location;
	public int superBats2Location;
	public int pitLocation;
	public int playerLocation;
	public int targetRoom;
	public int[] tempTarget;
	public List<int> target = new List<int>();

	public Text adjacentRoom1;
	public Text adjacentRoom2;
	public Text adjacentRoom3;
	public Text warnings;
	public Text gotRelocated;
	public Text missedWumpus;
	public Text target1;
	public Text target2;
	public Text target3;
	public Text target4;
	public Text target5;
	public Text playerPostion;

	public Transform wumpusAnitmation;
	public Transform batsAnimation;
	public Transform pitAnimation;

	public float cameraPostionX = 0f;
	public float cameraPostionY = 0f;
	public float cameraPostionZ = 0f;
	public float cameraRotationX = 0f;
	public float cameraRotationY = 0f;
	public float cameraRotationZ = 0f;

	bool playerGotRelocated = false;
	bool[] CameraPosition = new bool[21];
	bool ifWumpus = false;
	bool ifPit = false;
	bool ifBats = false;

	public string[] roomContent = new string[25];


	//adjacency list of rooms 
	int[] adj0  = { 0, 0, 0   };
	int[] adj1  = { 5, 2, 6   };
	int[] adj2  = { 1, 3, 8   };
	int[] adj3  = { 2, 4, 10  };
	int[] adj4  = { 3, 5, 11  };
	int[] adj5  = { 4, 1, 15  };
	int[] adj6  = { 1, 15, 7  };
	int[] adj7  = { 6, 8, 16  };
	int[] adj8  = { 7, 9, 2   };
	int[] adj9  = { 8, 10, 17 };
	int[] adj10 = { 9, 3, 11  };
	int[] adj11 = { 10, 12, 4 };
	int[] adj12 = { 11, 13, 18};
	int[] adj13 = { 12, 14, 19};
	int[] adj14 = { 15, 13, 20};
	int[] adj15 = { 5, 14, 6  };
	int[] adj16 = { 7, 20, 17 };
	int[] adj17 = { 9, 16, 18 };
	int[] adj18 = { 17, 12, 19};
	int[] adj19 = { 18, 13, 20};
	int[] adj20 = { 19, 16, 14};
	int[] adj21 = { 0, 0, 0   };

	List<int[]> adjacentRooms = new List<int[]>();

	//set the wumpus, pit, bats in player in random location
	void randonmize()
	{
		wumpusLocation = Random.Range(1, 20);
		pitLocation = Random.Range (1, 20);
		superBats1Location = Random.Range (1, 20);
		superBats2Location = Random.Range (1, 20);
		playerLocation = Random.Range (1, 20);
		playerPostion = GameObject.FindGameObjectWithTag ("currentPosition").GetComponent<Text> ();
		playerPostion.text = playerLocation.ToString();
	}

	//set all camera position to false 
	public void cameraPostionClear()
	{
		for (int i = 0; i < 21 ; i++) 
		{
			CameraPosition [i] = false;
		}
	}

	//set the camera position
	public void SetCameraPostion()
	{
		if (shootMode) 
		{
			cameraPostionClear ();
			CameraPosition [0] = true; 	
		} 
		else 
		{
			cameraPostionClear ();
			CameraPosition [playerLocation] = true;
		}
	}

	//populate the array with the adjacency list 
	void roomSetUp()
	{
		adjacentRooms.Add (adj0);
		adjacentRooms.Add (adj1);
		adjacentRooms.Add (adj2);
		adjacentRooms.Add (adj3);
		adjacentRooms.Add (adj4);
		adjacentRooms.Add (adj5);
		adjacentRooms.Add (adj6);
		adjacentRooms.Add (adj7);
		adjacentRooms.Add (adj8);
		adjacentRooms.Add (adj9);
		adjacentRooms.Add (adj10);
		adjacentRooms.Add (adj11);
		adjacentRooms.Add (adj12);
		adjacentRooms.Add (adj13);
		adjacentRooms.Add (adj14);
		adjacentRooms.Add (adj15);
		adjacentRooms.Add (adj16);
		adjacentRooms.Add (adj17);
		adjacentRooms.Add (adj18);
		adjacentRooms.Add (adj19);
		adjacentRooms.Add (adj20);
		adjacentRooms.Add (adj21);
	}

	//set the content of each room
	void roomContentSetUp()
	{
		roomContent[0]  = "nada";
		roomContent[1]  = "nada";
		roomContent[2]  = "nada";
		roomContent[3]  = "nada";
		roomContent[4]  = "nada";
		roomContent[5]  = "nada";
		roomContent[6]  = "nada";
		roomContent[7]  = "nada";
		roomContent[8]  = "nada";
		roomContent[9]  = "nada";
		roomContent[10] = "nada";
		roomContent[11] = "nada";
		roomContent[12] = "nada";
		roomContent[13] = "nada";
		roomContent[14] = "nada";
		roomContent[15] = "nada";
		roomContent[16] = "nada";
		roomContent[17] = "nada";
		roomContent[18] = "nada";
		roomContent[19] = "nada";
		roomContent[20] = "nada";
		roomContent[21] = "nada";
		roomContent[22] = "nada";
		roomContent[23] = "nada";
		roomContent[24] = "nada";
		roomContent[25] = "nada";

	}

	//place wumpus in a random room
	void setWumpus(int wumpusLocation)
	{
		roomContent[wumpusLocation] = "wumpus";
	}

	//place pit in a random room
	void setPit(int pitLocation)
	{
		roomContent[pitLocation] = "pit";
	}

	//place super bats 1 in a random room
	void setSuperBats1(int superBats1Location)
	{
		roomContent[superBats1Location] = "superbats";
	}

	//place super bats 2 in a random room
	void setSuperBats2(int superBats2Location)
	{
		roomContent[superBats2Location] = "superbats";
	}
		
	//move the player 
	public void move()
	{
		//if player got relocated by the bats 
		if (playerGotRelocated) 
		{
			playerLocation = Random.Range (1, 20);
			playerPostion = GameObject.FindGameObjectWithTag ("currentPosition").GetComponent<Text> ();
			playerPostion.text = playerLocation.ToString();
			gotRelocated = GameObject.FindGameObjectWithTag ("Relocated").GetComponent<Text> ();
			gotRelocated.text = " YOU GOT RELOCATED TO ROOM " + playerLocation;
			playerGotRelocated = false;
		}

		//clear the warning text when user clicks move
		missedWumpus = GameObject.FindGameObjectWithTag ("missedWumpus").GetComponent<Text> ();
		missedWumpus.text = null;

		moveMode = true;
		shootMode = false;
		SetCameraPostion ();
		ClearTarget ();
		TurnOff ();
		TurnOffPath ();
		LightUp (playerLocation);
		checkAdjacency (playerLocation);
		checkAdjacentContent (playerLocation);
		checkLife (playerLocation);
	}

	//shoot arrow
	public void Shoot()
	{
		//clear all text when player clicks shoot
		missedWumpus = GameObject.FindGameObjectWithTag ("missedWumpus").GetComponent<Text> ();
		missedWumpus.text = null;
		gotRelocated = GameObject.FindGameObjectWithTag ("Relocated").GetComponent<Text> ();
		gotRelocated.text = null;

		shootMode = true;
		moveMode = false;

		checkAdjacency (playerLocation);
		SetCameraPostion ();
	}

	//check all the rooms adjacent to the player location
	void checkAdjacency(int playerLocation)
	{
		adjacentRoom1 = GameObject.FindGameObjectWithTag ("textRoom1").GetComponent<Text> (); 
		adjacentRoom2 = GameObject.FindGameObjectWithTag ("textRoom2").GetComponent<Text> (); 
		adjacentRoom3 = GameObject.FindGameObjectWithTag ("textRoom3").GetComponent<Text> ();
		int[] temp = adjacentRooms [playerLocation];
		adjacentRoom1.text = temp[0].ToString();
		adjacentRoom2.text = temp[1].ToString();
		adjacentRoom3.text = temp[2].ToString();
	}

	//check the content of all the rooms adjacent to the player 
	void checkAdjacentContent(int playerLocation)
	{
		int[] temp = adjacentRooms [playerLocation];
		warnings = GameObject.FindGameObjectWithTag ("Warning").GetComponent<Text> ();
		warnings.text = null;

		for (int i = 0; i < 3; i++) 
		{
			if (roomContent [temp [i]].Equals ("wumpus")) 
			{
				warnings = GameObject.FindGameObjectWithTag ("Warning").GetComponent<Text> ();
				warnings.text = " I smell a WUMPUS ";
			} 
			if (roomContent [temp [i]].Equals ("pit")) 
			{
				warnings = GameObject.FindGameObjectWithTag ("Warning").GetComponent<Text> ();
				warnings.text += " I fell a WIND ";
			} 
			if (roomContent [temp [i]].Equals ("superbats")) 
			{
				warnings = GameObject.FindGameObjectWithTag ("Warning").GetComponent<Text> ();
				warnings.text += " I hear BATS ";
			} 
		}

	}

	//check the content of the room the player is currently in
	void checkLife(int playerLocation)
	{
		if (roomContent [playerLocation].Equals ("wumpus")) 
		{
			warnings = GameObject.FindGameObjectWithTag ("Warning").GetComponent<Text> ();
			warnings.fontSize = 20;
			warnings.color = Color.red;
			warnings.text = "YOU ARE DEAD\nTHE WUMPUS EAT YOU";
			ifWumpus = true;
			adjacentRooms.Clear ();
			adjacentRoom1 = GameObject.FindGameObjectWithTag ("textRoom1").GetComponent<Text> (); 
			adjacentRoom2 = GameObject.FindGameObjectWithTag ("textRoom2").GetComponent<Text> (); 
			adjacentRoom3 = GameObject.FindGameObjectWithTag ("textRoom3").GetComponent<Text> ();
			adjacentRoom1.text = null;
			adjacentRoom2.text = null;
			adjacentRoom3.text = null;
		}
		if (roomContent [playerLocation].Equals ("pit")) 
		{
			warnings = GameObject.FindGameObjectWithTag ("Warning").GetComponent<Text> ();
			warnings.fontSize = 20;
			warnings.color = Color.red;
			warnings.text = "YOU ARE DEAD YOU FELL IN THE PIT";
			ifPit = true;
			adjacentRooms.Clear ();
			adjacentRoom1 = GameObject.FindGameObjectWithTag ("textRoom1").GetComponent<Text> (); 
			adjacentRoom2 = GameObject.FindGameObjectWithTag ("textRoom2").GetComponent<Text> (); 
			adjacentRoom3 = GameObject.FindGameObjectWithTag ("textRoom3").GetComponent<Text> ();
			adjacentRoom1.text = null;
			adjacentRoom2.text = null;
			adjacentRoom3.text = null;
		}
		if(roomContent [playerLocation].Equals ("superbats"))
		{
			ifBats = true;
			warnings = GameObject.FindGameObjectWithTag ("Warning").GetComponent<Text> ();
			warnings.fontSize = 20;
			warnings.color = Color.red;
			warnings.text = "you got RELOCATED";
			playerGotRelocated = true;
			move ();

		}
	}

	//if the arrow mised the wumpus moves
	public void wumpusMove()
	{
		roomContent[wumpusLocation] = "nada";
		wumpusLocation = Random.Range(1, 20);
		roomContent[wumpusLocation] = "wumpus";
	}

	//the next room the arrow will travel to 
	public void nextTarget(int thisTarget)
	{
		targetRoom = thisTarget;
		checkAdjacency (targetRoom);
	}

	//helps determine where the arrow currently is 
	public int[] targetEmpty()
	{
		if (target.Count == 0) 
		{	
			return adjacentRooms [playerLocation];	
		}
		else 
		{
			return adjacentRooms [targetRoom];
		}
	}
		
	//when the user clicks the room1 button
	public void room1()
	{
		int[] temp = adjacentRooms [playerLocation];
		tempTarget = targetEmpty();

		//clear the text 
		gotRelocated = GameObject.FindGameObjectWithTag ("Relocated").GetComponent<Text> ();
		gotRelocated.text = null;

		//set these values to false 
		ifBats = false;
		ifPit = false;
		ifWumpus = false;

		GameObject tempObject1 = GameObject.FindGameObjectWithTag ("batsAnimation");
		Vector3 pos1 = new Vector3(500, 5, 500);
		tempObject1.transform.position = pos1;

		if (moveMode) 
		{
			playerLocation = temp [0];
			playerPostion = GameObject.FindGameObjectWithTag ("currentPosition").GetComponent<Text> ();
			playerPostion.text = playerLocation.ToString();
			move ();
		}
		if(shootMode)
		{
			
			target.Add (tempTarget [0]);
			nextTarget (tempTarget [0]);
			showTarget ();

		}
	}

	//when the user clicks the room2 button
	public void room2()
	{
		//clear the text 
		gotRelocated = GameObject.FindGameObjectWithTag ("Relocated").GetComponent<Text> ();
		gotRelocated.text = null;

		//set these values to false
		ifBats = false;
		ifPit = false;
		ifWumpus = false;

		int[] temp = adjacentRooms [playerLocation];
		tempTarget = targetEmpty();

		GameObject tempObject1 = GameObject.FindGameObjectWithTag ("batsAnimation");
		Vector3 pos1 = new Vector3(500, 5, 500);
		tempObject1.transform.position = pos1;

		if (moveMode) 
		{
			playerLocation = temp [1];
			playerPostion = GameObject.FindGameObjectWithTag ("currentPosition").GetComponent<Text> ();
			playerPostion.text = playerLocation.ToString();
			move ();
		}
		if(shootMode)
		{
			target.Add (tempTarget [1]);
			nextTarget (tempTarget [1]);
			showTarget ();

		}
	}

	//when the user clicks the room3 button
	public void room3()
	{
		//clear the text 
		gotRelocated = GameObject.FindGameObjectWithTag ("Relocated").GetComponent<Text> ();
		gotRelocated.text = null;

		//set these values to false
		ifBats = false;
		ifPit = false;
		ifWumpus = false;

		int[] temp = adjacentRooms [playerLocation];
		tempTarget = targetEmpty();

		GameObject tempObject1 = GameObject.FindGameObjectWithTag ("batsAnimation");
		Vector3 pos1 = new Vector3(500, 5, 500);
		tempObject1.transform.position = pos1;

		if (moveMode) 
		{
			playerLocation = temp [2];
			playerPostion = GameObject.FindGameObjectWithTag ("currentPosition").GetComponent<Text> ();
			playerPostion.text = playerLocation.ToString();
			move ();
		}
		if(shootMode)
		{
			target.Add (tempTarget [2]);
			nextTarget (tempTarget [2]);
			showTarget ();
		}
	}

	//lights up the room of current player location
	public void LightUp(int playerLocation) 
	{
		string CubeLocation = "cube" + playerLocation.ToString ();
		GameObject cube = GameObject.FindGameObjectWithTag (CubeLocation);
		cube.GetComponent<MeshRenderer> ().material.color = Color.green;
	}


	//turn off all the rooms 
	public void TurnOff() 
	{
		for (int i = 1; i < 21; i++) 
		{
			string CubeLocation = "cube" + i.ToString ();
			GameObject cube = GameObject.FindGameObjectWithTag (CubeLocation);
			cube.GetComponent<MeshRenderer> ().material.color = Color.gray;	
		}
	}
		
	//show the target room selected by the user in the box below
	public void showTarget()
	{
		if (target.Count == 1) 
		{
			target1 = GameObject.FindGameObjectWithTag ("target1").GetComponent<Text> (); 
			target1.text = target [0].ToString ();
		}
		if (target.Count == 2) 
		{
			target2 = GameObject.FindGameObjectWithTag ("target2").GetComponent<Text> (); 
			target2.text = target [1].ToString ();
		}
		if (target.Count == 3) 
		{
			target3 = GameObject.FindGameObjectWithTag ("target3").GetComponent<Text> (); 
			target3.text = target [2].ToString ();
		}
		if (target.Count == 4) 
		{
			target4 = GameObject.FindGameObjectWithTag ("target4").GetComponent<Text> (); 
			target4.text = target [3].ToString ();
		}
		if (target.Count == 5) 
		{
			target5 = GameObject.FindGameObjectWithTag ("target5").GetComponent<Text> (); 
			target5.text = target [4].ToString ();
			LightUpRoomInPath();
			LightUpPath ();
			CheckIfWin ();

		}
	}

	//check if the user shoot the wupus 
	public void CheckIfWin()
	{
		if (roomContent [target [0]].Equals ("wumpus") || roomContent [target [1]].Equals ("wumpus") ||
		   	roomContent [target [2]].Equals ("wumpus") || roomContent [target [3]].Equals ("wumpus") ||
		   	roomContent [target [4]].Equals ("wumpus")) 
		{
			warnings = GameObject.FindGameObjectWithTag ("Warning").GetComponent<Text> ();
			warnings.fontSize = 20;
			warnings.color = Color.red;
			warnings.text = "YOU WIN";
			target.Clear ();
			adjacentRooms.Clear ();
			adjacentRoom1 = GameObject.FindGameObjectWithTag ("textRoom1").GetComponent<Text> (); 
			adjacentRoom2 = GameObject.FindGameObjectWithTag ("textRoom2").GetComponent<Text> (); 
			adjacentRoom3 = GameObject.FindGameObjectWithTag ("textRoom3").GetComponent<Text> ();
			adjacentRoom1.text = null;
			adjacentRoom2.text = null;
			adjacentRoom3.text = null;
		}
		else 
		{
			missedWumpus = GameObject.FindGameObjectWithTag ("missedWumpus").GetComponent<Text> ();
			missedWumpus.fontSize = 12;
			missedWumpus.text = " YOU MISSED THE \nWUMPUS MOVED ";
			wumpusMove ();
			target.Clear ();
		}

	}

	//light the rooms the arrow is traveling 
	public void LightUpRoomInPath()
	{
		for (int i = 0; i < 5; i++) 
		{
			string CubeLocation = "cube" + target [i].ToString ();
			GameObject cube = GameObject.FindGameObjectWithTag (CubeLocation);
			cube.GetComponent<MeshRenderer> ().material.color = Color.red;
		}
	}

	//clear all text in the target boxes 
	public void ClearTarget()
	{
		target1 = GameObject.FindGameObjectWithTag ("target1").GetComponent<Text> (); 
		target1.text = null;

		target2 = GameObject.FindGameObjectWithTag ("target2").GetComponent<Text> (); 
		target2.text = null;

		target3 = GameObject.FindGameObjectWithTag ("target3").GetComponent<Text> (); 
		target3.text = null;

		target4 = GameObject.FindGameObjectWithTag ("target4").GetComponent<Text> (); 
		target4.text = null;

		target5 = GameObject.FindGameObjectWithTag ("target5").GetComponent<Text> (); 
		target5.text = null;
	}
		
	//light up the path the arrow is traveling
	public void LightUpPath()
	{
		if ((target [0] == 1 && target [1] == 2) || (target [0] == 2 && target [1] == 1) ||
		    (target [1] == 1 && target [2] == 2) || (target [1] == 2 && target [2] == 1) ||
		   	(target [2] == 1 && target [3] == 2) || (target [2] == 2 && target [3] == 1) ||
		   	(target [3] == 1 && target [4] == 2) || (target [3] == 2 && target [4] == 1)) 
		{
			GameObject cylinder1 = GameObject.FindWithTag("path1-2");
			cylinder1.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 2 && target [1] == 3) || (target [0] == 3 && target [1] == 2) ||
			(target [1] == 2 && target [2] == 3) || (target [1] == 3 && target [2] == 2) ||
			(target [2] == 2 && target [3] == 3) || (target [2] == 3 && target [3] == 2) ||
			(target [3] == 2 && target [4] == 3) || (target [3] == 3 && target [4] == 2)) 
		{
			GameObject cylinder2 = GameObject.FindWithTag("path2-3");
			cylinder2.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 3 && target [1] == 4) || (target [0] == 4 && target [1] == 3) ||
			(target [1] == 3 && target [2] == 4) || (target [1] == 4 && target [2] == 3) ||
			(target [2] == 3 && target [3] == 4) || (target [2] == 4 && target [3] == 3) ||
			(target [3] == 3 && target [4] == 4) || (target [3] == 4 && target [4] == 3)) 
		{
			GameObject cylinder3 = GameObject.FindWithTag("path3-4");
			cylinder3.GetComponent<MeshRenderer> ().material.color = Color.red;
			GameObject cylinder3x = GameObject.FindWithTag("path3-4x");
			cylinder3x.GetComponent<MeshRenderer> ().material.color = Color.red;
			GameObject cylinder3y = GameObject.FindWithTag("path3-4y");
			cylinder3y.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 4 && target [1] == 5) || (target [0] == 5 && target [1] == 4) ||
			(target [1] == 4 && target [2] == 5) || (target [1] == 5 && target [2] == 4) ||
			(target [2] == 4 && target [3] == 5) || (target [2] == 5 && target [3] == 4) ||
			(target [3] == 4 && target [4] == 5) || (target [3] == 5 && target [4] == 4)) 
		{
			GameObject cylinder4 = GameObject.FindWithTag("path4-5");
			cylinder4.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 5 && target [1] == 1) || (target [0] == 1 && target [1] == 5) ||
			(target [1] == 5 && target [2] == 1) || (target [1] == 1 && target [2] == 5) ||
			(target [2] == 5 && target [3] == 1) || (target [2] == 1 && target [3] == 5) ||
			(target [3] == 5 && target [4] == 1) || (target [3] == 1 && target [4] == 5)) 
		{
			GameObject cylinder5 = GameObject.FindWithTag("path5-1");
			cylinder5.GetComponent<MeshRenderer> ().material.color = Color.red;
			GameObject cylinder5x = GameObject.FindWithTag("path5-1x");
			cylinder5x.GetComponent<MeshRenderer> ().material.color = Color.red;
			GameObject cylinder5y = GameObject.FindWithTag("path5-1y");
			cylinder5y.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 1 && target [1] == 6) || (target [0] == 6 && target [1] == 1) ||
			(target [1] == 1 && target [2] == 6) || (target [1] == 6 && target [2] == 1) ||
			(target [2] == 1 && target [3] == 6) || (target [2] == 6 && target [3] == 1) ||
			(target [3] == 1 && target [4] == 6) || (target [3] == 6 && target [4] == 1)) 
		{
			GameObject cylinder6 = GameObject.FindWithTag("path1-6");
			cylinder6.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 2 && target [1] == 8) || (target [0] == 8 && target [1] == 2) ||
			(target [1] == 2 && target [2] == 8) || (target [1] == 8 && target [2] == 2) ||
			(target [2] == 2 && target [3] == 8) || (target [2] == 8 && target [3] == 2) ||
			(target [3] == 2 && target [4] == 8) || (target [3] == 8 && target [4] == 2)) 
		{
			GameObject cylinder7 = GameObject.FindWithTag("path2-8");
			cylinder7.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 3 && target [1] == 10) || (target [0] == 10 && target [1] == 3) ||
			(target [1] == 3 && target [2] == 10) || (target [1] == 10 && target [2] == 3) ||
			(target [2] == 3 && target [3] == 10) || (target [2] == 10 && target [3] == 3) ||
			(target [3] == 3 && target [4] == 10) || (target [3] == 10 && target [4] == 3)) 
		{
			GameObject cylinder8 = GameObject.FindWithTag("path3-10");
			cylinder8.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 4 && target [1] == 11) || (target [0] == 11 && target [1] == 4) ||
			(target [1] == 4 && target [2] == 11) || (target [1] == 11 && target [2] == 4) ||
			(target [2] == 4 && target [3] == 11) || (target [2] == 11 && target [3] == 4) ||
			(target [3] == 4 && target [4] == 11) || (target [3] == 11 && target [4] == 4)) 
		{
			GameObject cylinder9 = GameObject.FindWithTag("path4-11");
			cylinder9.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 5 && target [1] == 15) || (target [0] == 15 && target [1] == 5) ||
			(target [1] == 5 && target [2] == 15) || (target [1] == 15 && target [2] == 5) ||
			(target [2] == 5 && target [3] == 15) || (target [2] == 15 && target [3] == 5) ||
			(target [3] == 5 && target [4] == 15) || (target [3] == 15 && target [4] == 5)) 
		{
			GameObject cylinder10 = GameObject.FindWithTag("path5-15");
			cylinder10.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 6 && target [1] == 7) || (target [0] == 7 && target [1] == 6) ||
			(target [1] == 6 && target [2] == 7) || (target [1] == 7 && target [2] == 6) ||
			(target [2] == 6 && target [3] == 7) || (target [2] == 7 && target [3] == 6) ||
			(target [3] == 6 && target [4] == 7) || (target [3] == 7 && target [4] == 6)) 
		{
			GameObject cylinder11 = GameObject.FindWithTag("path6-7");
			cylinder11.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 7 && target [1] == 8) || (target [0] == 8 && target [1] == 7) ||
			(target [1] == 7 && target [2] == 8) || (target [1] == 8 && target [2] == 7) ||
			(target [2] == 7 && target [3] == 8) || (target [2] == 8 && target [3] == 7) ||
			(target [3] == 7 && target [4] == 8) || (target [3] == 8 && target [4] == 7)) 
		{
			GameObject cylinder12 = GameObject.FindWithTag("path7-8");
			cylinder12.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 8 && target [1] == 9) || (target [0] == 9 && target [1] == 8) ||
			(target [1] == 8 && target [2] == 9) || (target [1] == 9 && target [2] == 8) ||
			(target [2] == 8 && target [3] == 9) || (target [2] == 9 && target [3] == 8) ||
			(target [3] == 8 && target [4] == 9) || (target [3] == 9 && target [4] == 8)) 
		{
			GameObject cylinder13 = GameObject.FindWithTag("path8-9");
			cylinder13.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 9 && target [1] == 10) || (target [0] == 10 && target [1] == 9) ||
			(target [1] == 9 && target [2] == 10) || (target [1] == 10 && target [2] == 9) ||
			(target [2] == 9 && target [3] == 10) || (target [2] == 10 && target [3] == 9) ||
			(target [3] == 9 && target [4] == 10) || (target [3] == 10 && target [4] == 9)) 
		{
			GameObject cylinder14 = GameObject.FindWithTag("path9-10");
			cylinder14.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 10 && target [1] == 11) || (target [0] == 11 && target [1] == 10) ||
			(target [1] == 10 && target [2] == 11) || (target [1] == 11 && target [2] == 10) ||
			(target [2] == 10 && target [3] == 11) || (target [2] == 11 && target [3] == 10) ||
			(target [3] == 10 && target [4] == 11) || (target [3] == 11 && target [4] == 10)) 
		{
			GameObject cylinder15 = GameObject.FindWithTag("path10-11");
			cylinder15.GetComponent<MeshRenderer> ().material.color = Color.red;
			GameObject cylinder15x = GameObject.FindWithTag("path10-11x");
			cylinder15x.GetComponent<MeshRenderer> ().material.color = Color.red;
			GameObject cylinder15y = GameObject.FindWithTag("path10-11y");
			cylinder15y.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 11 && target [1] == 12) || (target [0] == 12 && target [1] == 11) ||
			(target [1] == 11 && target [2] == 12) || (target [1] == 12 && target [2] == 11) ||
			(target [2] == 11 && target [3] == 12) || (target [2] == 12 && target [3] == 11) ||
			(target [3] == 11 && target [4] == 12) || (target [3] == 12 && target [4] == 11)) 
		{
			GameObject cylinder16 = GameObject.FindWithTag("path11-12");
			cylinder16.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 12 && target [1] == 13) || (target [0] == 13 && target [1] == 12) ||
			(target [1] == 12 && target [2] == 13) || (target [1] == 13 && target [2] == 12) ||
			(target [2] == 12 && target [3] == 13) || (target [2] == 13 && target [3] == 12) ||
			(target [3] == 12 && target [4] == 13) || (target [3] == 13 && target [4] == 12)) 
		{
			GameObject cylinder17 = GameObject.FindWithTag("path12-13");
			cylinder17.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 13 && target [1] == 14) || (target [0] == 14 && target [1] == 13) ||
			(target [1] == 13 && target [2] == 14) || (target [1] == 14 && target [2] == 13) ||
			(target [2] == 13 && target [3] == 14) || (target [2] == 14 && target [3] == 13) ||
			(target [3] == 13 && target [4] == 14) || (target [3] == 14 && target [4] == 13)) 
		{
			GameObject cylinder18 = GameObject.FindWithTag("path13-14");
			cylinder18.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 14 && target [1] == 15) || (target [0] == 15 && target [1] == 14) ||
			(target [1] == 14 && target [2] == 15) || (target [1] == 15 && target [2] == 14) ||
			(target [2] == 14 && target [3] == 15) || (target [2] == 15 && target [3] == 14) ||
			(target [3] == 14 && target [4] == 15) || (target [3] == 15 && target [4] == 14)) 
		{
			GameObject cylinder19 = GameObject.FindWithTag("path14-15");
			cylinder19.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 15 && target [1] == 6) || (target [0] == 6 && target [1] == 15) ||
			(target [1] == 15 && target [2] == 6) || (target [1] == 6 && target [2] == 15) ||
			(target [2] == 15 && target [3] == 6) || (target [2] == 6 && target [3] == 15) ||
			(target [3] == 15 && target [4] == 6) || (target [3] == 6 && target [4] == 15)) 
		{
			GameObject cylinder20 = GameObject.FindWithTag("path15-6");
			cylinder20.GetComponent<MeshRenderer> ().material.color = Color.red;
			GameObject cylinder20x = GameObject.FindWithTag("path15-6x");
			cylinder20x.GetComponent<MeshRenderer> ().material.color = Color.red;
			GameObject cylinder20y = GameObject.FindWithTag("path15-6y");
			cylinder20y.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 7 && target [1] == 16) || (target [0] == 16 && target [1] == 7) ||
			(target [1] == 7 && target [2] == 16) || (target [1] == 16 && target [2] == 7) ||
			(target [2] == 7 && target [3] == 16) || (target [2] == 16 && target [3] == 7) ||
			(target [3] == 7 && target [4] == 16) || (target [3] == 16 && target [4] == 7)) 
		{
			GameObject cylinder21 = GameObject.FindWithTag("path7-16");
			cylinder21.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 9 && target [1] == 17) || (target [0] == 17 && target [1] == 9) ||
			(target [1] == 9 && target [2] == 17) || (target [1] == 17 && target [2] == 9) ||
			(target [2] == 9 && target [3] == 17) || (target [2] == 17 && target [3] == 9) ||
			(target [3] == 9 && target [4] == 17) || (target [3] == 17 && target [4] == 9)) 
		{
			GameObject cylinder22 = GameObject.FindWithTag("path9-17");
			cylinder22.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 12 && target [1] == 18) || (target [0] == 18 && target [1] == 12) ||
			(target [1] == 12 && target [2] == 18) || (target [1] == 18 && target [2] == 12) ||
			(target [2] == 12 && target [3] == 18) || (target [2] == 18 && target [3] == 12) ||
			(target [3] == 12 && target [4] == 18) || (target [3] == 18 && target [4] == 12)) 
		{
			GameObject cylinder23 = GameObject.FindWithTag("path12-18");
			cylinder23.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 13 && target [1] == 19) || (target [0] == 19 && target [1] == 13) ||
			(target [1] == 13 && target [2] == 19) || (target [1] == 19 && target [2] == 13) ||
			(target [2] == 13 && target [3] == 19) || (target [2] == 19 && target [3] == 13) ||
			(target [3] == 13 && target [4] == 19) || (target [3] == 19 && target [4] == 13)) 
		{
			GameObject cylinder24 = GameObject.FindWithTag("path13-19");
			cylinder24.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 14 && target [1] == 20) || (target [0] == 20 && target [1] == 14) ||
			(target [1] == 14 && target [2] == 20) || (target [1] == 20 && target [2] == 14) ||
			(target [2] == 14 && target [3] == 20) || (target [2] == 20 && target [3] == 14) ||
			(target [3] == 14 && target [4] == 20) || (target [3] == 20 && target [4] == 14)) 
		{
			GameObject cylinder25 = GameObject.FindWithTag("path14-20");
			cylinder25.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 16 && target [1] == 17) || (target [0] == 17 && target [1] == 16) ||
			(target [1] == 16 && target [2] == 17) || (target [1] == 17 && target [2] == 16) ||
			(target [2] == 16 && target [3] == 17) || (target [2] == 17 && target [3] == 16) ||
			(target [3] == 16 && target [4] == 17) || (target [3] == 17 && target [4] == 16)) 
		{
			GameObject cylinder26 = GameObject.FindWithTag("path16-17");
			cylinder26.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 17 && target [1] == 18) || (target [0] == 18 && target [1] == 17) ||
			(target [1] == 17 && target [2] == 18) || (target [1] == 18 && target [2] == 17) ||
			(target [2] == 17 && target [3] == 18) || (target [2] == 18 && target [3] == 17) ||
			(target [3] == 17 && target [4] == 18) || (target [3] == 18 && target [4] == 17)) 
		{
			GameObject cylinder27 = GameObject.FindWithTag("path17-18");
			cylinder27.GetComponent<MeshRenderer> ().material.color = Color.red;
			GameObject cylinder27x = GameObject.FindWithTag("path17-18x");
			cylinder27x.GetComponent<MeshRenderer> ().material.color = Color.red;
			GameObject cylinder27y = GameObject.FindWithTag("path17-18y");
			cylinder27y.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 18 && target [1] == 19) || (target [0] == 19 && target [1] == 18) ||
			(target [1] == 18 && target [2] == 19) || (target [1] == 19 && target [2] == 18) ||
			(target [2] == 18 && target [3] == 19) || (target [2] == 19 && target [3] == 18) ||
			(target [3] == 18 && target [4] == 19) || (target [3] == 19 && target [4] == 18)) 
		{
			GameObject cylinder28 = GameObject.FindWithTag("path18-19");
			cylinder28.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 19 && target [1] == 20) || (target [0] == 20 && target [1] == 19) ||
			(target [1] == 19 && target [2] == 20) || (target [1] == 20 && target [2] == 19) ||
			(target [2] == 19 && target [3] == 20) || (target [2] == 20 && target [3] == 19) ||
			(target [3] == 19 && target [4] == 20) || (target [3] == 20 && target [4] == 19)) 
		{
			GameObject cylinder29 = GameObject.FindWithTag("path19-20");
			cylinder29.GetComponent<MeshRenderer> ().material.color = Color.red;
		}

		if ((target [0] == 20 && target [1] == 16) || (target [0] == 16 && target [1] == 20) ||
			(target [1] == 20 && target [2] == 16) || (target [1] == 16 && target [2] == 20) ||
			(target [2] == 20 && target [3] == 16) || (target [2] == 16 && target [3] == 20) ||
			(target [3] == 20 && target [4] == 16) || (target [3] == 16 && target [4] == 20)) 
		{
			GameObject cylinder30 = GameObject.FindWithTag("path20-16");
			cylinder30.GetComponent<MeshRenderer> ().material.color = Color.red;
			GameObject cylinder30x = GameObject.FindWithTag("path20-16x");
			cylinder30x.GetComponent<MeshRenderer> ().material.color = Color.red;
			GameObject cylinder30y = GameObject.FindWithTag("path20-16y");
			cylinder30y.GetComponent<MeshRenderer> ().material.color = Color.red;
		}
	}

	//turn off the path
	public void TurnOffPath()
	{
		GameObject path1_2 = GameObject.FindGameObjectWithTag ("path1-2");
		path1_2.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path2_3 = GameObject.FindGameObjectWithTag ("path2-3");
		path2_3.GetComponent<MeshRenderer> ().material.color = Color.gray;	

		GameObject path3_4 = GameObject.FindGameObjectWithTag ("path3-4");
		path3_4.GetComponent<MeshRenderer> ().material.color = Color.gray;	
		GameObject path3_4x = GameObject.FindGameObjectWithTag ("path3-4x");
		path3_4x.GetComponent<MeshRenderer> ().material.color = Color.gray;	
		GameObject path3_4y = GameObject.FindGameObjectWithTag ("path3-4y");
		path3_4y.GetComponent<MeshRenderer> ().material.color = Color.gray;	

		GameObject path4_5 = GameObject.FindGameObjectWithTag ("path4-5");
		path4_5.GetComponent<MeshRenderer> ().material.color = Color.gray;	

		GameObject path5_1 = GameObject.FindGameObjectWithTag ("path5-1");
		path5_1.GetComponent<MeshRenderer> ().material.color = Color.gray;	
		GameObject path5_1x = GameObject.FindGameObjectWithTag ("path5-1x");
		path5_1x.GetComponent<MeshRenderer> ().material.color = Color.gray;	
		GameObject path5_1y = GameObject.FindGameObjectWithTag ("path5-1y");
		path5_1y.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path1_6 = GameObject.FindGameObjectWithTag ("path1-6");
		path1_6.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path2_8 = GameObject.FindGameObjectWithTag ("path2-8");
		path2_8.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path3_10 = GameObject.FindGameObjectWithTag ("path3-10");
		path3_10.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path4_11 = GameObject.FindGameObjectWithTag ("path4-11");
		path4_11.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path5_15 = GameObject.FindGameObjectWithTag ("path5-15");
		path5_15.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path6_7 = GameObject.FindGameObjectWithTag ("path6-7");
		path6_7.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path7_8 = GameObject.FindGameObjectWithTag ("path7-8");
		path7_8.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path8_9 = GameObject.FindGameObjectWithTag ("path8-9");
		path8_9.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path9_10 = GameObject.FindGameObjectWithTag ("path9-10");
		path9_10.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path10_11 = GameObject.FindGameObjectWithTag ("path10-11");
		path10_11.GetComponent<MeshRenderer> ().material.color = Color.gray;	
		GameObject path10_11x = GameObject.FindGameObjectWithTag ("path10-11x");
		path10_11x.GetComponent<MeshRenderer> ().material.color = Color.gray;	
		GameObject path10_11y = GameObject.FindGameObjectWithTag ("path10-11y");
		path10_11y.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path11_12 = GameObject.FindGameObjectWithTag ("path11-12");
		path11_12.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path12_13 = GameObject.FindGameObjectWithTag ("path12-13");
		path12_13.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path13_14 = GameObject.FindGameObjectWithTag ("path13-14");
		path13_14.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path14_15 = GameObject.FindGameObjectWithTag ("path14-15");
		path14_15.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path15_16 = GameObject.FindGameObjectWithTag ("path15-6");
		path15_16.GetComponent<MeshRenderer> ().material.color = Color.gray;	
		GameObject path15_16x = GameObject.FindGameObjectWithTag ("path15-6x");
		path15_16x.GetComponent<MeshRenderer> ().material.color = Color.gray;	
		GameObject path15_16y = GameObject.FindGameObjectWithTag ("path15-6y");
		path15_16y.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path7_16 = GameObject.FindGameObjectWithTag ("path7-16");
		path7_16.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path9_17 = GameObject.FindGameObjectWithTag ("path9-17");
		path9_17.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path12_18 = GameObject.FindGameObjectWithTag ("path12-18");
		path12_18.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path13_19 = GameObject.FindGameObjectWithTag ("path13-19");
		path13_19.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path14_20 = GameObject.FindGameObjectWithTag ("path14-20");
		path14_20.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path16_17 = GameObject.FindGameObjectWithTag ("path16-17");
		path16_17.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path17_18 = GameObject.FindGameObjectWithTag ("path17-18");
		path17_18.GetComponent<MeshRenderer> ().material.color = Color.gray;	
		GameObject path17_18x = GameObject.FindGameObjectWithTag ("path17-18x");
		path17_18x.GetComponent<MeshRenderer> ().material.color = Color.gray;	
		GameObject path17_18y = GameObject.FindGameObjectWithTag ("path17-18y");
		path17_18y.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path18_19 = GameObject.FindGameObjectWithTag ("path18-19");
		path18_19.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path19_20 = GameObject.FindGameObjectWithTag ("path19-20");
		path19_20.GetComponent<MeshRenderer> ().material.color = Color.gray;

		GameObject path20_16 = GameObject.FindGameObjectWithTag ("path20-16");
		path20_16.GetComponent<MeshRenderer> ().material.color = Color.gray;	
		GameObject path20_16x = GameObject.FindGameObjectWithTag ("path20-16x");
		path20_16x.GetComponent<MeshRenderer> ().material.color = Color.gray;	
		GameObject path20_16y = GameObject.FindGameObjectWithTag ("path20-16y");
		path20_16y.GetComponent<MeshRenderer> ().material.color = Color.gray;
	}
		
	// Use this for initialization
	void Start () 
	{
		target.Clear ();

		//clear all text
		warnings = GameObject.FindGameObjectWithTag ("Warning").GetComponent<Text> ();
		warnings.text = null;
		gotRelocated = GameObject.FindGameObjectWithTag ("Relocated").GetComponent<Text> ();
		gotRelocated.text = null;
		missedWumpus = GameObject.FindGameObjectWithTag ("missedWumpus").GetComponent<Text> ();
		missedWumpus.text += null;

		TurnOff ();
		randonmize ();
		roomSetUp ();
		setWumpus (wumpusLocation);
		setPit (pitLocation);
		setSuperBats1 (superBats1Location);
		setSuperBats2 (superBats2Location);

		Debug.Log ("pit:"+ pitLocation);
	}
		
	// Update is called once per frame
	void Update () 
	{
		//camera movement when in shoot mode
		if(shootMode)
		{
			cameraPostionX = Camera.main.transform.position.x;
			cameraPostionY = Camera.main.transform.position.y;
			cameraPostionZ = Camera.main.transform.position.z;

			//x axis in world coordinate 
			if( cameraPostionX < 20)Camera.main.transform.Translate(new Vector3(1f,0,0));
			if( cameraPostionX > 20)Camera.main.transform.Translate(new Vector3(-1f,0,0));
			if( cameraPostionX == 20)Camera.main.transform.Translate(new Vector3(0,0,0));

			//y axis in world coordinate 
			if( cameraPostionZ < -55)Camera.main.transform.Translate(new Vector3(0,1f,0));
			if( cameraPostionZ > -55)Camera.main.transform.Translate(new Vector3(0,-1f,0));
			if( cameraPostionZ == -55)Camera.main.transform.Translate(new Vector3(0,0,0));

			//z axis in world coordinate don't ever change this one here
			if( cameraPostionY < 65)Camera.main.transform.Translate(new Vector3(0,0,-1f));
			if( cameraPostionY > 65)Camera.main.transform.Translate(new Vector3(0,0,1f));
			if( cameraPostionY == 65)Camera.main.transform.Translate(new Vector3(0,0,0));

		}

		//cameramovement when inn move mode 
		if (moveMode) 
		{
			cameraPostionX = Camera.main.transform.position.x;
			cameraPostionY = Camera.main.transform.position.y;
			cameraPostionZ = Camera.main.transform.position.z;

			if (CameraPosition [1]) 
			{
				//x axis in world coordinate 
				if( cameraPostionX < 0)Camera.main.transform.Translate(new Vector3(1f,0,0));
				if( cameraPostionX > 0)Camera.main.transform.Translate(new Vector3(-1f,0,0));
				if( cameraPostionX == 0)Camera.main.transform.Translate(new Vector3(0,0,0));

				//y axis in world coordinate 
				if( cameraPostionZ < -20)Camera.main.transform.Translate(new Vector3(0,1f,0));
				if( cameraPostionZ > -20)Camera.main.transform.Translate(new Vector3(0,-1f,0));
				if( cameraPostionZ == -20)Camera.main.transform.Translate(new Vector3(0,0,0));

				//z axis in world coordinate don't ever change this one here
				if( cameraPostionY < 20)Camera.main.transform.Translate(new Vector3(0,0,-1f));
				if( cameraPostionY > 20)Camera.main.transform.Translate(new Vector3(0,0,1f));
				if( cameraPostionY == 20)Camera.main.transform.Translate(new Vector3(0,0,0));
			}
			if (CameraPosition [2]) 
			{
				//x axis in world coordinate 
				if( cameraPostionX < 20)Camera.main.transform.Translate(new Vector3(1f,0,0));
				if( cameraPostionX > 20)Camera.main.transform.Translate(new Vector3(-1f,0,0));
				if( cameraPostionX == 20)Camera.main.transform.Translate(new Vector3(0,0,0));

				//y axis in world coordinate 
				if( cameraPostionZ < -20)Camera.main.transform.Translate(new Vector3(0,1f,0));
				if( cameraPostionZ > -20)Camera.main.transform.Translate(new Vector3(0,-1f,0));
				if( cameraPostionZ == -20)Camera.main.transform.Translate(new Vector3(0,0,0));

				//z axis in world coordinate don't ever change this one here
				if( cameraPostionY < 20)Camera.main.transform.Translate(new Vector3(0,0,-1f));
				if( cameraPostionY > 20)Camera.main.transform.Translate(new Vector3(0,0,1f));
				if( cameraPostionY == 20)Camera.main.transform.Translate(new Vector3(0,0,0));
			}
			if (CameraPosition [3]) 
			{
				//x axis in world coordinate 
				if( cameraPostionX < 40)Camera.main.transform.Translate(new Vector3(1f,0,0));
				if( cameraPostionX > 40)Camera.main.transform.Translate(new Vector3(-1f,0,0));
				if( cameraPostionX == 40)Camera.main.transform.Translate(new Vector3(0,0,0));

				//y axis in world coordinate 
				if( cameraPostionZ < -20)Camera.main.transform.Translate(new Vector3(0,1f,0));
				if( cameraPostionZ > -20)Camera.main.transform.Translate(new Vector3(0,-1f,0));
				if( cameraPostionZ == -20)Camera.main.transform.Translate(new Vector3(0,0,0));

				//z axis in world coordinate don't ever change this one here
				if( cameraPostionY < 20)Camera.main.transform.Translate(new Vector3(0,0,-1f));
				if( cameraPostionY > 20)Camera.main.transform.Translate(new Vector3(0,0,1f));
				if( cameraPostionY == 20)Camera.main.transform.Translate(new Vector3(0,0,0));
			}
			if (CameraPosition [4]) 
			{
				//x axis in world coordinate 
				if( cameraPostionX < 40)Camera.main.transform.Translate(new Vector3(1f,0,0));
				if( cameraPostionX > 40)Camera.main.transform.Translate(new Vector3(-1f,0,0));
				if( cameraPostionX == 40)Camera.main.transform.Translate(new Vector3(0,0,0));

				//y axis in world coordinate 
				if( cameraPostionZ < -70)Camera.main.transform.Translate(new Vector3(0,1f,0));
				if( cameraPostionZ > -70)Camera.main.transform.Translate(new Vector3(0,-1f,0));
				if( cameraPostionZ == -70)Camera.main.transform.Translate(new Vector3(0,0,0));

				//z axis in world coordinate don't ever change this one here
				if( cameraPostionY < 20)Camera.main.transform.Translate(new Vector3(0,0,-1f));
				if( cameraPostionY > 20)Camera.main.transform.Translate(new Vector3(0,0,1f));
				if( cameraPostionY == 20)Camera.main.transform.Translate(new Vector3(0,0,0));
			}
			if (CameraPosition [5]) 
			{
				if( cameraPostionX < 0)Camera.main.transform.Translate(new Vector3(1f,0,0));
				if( cameraPostionX > 0)Camera.main.transform.Translate(new Vector3(-1f,0,0));
				if( cameraPostionX == 0)Camera.main.transform.Translate(new Vector3(0,0,0));

				//y axis in world coordinate 
				if( cameraPostionZ < -70)Camera.main.transform.Translate(new Vector3(0,1f,0));
				if( cameraPostionZ > -70)Camera.main.transform.Translate(new Vector3(0,-1f,0));
				if( cameraPostionZ == -70)Camera.main.transform.Translate(new Vector3(0,0,0));

				//z axis in world coordinate don't ever change this one here
				if( cameraPostionY < 20)Camera.main.transform.Translate(new Vector3(0,0,-1f));
				if( cameraPostionY > 20)Camera.main.transform.Translate(new Vector3(0,0,1f));
				if( cameraPostionY == 20)Camera.main.transform.Translate(new Vector3(0,0,0));
			}
			if (CameraPosition [6]) 
			{
				//x axis in world coordinate 
				if( cameraPostionX < 0)Camera.main.transform.Translate(new Vector3(1f,0,0));
				if( cameraPostionX > 0)Camera.main.transform.Translate(new Vector3(-1f,0,0));
				if( cameraPostionX == 0)Camera.main.transform.Translate(new Vector3(0,0,0));

				//y axis in world coordinate 
				if( cameraPostionZ < -30)Camera.main.transform.Translate(new Vector3(0,1f,0));
				if( cameraPostionZ > -30)Camera.main.transform.Translate(new Vector3(0,-1f,0));
				if( cameraPostionZ == -30)Camera.main.transform.Translate(new Vector3(0,0,0));

				//z axis in world coordinate don't ever change this one here
				if( cameraPostionY < 20)Camera.main.transform.Translate(new Vector3(0,0,-1f));
				if( cameraPostionY > 20)Camera.main.transform.Translate(new Vector3(0,0,1f));
				if( cameraPostionY == 20)Camera.main.transform.Translate(new Vector3(0,0,0));
			}
			if (CameraPosition [7]) 
			{
				//x axis in world coordinate 
				if( cameraPostionX < 10)Camera.main.transform.Translate(new Vector3(1f,0,0));
				if( cameraPostionX > 10)Camera.main.transform.Translate(new Vector3(-1f,0,0));
				if( cameraPostionX == 10)Camera.main.transform.Translate(new Vector3(0,0,0));

				//y axis in world coordinate 
				if( cameraPostionZ < -30)Camera.main.transform.Translate(new Vector3(0,1f,0));
				if( cameraPostionZ > -30)Camera.main.transform.Translate(new Vector3(0,-1f,0));
				if( cameraPostionZ == -30)Camera.main.transform.Translate(new Vector3(0,0,0));

				//z axis in world coordinate don't ever change this one here
				if( cameraPostionY < 20)Camera.main.transform.Translate(new Vector3(0,0,-1f));
				if( cameraPostionY > 20)Camera.main.transform.Translate(new Vector3(0,0,1f));
				if( cameraPostionY == 20)Camera.main.transform.Translate(new Vector3(0,0,0));
			}
			if (CameraPosition [8]) 
			{
				//x axis in world coordinate 
				if( cameraPostionX < 20)Camera.main.transform.Translate(new Vector3(1f,0,0));
				if( cameraPostionX > 20)Camera.main.transform.Translate(new Vector3(-1f,0,0));
				if( cameraPostionX == 20)Camera.main.transform.Translate(new Vector3(0,0,0));

				//y axis in world coordinate 
				if( cameraPostionZ < -30)Camera.main.transform.Translate(new Vector3(0,1f,0));
				if( cameraPostionZ > -30)Camera.main.transform.Translate(new Vector3(0,-1f,0));
				if( cameraPostionZ == -30)Camera.main.transform.Translate(new Vector3(0,0,0));

				//z axis in world coordinate don't ever change this one here
				if( cameraPostionY < 20)Camera.main.transform.Translate(new Vector3(0,0,-1f));
				if( cameraPostionY > 20)Camera.main.transform.Translate(new Vector3(0,0,1f));
				if( cameraPostionY == 20)Camera.main.transform.Translate(new Vector3(0,0,0));
			}
			if (CameraPosition [9]) 
			{
				//x axis in world coordinate 
				if( cameraPostionX < 30)Camera.main.transform.Translate(new Vector3(1f,0,0));
				if( cameraPostionX > 30)Camera.main.transform.Translate(new Vector3(-1f,0,0));
				if( cameraPostionX == 30)Camera.main.transform.Translate(new Vector3(0,0,0));

				//y axis in world coordinate 
				if( cameraPostionZ < -30)Camera.main.transform.Translate(new Vector3(0,1f,0));
				if( cameraPostionZ > -30)Camera.main.transform.Translate(new Vector3(0,-1f,0));
				if( cameraPostionZ == -30)Camera.main.transform.Translate(new Vector3(0,0,0));

				//z axis in world coordinate don't ever change this one here
				if( cameraPostionY < 20)Camera.main.transform.Translate(new Vector3(0,0,-1f));
				if( cameraPostionY > 20)Camera.main.transform.Translate(new Vector3(0,0,1f));
				if( cameraPostionY == 20)Camera.main.transform.Translate(new Vector3(0,0,0));
			}
			if (CameraPosition [10]) 
			{
				//x axis in world coordinate
				if( cameraPostionX < 40)Camera.main.transform.Translate(new Vector3(1f,0,0));
				if( cameraPostionX > 40)Camera.main.transform.Translate(new Vector3(-1f,0,0));
				if( cameraPostionX == 40)Camera.main.transform.Translate(new Vector3(0,0,0));

				//y axis in world coordinate 
				if( cameraPostionZ < -30)Camera.main.transform.Translate(new Vector3(0,1f,0));
				if( cameraPostionZ > -30)Camera.main.transform.Translate(new Vector3(0,-1f,0));
				if( cameraPostionZ == -30)Camera.main.transform.Translate(new Vector3(0,0,0));

				//z axis in world coordinate don't ever change this one here
				if( cameraPostionY < 20)Camera.main.transform.Translate(new Vector3(0,0,-1f));
				if( cameraPostionY > 20)Camera.main.transform.Translate(new Vector3(0,0,1f));
				if( cameraPostionY == 20)Camera.main.transform.Translate(new Vector3(0,0,0));
			}
			if (CameraPosition [11]) 
			{
				//x axis in world coordinate
				if( cameraPostionX < 40)Camera.main.transform.Translate(new Vector3(1f,0,0));
				if( cameraPostionX > 40)Camera.main.transform.Translate(new Vector3(-1f,0,0));
				if( cameraPostionX == 40)Camera.main.transform.Translate(new Vector3(0,0,0));

				//y axis in world coordinate 
				if( cameraPostionZ < -60)Camera.main.transform.Translate(new Vector3(0,1f,0));
				if( cameraPostionZ > -60)Camera.main.transform.Translate(new Vector3(0,-1f,0));
				if( cameraPostionZ == -60)Camera.main.transform.Translate(new Vector3(0,0,0));

				//z axis in world coordinate don't ever change this one here
				if( cameraPostionY < 20)Camera.main.transform.Translate(new Vector3(0,0,-1f));
				if( cameraPostionY > 20)Camera.main.transform.Translate(new Vector3(0,0,1f));
				if( cameraPostionY == 20)Camera.main.transform.Translate(new Vector3(0,0,0));
			}
			if (CameraPosition [12]) 
			{
				//x axis in world coordinate
				if( cameraPostionX < 30)Camera.main.transform.Translate(new Vector3(1f,0,0));
				if( cameraPostionX > 30)Camera.main.transform.Translate(new Vector3(-1f,0,0));
				if( cameraPostionX == 30)Camera.main.transform.Translate(new Vector3(0,0,0));

				//y axis in world coordinate 
				if( cameraPostionZ < -60)Camera.main.transform.Translate(new Vector3(0,1f,0));
				if( cameraPostionZ > -60)Camera.main.transform.Translate(new Vector3(0,-1f,0));
				if( cameraPostionZ == -60)Camera.main.transform.Translate(new Vector3(0,0,0));

				//z axis in world coordinate don't ever change this one here
				if( cameraPostionY < 20)Camera.main.transform.Translate(new Vector3(0,0,-1f));
				if( cameraPostionY > 20)Camera.main.transform.Translate(new Vector3(0,0,1f));
				if( cameraPostionY == 20)Camera.main.transform.Translate(new Vector3(0,0,0));
			}
			if (CameraPosition [13]) 
			{
				//x axis in world coordinate
				if( cameraPostionX < 20)Camera.main.transform.Translate(new Vector3(1f,0,0));
				if( cameraPostionX > 20)Camera.main.transform.Translate(new Vector3(-1f,0,0));
				if( cameraPostionX == 20)Camera.main.transform.Translate(new Vector3(0,0,0));

				//y axis in world coordinate 
				if( cameraPostionZ < -60)Camera.main.transform.Translate(new Vector3(0,1f,0));
				if( cameraPostionZ > -60)Camera.main.transform.Translate(new Vector3(0,-1f,0));
				if( cameraPostionZ == -60)Camera.main.transform.Translate(new Vector3(0,0,0));

				//z axis in world coordinate don't ever change this one here
				if( cameraPostionY < 20)Camera.main.transform.Translate(new Vector3(0,0,-1f));
				if( cameraPostionY > 20)Camera.main.transform.Translate(new Vector3(0,0,1f));
				if( cameraPostionY == 20)Camera.main.transform.Translate(new Vector3(0,0,0));
			}
			if (CameraPosition [14]) 
			{
				//x axis in world coordinate
				if( cameraPostionX < 10)Camera.main.transform.Translate(new Vector3(1f,0,0));
				if( cameraPostionX > 10)Camera.main.transform.Translate(new Vector3(-1f,0,0));
				if( cameraPostionX == 10)Camera.main.transform.Translate(new Vector3(0,0,0));

				//y axis in world coordinate 
				if( cameraPostionZ < -60)Camera.main.transform.Translate(new Vector3(0,1f,0));
				if( cameraPostionZ > -60)Camera.main.transform.Translate(new Vector3(0,-1f,0));
				if( cameraPostionZ == -60)Camera.main.transform.Translate(new Vector3(0,0,0));

				//z axis in world coordinate don't ever change this one here
				if( cameraPostionY < 20)Camera.main.transform.Translate(new Vector3(0,0,-1f));
				if( cameraPostionY > 20)Camera.main.transform.Translate(new Vector3(0,0,1f));
				if( cameraPostionY == 20)Camera.main.transform.Translate(new Vector3(0,0,0));
			}
			if (CameraPosition [15]) 
			{
				//x axis in world coordinate
				if( cameraPostionX < 0)Camera.main.transform.Translate(new Vector3(1f,0,0));
				if( cameraPostionX > 0)Camera.main.transform.Translate(new Vector3(-1f,0,0));
				if( cameraPostionX == 0)Camera.main.transform.Translate(new Vector3(0,0,0));

				//y axis in world coordinate 
				if( cameraPostionZ < -60)Camera.main.transform.Translate(new Vector3(0,1f,0));
				if( cameraPostionZ > -60)Camera.main.transform.Translate(new Vector3(0,-1f,0));
				if( cameraPostionZ == -60)Camera.main.transform.Translate(new Vector3(0,0,0));

				//z axis in world coordinate don't ever change this one here
				if( cameraPostionY < 20)Camera.main.transform.Translate(new Vector3(0,0,-1f));
				if( cameraPostionY > 20)Camera.main.transform.Translate(new Vector3(0,0,1f));
				if( cameraPostionY == 20)Camera.main.transform.Translate(new Vector3(0,0,0));
			}
			if (CameraPosition [16]) 
			{
				//x axis in world coordinate
				if( cameraPostionX < 10)Camera.main.transform.Translate(new Vector3(1f,0,0));
				if( cameraPostionX > 10)Camera.main.transform.Translate(new Vector3(-1f,0,0));
				if( cameraPostionX == 10)Camera.main.transform.Translate(new Vector3(0,0,0));

				//y axis in world coordinate 
				if( cameraPostionZ < -40)Camera.main.transform.Translate(new Vector3(0,1f,0));
				if( cameraPostionZ > -40)Camera.main.transform.Translate(new Vector3(0,-1f,0));
				if( cameraPostionZ == -40)Camera.main.transform.Translate(new Vector3(0,0,0));

				//z axis in world coordinate don't ever change this one here
				if( cameraPostionY < 20)Camera.main.transform.Translate(new Vector3(0,0,-1f));
				if( cameraPostionY > 20)Camera.main.transform.Translate(new Vector3(0,0,1f));
				if( cameraPostionY == 20)Camera.main.transform.Translate(new Vector3(0,0,0));
			}
			if (CameraPosition [17]) 
			{
				//x axis in world coordinate
				if( cameraPostionX < 30)Camera.main.transform.Translate(new Vector3(1f,0,0));
				if( cameraPostionX > 30)Camera.main.transform.Translate(new Vector3(-1f,0,0));
				if( cameraPostionX == 30)Camera.main.transform.Translate(new Vector3(0,0,0));

				//y axis in world coordinate 
				if( cameraPostionZ < -40)Camera.main.transform.Translate(new Vector3(0,1f,0));
				if( cameraPostionZ > -40)Camera.main.transform.Translate(new Vector3(0,-1f,0));
				if( cameraPostionZ == -40)Camera.main.transform.Translate(new Vector3(0,0,0));

				//z axis in world coordinate don't ever change this one here
				if( cameraPostionY < 20)Camera.main.transform.Translate(new Vector3(0,0,-1f));
				if( cameraPostionY > 20)Camera.main.transform.Translate(new Vector3(0,0,1f));
				if( cameraPostionY == 20)Camera.main.transform.Translate(new Vector3(0,0,0));
			}
			if (CameraPosition [18]) 
			{
				//x axis in world coordinate
				if( cameraPostionX < 30)Camera.main.transform.Translate(new Vector3(1f,0,0));
				if( cameraPostionX > 30)Camera.main.transform.Translate(new Vector3(-1f,0,0));
				if( cameraPostionX == 30)Camera.main.transform.Translate(new Vector3(0,0,0));

				//y axis in world coordinate 
				if( cameraPostionZ < -50)Camera.main.transform.Translate(new Vector3(0,1f,0));
				if( cameraPostionZ > -50)Camera.main.transform.Translate(new Vector3(0,-1f,0));
				if( cameraPostionZ == -50)Camera.main.transform.Translate(new Vector3(0,0,0));

				//z axis in world coordinate don't ever change this one here
				if( cameraPostionY < 20)Camera.main.transform.Translate(new Vector3(0,0,-1f));
				if( cameraPostionY > 20)Camera.main.transform.Translate(new Vector3(0,0,1f));
				if( cameraPostionY == 20)Camera.main.transform.Translate(new Vector3(0,0,0));
			}
			if (CameraPosition [19]) 
			{
				//x axis in world coordinate
				if( cameraPostionX < 20)Camera.main.transform.Translate(new Vector3(1f,0,0));
				if( cameraPostionX > 20)Camera.main.transform.Translate(new Vector3(-1f,0,0));
				if( cameraPostionX == 20)Camera.main.transform.Translate(new Vector3(0,0,0));

				//y axis in world coordinate 
				if( cameraPostionZ < -50)Camera.main.transform.Translate(new Vector3(0,1f,0));
				if( cameraPostionZ > -50)Camera.main.transform.Translate(new Vector3(0,-1f,0));
				if( cameraPostionZ == -50)Camera.main.transform.Translate(new Vector3(0,0,0));

				//z axis in world coordinate don't ever change this one here
				if( cameraPostionY < 20)Camera.main.transform.Translate(new Vector3(0,0,-1f));
				if( cameraPostionY > 20)Camera.main.transform.Translate(new Vector3(0,0,1f));
				if( cameraPostionY == 20)Camera.main.transform.Translate(new Vector3(0,0,0));
			}
			if (CameraPosition [20]) 
			{
				//x axis in world coordinate
				if( cameraPostionX < 10)Camera.main.transform.Translate(new Vector3(1f,0,0));
				if( cameraPostionX > 10)Camera.main.transform.Translate(new Vector3(-1f,0,0));
				if( cameraPostionX == 10)Camera.main.transform.Translate(new Vector3(0,0,0));

				//y axis in world coordinate 
				if( cameraPostionZ < -50)Camera.main.transform.Translate(new Vector3(0,1f,0));
				if( cameraPostionZ > -50)Camera.main.transform.Translate(new Vector3(0,-1f,0));
				if( cameraPostionZ == -50)Camera.main.transform.Translate(new Vector3(0,0,0));

				//z axis in world coordinate don't ever change this one here
				if( cameraPostionY < 20)Camera.main.transform.Translate(new Vector3(0,0,-1f));
				if( cameraPostionY > 20)Camera.main.transform.Translate(new Vector3(0,0,1f));
				if( cameraPostionY == 20)Camera.main.transform.Translate(new Vector3(0,0,0));
			}

			//sprite movement if player is in the same rooms as a bat
			if (ifBats) 
			{
				GameObject temp = GameObject.FindGameObjectWithTag ("batsAnimation");
				Vector3 pos = new Vector3(Camera.main.transform.position.x, 5, Camera.main.transform.position.z-5);
				temp.transform.position = pos;
			}

			//sprite movement if player is in the same rooms as a pit
			if (ifPit) 
			{
				GameObject temp = GameObject.FindGameObjectWithTag ("pitAnimation");
				Vector3 pos = new Vector3(Camera.main.transform.position.x, 5, Camera.main.transform.position.z);
				temp.transform.position = pos;
			}

			//sprite movement if player is in the same rooms as a wumpus
			if (ifWumpus) 
			{
				GameObject temp = GameObject.FindGameObjectWithTag ("wumpusAnimation");
				Vector3 pos = new Vector3(Camera.main.transform.position.x, 5, Camera.main.transform.position.z);
				temp.transform.position = pos;
			}
		}
	}
}
