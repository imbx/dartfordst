using UnityEngine;
using UnityEngine.UI;

public class SafeBox : PuzzleBase
{
    [HideInInspector] public int FirstNumber = 0;
    [HideInInspector] public int SecondNumber = 0;
    [Header("Safe Box Parameters")]
    public int FirstTargetNumber = 20;
    public int SecondTargetNumber = 80;

    public Text targetText;

    public Door childDoor;
    public override void Execute(bool isLeftAction = true)
    {
        // transform.tag = "Safebox";
        base.Execute();
    }    
    void Update()
    {
        if(!isInteractingThis) return;
        gameControllerObject.playerTargetTag = "Safebox";

        if(gameControllerObject.isInPuzzle)
            targetText.text = FirstNumber + " , " + SecondNumber;

        if(controller.isEscapePressed)
        {
            Debug.Log("[Safebox] Escape pressed");
            this.OnExit();
        }
    }

    public void CheckNumbers()
    {
        if(FirstNumber == FirstTargetNumber &&
           SecondNumber == SecondTargetNumber)
           {
               childDoor.Execute();
               this.OnEnd();
           }
    }

    protected override void OnEnd(bool destroyGameObject = false)
    {
        // transform.tag = "Untagged";
        base.OnEnd(destroyGameObject);
    }

    public override void OnExit()
    {
        // transform.tag = "BasicInteraction";
        base.OnExit();
    }
}
