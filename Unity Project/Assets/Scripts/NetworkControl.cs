using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SocketIO;
using System;

public class NetworkControl : MonoBehaviour {
    #region Public Variables
   
    public InputField usernameField;
    public static NetworkControl Instance;
    #endregion

    #region Private Variables
    static SocketIOComponent socket;
    #endregion

    #region Monobehaviour
    void Awake()
    {
        Instance = this;
        socket = GetComponent<SocketIOComponent>();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // socket.On("open", OnConnected);
        socket.On("Connected", OnConnected);
        socket.On("serverreceivedname", OnServerReceivedName);
        socket.On("serverreceivedavatar", OnServerReceivedAvatar);
        socket.On("SpawnForOthers", onSpawnForOthers);
        socket.On("moveForOthers", onMoveForOthers);
        socket.On("playerFireCast", onPlayerFireCast);
        socket.On("KillHimClient", onKillHimClient);
        
        socket.On("disconnected", onDisconnected);
    }






    #endregion

    #region Méthodes
    #region Boutons
    public void Connect()
    {
        socket.Connect();

    }
    #endregion

    #region Networking
    private void OnConnected(SocketIOEvent obj)
    {
        Debug.Log("Connected");
        JSONObject JSONObj = new JSONObject(JSONObject.Type.OBJECT);
        JSONObj.AddField("name", usernameField.text);
        //JSONObj.AddField("menu","cs");
        Debug.Log("Emitting name to server ");
        socket.Emit("SendName", JSONObj);
    }

    private void onDisconnected(SocketIOEvent obj) {
        Transform Player;
        Player = Spawner.Instance.playersParent.Find("Player" + "(" + obj.data["name"] + ")" + "(" + obj.data["id"] + ")");
        Destroy(Player.gameObject);
        Debug.Log(Player.gameObject.name);
        Debug.Log("Disconnected and deleted"+ "Player" + "(" + obj.data["name"] + ")" + "(" + obj.data["id"] + ")");
    }

    private void onKillHimClient(SocketIOEvent obj) {
        Transform Player;
        Debug.Log(obj);
        Player = Spawner.Instance.playersParent.Find("Player" + "(" + obj.data["name"] + ")" + "(" + obj.data["deadid"] + ")");
        if (("Player" + "(" + obj.data["name"] + ")" + "(" + obj.data["deadid"] + ")") == "Player" + "(" + '"' + Informations.Nom + '"' + ")" + "(" + '"' + Informations.ID + '"' + ")") {
            SceneManager.LoadScene("CharacterChoose");
        }
        Destroy(Player.gameObject);
        Debug.Log("Dieeeeeeed " + "Player" + "(" + obj.data["name"] + ")" + "(" + obj.data["deadid"] + ")");
    }
    

    public void SendFireToServer(Ray ray, Vector3 MuzzleP, Quaternion MuzzleR) {
        JSONObject JSONObj = new JSONObject(JSONObject.Type.OBJECT);
        JSONObj.AddField("ray", AddRayObjectToJson(ray));
        JSONObj.AddField("pos", AddVector3ObjectToJson(MuzzleP));
        JSONObj.AddField("Rot", AddQuaternionObjectToJson(MuzzleR));
        socket.Emit("PlayerFired", JSONObj);
    }



    public void sendKilled2(string name, GameObject deadPerson) {
        JSONObject JSONObj = new JSONObject(JSONObject.Type.OBJECT);
        Debug.Log("dead person id " + name);
        JSONObj.AddField("deadid", name);
        Debug.Log(JSONObj);
        Destroy(deadPerson);
        socket.Emit("somedead", JSONObj);
        Debug.Log("freaking server sent motherf*****");
    }

    private void OnServerReceivedName(SocketIOEvent obj)
    {
        Debug.Log("Received ID from server : " + obj.data["id"].str);
        Informations.ID = obj.data["id"].str;
        SceneManager.LoadScene("CharacterChoose");
        //getPlayersInCharacterChoose();
    }

    //public void getPlayersInCharacterChoose() {
    //    socket.Emit("getCharachtersInCS");
    //}


    public static void SendAvatar(int number)
    {
        Informations.Avatar = number;
        JSONObject JSONObj = new JSONObject(JSONObject.Type.OBJECT);
        JSONObj.AddField("avatar", number);
        //JSONObj.AddField("menu", "game");
        Debug.Log("Emitting avatar " + number + " to server ");
        socket.Emit("SendAvatar", JSONObj);
    }

    private void OnServerReceivedAvatar(SocketIOEvent obj)
    {
        Informations.Nom = obj.data["name"].str;
        Informations.Avatar = (int)obj.data["avatar"].n;
        Debug.Log("Spawning you - ID : " + Informations.ID + " Name : " + Informations.Nom + " Avatar : " + Informations.Avatar);
        SceneManager.LoadScene("GameplayScene");
    }

