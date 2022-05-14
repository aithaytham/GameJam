// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using UnityEngine.Playables;


    public class HandInteractionTouchCustom : MonoBehaviour, IMixedRealityTouchHandler
    {
        [SerializeField]
        private TextMesh debugMessage = null;
        [SerializeField]
        private TextMesh debugMessage2 = null;

        #region Event handlers
        public TouchEvent OnTouchCompleted;
        public TouchEvent OnTouchStarted;
        public TouchEvent OnTouchUpdated;
        #endregion

        private Renderer TargetRenderer;
       // private Color originalColor;
       // private Color highlightedColor;

        protected float duration = 1.5f;
        protected float t = 0;

        //-----------Add For Prelude-----------//
        public Material animTouchMat;


        private void Start()
        {
            //string s=GetComponent<EyeTrackingTarget>().ToString();
            //print(s);
            TargetRenderer = GetComponentInChildren<Renderer>();
            if ((TargetRenderer != null) && (TargetRenderer.sharedMaterial != null))
            {
            //    originalColor = TargetRenderer.sharedMaterial.color;
            //    highlightedColor = new Color(originalColor.r + 0.2f, originalColor.g + 0.2f, originalColor.b + 0.2f);
            }
        }

       /* /// <summary>
        /// Call when click on a podium
        /// </summary>
        public void WhenTouchStarted()
        {
            //print("touch");
            if (TargetRenderer != null)
            {

               *//* if (GetComponent<EyeTrackingTarget>())
                {
                   
                    GetComponent<EyeTrackingTarget>().enabled = false;
                    TargetRenderer.material = animTouchMat;
                }*//*
                //TargetRenderer.material = animTouchMat;
                //    TargetRenderer.sharedMaterial.color = Color.Lerp(originalColor, highlightedColor, 2.0f);
            }
        }*/

        private void Update()
        {

            /*if (GetComponent<EyeTrackingTarget>())
            {


                //print(GetComponent<PlayableDirector>().time);

                if (!GetComponent<EyeTrackingTarget>().enabled && GetComponent<PlayableDirector>().time > (int)GetComponent<PlayableDirector>().duration)
                {

                    //print("reset");
                    GetComponent<EyeTrackingTarget>().enabled = true;
                    GetComponent<EyeTrackingTarget>().OnLookAway.Invoke();
                }

                if (!GetComponent<EyeTrackingTarget>().enabled && GetComponent<PlayableDirector>().time == 0 && states == 1)
                {
                    print("anim");
                    states = 2;
                    TargetRenderer.material = animTouchMat;
                }






            }*/

        }

        void IMixedRealityTouchHandler.OnTouchCompleted(HandTrackingInputEventData eventData)
        {
            OnTouchCompleted.Invoke(eventData);

            if (debugMessage != null)
            {
                debugMessage.text = "OnTouchCompleted: " + Time.unscaledTime.ToString();
            }

            if ((TargetRenderer != null) && (TargetRenderer.material != null))
            {

                
                //   TargetRenderer.material.color = originalColor;
                //TargetRenderer.material = animTouchMat;
            }
        }

        void IMixedRealityTouchHandler.OnTouchStarted(HandTrackingInputEventData eventData)
        {
            
            OnTouchStarted.Invoke(eventData);

            if (debugMessage != null)
            {
                debugMessage.text = "OnTouchStarted: " + Time.unscaledTime.ToString();
            }

           



        }

        void IMixedRealityTouchHandler.OnTouchUpdated(HandTrackingInputEventData eventData)
        {
            OnTouchUpdated.Invoke(eventData);

            if (debugMessage2 != null)
            {
                debugMessage2.text = "OnTouchUpdated: " + Time.unscaledTime.ToString();
            }

            if ((TargetRenderer != null) && (TargetRenderer.material != null))
            {
                //TargetRenderer.material.color = Color.Lerp(Color.green, Color.red, t);
                t = Mathf.PingPong(Time.time, duration) / duration;
            }
        }
    }
