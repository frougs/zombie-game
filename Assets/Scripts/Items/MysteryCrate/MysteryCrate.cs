using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MysteryCrate : Purchasable, IInteractable
{
    [Header("Sound Stuff")]
    [SerializeField] public AudioSource soundSource;
    [SerializeField] public AudioClip errorPurchase;
    [SerializeField] public AudioClip mysteryCrateJingle;
    [SerializeField] public AudioClip purchase;

    [Header("Animation Stuff")]
    [SerializeField] Animator lidAnimator;
    [SerializeField] GameObject light;
    [SerializeField] ParticleSystem soulParticles;
    [Header("Gun References")]
    [SerializeField] GameObject[] guns;
    [SerializeField] GameObject gunSpawnPOS;
    [SerializeField] GameObject gunContainer;
    [SerializeField] GameObject gunParent;
    [SerializeField] GameObject gunEndPos;
    [Header("Stats")]
    [SerializeField] float itemDuration;
    [SerializeField] float lerpDuration;
    [SerializeField] float itemSwapSpeed;
    private bool used = false;
    private GameObject cycledGun;
    private GameObject collectItem;
    [Header("Misc")]
    [SerializeField] TextMeshPro priceText;
    public bool fireSale;

    

    private void Start(){
        if(soundSource == null){
            soundSource = GetComponentInChildren<AudioSource>();
        }
    }

    public void Interacted(GameObject gunRoot, InteractionController interactionCon){
        if(used == false){
            if(!fireSale){
                var scoreSystem = interactionCon.GetComponent<ScoreSystem>();
                if(scoreSystem.score >= price){
                    scoreSystem.SubtractScore(price);
                    ActivateBox();
                }
                else{
                    soundSource.PlayOneShot(errorPurchase);
                }
            }
            else{
                ActivateBox();
            }
        }
        else{
            soundSource.PlayOneShot(errorPurchase);
        }
    }

    public void ActivateBox(){
        soulParticles.Play();
        soundSource.PlayOneShot(purchase);
        soundSource.PlayOneShot(mysteryCrateJingle);
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        used = true;
        lidAnimator.SetBool("open", true);
        lidAnimator.SetBool("close", false);
        light.SetActive(true);
        StartCoroutine(LerpGun());

    }
    private IEnumerator LerpGun(){
        float elapsedTime = 0f;
        var gun = Instantiate(gunContainer, gunSpawnPOS.transform);
        while(elapsedTime < lerpDuration){
            gun.transform.position = Vector3.Lerp(gunSpawnPOS.transform.position, gunEndPos.transform.position, elapsedTime / lerpDuration);
            if(cycledGun == null){
                StartCoroutine(RandomGun(gun));
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        var randomGun = Random.Range(0, guns.Length);
        var finalGun = guns[randomGun];
        gun.transform.position = gunEndPos.transform.position;
        Destroy(gun);
        //Instantiate a variant of a wallbuy script that is free and doesnt display price, but instead just says press [E] or whatever then after duration put layer back to default
        StartCoroutine(SpawnSelectedGun(finalGun));
    }
    private IEnumerator RandomGun(GameObject gun){
        var index = Random.Range(0, guns.Length);
        cycledGun = Instantiate(guns[index].GetComponent<BaseGun>().buyModel, gun.transform);
        yield return new WaitForSeconds(itemSwapSpeed);
        Destroy(cycledGun);
        cycledGun = null;
    }
    private IEnumerator SpawnSelectedGun(GameObject selected){
        float elapsedTime = 0f;
        collectItem = Instantiate(gunParent, gunEndPos.transform);
        collectItem.gameObject.layer = LayerMask.NameToLayer("Default");
        collectItem.GetComponent<MysteryCrateSelectedItem>().buyItem = selected;
        collectItem.GetComponent<MysteryCrateSelectedItem>().crate = this.GetComponent<MysteryCrate>();
        while(elapsedTime < itemDuration){
            collectItem.transform.position = Vector3.Lerp(gunEndPos.transform.position, gunSpawnPOS.transform.position, elapsedTime / itemDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        CloseBox();
    }
    public void CloseBox(){
        Destroy(collectItem);
        collectItem = null;
        lidAnimator.SetBool("open", false);
        lidAnimator.SetBool("close", true);
        light.SetActive(false);
        gameObject.layer = LayerMask.NameToLayer("Default");
        used = false;
    }

    private void Update(){
        if(!fireSale){
            priceText.text = "$" + price.ToString();
        }
        else{
            priceText.text = "FREE!!";
        }
    }
}
