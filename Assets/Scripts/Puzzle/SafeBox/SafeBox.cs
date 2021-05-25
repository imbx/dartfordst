using UnityEngine;
public class SafeBox : PuzzleBase
{
    [HideInInspector] public int FirstNumber = 0;
    [HideInInspector] public int SecondNumber = 0;
    [Header("Safe Box Parameters")]
    public int FirstTargetNumber = 20;
    public int SecondTargetNumber = 80;
    
    void Update()
    {
        
    }

    public void CheckNumbers()
    {
        if(FirstNumber == FirstTargetNumber &&
           SecondNumber == SecondTargetNumber)
            this.OnEnd();
    }
}
