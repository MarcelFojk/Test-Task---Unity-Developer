using MixedReality.Toolkit;
using MixedReality.Toolkit.SpatialManipulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

            if (interactable.transform.TryGetComponent(out Follow follow))
            {
                follow.IgnoreDistanceClamp = true;
            }

            // tracks the end of object manipulation
            interactable.selectExited.AddListener(OnSelectExit);
        }

        public override Vector3 Update(List<IXRSelectInteractor> interactors, IXRSelectInteractable interactable, MixedRealityTransform currentTarget, bool centeredAnchor)
        {
            base.Update(interactors, interactable, currentTarget, centeredAnchor);

            Vector3 targetPosition = Vector3.MoveTowards(currentTarget.Position, Camera.main.transform.position, Time.deltaTime * 20f);
            return targetPosition;
        }

        private void OnSelectExit(SelectExitEventArgs args)
        {
            if (args.interactableObject.transform.TryGetComponent(out Follow follow))
            {   
                follow.IgnoreDistanceClamp = false;
            }
        }
    }
}