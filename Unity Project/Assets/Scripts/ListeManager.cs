using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListeManager : MonoBehaviour
{

    public GameObject itemTemplate;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddNewItem(string id,string name,string url=null) {
        GameObject item = Instantiate(itemTemplate);
        item.transform.SetParent(itemTemplate.transform.parent,false);
        item.name = id;

        if (url!=null) {
            //LoadImageFromUrl(url,item.transform.GetChild(0).GetComponent<Image>());
        }

        Text itemText = item.transform.GetChild(1).GetComponent<Text>();
        itemText.text = name;
    }

    public void RemoveItem(string id) {
        Transform itemsParent = itemTemplate.transform.parent;
        GameObject item = itemsParent.Find(id).gameObject;
        if (item != null) {
            Destroy(item);
        }
    }
}
