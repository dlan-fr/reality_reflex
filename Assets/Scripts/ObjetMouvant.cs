using UnityEngine;
using System.Collections;

public class ObjetMouvant : MonoBehaviour {
	public float speed;
	public Vector3 depart;
	public Vector3 arrivee;
	
	public enum SensMouvement
    {
        Horizontal,
        Vertical
    };
	public SensMouvement sens;
	
	private bool direction;
	private float prop;
	private float condition;
	private float distancex;
	private float distancey;
	
	// Use this for initialization
	void Start () {

		depart += transform.parent.position;
		arrivee += transform.parent.position;

		direction = false;
		Vector3 d = new Vector3();
		switch (sens){
			case SensMouvement.Horizontal:
			{
				prop=(arrivee.y-depart.y)/((arrivee.x-depart.x)/speed);
				distancex = speed/(arrivee.x-depart.x);
				condition = distancex;
				d.x += speed;
				d.y += prop;
				break;
			}
			case SensMouvement.Vertical:
			{
				prop=(arrivee.x-depart.x)/((arrivee.y-depart.y)/speed);
				distancey = speed/(arrivee.y-depart.y);
				condition = distancey;
				d.y += speed;
				d.x += prop;
				break;
			}
		}
		transform.position=depart+d;
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 deplac = new Vector3();
		switch(sens){
			case SensMouvement.Horizontal:
			{
				distancex = speed/(arrivee.x-depart.x);
				if ((condition<distancex)||(condition>(1-distancex))) 
				{
					direction=!direction;
				}
				if (direction) 
				{
					deplac.x += speed;
					deplac.y += prop;
					condition+=distancex;
				}
				else 
				{
					deplac.x -= speed;
					deplac.y -= prop;
					condition-=distancex;
				}
				break;
			}
			case SensMouvement.Vertical:
			{
				distancey = speed/(arrivee.y-depart.y);
				if ((condition<distancey)||(condition>(1-distancey))) 
				{
					direction=!direction;
				}
				if (direction) 
				{
					deplac.y += speed;
					deplac.x += prop;
					condition+=distancey;
				}
				else 
				{
					deplac.y -= speed;
					deplac.x -= prop;
					condition-=distancey;
				}
				break;
			}
		}
		transform.position += deplac;
	}
	
}
