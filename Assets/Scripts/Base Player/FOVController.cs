using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FOVController : MonoBehaviour
{
    public float baseFOV;
    public bool canRevert;
    private CinemachineVirtualCamera cam;
    [SerializeField] float revertSpeed;
    private Coroutine changingFOV;
    public bool sprinting;
    private void Start(){
        cam = GetComponent<CinemachineVirtualCamera>();
        if(!PlayerPrefs.HasKey("FOV")){
            PlayerPrefs.SetFloat("FOV", baseFOV);
        }
    }
    private void Update(){
        baseFOV = PlayerPrefs.GetFloat("FOV");
        //If currently sprinting, change FOV based on speed

        //If currently ADS'ing change FOV to ADS FOV

        //ELSE IF: If current FOV != base FOV change it back to normal slowly

        //Check if FOV is currently being manipuleted: IE if a coroutine is currently running, if not, get current FOV and gradually change it back to base
        if(canRevert){
            //cam.m_Lens.FieldOfView = Mathf.Lerp(baseFOV, cam.m_Lens.FieldOfView, revertSpeed * Time.deltaTime);
            FOVChange(baseFOV, revertSpeed);
        }
    }

    public void FOVChange(float newFOV, float transitionTime){
        canRevert = false;
        if(changingFOV == null){
            changingFOV = StartCoroutine(LerpFOV(newFOV, transitionTime));
        }
        else{
            StopCoroutine(changingFOV);
            changingFOV = StartCoroutine(LerpFOV(newFOV, transitionTime));
        }
    }
    private IEnumerator LerpFOV(float newFOV, float transitionTime){
        float elapsedTime = 0f;
        while(elapsedTime < transitionTime){
            cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView,newFOV, transitionTime * Time.deltaTime);
            yield return null;
        }
    }
}
