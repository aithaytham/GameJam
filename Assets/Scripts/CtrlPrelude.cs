//using Microsoft.MixedReality.Toolkit.Examples.Demos;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class CtrlPrelude : MonoBehaviour
{
    /// <summary>
    /// notes used for experience
    /// </summary>
    public GameObject notes;
    // Start is called before the first frame update

    public string currentStateNote = "";


    public string[] partition = { "do", "do", "do", "re", "mi", "re", "do", "mi", "re", "re", "do" };

    public int posPartition = 0;

    public GameObject preludeEditorGO;

    public string noteEyeTracked = "";

    public GameObject triggerGo;

    HttpClient httpClient;

    public int nbrPodiums = 8;
    private string url= "http://localhost:9090/set_dmx";
    //private string url = "http://192.168.1.29:9090/set_dmx";

    private List<GameObject> abatJoursGO;

    public void ResetPrelude()
    {
        print("reset");
        ResetAllAnim();
        currentStateNote = "";
        posPartition = 0;
        abatJoursGO.Clear();
        InitSpots("notesGammes1");
        InitSpots("notesGammes1 (1)");
        InitSpots("notesGammes1 (2)");
        InitSpots("notesGammes1 (3)");
        StartCoroutine("GererLedResetAll");


    }

    public void ResetAllAnim()
    {
        foreach (ManageAnimTest man in FindObjectsOfType<ManageAnimTest>())
        {
            man.ResetAnim();
        }
    }
    void Start()
    {
        abatJoursGO = new List<GameObject>();
        InitSpots("notesGammes1");
        InitSpots("notesGammes1 (1)");
        InitSpots("notesGammes1 (2)");
        InitSpots("notesGammes1 (3)");

        StartCoroutine("GererAllLedPerAbatJour");
        //GererLedResetAll();
        //GererLedRandomAll();

      //  triggerGo.SetActive(true);

//#if UNITY_EDITOR
//        Camera.main.transform.localPosition = new Vector3(0.4214544f, 1.761288f, -2.656912f);
//        Camera.main.transform.Rotate(0, -30, 0);

//#else

//#endif
    }

    public void InitSpots(string nomGamme)
    {
        float r;
        float g;
        float b;
        Color c;
       // print(GameObject.Find(nomGamme));
        /*r = UnityEngine.Random.Range(0, 255);
        g = UnityEngine.Random.Range(0, 255);
        b = UnityEngine.Random.Range(0, 255);*/
        //c = new Color(r/255f, g/255f, b/255f,150f/255f);
        //print(FindObjectOfType<SkinnedMeshRenderer>().name);
        //FindObjectOfType<SkinnedMeshRenderer>().material.color = c;
        //foreach (Renderer me in GameObject.Find(nomGamme).GetComponentsInChildren<Renderer>())
        //{
        //    //print(me.gameObject);
        //    r = UnityEngine.Random.Range(0, 255);
        //    g = UnityEngine.Random.Range(0, 255);
        //    b = UnityEngine.Random.Range(0, 255);
        //    c = new Color(r / 255f, g / 255f, b / 255f, 150f / 255f);
        //    me.material.color = c;
        //    //print(me.gameObject.transform.parent.parent);
        //    abatJoursGO.Add(me.gameObject);
        //}
    }



    public IEnumerator GererLedRandomAll()
    {





        string data = "";
        int j;
        for (int i = 0; i < nbrPodiums - 1; i++)
        {

            for (int k = 0; k < 4 - 1; k++)
            {
                j = UnityEngine.Random.Range(0, 255);
                data += j + ",";
            }
            data += "0,";

        }

        for (int k = 0; k < 4 - 1; k++)
        {
            j = UnityEngine.Random.Range(0, 255);
            data += j + ",";
        }
        data += "0";
        //print(data);

        WWWForm form = new WWWForm();
        form.AddField("u", "1");
        form.AddField("d", data);

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }

    public IEnumerator GererLedResetAll()
    {

        string data = "";
        int j;
        for (int i = 0; i < nbrPodiums - 1; i++)
        {

            for (int k = 0; k < 4 - 1; k++)
            {
                j = 0;
                data += j + ",";
            }
            data += "0,";

        }

        for (int k = 0; k < 4 - 1; k++)
        {
            j = 0;
            data += j + ",";
        }
        data += "0";
        //print(data);
        WWWForm form = new WWWForm();
        form.AddField("u", "1");
        form.AddField("d", data);

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }

    public IEnumerator GererLedTestOLA()
    {
        WWWForm form = new WWWForm();

        form.AddField("u", "1");
        form.AddField("d", "255");
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }


    public IEnumerator GererLedPerAbatJour(string note, int numeroGamme)
    {



        int numeroPodium = NoteToInt(note) + numeroGamme * 7;
        string data = "";
        int j = 0;
        for (int i = 0; i < nbrPodiums - 1; i++)
        {
            for (int k = 0; k < 4 - 1; k++)
            {


                if (i == numeroPodium)
                {
                    if (k == 0)
                    {
                        j = (int)(abatJoursGO[numeroPodium].GetComponent<Renderer>().material.color.r * 255);
                    }
                    else if (k == 1)
                    {
                        j = (int)(abatJoursGO[numeroPodium].GetComponent<Renderer>().material.color.g * 255);
                    }
                    else if (k == 2)
                    {
                        j = (int)(abatJoursGO[numeroPodium].GetComponent<Renderer>().material.color.b * 255);
                    }

                    data += j + ",";
                }
                else
                {
                    j = 0;
                    data += j + ",";
                }
            }
            data += "0,";



        }

        for (int k = 0; k < 4 - 1; k++)
        {
            if (numeroPodium == nbrPodiums - 1)
            {
                if (k == 0)
                {
                    j = (int)(abatJoursGO[numeroPodium].GetComponent<Renderer>().material.color.r * 255);
                }
                else if (k == 1)
                {
                    j = (int)(abatJoursGO[numeroPodium].GetComponent<Renderer>().material.color.g * 255);
                }
                else if (k == 2)
                {
                    j = (int)(abatJoursGO[numeroPodium].GetComponent<Renderer>().material.color.b * 255);
                }

                data += j + ",";
            }
            else
            {
                j = 0;
                data += j + ",";
            }
        }
        data += "0";
        //print(data);
        WWWForm form = new WWWForm();

        form.AddField("u", "1");
        form.AddField("d", data);
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        yield return new WaitForSeconds(2);
        
       
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);

            if (NoteExcited())
            {
                StartCoroutine("GererLedResetAll");
            }
            else 
            { 

                StartCoroutine("GererAllLedPerAbatJour");
            }



        }



    }

    public IEnumerator GererAllLedPerAbatJour()
    {

        yield return null;
        
        //string data = "";
        //int j = 0;
        //for (int i = 0; i < nbrPodiums - 1; i++)
        //{
        //    for (int k = 0; k < 4 - 1; k++)
        //    {



        //        if (k == 0)
        //        {
        //            j = (int)(abatJoursGO[i].GetComponent<Renderer>().material.color.r * 255);
        //        }
        //        else if (k == 1)
        //        {
        //            j = (int)(abatJoursGO[i].GetComponent<Renderer>().material.color.g * 255);
        //        }
        //        else if (k == 2)
        //        {
        //            j = (int)(abatJoursGO[i].GetComponent<Renderer>().material.color.b * 255);
        //        }

        //        data += j + ",";

        //    }
        //    data += "0,";



        //}

        //for (int k = 0; k < 4 - 1; k++)
        //{

        //    if (k == 0)
        //    {
        //        j = (int)(abatJoursGO[nbrPodiums - 1].GetComponent<Renderer>().material.color.r * 255);
        //    }
        //    else if (k == 1)
        //    {
        //        j = (int)(abatJoursGO[nbrPodiums - 1].GetComponent<Renderer>().material.color.g * 255);
        //    }
        //    else if (k == 2)
        //    {
        //        j = (int)(abatJoursGO[nbrPodiums - 1].GetComponent<Renderer>().material.color.b * 255);
        //    }

        //    data += j + ",";

        //}
        //data += "0";
        //WWWForm form = new WWWForm();

        //form.AddField("u", "1");
        //form.AddField("d", data);
        
        //UnityWebRequest www = UnityWebRequest.Post(url, form);

        //yield return www.SendWebRequest();

        //if (www.isNetworkError || www.isHttpError)
        //{
        //    Debug.Log(www.error);
        //}
        //else
        //{
        //    print("BOUQUET");
        //    Debug.Log(www.downloadHandler.text);
        //}


    }


    public int NoteToInt(string note)
    {
        int i = 0;
        switch (note.ToUpper())
        {

            case "DO":
                i = 0;
                break;
            case "RE":
                i = 1;
                break;
            case "MI":
                i = 2;
                break;
            case "FA":
                i = 3;
                break;
            case "SOL":
                i = 4;
                break;
            case "LA":
                i = 5;
                break;
            case "SI":
                i = 6;
                break;
        }
        return i;
    }

    public void PlayMusic()
    {
        StopAllCoroutines();
        StartCoroutine("PlayMusicCorout");
    }

    public IEnumerator PlayMusicCorout()
    {
        int i = 0;
        while (i < partition.Length)
        {
            PlayNote(partition[i]);
            i++;
            yield return new WaitForSeconds(1.5f);
        }

    }

    public void PlayNote(string note)
    {
        //print(note);
        switch (note)
        {
            case "do":
                notes.transform.Find("DO").GetComponent<HandInteractionTouchCustom>().OnTouchStarted.Invoke(null);
                break;
            case "re":
                notes.transform.Find("RE").GetComponent<HandInteractionTouchCustom>().OnTouchStarted.Invoke(null);
                break;
            case "mi":
                notes.transform.Find("MI").GetComponent<HandInteractionTouchCustom>().OnTouchStarted.Invoke(null);
                break;
        }
    }

    public GameObject GetNote(string note)
    {
        //print(note);

        return notes.transform.Find("DO").GetComponent<HandInteractionTouchCustom>().gameObject;

    }

    public GameObject NoteExcited()
    {
        if (posPartition < partition.Length)
        {
            string note = partition[posPartition].ToUpper();
            return notes.transform.Find(note).gameObject;
        }
        else
        {
            return null;
        }

    }

    public void Trigger()
    {

        if (NoteExcited() != null && NoteExcited().GetComponentInChildren<ManageAnimTest>().currentStateAnim("EyeTracked") && (NoteExcited().name == this.noteEyeTracked))
        {

            currentStateNote = "Trigger";
            NoteExcited().GetComponent<AudioSource>().PlayOneShot(NoteExcited().GetComponent<AudioSource>().clip);
            NoteExcited().GetComponentInChildren<ManageAnimTest>().Trigger();
        }

    }

    public void NoteExcitedEyeTracked(string noteEyeTracked)
    {
        //print(noteEyeTracked);
        if (NoteExcited() != null && NoteExcited().GetComponentInChildren<ManageAnimTest>().currentStateAnim("WaitPlay") && (NoteExcited().name == noteEyeTracked))
        {

            this.noteEyeTracked = noteEyeTracked;
            currentStateNote = "EyeTracked";
            NoteExcited().GetComponentInChildren<ManageAnimTest>().EyeTracked();
        }
    }

    public void NoteExcitedNoEyeTracked(string noteEyeTracked)
    {
        //print("noeyeTracked");
        if (NoteExcited() != null && NoteExcited().GetComponentInChildren<ManageAnimTest>().currentStateAnim("EyeTracked") && (NoteExcited().name == noteEyeTracked))
        {
            this.noteEyeTracked = "";
            currentStateNote = "WaitPlay";
            NoteExcited().GetComponentInChildren<ManageAnimTest>().WaitPlay();
        }
    }





    // Update is called once per frame
    void Update()
    {


        //print(posPartition);
        if (NoteExcited() != null && NoteExcited().GetComponentInChildren<ManageAnimTest>().currentStateAnim("Idle") && currentStateNote != "WaitPlay")
        {
            currentStateNote = "WaitPlay";
            NoteExcited().GetComponentInChildren<ManageAnimTest>().WaitPlay();
        }

        if (NoteExcited() != null && NoteExcited().GetComponentInChildren<ManageAnimTest>().currentStateAnim("Trigger") && currentStateNote != "Idle")
        {
           // print("heheee");
            currentStateNote = "Idle";
            NoteExcited().GetComponentInChildren<ManageAnimTest>().Idle();
            //triggerGo.SetActive(false);
            this.noteEyeTracked = "";
            //GererLedRandomAll();
           // StartCoroutine(GererLedPerAbatJour(NoteExcited().name, 0));

            posPartition++;
            if (NoteExcited() != null)
            {
                //print(partition[posPartition]);
            }


        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {

            Trigger();
        }








    }
}
