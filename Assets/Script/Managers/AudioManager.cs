using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	#region Singleton
	public static AudioManager m_instance;
	void Awake(){
		if(m_instance == null){
			//If I am the first instance, make me the Singleton
			m_instance = this;
			DontDestroyOnLoad(this.gameObject);
		}else{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != m_instance)
				Destroy(this.gameObject);
		}
	}
	#endregion Singleton

	[SerializeField]
	private AudioClip m_menuAudioClip;
	[SerializeField]
	private AudioClip m_fightAudioClip;

	private static Transform m_transform;

	// Use this for initialization
	void Start () {
		m_transform = this.transform;
		PlayMenuMusic ();
	}

	public void PlayMenuMusic(){
		//Create an empty game object
		GameObject go = new GameObject ("Audio_" +  m_menuAudioClip.name);
		go.transform.parent = null;

		//Add and bind an audio source
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = m_menuAudioClip;
		//Play and destroy the component
		source.Play();
		source.loop = true;
	}


	public void PlayFightMusic(){
		//Create an empty game object
		GameObject go = new GameObject ("Audio_" +  m_fightAudioClip.name);
		go.transform.parent = null;

		//Add and bind an audio source
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = m_fightAudioClip;
		//Play and destroy the component
		source.Play();
		source.loop = true;
	}

	public static void PlayMenuMusic(string clipname){
		//Create an empty game object
		GameObject go = new GameObject ("Audio_" +  clipname);
		go.transform.parent = m_transform;
		//Load clip from ressources folder
		AudioClip newClip =  Instantiate(Resources.Load (clipname, typeof(AudioClip))) as AudioClip;

		//Add and bind an audio source
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = newClip;
		//Play and destroy the component
		source.Play();
		Destroy (go, newClip.length);

	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