    private void onPlayerFireCast(SocketIOEvent obj) {
        Ray ray = new Ray(new Vector3(obj.data["ray"]["Ox"].n, obj.data["ray"]["Oy"].n, obj.data["ray"]["Oz"].n), new Vector3(obj.data["ray"]["Dx"].n, obj.data["ray"]["Dy"].n, obj.data["ray"]["Dz"].n));
        RPG.Instance.FireForOthers(ray,GetVectorFromJson(obj),GetQuaternionFromJson(obj));
    }



    IEnumerator stopAnimation(float waitTime,Transform Player) {
        yield return new WaitForSeconds(waitTime);
        if (Player!=null && Player.GetComponent<Rigidbody>().velocity.magnitude == 0) { 
            Player.GetComponent<Animator>().SetFloat("CanWalk",0);
            print("WaitAndPrint stopped animation");
        } else {
            StartCoroutine(stopAnimation(0.5f, Player));
        }
    }

    private void onMoveForOthers(SocketIOEvent obj) {
     Transform Player;
        Player =  Spawner.Instance.playersParent.Find("Player" + "(" + obj.data["name"] + ")" + "(" + obj.data["id"] + ")");
        Player.GetComponent<Animator>().SetFloat("CanWalk", 0.2f);
        Player.position = GetVectorFromJson(obj);
        Player.eulerAngles = (GetQuaternionFromJson(obj));
        StartCoroutine(stopAnimation(0.5f,Player));
    }


    public void sendTransformToServerOnSpawn(Vector3 position,string id)
    {
        string pos;
        pos = position.ToString();
        Debug.LogError(pos);
        JSONObject jsonObj = new JSONObject(JSONObject.Type.OBJECT);
       
        string nom2 = "";
        for (int i = 0; i < pos.Length; i++)
        {
            if (pos[i] == '"')
            {

            }
            else
            {
                nom2 = nom2 + pos[i];
            }
        }
        jsonObj.AddField("pos", nom2);
        Debug.Log("omar pos " + jsonObj);
        socket.Emit("playerTransform", jsonObj);
        Debug.Log("Sent positions from client");
    }

    public void UpdateMovement(Vector3 pos) {
        JSONObject jsonObj = new JSONObject(JSONObject.Type.OBJECT);
        jsonObj.AddField("pos", AddVector3ObjectToJson(pos));
        //Debug.Log("CurrentPosition is !!" + rot);
        jsonObj.AddField("Rot", AddVector3ObjectToJson(ThirdPersonCamera.Instance.PlayerRotation));
        socket.Emit("UpdatePosition",jsonObj);
    }

    private static Vector3 GetVectorFromJson(SocketIOEvent obj) {

        return new Vector3(obj.data["pos"]["x"].n, obj.data["pos"]["y"].n, obj.data["pos"]["z"].n);
    }

    private static Vector3 GetQuaternionFromJson(SocketIOEvent obj) {
        return new Vector3(obj.data["Rot"]["x"].n, obj.data["Rot"]["y"].n, obj.data["Rot"]["z"].n);
    }

    public void onSpawnForOthers(SocketIOEvent obj) {
        Spawner.Instance.SpawnPlayer(obj.data["id"].ToString(),obj.data["nom"].ToString(), obj.data["avatar"].ToString(), GetVectorFromJson(obj), GetQuaternionFromJson(obj));
    }


    public JSONObject AddVector3ObjectToJson(Vector3 vector3) {
        JSONObject JSONObj = new JSONObject(JSONObject.Type.OBJECT);
        JSONObj.AddField("x", vector3.x);
        JSONObj.AddField("y", vector3.y);
        JSONObj.AddField("z", vector3.z);
        return JSONObj;
    }



    public JSONObject AddQuaternionObjectToJson(Quaternion quat) {
        JSONObject JSONObj = new JSONObject(JSONObject.Type.OBJECT);
        JSONObj.AddField("x", quat.x);
        JSONObj.AddField("y", quat.y);
        JSONObj.AddField("z", quat.z);
        return JSONObj;
    }

    public JSONObject AddRayObjectToJson(Ray ray) {
        JSONObject JSONObj = new JSONObject(JSONObject.Type.OBJECT);
        JSONObj.AddField("Ox", ray.origin.x);
        JSONObj.AddField("Oy", ray.origin.y);
        JSONObj.AddField("Oz", ray.origin.z);
        JSONObj.AddField("Dx", ray.direction.x);
        JSONObj.AddField("Dy", ray.direction.y);
        JSONObj.AddField("Dz", ray.direction.z);
        return JSONObj;
    }
    
    #endregion

    #endregion
}
