using UnityEngine;
using System.Collections;

public class Scene1_script : MonoBehaviour {


	public GameObject player;


	// Use this for initialization
	void Start () {



	
	}
	
	// Update is called once per frame
	void Update () {


		if(Input.GetKey(KeyCode.LeftArrow))
			player.transform.position -= new Vector3(0.1f,0,0);

		if(Input.GetKey(KeyCode.RightArrow))
			player.transform.position += new Vector3(0.1f,0,0);

	
	}


}
