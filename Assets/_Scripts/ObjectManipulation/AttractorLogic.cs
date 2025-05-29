using Animation;
using MixedReality.Toolkit;
using MixedReality.Toolkit.SpatialManipulation;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace ObjectManipulation
{
    public class AttractorLogic : ManipulationLogic<Vector3>
    {
        public override void Setup(List<IXRSelectInteractor> interactors, IXRSelectInteractable interactable, MixedRealityTransform currentTarget)
        {   
            base.Setup(interactors, interactable, currentTarget);

            // tracks the end of object manipulation
            interactable.selectExited.AddListener(OnSelectExit);

            if (interactable.transform.TryGetComponent(out LissajousAnimation animation))
            {
                animation.PauseAnimation();
            }

            if (interactable.transform.parent.TryGetComponent(out RotationAnimation rotation))
            {
                rotation.PauseAnimation();
            }
        }

        public override Vector3 Update(List<IXRSelectInteractor> interactors, IXRSelectInteractable interactable, MixedRealityTransform currentTarget, bool centeredAnchor)
        {
            base.Update(interactors, interactable, currentTarget, centeredAnchor);

            Vector3 targetPosition = Vector3.MoveTowards(currentTarget.Position, Camera.main.transform.position, Time.deltaTime * 50f);
            return targetPosition;
        }

        private void OnSelectExit(SelectExitEventArgs args)
        {   
            args.interactableObject.transform.localPosition = Vector3.zero;

            if (args.interactableObject.transform.TryGetComponent(out LissajousAnimation animation))
            {
                animation.ContinueAnimation();
            }

            if (args.interactableObject.transform.parent.TryGetComponent(out RotationAnimation rotation))
            {
                rotation.ContinueAnimation();
            }

            args.interactableObject.selectExited.RemoveListener(OnSelectExit);
        }
    }
}