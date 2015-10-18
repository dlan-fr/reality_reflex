using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PouvoirJoueur
{

	public enum ListePouvoir
	{
		SAUT
	}

	private List<ListePouvoir> _pouvoirObtenus = new List<ListePouvoir>();

	public PouvoirJoueur ()
	{

	}

	public void AjoutPouvoir(ListePouvoir newpov)
	{
		if(!_pouvoirObtenus.Contains(newpov))
			_pouvoirObtenus.Add(newpov);
	}

	public bool PossedePouvoir(ListePouvoir pov)
	{
		return _pouvoirObtenus.Contains(pov);
	}
}



