using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected InteractableState state = InteractableState.primed;

    protected enum InteractableState 
    {
        primed,
        firing,
        finished
    }

    public GameObject Player;
    public TMPro.TextMeshProUGUI TextField;
    public float Cutoff = 10f;
    
    protected bool PlayerInRange(float cutoff)
    {
        Vector3 adjustedPosition = transform.position;
        adjustedPosition.y = Player.transform.position.y;
        return Vector3.Distance(Player.transform.position, adjustedPosition) < cutoff;
    }

    public virtual void checkTrigger(){}

    public virtual void fireEvent(){}

    void Start()
    {
        Player = GameObject.Find("Player");
        TextField = Player.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (Application.isEditor)
            gameObject.transform.GetChild(0).localScale = new Vector3(Cutoff, Cutoff, Cutoff) * 2f;
    }

    void Update()
    {
        switch (state)
        {
            case InteractableState.primed:
                checkTrigger();
                break;
            case InteractableState.firing:
                fireEvent();
                break;
            case InteractableState.finished:
                break;
            default:
                break;
        }
    }
}
