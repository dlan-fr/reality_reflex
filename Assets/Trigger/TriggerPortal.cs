using UnityEngine;
using System.Collections;

public class TriggerPortal : MonoBehaviour {

	public PlayerEffect currentEffect;
	public string ToScene = "scene_2";
	public PortalBehav currentBehav;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


	
	}

	void OnTriggerEnter2D(Collider2D other) {

		if(currentBehav == PortalBehav.SWITCH_LEVEL)
			GameDataMngr.Singleton.SetNewLevel(ToScene,currentEffect);
		else if(currentBehav == PortalBehav.SWITCH_MECHANICS)
		{
			GameDataMngr.Singleton.currentEffect = currentEffect;
			GameDataMngr.Singleton.ApplyEffect(GameObject.Find("Playercontroller"));
		}
	}

}
