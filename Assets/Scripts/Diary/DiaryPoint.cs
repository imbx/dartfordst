using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DiaryPoint : MonoBehaviour {
    public int destinationPage = 0;
    public Image DotImage;
    private DiaryNodes nodesParent;

    public void SetData(int dest, DiaryNodes dn)
    {
        destinationPage = dest;
        nodesParent = dn;
    }

    public void SendPoint()
    {
        Debug.Log("[DiaryPoint] Should send point " + destinationPage);
        nodesParent.SetPointAction(destinationPage);
    }

}