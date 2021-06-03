using UnityEngine;

public class LockCombinationController : MonoBehaviour {

    private int currentNumber = 0;

    public int Number { get { return currentNumber; } }


    public void Operate (bool isPositive = true)
    {
        if(isPositive)
        {
            if(currentNumber < 9)
                currentNumber++;
            else currentNumber = 0;
        }
        else
        {
             if(currentNumber > 0)
                currentNumber--;
            else currentNumber = 9;
        }

        // textMesh.text = currentNumber.ToString();
    }
    
}