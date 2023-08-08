using System.Collections.Generic;
using System.Linq;
using Automata;
using UnityEngine.Serialization;

namespace Interactables
{
    public class AutomataTrigger : BaseInteractable
    {
        [FormerlySerializedAs("Automata")] public List<BaseAutomata> automata;

        protected override void Action()
        {
            foreach (var automaton in automata)
                automaton.Run();
        }

        protected override bool ExitCondition() => automata.All(automaton => automaton.HasFinished());

        protected override void Exit()
        {
            foreach (var automaton in automata)
                automaton.Exit();
        }
    }
}