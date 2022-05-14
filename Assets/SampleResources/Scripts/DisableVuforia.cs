/*===============================================================================
Copyright (c) 2020 PTC Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
===============================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Vuforia;
using System.IO;


/// <summary>
/// Vuforia's lifecycle is different when working with the MixedRealityToolkit. 
/// MRTK keeps always active the first loaded scene that contains the MixedRealityToolkit and 
/// MixedRealityPlayspace GameObjects, then deactivates those GameObjects in the later loaded scences. 
/// Since the MainCamera has to be a child of the MixedRealityPlayspace, it is not possible to add a 
/// VuforiaBehaviour to the cameras in the scenes, because those get deactivated as soon as the scene is loaded.
/// In this sample, the 0-Base scene is the one that loads the MRTK and then functions as a base scene during
/// the whole application's lifecycle. Therefore, the camera in this scene will be the MainCamera used by MRTK,
/// and also the one that has the VuforiaBehaviour attached.
/// Since the VuforiaBehaviour is shared between the Vuforia scenes, it has to be deactivated when the
/// scene is unloaded to avoid inconsistencies.
/// </summary>
public class DisableVuforia : MonoBehaviour
{
	#region PUBLIC_MEMBERS

	public GameObject homeButton;

	#endregion //PUBLIC_MEMBERS

	   
	#region PUBLIC_METHODS

	public void ExitVuforiaScene()
    {
        if (VuforiaBehaviour.Instance.enabled)
        {
            DeactivateActiveDataSets(true);
            VuforiaBehaviour.Instance.enabled = false;
        }
    }

    #endregion //PUBLIC_METHODS


    #region PRIVATE_METHODS

    void DeactivateActiveDataSets(bool destroyDataSets = false)
    {
        var objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        var stateManager = TrackerManager.Instance.GetStateManager();

        if (objectTracker != null && objectTracker.IsActive)
        {
			// First unregister all the callbacks
            var behaviours = stateManager.GetTrackableBehaviours();
            foreach (TrackableBehaviour behaviour in behaviours)
            {
                var handler = behaviour.gameObject.GetComponent<ModelTargetAugmentationHandler>();
                if (handler != null)
                {
                    behaviour.UnregisterOnTrackableStatusChanged(handler.OnTrackableStatusChanged);
                }
            }

            List<DataSet> allDataSets = objectTracker.GetDataSets().ToList();
            List<DataSet> activeDataSets = objectTracker.GetActiveDataSets().ToList();

            objectTracker.Stop();

			// Deactivate all the DataSets
            foreach (DataSet ds in activeDataSets)
            {
                // When in PlayMode, the VuforiaEmulator.xml dataset (used by GroundPlane) is managed by Vuforia.
                var dsFileName = Path.GetFileName(ds.Path);

                if (dsFileName != "VuforiaEmulator.xml")
                {
                    objectTracker.DeactivateDataSet(ds);
                }
            }

			// Destroy all the DataSets
            foreach (DataSet ds in allDataSets)
            {
                var dsFileName = Path.GetFileName(ds.Path);

                if (dsFileName != "VuforiaEmulator.xml")
                {
                    DestroyTrackableBehavioursForDataSet(ds);

                    if (destroyDataSets)
                    {
                        objectTracker.DestroyDataSet(ds, false);
                    }
                }
            }

            objectTracker.Start();
        }
    }

    void DestroyTrackableBehavioursForDataSet(DataSet dataSet)
    {
        var stateManager = TrackerManager.Instance.GetStateManager();

        if (stateManager != null)
        {
            var trackables = dataSet.GetTrackables();
            foreach (Trackable trackable in trackables)
            {
                stateManager.DestroyTrackableBehavioursForTrackable(trackable, true);
            }

            var vumarkManager = TrackerManager.Instance.GetStateManager().GetVuMarkManager();

            if (vumarkManager != null)
            {
                var vumarkTrackableBehaviours = vumarkManager.GetAllBehaviours();
                foreach (VuMarkBehaviour vb in vumarkTrackableBehaviours)
                {
                    stateManager.DestroyTrackableBehavioursForTrackable(vb.Trackable, true);
                    if (vb && vb.gameObject)
                    {
                        var obj = vb.gameObject;
                        Destroy(vb);
                        Destroy(obj);
                    }
                }

                var allRemainingVuMarkBehaviours = FindObjectsOfType<VuMarkBehaviour>();
                foreach (VuMarkBehaviour vb in allRemainingVuMarkBehaviours)
                {
                    VLog.Log("red", "Manually Destroying VuMark GameObject.");
                    Destroy(vb);

                }
            }
        }
    }

    #endregion //PRIVATE_METHODS
}
