/*===============================================================================
Copyright (c) 2020 PTC Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
===============================================================================*/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
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
/// Since the VuforiaBehaviour is shared between the Vuforia scenes and it can be deactivated, it needs to be
/// enabled and we need to check that the correct DataSets are loaded.
/// </summary>
public class EnableVuforia : MonoBehaviour
{
    #region PUBLIC_MEMBERS

    public string datasetName;

    #endregion //PUBLIC_MEMBERS

    #region MONOBEHAVIOUR_METHODS
	
    public void OnEnable()
    {
        // If the VuforiaBehaviour already exists on the Camera, check that it is enabled.
        if (VuforiaBehaviour.Instance && !VuforiaBehaviour.Instance.isActiveAndEnabled)
        {
            VLog.Log("yellow", "Enabling VuforiaBehaviour");
            VuforiaBehaviour.Instance.enabled = true;
        }

        // If Vuforia is in the process of starting, register a callback, otherwise call it directly.
        if (!VuforiaARController.Instance.HasStarted)
        {
            VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        }
        else
        {
            OnVuforiaStarted();
        }

    }

    #endregion //MONOBEHAVIOUR_METHODS


    #region VUFORIA_CALLBACK_METHODS

    void OnVuforiaStarted()
    {
        var objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        var stateManager = TrackerManager.Instance.GetStateManager();

        // We need to call this for cleaning out VuMarks
        stateManager.ReassociateTrackables();

        bool isDataSetAlreadyActive = false;

        var allDataSets = objectTracker.GetDataSets();
        foreach (DataSet ds in allDataSets)
        {
            var dsName = Path.GetFileNameWithoutExtension(ds.Path);
        }

        var activeDataSets = objectTracker.GetActiveDataSets();
        foreach (DataSet ds in activeDataSets)
        {
            var dsName = Path.GetFileNameWithoutExtension(ds.Path);
            if (dsName == (this.datasetName))
            {
                isDataSetAlreadyActive = true;
            }
        }

		// We need to reassociate the trackables in case the needed DataSet already exists in the scene
        if (DataSet.Exists(this.datasetName))
        {
            if (!isDataSetAlreadyActive)
            {
                LoadDataSet(this.datasetName);
            }
            else
            {
                stateManager.ReassociateTrackables();
            }
        }

        if (objectTracker != null && objectTracker.IsActive == false)
        {
            objectTracker.Start();
        }
    }

    #endregion // VUFORIA_CALLBACK_METHODS


    #region PRIVATE_METHODS

    void LoadDataSet(string datasetName)
    {
        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

        if (DataSet.Exists(datasetName))
        {
            DataSet dataset = objectTracker.CreateDataSet();

            if (dataset.Load(datasetName))
            {
                objectTracker.ActivateDataSet(dataset);
                StateManager stateManager = TrackerManager.Instance.GetStateManager();
            }
            else
            {
                Debug.LogError("Failed to load DataSet: " + datasetName);
            }
        }
        else
        {
            VLog.Log("orange", "The following DataSet not found in 'StreamingAssets/Vuforia': " + datasetName);
        }
    }

    #endregion //PRIVATE_METHODS

}
