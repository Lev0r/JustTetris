using UnityEngine;
using System.Collections;

public class GameHelper : MonoBehaviour
{
    public AudioClip FullRowSound;
    private static AudioSource _audioSource;
    private static AudioClip _fullRowSound;

    private static float _fullRowSoundDuration = 0.6f;
    private static float _willPlayTillTime;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _fullRowSound = FullRowSound;
        _willPlayTillTime = Time.time;
    }

    // Use this for initialization
    void Start () { }
	
	// Update is called once per frame
	void Update () { }

    public static void PlayFullRawSound()
    {
        if (_willPlayTillTime < Time.time)
        {
            _audioSource.PlayOneShot(_fullRowSound);
            _willPlayTillTime = Time.time + _fullRowSoundDuration;
        }
    }
}
