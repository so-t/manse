using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected InteractableState state = InteractableState.primed;

    protected enum InteractableState 
    {
        primed,
        triggered,
        post,
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

    public virtual void firePostEvent(){}

    void Start()
    {
        Player = GameObject.Find("Player");
        TextField = Player.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (Application.isEditor)
            gameObject.transform.GetChild(0).localScale = new Vector3(gameObject.transform.localScale.x + Cutoff, 
                                                                        gameObject.transform.localScale.y + Cutoff, 
                                                                        gameObject.transform.localScale.z + Cutoff);
    }

    void Update()
    {
        switch (state)
        {
            case InteractableState.primed:
                checkTrigger();
                break;
            case InteractableState.triggered:
                fireEvent();
                break;
            case InteractableState.post:
                firePostEvent();
                break;
            case InteractableState.finished:
                break;
            default:
                break;
        }
    }
}
