using Automata;
using UnityEngine;

namespace Interactables
{
    public class AutomataTrigger : Interactable
    {
        [SerializeField] private BaseAutomata automata;

        protected override void Action() => automata.Run();

        protected override bool ExitCondition() => automata.HasFinished();

        protected override void Exit() => automata.Exit();
    }
}