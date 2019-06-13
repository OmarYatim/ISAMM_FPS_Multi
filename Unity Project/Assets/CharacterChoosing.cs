using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterChoosing : MonoBehaviour
{
    public GameObject Image;
    public RenderTexture[] Textures;
    int curAvatar = 1;

    void Start()
    {
        Image.GetComponent<RawImage>().texture = Textures[0];
        DontDestroyOnLoad(Gm.Instance.gameObject);
    }

    public void ChooseAvatar(int number)
    {
        curAvatar = number;
        Image.GetComponent<RawImage>().texture = Textures[curAvatar - 1];
    }

    public void EnterMatch()
    {
        NetworkControl.SendAvatar(curAvatar-1);
    }
}
