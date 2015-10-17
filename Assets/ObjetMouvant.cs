using UnityEngine;
using System.Collections;

public class ObjetMouvant : MonoBehaviour {
	public float speed;
	public Vector3 depart;
	public Vector3 arrivee;
	
	private bool direction;
	private float prop;
	private float condition;
	private float distancex;
	
	// Use this for initialization
	void Start () {
		direction = false;
		prop=(arrivee.y-depart.y)/((arrivee.x-depart.x)/speed);
		Vector3 d = new Vector3();
		distancex = speed/(arrivee.x-depart.x);
		condition = distancex;
		d.x += speed;
		d.y += prop;
		transform.position=depart+d;
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 deplac = new Vector3();
		distancex = speed/(arrivee.x-depart.x);
		if ((condition<distancex)||(condition>(1-distancex))) {
			direction=!direction;
		}
		if (direction) {
			deplac.x += speed;
			deplac.y += prop;
			condition+=distancex;
		}
		else {
			deplac.x -= speed;
			deplac.y -= prop;
			condition-=distancex;
		}
		
		transform.position += deplac;
	}
	
}
