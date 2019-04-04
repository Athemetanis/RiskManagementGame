using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssincTextRewriter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnDisable()
    {
        string textChanged = this.gameObject.GetComponent<Text>().text;
        this.gameObject.GetComponent<Text>().text = textChanged;
        Debug.Log("setted active");
        //this.gameObject.SetActive(true);
    }
}
