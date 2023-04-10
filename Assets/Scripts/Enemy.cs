using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Enemy : MonoBehaviour
{
    //first number in the multiplication problem
    public int number1;
    //second number
    public int number2;
    //Dislay text for the question
    public TextMeshPro text;


    // Start is called before the first frame update
    
    //called just after the enemy is instantiated and all the values have been set
    public void Spawn()
    {
        //sets the text
        text.text = $"{number1} × {number2}";
    }

    // Update is called once per frames
    void Update()
    {
        
    }
}
