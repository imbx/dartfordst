using UnityEngine;

public class MessageBox : MonoBehaviour {
    float lifeTime = 4f;
    private bool willClose = false;
    private void OnEnable() {
        
        willClose = false;
        lifeTime = 4f;
        GetComponent<Animator>().Rebind();
        GetComponent<Animator>().Update(0f);
    }
    private void Update() {
        lifeTime -= Time.deltaTime;
        if(!willClose)
        {
            if(lifeTime <= 0)
            {
                willClose = true;
                lifeTime = 2f;
                GetComponent<Animator>().SetTrigger("TriggerAnim");
            }
        }

        if(willClose && lifeTime <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}