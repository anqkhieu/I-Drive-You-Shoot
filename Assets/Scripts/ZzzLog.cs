using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZzzLog : MonoBehaviour
{
    uint qsize = 3;  // number of messages to keep
    Queue myLogQueue = new Queue();

    void Start() {
        DontDestroyOnLoad(this.gameObject);
    }

    void OnEnable() {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable() {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type) {
        myLogQueue.Enqueue("[" + type + "] : " + logString);
        if (type == LogType.Exception)
            myLogQueue.Enqueue(stackTrace);
        while (myLogQueue.Count > qsize)
            myLogQueue.Dequeue();
    }

    void OnGUI() {
        GUILayout.BeginArea(new Rect(50, 0, 400, Screen.height));
        GUILayout.Label("\n" + string.Join("\n", myLogQueue.ToArray()));
        GUILayout.EndArea();
    }
}