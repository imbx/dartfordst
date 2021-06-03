using UnityEngine;

public class Lantern : MonoBehaviour {

    public int reqId;
    [SerializeField] private bool reqIdBool = false;
    public int reqIdUV;
    [SerializeField] private bool reqIdUVBool = false;
    public PrimaryController playerController;
    private Light lanternLight;
    [SerializeField] private bool isLanternActive;
    [SerializeField] private float lanternInputCd = 0f;
    [SerializeField] private LayerMask UVLayer;
    [SerializeField] private LayerMask NotUVLayer;
    private int lanternState = 0;

    [SerializeField] private Color MainLanternColor;
    [SerializeField] private Color UVLanternColor;

    [FMODUnity.EventRef]
    public string eventoSound = "event:/candado";
    

    void OnEnable()
    {
        lanternLight = GetComponent<Light>();
        if(GameController.current) UpdateChecks();
    }

    public void UpdateChecks()
    {
        reqIdBool = GameController.current.database.GetProgressionState(reqId);
        reqIdUVBool = GameController.current.database.GetProgressionState(reqIdUV);
    }

    void Update() 
    {
        UpdateChecks();
        
        if (reqIdBool)
        {
            if(lanternInputCd > 0f) lanternInputCd -= Time.deltaTime;

            if(playerController.isLanternPressed &&
                !isLanternActive &&
                lanternInputCd <= 0f)
            {
                TurnOn(reqIdUVBool);
                // GameController.current.lanternActive = true;
            }

            if(playerController.isLanternPressed &&
                isLanternActive &&
                lanternInputCd <= 0f)
            {
                TurnOff();
            }
        }
        lanternLight.enabled = isLanternActive;
    }

    private void TurnOn(bool isUV = false)
    {
        lanternState = 1;
        
        if(isUV){
            lanternLight.color = UVLanternColor;
            Camera.main.cullingMask = ~(1 << UVLayer);
            lanternLight.cullingMask = ~(1 << UVLayer);
        }
        isLanternActive = true;
        lanternInputCd = 1f;
        GameController.current.music.playMusic(eventoSound);
    }

    private void TurnOff() 
    {
        lanternState = 0;
        isLanternActive = false;
        
        Camera.main.cullingMask = NotUVLayer;
        lanternLight.cullingMask =  -1;
        lanternInputCd = 1f;
        GameController.current.music.StopMusic(eventoSound);
    }
}