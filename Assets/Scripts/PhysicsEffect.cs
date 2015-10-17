using UnityEngine;
using System.Collections;

public class PhysicsEffect : MonoBehaviour {

	private Animator animator;
	private SpriteRenderer render;

	public AudioClip son_effet;



	public float timewait = 500.0f;

	private float c_timewait;

	private AudioSource sound_player;

	// Use this for initialization
	void Start () {

		animator = GetComponent<Animator>();
		render = GetComponent<SpriteRenderer>();
		c_timewait = timewait;

		sound_player = GetComponent<AudioSource>();

		sound_player.PlayOneShot(son_effet);
	
	}
	
	// Update is called once per frame
	void Update () {

		if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
		{
			render.enabled = false;

			if(c_timewait <= 0.0f)
			{
				animator.Play(0);
				c_timewait = timewait;
				render.enabled = true;
				sound_player.PlayOneShot(son_effet);
			}
			else
			{
				c_timewait -= Time.deltaTime * 1000.0f;
			}
		}
	
	}

	void OnTriggerStay2D(Collider2D other) {
		if(render.enabled)
		{
			GameDataMngr.Singleton.nbreVies--;
			GameDataMngr.Singleton.Respawn(other.gameObject);
		}
	}
}
