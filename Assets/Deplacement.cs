using UnityEngine;
using System.Collections;

public class Deplacement : MonoBehaviour {
	public float speed;
	public float jump;
	
	private Vector3 zeroPlateForme;
	private Vector3 decalage = Vector3.zero;
	
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
				deplac+=decalage;
 
                transform.position += deplac;
	}
	
	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag=="PF"){
			zeroPlateForme=coll.transform.position;
			this.GetComponent<Rigidbody2D>().gravityScale=0;
			Debug.Log("Collision");
		}
	}
	
	void OnCollisionStay2D(Collision2D coll){
		if (coll.gameObject.tag=="PF"){
			decalage=coll.transform.position-zeroPlateForme;
			zeroPlateForme=coll.transform.position;
		}
	}
	
	void OnCollisionExit2D(Collision2D coll){
		if (coll.gameObject.tag=="PF"){
			zeroPlateForme=Vector3.zero;
			this.GetComponent<Rigidbody2D>().gravityScale=1;
			decalage=Vector3.zero;
		}
	}
}
