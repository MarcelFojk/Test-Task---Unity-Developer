using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Utils
{
    public class HandFilter : MonoBehaviour, IXRSelectFilter
    {
        private enum EAllowedHand
        {   
            None = 0,
            Left = 1,
            Right = 2,
            Both = 3,
        }

        [SerializeField] private EAllowedHand AllowerdHand = EAllowedHand.Both;

        public bool canProcess => true;

        public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
        {
            var hand = interactor.handedness;

            bool allow = AllowerdHand switch
            {
                EAllowedHand.None => false,
                EAllowedHand.Both => true,
                EAllowedHand.Left => hand == InteractorHandedness.Left,
                EAllowedHand.Right => hand == InteractorHandedness.Right,
                _ => false
            };

            return allow;
        }
    }
}