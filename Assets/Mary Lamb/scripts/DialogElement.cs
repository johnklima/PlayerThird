using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogElement : MonoBehaviour
{

    [TextArea(10,10)]public string elementText;
    public Text textElement;
    public Transform nextNode;

    public Text buttonText;
    public GameObject geometry;

    

    public void RefreshText()
    {
        //keep telling the story!
        textElement.text = textElement.text + " " + elementText;

        if(geometry != null)
        {
            geometry.SetActive(true);
        }
        
        
    }
    public void RefreshButton()
    {
        //the answer is the next question!
        if(buttonText != null)
        {
            buttonText.text = elementText;
            buttonText.transform.parent.gameObject.SetActive(true);


        }
        
    }
}

