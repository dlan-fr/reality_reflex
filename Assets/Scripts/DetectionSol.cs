using UnityEngine;
using System.Collections;

public class DetectionSol : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnEnterCollision2D(Collision2D coll)
	{
		GameDataMngr.Singleton.collision = true;
		Debug.Log("sol");
	}
	
	void OnStayCollision2D(Collision2D coll)
	{
		GameDataMngr.Singleton.collision = true;
		
		Debug.Log("sols");
	}
	
	void OnExitCollision2D(Collision2D coll)
	{
		GameDataMngr.Singleton.collision = false;
		
		Debug.Log("solq");
	}
}
