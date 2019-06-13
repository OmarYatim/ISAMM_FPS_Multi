using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Network : MonoBehaviour {

    #region Singelton
    public static Network Instance;
    #endregion
    #region Public params
    public GameObject Player;
    bool isWorking;
    #endregion
    #region Static params
    static SocketIOComponent socket;
    #endregion

    #region MonoBehaviour
    void Start () {
        Instance = this;
        socket = GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        socket.On("Register", OnRegister);
	}

    private void Update()
    {
        Vector3 position = Player.transform.position;
        if (!isWorking)
            StartCoroutine(SendPosition(position));
    }
    #endregion

    #region Private Methods
    private void OnConnected(SocketIOEvent obj)
    {
        Debug.Log("conected");
    }

    private void OnRegister(SocketIOEvent obj)
    {
        Debug.Log("client conected with id : " + obj.data["id"]);
    }
    #endregion

    #region Static Methods
    public static void Position(Vector3 position)
    {
        JSONObject JsonObject = new JSONObject(JSONObject.Type.OBJECT);
        JsonObject.AddField("date", DateTime.Now.ToString());
        JsonObject.AddField("x", position.x);
        JsonObject.AddField("y", position.y);
        JsonObject.AddField("z", position.z);
        socket.Emit("Position", JsonObject);
    }
    #endregion
    #region coroutines
    IEnumerator SendPosition(Vector3 position)
    {
        isWorking = true;
        Position(position);
        yield return new WaitForSeconds(2.0f);
        isWorking = false;
    }
    #endregion
}
