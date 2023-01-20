using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA : MonoBehaviour
{
    //gene for color
    public float r;//red
    public float g;//green
    public float b;//blue

    bool dead = false;//onclick
    public float timeToDie = 0;//for finding last person
    SpriteRenderer sRenderer;//for color
    Collider2D sCollidder;//for object
    int trialTime = 10;//generation maxs time
    int generation = 1;

    

    void OnMouseDown()
    {
        timeToDie = timeToDie + 1.0f;
        dead = true;
        timeToDie = PopulationManager.elapsed;//
        Debug.Log("Dead at :" + timeToDie);//controll
        sCollidder.enabled = false;
        sRenderer.enabled = false;
        
    }
    void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        sCollidder = GetComponent<Collider2D>();
        sRenderer.color = new Color(r, b, g);
    }

  
    void Update()
    {
       
    }
}
