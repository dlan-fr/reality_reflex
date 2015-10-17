using UnityEngine;
using System.Collections;

public class Effet {

	private GraphicEffect typeEffet = GraphicEffect.GUI_FADEOUT;

	private GameObject refGameobject;

	private float fade_speed = 2.0f;

	private bool effectEnded = false;

	public bool Ended
	{
		get
		{
			return effectEnded;
		}
	}

	public Effet(GameObject gameobject,GraphicEffect effet,float ofade_speed)
	{
		typeEffet = effet;

		refGameobject = gameobject;

		ofade_speed = fade_speed;
		effectEnded = false;

	}

	public void UpdateEffect()
	{
		switch(this.typeEffet)
		{
			case GraphicEffect.FADEOUT:
				Material mat = refGameobject.GetComponent<SpriteRenderer>().material;
				DoFade(mat);
			break;
			case GraphicEffect.GUI_FADEOUT:
				Material mat2 =  refGameobject.GetComponent<GUIText>().material;
				DoFade(mat2);
			break;
		}

	}

	public void DoFade(Material mat)
	{
		if(mat.color.a > 0)
			mat.color = new Color(mat.color.r,mat.color.g,mat.color.b,Mathf.Lerp(mat.color.a,0,Time.deltaTime * fade_speed));
		else
			effectEnded = true;
	}




}
