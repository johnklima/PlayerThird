using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
/* This module handles dialog tree behaviour. To enable re-entrant dialog, the DialogElement
 * has an additional reference, which one could imagine as just another child empty in the
 * scene graph. As a scene graphic is "acyclic" we need to provide an "out point" to somewhere else
 * in the scene graph, thus making a DAG into DCG 
 * (Directed Acyclic Graph, into a Directed Cyclic Graph."
*/

public class NarrativeRoot : MonoBehaviour
{
    public Transform currentDecision;  //our current position in the tree    
    public Button[] buttons;  
    private void Start()
    {

        //turn off all buttons
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false); 
        }

        DialogElement de = currentDecision.GetComponent<DialogElement>();
        de.RefreshText();

        foreach (Transform t in currentDecision)
        {
            t.GetComponent<DialogElement>().RefreshButton();
        }



    }

    public void Choice(int index)   //index maps to the button that was pressed
    {                               //or some other mechanism that made a choice

        //turn off all buttons
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }


        //chose the branch assuming it exists
        if (currentDecision.childCount > index)
        {
            //increment the decision 
            currentDecision = currentDecision.GetChild(index);
            DialogElement de = currentDecision.GetComponent<DialogElement>();

            de.RefreshText(); //populate text box with its current level

            //does this one now have a reentry node?
            if(de.nextNode)
            {
                //refresh each button with the next decision options
                foreach (Transform t in de.nextNode)
                {
                    t.GetComponent<DialogElement>().RefreshButton();
                }

            }

            //We have a new currentDecision, therefor we need to look at it, to prep
            //the interface for a new batch of answers.

            //WOW!! in the case of no kids, but a nextNode, make the nextNode current
            //as each node can be both a question and an answer, or maybe neither
            if (currentDecision.childCount == 0 && de.nextNode)
            {
                currentDecision = de.nextNode;
                //and it gets even more crazy
                currentDecision.GetComponent<DialogElement>().RefreshText();
            }

        }
        else if (currentDecision.childCount == 0)
        {

            //if no children, check if the component has a "next" node re-entry point
            //which is the equavalent to having one more child
            
            DialogElement de = currentDecision.GetComponent<DialogElement>();

            //does this have a "re-entrant" child node?
            if (de.nextNode)
            {
                //show the text from the NEXT next node, this next node??
                de.nextNode.GetComponent<DialogElement>().RefreshText();
                currentDecision = de.nextNode;
                foreach (Transform t in de.nextNode)
                {
                    t.GetComponent<DialogElement>().RefreshButton();
                }
            }           
        }        
    }
}
