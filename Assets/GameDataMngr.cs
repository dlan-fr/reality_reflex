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

	
	private static GameDataMngr _singleton = null;
	
	public Vector3 PositionPlateForme = Vector3.zero;

	public static GameDataMngr Singleton

	{
		get
		{
			if(_singleton == null)
				_singleton = new GameDataMngr();

			return _singleton;
		}
	}

	public GameObject CreateHud()
	{
		GameObject text = new GameObject ("text_ui", typeof(GUIText));
		text.GetComponent<GUIText>().text = level_str[this.CurrentLevel];
		text.GetComponent<GUIText>().transform.position = new Vector3(10f / (float)Screen.width, 1.0f, 0f); 
		return text;
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
		}
	}


	public void SetNewLevel(string newlevel,SpecialEffect effect)
	{
		this.CurrentLevel = newlevel;
		Application.LoadLevel(newlevel);

		this.currentEffect = effect;
	}
	



}
