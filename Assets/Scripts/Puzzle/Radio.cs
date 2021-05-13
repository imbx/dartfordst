using UnityEngine;
using System.Collections.Generic;
using BoxScripts;

public class Radio : InteractBase {
    
    private bool isRadioOn = false;
    public List<RadioSound> sequence;
    private int currentSeqPos = 0;
    private AudioSource audioSource;
    [SerializeField] private AudioSource bgAudioSource;
    [SerializeField] private AudioClip Beep;
    [SerializeField] private AudioClip Boop;
    private float delay = 0.5f;

    private float rad = 0.00f;




    private bool hasGivenPage = false;
    
    void OnEnable()
    {
        isRadioOn = false;
        audioSource = GetComponent<AudioSource>();
        audioSource.Pause();
        bgAudioSource.Pause();
    }

    public override void Execute(bool isLeftAction = false)
    {
        if(isLeftAction)
        {
            isRadioOn = !isRadioOn;

            if(isRadioOn)
            {
                currentSeqPos = 0;

                if(!hasGivenPage)
                {
                    hasGivenPage = true;
                    GameController.current.database.AddNotePage(1243, "...-..--.");
                }
            }
            else
            {
                audioSource.Pause();
                bgAudioSource.Pause();
            }
        }
    }

    void Update()
    {
        if(isRadioOn && !audioSource.isPlaying && delay <= 0f)
        {
            if(!bgAudioSource.isPlaying) bgAudioSource.Play();
            if(!audioSource.isPlaying)
            {
                delay = 0.5f;
                RadioSound rs = sequence[currentSeqPos];
                switch(rs)
                {
                    case RadioSound.Beep:
                        audioSource.clip = Beep;
                        audioSource.Play();
                        break;
                    case RadioSound.Peep:
                        audioSource.clip = Boop;
                        audioSource.Play();
                        break;
                    default:
                        break;
                }
                currentSeqPos++;
                if(currentSeqPos >= sequence.Count) currentSeqPos = 0;
            }
        } else delay -= Time.deltaTime;
    }
    
}