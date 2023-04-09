using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Interactable
{
    public string str = "";
    
    bool fired = false;

    TeleType t;

    public override void checkTrigger(){
        if (PlayerInRange(Cutoff) && Input.GetButtonDown("Interact"))
        {
            Player.GetComponent<PlayerController>().State = new Interacting(Player);
            Player.GetComponentInChildren<CameraRotation>().LookAt(transform);
            state = InteractableState.firing;
        }
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
                Player.GetComponent<PlayerController>().State = new BaseState(Player);
                fired = false;
                state = InteractableState.finished;
            }
        }
    }
}
