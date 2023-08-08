using UnityEngine;

namespace Automata
{
    public class BaseAutomaton : MonoBehaviour
    {
        public bool RequiresCameraFocus { get; protected set; }
        public bool RequiresPlayerInteraction { get; protected set; }
        
        public virtual void Run(){}
        
        public virtual void Exit(){}

        public virtual bool HasFinished() { return false; }
    }
}