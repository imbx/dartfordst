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
    private int lanternState = 0;

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
        if(reqIdBool)
        {
            if(lanternInputCd > 0f) lanternInputCd -= Time.deltaTime;

            if(playerController.isLanternPressed &&
                !isLanternActive &&
                lanternInputCd <= 0f)
            {
                TurnOn();
                // GameController.current.lanternActive = true;
            }

            if(playerController.isLanternPressed &&
                isLanternActive &&
                lanternInputCd <= 0f)
            {
                if(reqIdUVBool)
                    if(lanternState == 1) TurnOn(true);
                    else TurnOff();
                else TurnOff();
            }
        }
        lanternLight.enabled = isLanternActive;
    }

    private void TurnOn(bool isUV = false)
    {
        lanternState = 1;
        
        if(isUV){
            lanternState = 2;
            lanternLight.color = Color.magenta;
            lanternLight.cullingMask =  ~(1 << UVLayer);
        }
        isLanternActive = true;
        lanternInputCd = 1f;
    }

    private void TurnOff() 
    {
        lanternState = 0;
        isLanternActive = false;
        lanternLight.color = Color.white;
        lanternLight.cullingMask =  -1;
        lanternInputCd = 1f;
    }
}