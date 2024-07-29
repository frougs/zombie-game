using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BaseGun : MonoBehaviour, IShootable
{
    [SerializeField] float damage;
    [SerializeField] float fireRate;
    [SerializeField] public int maxAmmo;
    [SerializeField] float reloadSpeed;
    [SerializeField] float range;
    [SerializeField] float critMultiplier;
    [SerializeField] public int currentAmmo;
    private bool canShoot = true;
    private bool hasAmmo = true;
    //private GameObject progressBar;
    private UIContainer uiStuff;
    public bool held;
    private Image progressBar;
    public float reloadProgress;
    private float rProgress;
    [HideInInspector] public bool reloading;
    [HideInInspector] public bool fullAmmo;
    [SerializeField] private AudioClip gunshot;
    [SerializeField] private AudioSource soundSource;

    private void Start(){
        currentAmmo = maxAmmo;
        soundSource = this.GetComponent<AudioSource>();
    }
    public void Shot(GameObject shooter){
        if(canShoot && hasAmmo){
            currentAmmo -= 1;
            if(soundSource != null){
                soundSource.PlayOneShot(gunshot);
            }
            if(Physics.Raycast(shooter.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.position, shooter.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.forward, out RaycastHit hitData, Mathf.Infinity)){
                IDamagable damagable = hitData.transform.gameObject.GetComponent<IDamagable>();
                if(damagable != null && hitData.transform.gameObject.tag != "CriticalSpot"){
                    damagable.Damaged(damage);
                }
                else if(damagable != null && hitData.transform.gameObject.tag == "CriticalSpot"){
                    damagable.Damaged(damage * critMultiplier);
                }
                else{
                    Debug.LogWarning("Damagable not found");
                }
            }
            StartCoroutine(ShotDelay());
        }
        else if(canShoot && !hasAmmo){
            Reload();
        }
    }
    IEnumerator ShotDelay(){
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
    private void Update(){
        if(currentAmmo <= 0){
            hasAmmo = false;
        }
        else{
            hasAmmo = true;
        }
        held = GetComponentInChildren<PickupController>().currentlyHeld;
        if(uiStuff == null){
            uiStuff = FindObjectOfType<UIContainer>();
        }
        else{
            progressBar = uiStuff.progressBar;
        }
        if(held){
            uiStuff.UpdateAmmo(currentAmmo, maxAmmo);
        }
        if(reloading){
            if(rProgress < reloadSpeed){
                rProgress += Time.deltaTime;
                progressBar.fillAmount = rProgress/reloadSpeed;
            }
        }
        else{
            rProgress = 0;
        }
        if(currentAmmo == maxAmmo){
            fullAmmo = true;
        }
        else{
            fullAmmo = false;
        }
    }
    public void Reload(){
        StartCoroutine(ReloadTimer());
    }
    IEnumerator ReloadTimer(){
        reloading = true;
        progressBar.gameObject.SetActive(true);
        yield return new WaitForSeconds(reloadSpeed);
        progressBar.gameObject.SetActive(false);
        currentAmmo = maxAmmo;
        progressBar.fillAmount = 0f;
        reloadProgress = 0f;
        reloading = false;
    }
}
