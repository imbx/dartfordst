using System.Collections;
using UnityEngine;

public class FilmProjector : InteractBase {

    public Material TargetMaterial;

    public Texture2D[] ProjectorTextures;

    public Transform ProjectorTop;

    public float RotationSpeed;

    private bool isRotating = false;

    private int CurrentProjectorTexture = 0;


    public override void Execute(bool isLeftAction = true)
    {
        if(isRotating) return;
        base.Execute();

        StartCoroutine(RotateProjector());

    }

    IEnumerator RotateProjector()
    {
        float timer = 0;
        float startAngle = ProjectorTop.localEulerAngles.y;
        float targetAngle = startAngle + (360f / 3);
        isRotating = true;
        CurrentProjectorTexture++;
        if (CurrentProjectorTexture > 2) CurrentProjectorTexture = 0;

        while(timer < 1f)
        {
            timer += RotationSpeed * Time.deltaTime;
            ProjectorTop.localEulerAngles = new Vector3(
                ProjectorTop.localEulerAngles.x,
                Mathf.Lerp(startAngle, targetAngle, timer),
                ProjectorTop.localEulerAngles.z);

            yield return null;
        }
        
        ProjectorTop.localEulerAngles = new Vector3(
                ProjectorTop.localEulerAngles.x,
                targetAngle,
                ProjectorTop.localEulerAngles.z);

        isRotating = false;

        yield return null;
    }
}