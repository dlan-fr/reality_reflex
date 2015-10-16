using UnityEngine;
using System.Collections;

public class TriggerPortal : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


	
	}

	void OnTriggerEnter2D(Collider2D other) {
		GameDataMngr.Singleton.SetNewLevel("scene_2");
	}

}
