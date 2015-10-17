using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deplacement : MonoBehaviour {
	public float speed;
	public float jump;
	
	
	// Use this for initialization
	void Start () {
		GameDataMngr.Singleton.SetRespawn(GameObject.Find("Playercontroller"),GameObject.Find("Respawn"));
		GameDataMngr.Singleton.ApplyEffect(GameObject.Find("Playercontroller"));

		List<GameObject> liste =  GameDataMngr.Singleton.CreateHud();

		/*foreach(GameObject t in liste){
			Instantiate(t);
		}*/

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
						deplac.y += jump*this.GetComponent<Rigidbody2D>().gravityScale;
				}
                transform.position += deplac;

		GameDataMngr.Singleton.UpdateEffects();
	}

	
	void OnCollisionEnter2D(Collision2D coll){
		//Déplacement avec les plateformes
		if (coll.gameObject.tag=="PF"){
			transform.parent=coll.transform;
		}
		
		//Ramassage de reliques
		if (coll.gameObject.tag=="Relique"){
			GameDataMngr.Singleton.nbreReliques++;
			coll.gameObject.GetComponent<Renderer>().enabled = false;
			coll.gameObject.GetComponent<Collider2D>().enabled = false;
			GameObject.Find("Reliques").GetComponent<GUIText>().text = "Reliques : "+ GameDataMngr.Singleton.nbreReliques.ToString();
		}
	}
	
	void OnCollisionExit2D(Collision2D coll){
		if (coll.gameObject.tag=="PF"){
			transform.parent=null;
		}
	}
	
}
