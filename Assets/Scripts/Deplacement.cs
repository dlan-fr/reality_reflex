using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deplacement : MonoBehaviour {
	public float speed;
	public float jump;

	private Animator animator;

	const int STATE_IDLE = 0;
	const int STATE_WALK = 1;

	private int _currentAnimationState;

	private string _currentDirection = "right";

	private SpriteRenderer sprite;
	
	
	// Use this for initialization
	void Start () {
		GameDataMngr.Singleton.SetRespawn(GameObject.Find("Playercontroller"),GameObject.Find("Respawn"));
		GameDataMngr.Singleton.ApplyEffect(GameObject.Find("Playercontroller"));

		List<GameObject> liste =  GameDataMngr.Singleton.CreateHud();

		GameDataMngr.Singleton.graphEffects.Add(new Effet(GameObject.Find("text_ui"),GraphicEffect.GUI_FADEOUT,2));

		animator = GetComponentInChildren<Animator>();
		_currentAnimationState = STATE_IDLE;

		sprite = GetComponentInChildren<SpriteRenderer>();
		

	}

	void changeState(int state){
		
		if (_currentAnimationState == state)
			return;
		
		switch (state) {
			
			case STATE_WALK:
				//animator.Play(0);
				animator.SetInteger ("state", STATE_WALK);
				break;
				
			case STATE_IDLE:
				//animator.Play(1);
				animator.SetInteger ("state", STATE_IDLE);
				break;
			

			
		}
		
		_currentAnimationState = state;
	}

	void changeDirection(string direction)
	{
		
		if (_currentDirection != direction)
		{
			if (direction == "right")
			{
				sprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
				_currentDirection = "right";
			}
			else if (direction == "left")
			{
				sprite.transform.localRotation = Quaternion.Euler(0, 180, 0);
				_currentDirection = "left";
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 deplac = new Vector3();
 
                if (Input.GetKey (KeyCode.Q)) {
                        deplac.x -= speed;
					changeDirection("left");

                }
 
                if (Input.GetKey (KeyCode.D)) {
                        deplac.x += speed; 
					changeDirection("right");
                }
				
				if(Input.GetKeyDown(KeyCode.Z))// && (GameDataMngr.Singleton.collision))
				{
					GetComponent<Rigidbody2D>().AddForce(new Vector2(0,100*jump));
				}

			if(deplac != Vector3.zero)
				changeState(STATE_WALK);
			else
				changeState(STATE_IDLE);

                transform.position += deplac;

		//float progress = (int)((animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f) * 100);

			//Debug.Log("rank ? "+progress);

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
