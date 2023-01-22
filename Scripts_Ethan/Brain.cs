using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]

//Bir GameObject'e RequireComponent kullanan bir komut dosyas� ekledi�inizde,
//gerekli bile�en otomatik olarak GameObject'e eklenir.
//Bu, kurulum hatalar�n� �nlemek i�in kullan��l�d�r.
//�rne�in, bir komut dosyas� her zaman ayn� GameObject'e bir Rigidbody eklenmesini gerektirebilir.
//RequireComponent'i kulland���n�zda, bu otomatik olarak yap�l�r,
//bu nedenle kurulumu yanl�� yapman�z olas� de�ildir.
//burda da objeye thirdPersonCharactr otomatik olarak ekleniyor

public class Brain : MonoBehaviour
{
    //controller the character set between character and dna
    //read the DNA and determine what to do

    public int DNALength = 1;//for only one properties
    public float timeAlive;
    public DNA1 dna;

    public float distanceTravelled;
    private ThirdPersonCharacter m_Character;
    private Vector3 m_Move;
    Vector3 startPosition;
    private bool m_Jump;
    bool alive = true;// for recording timeAlive

    void OnCollisionEnter(Collision obj)//�arp��ma 
    {
        if (obj.gameObject.tag == "dead")//white plane
        {
            alive = false;
        }
    }
    public void Init()
    {
        //initialise DNA
        //0 forward
        //1 back
        //2 left
        //3 right
        //4 jump
        //5 crouch
        dna = new DNA1(DNALength, 6);//DNA1 constructor dnalength,maxvalues
        m_Character = GetComponent<ThirdPersonCharacter>();
        timeAlive = 0;
        alive = true;


    }
    private void FixedUpdate()
        //replacing the character controller becomes on the ethan otomaticly controller
    {
        //read DNA

        float h = 0;//horizontal
        float v = 0;//vertical
        bool crouch = false;
        if (dna.GetGene(0) == 0) v = 1;//forword
        else if (dna.GetGene(0) == 1) v = -1;//back
        else if (dna.GetGene(0) == 2) h = -1;//left
        else if (dna.GetGene(0) == 3) h = 1;//right
        else if (dna.GetGene(0) == 4) m_Jump = true;
        else if (dna.GetGene(0) == 5) crouch = true;
        m_Move = v * Vector3.forward + h * Vector3.right;
        m_Character.Move(m_Move, crouch, m_Jump);
        //thirdpersoncharacter scriptinde move fonksiyonu var 3 parametre al�yor
        //biri hareket di�eri e�ilme di�eri z�plama m_character bu scriptten olu�an nesne
        //hareket de�erleri orayla e� 
        m_Jump = false;
        if (alive)
        {
            timeAlive += Time.deltaTime;
            distanceTravelled = Vector3.Distance(this.transform.position, startPosition);
        }
    }

}
