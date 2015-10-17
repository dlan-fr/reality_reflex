using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDataMngr {

	private Dictionary<string,string> level_str = new Dictionary<string, string>()
	{
		{"niveau1","Et si la gravité changeait ?"},
		{"niveau2", "Et si la gravité changeait ? 2 le retour"}
	};


	private string CurrentLevel = "niveau1";

	private Vector3 LevelStartPos = Vector3.zero;

	public SpecialEffect currentEffect = SpecialEffect.NONE;
	
	public int nbreReliques = 0;
	public int nbreMorts = 0;

	
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

	public List<GameObject> CreateHud()
	{
		List<GameObject> HUD = new List<GameObject>();
		GameObject text = new GameObject ("text_ui", typeof(GUIText));
		text.GetComponent<GUIText>().text = level_str[this.CurrentLevel];
		text.GetComponent<GUIText>().transform.position = new Vector3(10f / (float)Screen.width, 1.0f, 0f); 
		GameObject text2 = new GameObject ("Reliques", typeof(GUIText));
		text2.GetComponent<GUIText>().text = "Reliques : "+ nbreReliques.ToString();
		text2.GetComponent<GUIText>().transform.position = new Vector3(0.9f, 1.0f, 0f); 
		HUD.Add(text);
		HUD.Add(text2);
		return HUD;
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

		currentEffect = SpecialEffect.NONE;
		player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		player.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
		this.ApplyEffect(player);

	}

	public GameObject fond1;
	public GameObject fond2;

	private const float FADE_SPEED = 2;

	public void ApplyEffect(GameObject player)
	{
		switch(currentEffect)
		{
			case SpecialEffect.NONE:
				player.GetComponent<Rigidbody2D>().gravityScale = 1;
			break;
			case SpecialEffect.GRAVITY_INVERSE:
				player.GetComponent<Rigidbody2D>().gravityScale = -1;
			break;
			case SpecialEffect.BACKGROUND_FADEOUT:

				fond1 = GameObject.Find("fond1");
				

			break;
		}
	}



	public void UpdateEffects()
	{
		switch(currentEffect)
		{
			case SpecialEffect.BACKGROUND_FADEOUT:
				if(fond1 != null)
				{
					Color fond_color = fond1.GetComponent<SpriteRenderer>().material.color;

					if(fond_color.a > 0)
						fond1.GetComponent<SpriteRenderer>().material.color = new Color(fond_color.r,fond_color.g,fond_color.b,Mathf.Lerp(fond_color.a,0,Time.deltaTime * FADE_SPEED));
					else
						fond1 = null;

					
				}
				
			break;
			/*case SpecialEffect.GUI_FADEOUT:
				
			break;*/
		}

	}


	public void SetNewLevel(string newlevel,SpecialEffect effect)
	{
		this.CurrentLevel = newlevel;
		Application.LoadLevel(newlevel);

		this.currentEffect = effect;
	}
	



}
