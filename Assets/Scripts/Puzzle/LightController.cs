using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour {
    public List<GameObject> lights;
    private List<float> lightsIntensity;

    public float IntensitySpeed = 4f;

    private bool isAnimating = false;

    public void SwitchLights(bool isOn = true, bool saveIntensity = true)
    {
        if(lights.Count <= 0) return;
        if(saveIntensity) lightsIntensity = new List<float>();
        lights.ForEach(l => {
            l.SetActive(isOn);
            if(saveIntensity) lightsIntensity.Add(l.GetComponent<Light>().intensity);
        });
    }

    public void LightController_AnimLights()
    {
        if(isAnimating) return;
        StartCoroutine(AnimLights());
    }

    IEnumerator AnimLights()
    {
        yield return new WaitForSeconds(1f);
        isAnimating = true;
        SwitchLights(false, true);
        lights.ForEach(l => {
            l.GetComponent<Light>().intensity = 0;
        });
        SwitchLights(true, false);

        yield return new WaitForSeconds(0.35f);

        // SwitchLights(true, false);

        float timer = 0;

        while(timer < 1f)
        {
            timer += Time.deltaTime * IntensitySpeed;
            for(int i = 0; i < lights.Count; i++)
            {
                lights[i].GetComponent<Light>().intensity = Mathf.Lerp(0, lightsIntensity[i], timer);
                yield return null;
            }
            yield return null;
        }
        isAnimating = false;
        yield return null;
    }
    
}