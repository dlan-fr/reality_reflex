using UnityEngine;
using System.Collections;

public class TriggerDeath : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	void OnTriggerEnter2D(Collider2D other) {

		GameObject player = GameObject.Find("Playercontroller");
		
		Deplacement player_script = player.GetComponent<Deplacement>();

		if(!player_script.isDead)
			GameDataMngr.Singleton.nbreVies--;
	}
}
