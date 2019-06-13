using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gm 
{
    public event System.Action<Player> OnLocalPlayerJoined;
    public GameObject gameObject;
    private static Gm m_Instance ;
    public static Gm Instance
    {
        get
        {
            if (m_Instance==null)
            {
                m_Instance = new Gm();
                m_Instance.gameObject = new GameObject("_gameManager");
                m_Instance.gameObject.AddComponent<InputController>();
            }
            return m_Instance;
        }
    }


    private InputController m_InputController;
    public InputController InputController
    {
        get
        {
            if (m_InputController == null)
            {
                if (gameObject!= null) { 
                     m_InputController = gameObject.GetComponent<InputController>();
                }
            }
            return m_InputController;
        }
    }
    private Player m_LocalPlayer;
    public Player LocalPlayer
    {
        get
        {
            return m_LocalPlayer;
        }
        set
        {
            m_LocalPlayer = value;
            if (OnLocalPlayerJoined != null)
                OnLocalPlayerJoined(m_LocalPlayer);
        }
    }


  
        

}
