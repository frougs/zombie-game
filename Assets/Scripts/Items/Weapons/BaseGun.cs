using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGun : MonoBehaviour, IShootable
{
    [SerializeField] float damage;
    [SerializeField] float fireRate;
    [SerializeField] float ammo;
    [SerializeField] float reloadSpeed;
    [SerializeField] float range;
    private bool canShoot = true;
    //private bool shootOnce = false;

    public void Shot(GameObject shooter){
        if(canShoot){
            //shootOnce = true;
            if(Physics.Raycast(shooter.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.position, shooter.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.forward, out RaycastHit hitData, Mathf.Infinity)){
                //Debug.Log("Raycast hit summin");
                IDamagable damagable = hitData.transform.gameObject.GetComponent<IDamagable>();
                if(damagable != null){
                    damagable.Damaged(damage);
                }
                else{
                    Debug.LogWarning("Damagable not found");
                }
            }
            //Debug.Log("Pew");
            StartCoroutine(ShotDelay());
        }
    }
    IEnumerator ShotDelay(){
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
    /*private void FixedUpdate(){
        if(shootOnce){
            if(Physics.Raycast(this.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.position, this.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.forward, out RaycastHit hitData, Mathf.Infinity)){
                Debug.Log("Raycast hit summin");
                IDamagable damagable = hitData.transform.gameObject.GetComponent<IDamagable>();
                if(damagable != null){
                    damagable.Damaged(damage);
                }
                else{
                    Debug.LogWarning("Damagable not found");
                }
            }
            shootOnce = false;
        }
    }*/
}
