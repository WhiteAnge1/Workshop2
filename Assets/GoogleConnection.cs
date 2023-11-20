using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System;
using Unity.VisualScripting;

public class NewBehaviourScript : MonoBehaviour
{
    public AudioClip Enough;
    public AudioClip Many;
    public AudioClip Little;
    private AudioSource selectAudio;
    private Dictionary<string, int> dataSet = new Dictionary<string, int>();
    private bool statusStart = false;
    private int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GoogleSheets());
    }

    // Update is called once per frame
    void Update()
    {
        //currentMin + dataSet[currentMin]
        if (dataSet.Count == 0) return;
        if (i < dataSet.Count && statusStart == false)
        {
            var currentMin = "Min_" + i.ToString();
            var messageInLog = string.Format("{0} {1}", currentMin, dataSet[currentMin]);
            if (dataSet[currentMin] <= 500)
            {
                StartCoroutine(PlaySelectAudioLittle());
                Debug.Log(messageInLog);
            }

            else if (dataSet[currentMin] > 500 && dataSet[currentMin] < 1500)
            {
                StartCoroutine(PlaySelectAudioEnough());
                Debug.Log(messageInLog);
            }

            else if (dataSet[currentMin] >= 1500)
            {
                StartCoroutine(PlaySelectAudioMany());
                Debug.Log(messageInLog);
            }
        }
    }

    IEnumerator GoogleSheets()
    {
        UnityWebRequest curentResp = UnityWebRequest.Get("https://sheets.googleapis.com/v4/spreadsheets/1H_RTNYsqaxlCrhprDKanMkRFXFWyNYn-QGtP4OQSxwU/values/Лист1?key=AIzaSyDiGCCXWMafLcWChNfHkQcxVFaXFRhB5AY");
        yield return curentResp.SendWebRequest();
        string rawResp = curentResp.downloadHandler.text;
        var rawJson = JSON.Parse(rawResp);
        foreach (var itemRawJson in rawJson["values"])
        {
            var parseJson = JSON.Parse(itemRawJson.ToString());
            var selectRow = parseJson[0].AsStringList;
            try
            {
                int.Parse(selectRow[0]);
            }
            catch
            {
                continue;
            }
            dataSet.Add(("Min_" + selectRow[0]), int.Parse(selectRow[1]));
        }
        if (dataSet.Count == 0)
            throw new Exception("Empty!");
    }

    IEnumerator PlaySelectAudioMany()
    {
        statusStart = true;
        selectAudio = GetComponent<AudioSource>();
        selectAudio.clip = Many;
        selectAudio.Play();
        yield return new WaitForSeconds(4);
        statusStart = false;
        i++;
    }
    IEnumerator PlaySelectAudioEnough()
    {
        statusStart = true;
        selectAudio = GetComponent<AudioSource>();
        selectAudio.clip = Enough;
        selectAudio.Play();
        yield return new WaitForSeconds(4);
        statusStart = false;
        i++;
    }
    IEnumerator PlaySelectAudioLittle()
    {
        statusStart = true;
        selectAudio = GetComponent<AudioSource>();
        selectAudio.clip = Little;
        selectAudio.Play();
        yield return new WaitForSeconds(2);
        statusStart = false;
        i++;
    }
}


