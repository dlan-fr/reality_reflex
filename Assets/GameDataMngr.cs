using UnityEngine;
using System.Collections;

public class GameDataMngr {

	private string CurrentLevel = "scene_1";
	private string data = "balal";

	public string odata
	{
		get
		{
			return data;
		}
	}

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

	public void setdata(string data)
	{
		this.data = data;
	}

	public void SetNewLevel(string newlevel)
	{
		this.CurrentLevel = newlevel;
		Application.LoadLevel(newlevel);
	}



}
