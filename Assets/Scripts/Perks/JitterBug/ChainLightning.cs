using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ChainLightning : MonoBehaviour
{
    public List<GameObject> alreadyHit = new List<GameObject>();
    [HideInInspector] public int maxChains;
    [HideInInspector] public float damage;
    [HideInInspector] public float chainRange;
    [HideInInspector] public float lightningDuration;
    private LineRenderer lineRenderer;
    public GameObject lightningParticles;
    public GameObject player;
    [SerializeField] GameObject damageNumberParticles;
    private void Start(){
        lineRenderer = GetComponent<LineRenderer>();
        this.transform.position = Vector3.zero;
    }
    public void StartLightning(GameObject initialHit)
    {
        if (initialHit != null)
        {
            StartCoroutine(ChainLightningCoroutine(initialHit));
        }
        else
        {
            Debug.LogError("Initial hit is null! Cannot start chain lightning.");
        }
    }

private IEnumerator ChainLightningCoroutine(GameObject lastHit)
{
    int currentChain = 0;

    // Ensure lineRenderer is set up
    lineRenderer = GetComponent<LineRenderer>();
    lineRenderer.positionCount = 0; // Start with 0 points and add as needed

    while (currentChain < maxChains)
    {
        // Check if lastHit is valid
        if (lastHit == null)
        {
            Debug.LogError("lastHit is null before attempting to set position. Exiting coroutine.");
            Destroy(this.gameObject);
            yield break; // Exit if lastHit is null
        }

        // Increase line renderer position count and set the position
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(currentChain, lastHit.transform.position);
        currentChain++;

        // Find the next target
        GameObject closestObject = FindNextTarget(lastHit);
        if (closestObject == null) 
        {
            Debug.LogWarning("No valid target found for lightning chain.");
            break; // No valid targets found
        }

        // Deal damage and spawn damage particle effect
        var damageable = closestObject.GetComponent<IDamagable>();
        if (damageable != null)
        {
            damageable.Damaged(damage, player, closestObject.transform.position);
            UpdateParticle(closestObject, damage);
            Instantiate(lightningParticles, closestObject.transform.position, closestObject.transform.rotation, closestObject.transform);
            alreadyHit.Add(closestObject);
        }
        else
        {
            Debug.LogWarning($"The object {closestObject.name} does not implement IDamagable.");
        }

        // Update for the next chain link
        lastHit = closestObject;
        yield return new WaitForSeconds(0.1f); // Wait a bit to show the lightning effect
    }

    // Optionally, wait before cleaning up
    yield return new WaitForSeconds(lightningDuration);
    Destroy(gameObject); // Clean up after the effect
}




    private GameObject FindNextTarget(GameObject lastHit)
    {
        var damageables = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<IDamagable>().ToArray(); // Convert to an array for safety

        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (var damageable in damageables)
        {
            GameObject obj = (damageable as MonoBehaviour).gameObject;
            if (obj != lastHit && obj.gameObject.GetComponent<BarrierScript>() == null && obj.gameObject.GetComponent<ThirdPersonController>() == null && !alreadyHit.Contains(obj.gameObject))
            {
                float distance = Vector3.Distance(lastHit.transform.position, obj.transform.position);
                if (distance < chainRange && distance < closestDistance)
                {
                    closestObject = obj;
                    closestDistance = distance;
                }
            }
        }

        if (closestObject != null)
        {
            //Debug.Log($"Found closest object: {closestObject.name} at distance: {closestDistance}");
        }
        else
        {
            Debug.LogWarning("No valid targets found within range.");
        }

        return closestObject; // Return the closest target found
    }

    private void UpdateParticle(GameObject hitData, float damageAmount){
        try{
        damageAmount = Mathf.Floor(damageAmount);
        bool foundCurrentParticle = false;
        var activeDamageNumbers = FindObjectsOfType<AlreadyActiveDamageParticle>();
        var damageableIDGenerator = hitData.transform.gameObject.GetComponentInParent<DamageableIDGenerator>();
        if(damageableIDGenerator == null){
            damageableIDGenerator = hitData.transform.gameObject.GetComponent<DamageableIDGenerator>();
        }
        //Debug.Log(damageableIDGenerator.ID);
        if(activeDamageNumbers != null && activeDamageNumbers.Length > 0){
            foreach (var particle in activeDamageNumbers){
                if (damageableIDGenerator != null && particle.GetComponent<AlreadyActiveDamageParticle>().enemyID == damageableIDGenerator.ID){
                    //Reset the particle
                    foundCurrentParticle = true;
                    particle.GetComponent<AlreadyActiveDamageParticle>().ResetParticle(damageAmount, hitData.transform.position);
                    //Debug.Log("Found Currently Active Particle");
                    break;
                }
            }
        }
        if(!foundCurrentParticle){
            //Spawn New particle and assign the ID
            var newDmgParticles = Instantiate(damageNumberParticles, hitData.transform.position, Quaternion.LookRotation((player.transform.position - hitData.transform.position).normalized));
            newDmgParticles.GetComponent<AlreadyActiveDamageParticle>().NewParticle(damageAmount);
            newDmgParticles.GetComponent<AlreadyActiveDamageParticle>().enemyID = damageableIDGenerator.ID;
        }
        }
        catch(Exception e){
            Debug.LogWarning(e.ToString());
        }
    }
}
