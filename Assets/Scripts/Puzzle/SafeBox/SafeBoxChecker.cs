using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SafeBoxChecker : InteractBase {
    [Header("Safe Box Checker Parameters")]
    public UnityEvent Action;
    private bool isOpening = false;

    public override void Execute(bool isLeftAction = true)
    {
        if(isOpening) return;
        base.Execute(isLeftAction);
        isOpening = true;
        StartCoroutine(Open());
    }
    

    IEnumerator Open()
    {
        float timer = 0f;
        while(timer < 1f)
        {
            transform.localEulerAngles = new Vector3(0, Mathf.Lerp(0, 66f, timer), 0);
            timer += Time.deltaTime * 2f;
            yield return null;
        }
        Action.Invoke();
        timer = 0;

        while(timer < 1f)
        {
            transform.localEulerAngles = new Vector3(0, Mathf.Lerp(66f, 0, timer), 0);
            timer += Time.deltaTime * 2f;
            yield return null;
        }
        isOpening = false;
        yield return null;
    }
}