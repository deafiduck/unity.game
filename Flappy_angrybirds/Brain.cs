using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    int DNALength = 5;
    public DNA dna;
    public GameObject eyes;
    //if we hit anyting
    bool seeDownWall = false;
    bool seeUpWall = false;
    bool seeBottom = false;
    bool seeTop = false;
    Vector3 startPosition;
    public float timeAlive = 0;
    public float distanceTravelled = 0;
    public int crash = 0;//
    bool alive = true;
    Rigidbody2D rb;
    //

    public void Init()
    {
        //0 forward
        //1 UpWall;
        //2 downwall
        //3 normal upward
        dna = new DNA(DNALength, 200);//itme 
        this.transform.Translate(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0);
        //Translate komutu , kodu uyguladýðýmýz objenin x,y,z kordinatlarý boyunca verdiðimiz deðerler doðrultusunda hareket etmesini saðlar.
        //Rotate komutu ise , kodu uyguladýðýmýz objenin kendi ekseninde x,y,z koordinatlarý boyunca verdiðmiz deðerler doðrultusunda dönmesini saðlar. 
        startPosition = this.transform.position;
        rb = this.GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter(Collision2D col)
    {
        if(col.gameObject.tag=="top"||
            col.gameObject.tag=="bottom"||
            col.gameObject.tag=="upwall"||
            col.gameObject.tag == "downwall")
        {
            crash++;
        }
        else if (col.gameObject.tag == "dead")
        {
            alive = false;
        }
    }
    void Update()
    {
        if (!alive) return;
        seeUpWall = false;
        seeDownWall = false;
        seeTop = false;
        seeBottom = false;
        RaycastHit2D hit = Physics2D.Raycast(eyes.transform.position, eyes.transform.forward, 1.0f);
        Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 1.0f, Color.red);
        Debug.DrawRay(eyes.transform.position, eyes.transform.up* 1.0f, Color.red);
        Debug.DrawRay(eyes.transform.position, -eyes.transform.up * 1.0f, Color.red);
        //kuþun yukardan ve aþaðýdan da bi þeye vurup vurmadýðýna bakýlýr
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "upwall")
            {
                seeUpWall = true;
            }
            else if (hit.collider.gameObject.tag == "downwall")
            {
                seeDownWall = true;
            }
        }
        hit = Physics2D.Raycast(eyes.transform.position, eyes.transform.up, 1.0f);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "top")
            {
                seeTop = true;
            }
        }
        hit = Physics2D.Raycast(eyes.transform.position, -eyes.transform.up, 1.0f);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "bottom")
            {
                seeBottom = true;
            }
        }
        timeAlive = PopulationManager.elapsed;
    }
    void FixedUpdate()
    {

        if (!alive) return;

        float upforce = 0;
        float forwardForce = 1.0f;//bir is always push forward
        //we only interested upforced

        //0 forward
        //1 UpWall;
        //2 downwall
        //3 normal upward
        if (seeUpWall)
        {
            upforce = dna.GetGene(0);
        }
        else if (seeDownWall)
        {
            upforce = dna.GetGene(1);
        }
        else if (seeTop)
        {
            upforce = dna.GetGene(2);
        }
        else if (seeBottom)
        {
            upforce = dna.GetGene(3);
        }
        else
        {
            upforce = dna.GetGene(4);
        }
        rb.AddForce(this.transform.right * forwardForce);//forwardlarý right yap
        rb.AddForce(this.transform.up * upforce * 0.1f);//bring it down a little bit
        distanceTravelled = Vector3.Distance(startPosition, this.transform.position);
    }
}
