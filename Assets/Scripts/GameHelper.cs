using UnityEngine;

public class GameHelper : MonoBehaviour
{
    public AudioClip FullRowSound;
    public AudioClip LandSound;
    public AudioClip FlipSound;
    public AudioClip MoveSound;

    private static AudioSource _audioSource;
    private static AudioClip _fullRowSound;
    private static AudioClip _landSound;
    private static AudioClip _flipSound;
    private static AudioClip _moveSound;

    private const float FullRowSoundDuration = 0.6f;
    private const float LandSoundDuration = 0.2f;
    private const float FlipSoundDuration = 0.131f;
    private const float MoveSoundDuration = 0.049f;

    private static float _willPlayTillTime;
    private static int _currentSoundPriority;

    public Rect ScorePosition;
    private static int _score = 0;
    public GUISkin ScoreboardSkin;

    public static float GameSpeed = 0.9f;
    private const float gameSpeedStep = 0.07f;

    void Awake()
    {        
        _fullRowSound = FullRowSound;
        _landSound = LandSound;
        _flipSound = FlipSound;
        _moveSound = MoveSound;

        _willPlayTillTime = Time.time;
        _audioSource = GetComponent<AudioSource>();
        _currentSoundPriority = 0;
    }

    public static void PlayFullRawSound()
    {
        PlaySound(_fullRowSound, FullRowSoundDuration, 5);
    }

    public static void PlayLandSound()
    {
        PlaySound(_landSound, LandSoundDuration, 4);
    }

    public static void PlayFlipSound()
    {
        PlaySound(_flipSound, FlipSoundDuration, 3, 0.6f);
    }

    public static void PlayMoveSound()
    {
        PlaySound(_moveSound, MoveSoundDuration, 3, 0.5f);
    }

    private static void PlaySound(AudioClip sound, float duration, int priority = 0, float volume = 1)
    {
        if (_willPlayTillTime < Time.time || _currentSoundPriority < priority)
        {
            if (_currentSoundPriority < priority)
                _audioSource.Stop();
            
            _currentSoundPriority = priority;
            _audioSource.PlayOneShot(sound, volume);
            _willPlayTillTime = Time.time + duration;
        }
    }

    public static void IncreaseScore()
    {
        _score ++;
        if(_score % 5 == 0)
            GameSpeed -= gameSpeedStep;
    }

    void OnGUI()
    {
        var w = Screen.width/100;
        var h = Screen.height/100;
        GUI.skin = ScoreboardSkin;
        GUI.Label(
            new Rect(ScorePosition.x*w, ScorePosition.y*h, ScorePosition.width*w, ScorePosition.height*h),
            "Score: " + _score);
    }
}
