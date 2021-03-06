﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Deplacement : MonoBehaviour {
	public float speed;
	public float jump;

	private Animator animator;

	const int STATE_IDLE = 0;
	const int STATE_WALK = 1;
	const int STATE_DEATH = 2;
	const int STATE_JUMP = 3;

	private int _currentAnimationState;

	private string _currentDirection = "right";

	public string CurrentDirection
	{
		get
		{
			return _currentDirection;
		}
	}

	private float VelmaxX = 10.0f;

	private float VelmaxY = 10.0f;
	
	public AudioClip son_saut;
	public AudioClip son_gravite;
	public AudioClip son_inverse_gravite;

	public AudioClip[] sons_pas;

	public AudioClip son_ambi_1;
	public AudioClip son_ambi_2;

	public Font steamfont;


	public Sprite default_sprite;

	public Sprite sprite_morte;



	private SpriteRenderer sprite;

	private CircleCollider2D feet;
	private CircleCollider2D head;


	private AudioSource sound_player;

	public bool isDead = false;

	public bool IsGrave = false;

	private const float DEATH_TIME = 2000.0f;

	private float DeathTimeout = DEATH_TIME;
	
	// Use this for initialization
	void Start () {
		GameDataMngr.Singleton.SetRespawn(GameObject.Find("Playercontroller"),GameObject.Find("Respawn"));
		GameDataMngr.Singleton.ApplyEffect(GameObject.Find("Playercontroller"));

		List<GameObject> liste =  GameDataMngr.Singleton.CreateHud(steamfont);
		GameDataMngr.Singleton.CreateMusic(son_ambi_1,son_ambi_2);


		if(liste.Count > 0)
			GameDataMngr.Singleton.graphEffects.Add(new Effet(GameObject.Find("text_ui"),GraphicEffect.GUI_FADEOUT,2));

		animator = GetComponentInChildren<Animator>();
		_currentAnimationState = STATE_IDLE;
		sprite = GetComponentInChildren<SpriteRenderer>();
		CircleCollider2D[] val = GetComponentsInChildren<CircleCollider2D>();

		foreach(CircleCollider2D oval in val)
		{
			if(oval.name =="Pied")
				feet = oval;
			else if(oval.name == "Tete")
				head = oval;

		}

		sound_player = GetComponent<AudioSource>();

		GameObject obj = GameObject.Find("end_text");

		if(GameDataMngr.Singleton.gameended)
		{
			if(obj != null)
			{
				obj.GetComponentInChildren<Text>().enabled = true;
				GameObject text4 = new GameObject ("Resultat", typeof(GUIText));
				text4.GetComponent<GUIText>().font = steamfont;
				text4.GetComponent<GUIText>().text = "Reliques ramassées : "+ GameDataMngr.Singleton.nbreReliques.ToString();
				text4.GetComponent<GUIText>().transform.position = new Vector3(0.6f, 0.6f, 0f); 
				text4.GetComponent<GUIText>().fontSize = 36;
				if (GameDataMngr.Singleton.nbreReliques==7){
					GameObject text5 = new GameObject ("Resultat2", typeof(GUIText));
					text5.GetComponent<GUIText>().font = steamfont;
					text5.GetComponent<GUIText>().text = "Vous avez ramassé le nombre max de reliques!";
					text5.GetComponent<GUIText>().transform.position = new Vector3(0.5f, 0.5f, 0f); 
					text5.GetComponent<GUIText>().fontSize = 24;
				}
				
			}
		}
		else
		{
			if(obj != null)
			{
				obj.GetComponentInChildren<Text>().enabled = false;
			}
		}



	}


	public void changeState(int state){
		
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

			case STATE_DEATH:
				animator.SetInteger ("state", STATE_DEATH);
			break;

			case STATE_JUMP:
				animator.SetInteger("state",STATE_JUMP);
			break;
			

			
		}

		_currentAnimationState = state;
	}

	public void changeDirection(string direction,bool overridedir)
	{
		if(sprite == null)
			return;

		Quaternion current_rotation = GetComponentInChildren<SpriteRenderer>().transform.localRotation;


		if (_currentDirection != direction || overridedir)
		{
			if (direction == "right")
			{
				sprite.transform.localRotation = Quaternion.Euler(current_rotation.eulerAngles.x, (current_rotation.eulerAngles.z == 0.0f)?0.0f:180.0f, current_rotation.eulerAngles.z);
				_currentDirection = "right";
			}
			else if (direction == "left")
			{
				sprite.transform.localRotation = Quaternion.Euler(current_rotation.eulerAngles.x, (current_rotation.eulerAngles.z == 0.0f)?180.0f:0.0f, current_rotation.eulerAngles.z);
				_currentDirection = "left";
			}
		}
		
	}

	public void PlayAudioGravity(bool inversegravity)
	{
		sound_player.PlayOneShot((inversegravity)?son_inverse_gravite:son_gravite);
	}

	public void Death()
	{
		if(!isDead)
		{
			isDead = true;
			sprite.sprite = sprite_morte;
			Vector2 currentvelocity = GetComponent<Rigidbody2D>().velocity;

			GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f,currentvelocity.y);
			changeState(STATE_DEATH);
		}

	}

	float dep = 50.0f;

	bool checkonair(Vector2 force,Vector2 velocity)
	{

		if(GetComponent<Rigidbody2D>().gravityScale > 0)
			return (force.y > 0.1f || velocity.y > 0.1f);
		else if(GetComponent<Rigidbody2D>().gravityScale < 0)
			return (force.y < -0.1f || velocity.y < -0.1f);
		else
			return false;

	}
	
	// Update is called once per frame
	void Update () {

		if(GameDataMngr.Singleton.doTransition)
		{
			changeState(STATE_IDLE);
			GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f,0.0f);

			GameDataMngr.Singleton.UpdateMngr();
			return;
		}


		if(isDead)
		{
			DeathTimeout -= Time.deltaTime * 1000.0f;

			if(DeathTimeout <= 0.0f)
			{
				DeathTimeout = DEATH_TIME;
				sprite.sprite = default_sprite;
				changeState(STATE_IDLE);
				isDead = false;
				GameDataMngr.Singleton.AfterDeath(this.gameObject);
			}

			return;
		}

		bool onground = false;
		bool onslope = false;

		int layer_u = (1 << LayerMask.NameToLayer("layerslope"));


		if(GetComponent<Rigidbody2D>().gravityScale > 0)
		{
			onslope = feet.IsTouchingLayers(layer_u);
			onground = feet.IsTouchingLayers(1) || onslope;

		}
		else if(GetComponent<Rigidbody2D>().gravityScale < 0)
		{
			onslope = head.IsTouchingLayers(layer_u);
			onground = head.IsTouchingLayers(1) || onslope;

		}

	


		Vector2 force = new Vector2();

		bool pressed = false;

		Vector2 currentvelocity = GetComponent<Rigidbody2D>().velocity;

		//Vector3 deplac = new Vector3();
 
		//if (Input.GetKey (KeyCode.Q) ) {
		if (Input.GetAxis("Horizontal")<0){
			if(Mathf.Abs(currentvelocity.x) < VelmaxX)

				if(onslope)
				{
					force.x = -dep;
					force.y = dep;
				}
				else
				{
					force.x = -dep;
				}
				changeDirection("left",false);
				pressed = true;

                }
 
		if (Input.GetAxis("Horizontal")>0 ) {

			if(Mathf.Abs(currentvelocity.x) < VelmaxX)

				if(onslope)
				{
					force.x = dep;
					force.y = dep;
				}
				else
				{
					force.x = dep;
				}
			// deplac.x += speed; 
					changeDirection("right",false);
			pressed = true;
                }

		
			if((Input.GetButtonDown("Jump")) && onground)
			{
				sound_player.PlayOneShot(son_saut);

				
				
				if(Mathf.Abs(currentvelocity.y) < VelmaxY)
					force.y = (70 * jump) * Mathf.Sign(GetComponent<Rigidbody2D>().gravityScale);

				pressed = true;
			}



		if(checkonair(force,currentvelocity) && !onslope)
			changeState(STATE_JUMP);



		if(pressed)
			GetComponent<Rigidbody2D>().AddForce(force,ForceMode2D.Force);
		else
			GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f,currentvelocity.y);
			

		if(pressed)
		{
			if(onground)
			{
				if(!sound_player.isPlaying)
				{
					//sélection d'un son de pas au hazard
					int rand_son = Random.Range(0,sons_pas.Length-1);

					sound_player.PlayOneShot(sons_pas[rand_son]);
				}

				if(_currentAnimationState != STATE_JUMP || !checkonair(force,currentvelocity) || onslope )
					changeState(STATE_WALK);


			}
		}
		else
		{
			if((onground && !checkonair(force,currentvelocity)) || onslope)
				changeState(STATE_IDLE);
		}

		//if(_currentAnimationState == STATE_JUMP && currentvelocity.y <= 0 && onground && !justjump)
			//changeState(STATE_IDLE);

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
		
		//Ramassage de vies
		if (coll.gameObject.tag=="Vie"){
			GameDataMngr.Singleton.nbreVies++;
			coll.gameObject.GetComponent<Renderer>().enabled = false;
			coll.gameObject.GetComponent<Collider2D>().enabled = false;
			GameObject.Find("Vies").GetComponent<GUIText>().text = "Vies : "+ GameDataMngr.Singleton.nbreVies.ToString();
		}
	}
	
	void OnCollisionExit2D(Collision2D coll){
		if (coll.gameObject.tag=="PF"){
			transform.parent=null;
		}
	}
	
}
