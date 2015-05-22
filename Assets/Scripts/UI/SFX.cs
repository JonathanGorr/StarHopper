using UnityEngine;
using System.Collections;

public class SFX : MonoBehaviour {

	public AudioClip[] collect;
	public AudioClip[] jetPack;
	public AudioClip[] hurt;
	public AudioClip[] jump;

	public void Play(string clip)
	{
		GetComponent<AudioSource>().PlayOneShot(collect[Random.Range(0, collect.Length)]);
	}
}
