using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDataMngr {

	private Dictionary<string,string> level_str = new Dictionary<string, string>()
	{
		{"Menu","message vide"},
		{"Credit","message vide"},
		{"multiverse","Ici abandonne tout espoir..."},
		{"niveau1","Et si la gravité changeait ?"},
		{"niveau2", "Et si la gravité changeait ? 2 le retour"},
		{"niveau3","Le saut est la clé"},
		{"niveau4","De la gravité dans l'espace ?"}
	};


	private string CurrentLevel = "Menu";

	private Vector3 LevelStartPos = Vector3.zero;

	public PlayerEffect currentEffect = PlayerEffect.NONE;
	
	public int nbreReliques = 0;
	private int _nbreVies = 3;
	public int nbreVies 
	{
		get
		{
			return _nbreVies;
		} 
		set
		{

			GameObject player = GameObject.Find("Playercontroller");
			
			Deplacement player_script = player.GetComponent<Deplacement>();

			if(value >= 0)
			{
				_nbreVies=value;

				GameObject.Find("Vies").GetComponent<GUIText>().text = "Vies : "+ _nbreVies;
			}

			if(!player_script.isDead && _nbreVies <= 0)
				player_script.Death();

		}
	}
	
	public bool collision = false;

	public float gravity = 5;

	public List<Effet> graphEffects = new List<Effet>();


	private GameObject music_node = null;

	private AudioClip music_amb_1;
	private AudioClip music_amb_2;

	//private AudioSource music_player = null;

	private const float TRANSITION_TIME = 1000.0f;
	private float switch_level_transtion;


	public PouvoirJoueur PouvJoueur = new PouvoirJoueur();
	
	private static GameDataMngr _singleton = null;
	

	public static GameDataMngr Singleton

	{
		get
		{
			if(_singleton == null)
				_singleton = new GameDataMngr();

			return _singleton;
		}
	}

	public  GameDataMngr()
	{
		switch_level_transtion = TRANSITION_TIME;
	}

	public List<GameObject> CreateHud()
	{
		List<GameObject> HUD = new List<GameObject>();

		if(CurrentLevel != "Menu" && CurrentLevel != "Credit")
		{
			GameObject text = new GameObject ("text_ui", typeof(GUIText));
			text.GetComponent<GUIText>().text = level_str[this.CurrentLevel];
			text.GetComponent<GUIText>().transform.position = new Vector3(10f / (float)Screen.width, 1.0f, 0f); 
			GameObject text2 = new GameObject ("Reliques", typeof(GUIText));
			text2.GetComponent<GUIText>().text = "Reliques : "+ nbreReliques.ToString();
			text2.GetComponent<GUIText>().transform.position = new Vector3(0.9f, 1.0f, 0f); 
			GameObject text3 = new GameObject ("Vies", typeof(GUIText));
			text3.GetComponent<GUIText>().text = "Vies : "+ nbreVies.ToString();
			text3.GetComponent<GUIText>().transform.position = new Vector3(0.8f, 1.0f, 0f); 
			HUD.Add(text);
			HUD.Add(text2);
			HUD.Add(text3);
		}
		else
		{
			//creer objet spécifique au menu

		}

		return HUD;
	}

	public void CreateMusic(AudioClip music_1,AudioClip music_2)
	{
		if(music_node == null)
		{
			music_node = new GameObject("music_node",typeof(AudioSource));
			music_amb_1 = music_1;
			music_amb_2 = music_2;
			music_node.GetComponent<AudioSource>().clip = music_1;
			music_node.GetComponent<AudioSource>().loop = true;
			music_node.GetComponent<AudioSource>().Play();
			Object.DontDestroyOnLoad(music_node);

		}
	}

	public void AfterDeath(GameObject player)
	{
		if (_nbreVies<=0)
		{
			_nbreVies = 3; 
			SetNewLevel("multiverse",PlayerEffect.NONE);
		}
		else
		{
			Respawn(player);
		}
	}


	public void SetRespawn(GameObject player, GameObject respawn)
	{
		respawn.GetComponent<SpriteRenderer>().enabled = false;
		LevelStartPos = respawn.transform.position;
		player.transform.position = LevelStartPos;
	}

	public void Respawn(GameObject player)
	{
		player.transform.position = LevelStartPos;

		currentEffect = PlayerEffect.NONE;
		player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		player.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
		this.ApplyEffect(player);
		

	}

	public GameObject fond1;
	public GameObject fond2;

	private const float FADE_SPEED = 2;

	public void ApplyEffect(GameObject player)
	{
		Quaternion current_rotation = player.GetComponentInChildren<SpriteRenderer>().transform.localRotation;

		switch(currentEffect)
		{
			case PlayerEffect.NONE:
			player.GetComponent<Rigidbody2D>().gravityScale = gravity;
			player.GetComponentInChildren<SpriteRenderer>().transform.localRotation = Quaternion.Euler(current_rotation.eulerAngles.x, current_rotation.eulerAngles.y, 0);

			//Deplacement player_script = player.GetComponent<Deplacement>();
			//player_script.PlayAudioGravity(false);

			//player.GetComponent<Rigidbody2D>().rotation = 0;
			//player.GetComponentInChildren<Camera>().transform.rotation = Quaternion.Euler(0, 0, 0);
			break;
			case PlayerEffect.GRAVITY_INVERSE:
			Deplacement player_script = player.GetComponent<Deplacement>();
			player_script.PlayAudioGravity(true);
			player.GetComponent<Rigidbody2D>().gravityScale = -gravity;
			player.GetComponentInChildren<SpriteRenderer>().transform.localRotation = Quaternion.Euler(current_rotation.eulerAngles.x, current_rotation.eulerAngles.y, 180);
			//player.GetComponent<Rigidbody2D>().rotation = 180;
			//player.GetComponentInChildren<Camera>().transform.rotation = Quaternion.Euler(0, 0, -180);
			break;
			case PlayerEffect.BACKGROUND_FADEOUT:

				fond1 = GameObject.Find("fond1");
			break;
		}
	}



	public void UpdateEffects()
	{
		switch(currentEffect)
		{
			case PlayerEffect.BACKGROUND_FADEOUT:
				if(fond1 != null)
				{
					Color fond_color = fond1.GetComponent<SpriteRenderer>().material.color;

					if(fond_color.a > 0)
						fond1.GetComponent<SpriteRenderer>().material.color = new Color(fond_color.r,fond_color.g,fond_color.b,Mathf.Lerp(fond_color.a,0,Time.deltaTime * FADE_SPEED));
					else
						fond1 = null;

					
				}
				
			break;
		}

		List<Effet> rm = new List<Effet>();

		foreach(Effet ceff in graphEffects)
		{
			ceff.UpdateEffect();

			if(ceff.Ended)
				rm.Add(ceff);
		}

		foreach(Effet crm in rm)
			graphEffects.Remove(crm);


	}

	private string requested_level = string.Empty;
	private PlayerEffect request_effect = PlayerEffect.NONE;

	public bool doTransition = false;

	public void UpdateMngr()
	{
		if(doTransition)
		{
			if(switch_level_transtion <= 0)
			{

				this.CurrentLevel = requested_level;
				Application.LoadLevel(requested_level);

				graphEffects.Clear();

				this.currentEffect = request_effect;
				doTransition = false;
				switch_level_transtion = TRANSITION_TIME;
			}
			else
			{
				switch_level_transtion -= Time.deltaTime * 1000.0f;
			}
		}
	}


	public void SetNewLevel(string newlevel,PlayerEffect effect)
	{
		requested_level = newlevel;
		request_effect = effect;
		doTransition = true;

	}
	


}
