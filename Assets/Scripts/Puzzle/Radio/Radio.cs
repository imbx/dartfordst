using UnityEngine;
using System.Collections.Generic;
using BoxScripts;
using UnityEngine.UI;

public class Radio : InteractBase {
    [Header("Radio Parameters")]
    public int Identifier = 1243;
    public PrimaryController controller;
    public EntityData player;
    private bool isRadioOn = false;
    public List<RadioSound> sequence;
    private int currentSeqPos = 0;
    private AudioSource audioSource;
    [SerializeField] private AudioSource bgAudioSource;
    [SerializeField] private AudioClip Beep;
    [SerializeField] private AudioClip Boop;
    private float delay = 0.5f;
    private bool hasGivenPage = false;

    private float FrequencyLerp = 0f;
    public Vector2 FrequencyInterval = new Vector2(70, 75);

    public Vector2 FinalFrequencyInterval = new Vector2(73.3f, 73.8f);

    private float currentFreq = 70f;

    private bool hasPressedRight = false;

    public Text uiText;

    void OnEnable()
    {
        if(GameController.current.database.ProgressionExists(Identifier)){
            Debug.Log("Deactivating " + name);
        }
        else GameController.current.database.AddProgressionID(Identifier);

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

            tag = (isRadioOn) ? "RadioOn": "RadioOff";

            if(isRadioOn)
            {
                currentSeqPos = 0;

                
            }
            else
            {
                audioSource.Pause();
                bgAudioSource.Pause();
            }
        }  
        hasPressedRight = !isLeftAction;
    }

    void Update()
    {
        if(isRadioOn && !audioSource.isPlaying && delay <= 0f)
        {
            if(!bgAudioSource.isPlaying) bgAudioSource.Play();
            if(!audioSource.isPlaying && currentFreq < FinalFrequencyInterval.y && currentFreq > FinalFrequencyInterval.x)
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
                if(!hasGivenPage && currentSeqPos == sequence.Count - 1)
                {
                    hasGivenPage = true;
                    Debug.Log("Youll get a diary page");
                    GameController.current.database.EditProgression(Identifier);
                }
                if(currentSeqPos >= sequence.Count) currentSeqPos = 0;
            }
        } else delay -= Time.deltaTime;

        if(isRadioOn && hasPressedRight && controller.isInput2Hold)
        {
            gameControllerObject.isInPuzzle = true;
            player.CanMove = false;
            player.CanLook = false;

            int mouseDir = BoxScripts.BoxUtils.ConvertTo01((int)controller.CameraAxis.x);
            currentFreq = Mathf.Lerp(FrequencyInterval.x, FrequencyInterval.y, FrequencyLerp);
            FrequencyLerp += mouseDir * 0.25f * Time.deltaTime;
            uiText.text = (Mathf.Round(currentFreq * 10f) /10f).ToString();
            Debug.Log("[Radio] Current freq : " + (Mathf.Round(currentFreq * 10f) /10f));
        }

        if(hasPressedRight && !controller.isInput2Pressed)
        {
            gameControllerObject.isInPuzzle = false;
            hasPressedRight = false;
            player.CanLook = true;
            player.CanMove = true;
        }
    }
    
}