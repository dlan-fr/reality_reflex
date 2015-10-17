using UnityEngine;
using System.Collections;

public class GameDataMngr {

	private string CurrentLevel = "scene_1";
	private string data = "balal";

	private Vector3 LevelStartPos = Vector3.zero;

	public SpecialEffect currentEffect = SpecialEffect.NONE;

	
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


	public void SetRespawn(GameObject player, GameObject respawn)
	{
		respawn.GetComponent<SpriteRenderer>().enabled = false;
		LevelStartPos = respawn.transform.position;
		player.transform.position = LevelStartPos;
	}

	public void Respawn(GameObject player)
	{
		player.transform.position = LevelStartPos;
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
