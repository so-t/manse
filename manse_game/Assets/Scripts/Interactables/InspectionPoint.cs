using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class InspectionPoint : Interactable
{
    public string str = "";
    
    bool fired = false;

    TeleType t;

    public override void checkTrigger()
    {
        if (PlayerInRange(Cutoff) 
            && Input.GetButtonDown("Interact") 
            && (Player.GetComponent<PlayerController>().Interact(transform))) 
                state = InteractableState.triggered;
    }

    public override void fireEvent()
    {   
        if (!Player.GetComponentInChildren<CameraRotation>().HasTarget && !fired)
        {
            t = Player.GetComponent<PlayerController>().TextField.gameObject.AddComponent<TeleType>();
            t.enabled = false;
            t.str = this.str;
            t.textMeshPro = Player.GetComponent<PlayerController>().TextField;
            t.enabled = true;
            fired = true;
        }
        else if (!Player.GetComponentInChildren<CameraRotation>().HasTarget && Input.GetButtonDown("Interact"))
        {
            if (t != null && t.HasFinished())
            {   
                t.Clear();
                Player.GetComponentInChildren<CameraRotation>().returnToLookTarget();
                fired = false;
                state = InteractableState.post;
            }
        }
    }

    public override void firePostEvent()
    {
        if (!Player.GetComponentInChildren<CameraRotation>().HasTarget)
        {
            Player.GetComponent<PlayerController>().State = new Playing(Player);
            state = InteractableState.finished;
        }
    }
}