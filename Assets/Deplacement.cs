using UnityEngine;
using System.Collections;

public class Deplacement : MonoBehaviour {
	public float speed;
	public float jump;
	// Use this for initialization
	void Start () {
		GameDataMngr.Singleton.SetRespawn(GameObject.Find("Playercontroller"),GameObject.Find("Respawn"));
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 deplac = new Vector3();
 
                if (Input.GetKey (KeyCode.Q)) {
                        deplac.x -= speed;
                }
 
                if (Input.GetKey (KeyCode.D)) {
                        deplac.x += speed; 
                }
				
				if (Input.GetKey (KeyCode.Z)) {
						deplac.y += jump;
				}
 
                transform.position += deplac;
	}
}
