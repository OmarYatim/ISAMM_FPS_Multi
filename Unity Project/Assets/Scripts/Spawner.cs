using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    NetworkEntity playerParms;
    NetworkControl network;
    public GameObject[] Characters;
    public GameObject[] CharactersForOthers;
    public Transform playersParent;
    public static Spawner Instance;
    void Start()
    {
        Instance = this;
        SpawnMe();
        
    }

    void SpawnMe()
    {
        GameObject Player = Instantiate(Characters[Informations.Avatar]);
        playerParms = Player.GetComponent<NetworkEntity>();
        Player.gameObject.name = "Player"+"("+'"'+Informations.Nom+'"'+")" +"(" + '"' + Informations.ID + '"'+ ")";
        playerParms.ID = Informations.ID.ToString();
        playerParms.Avatar = Informations.Avatar.ToString();
        playerParms.userName = Informations.Nom;
        network = GetComponent<NetworkControl>();
        NetworkControl.Instance.sendTransformToServerOnSpawn(Player.transform.position,playerParms.ID);
        Player.transform.Find("Name").GetComponent<TextMesh>().text ="";
    }

    public void SpawnPlayer(string id,string name,string Avatar,Vector3 pos,Vector3 Rot)
    {
        GameObject Player = Instantiate(CharactersForOthers[int.Parse(Avatar)],pos,Quaternion.Euler(Rot));
        Player.transform.SetParent(playersParent);
        playerParms = Player.GetComponent<NetworkEntity>();
        Player.gameObject.name = "Player" + "(" + name + ")" + "(" + id + ")";
        playerParms.ID = id;
        playerParms.Avatar = Avatar;
        playerParms.userName = name;

        string nom2 = "";
        for (int i = 0; i < name.Length; i++) {
            if (name[i] == '"') {

            } else {
                nom2 = nom2 + name[i];
            }
        }
        Player.transform.Find("Name").GetComponent<TextMesh>().text = nom2;

    }
}
