using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoundEventScript : MonoBehaviour
{
    private RoundsScript roundController;
    public UnityEvent eventRound;
    public int eventRoundNumber;
    private int triggeredForThisRound;
    void Start()
    {
        roundController = GetComponent<RoundsScript>();
    }

    void Update()
    {
        if(roundController.roundNumber % eventRoundNumber == 0 && roundController.roundNumber != triggeredForThisRound){
            eventRound?.Invoke();
            triggeredForThisRound = roundController.roundNumber;
        }
    }
}
