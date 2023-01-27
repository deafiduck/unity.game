

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bowlingball : MonoBehaviour
{

    int[] kullanici1 = new int[10];
    int[] kullanici2 = new int[10];
    int counter1 = 0;
    int counter = 0;
    int sira1 = 0;
    int sira2 = 0;
    int max = 6;
    public float force;//topun at�l�� h�z�
    // Use this for initialization
    private List<Vector3> pinPositions;//dubalar�n ba�lang�� konumlar�n� taip etmek 
    private List<Quaternion> pinRotations;// quaternion 3 vektor degerinin �ak��ma ihtimaller i�in kullan�l�r 
    private Vector3 ballPosition;

    void Start()
    {
        var pins = GameObject.FindGameObjectsWithTag("Pin");
        pinPositions = new List<Vector3>();
        pinRotations = new List<Quaternion>();
        foreach (var pin in pins)
        {
            pinPositions.Add(pin.transform.position);
            pinRotations.Add(pin.transform.rotation);
        }

        ballPosition = GameObject.FindGameObjectWithTag("Ball").transform.position;
    }


    /*Siz mesela W tu�una bas�ld���nda gidecek olan bir araba yapt�n�z diyelim. 
      Siz bu arabaya velocity ile 200 birimlik bir kuvvet uygulad�n�z diyelim.
      W tu�una bast���n�z anda araba birden 200 birim ileri ok gibi f�rlayacak 
      ve elinizi tu�tan �ekti�iniz anda ayarlad���n�z diren� 
      (Drag, T�rk�esi ile hava s�rt�nmesi de denebilir)
      etkisiyle yava�lay�p duracak.
      Mant�ken bu �ok sa�ma bir durum.
      Bu �rnekte AddForce kullan�rsak, W tu�una bast���m�z anda
      araba 0'dan 200'e birden de�il de yava� yava� ivmelenerek
      veyahut h�zlanarak ��kacak ve siz elinizi W'den �ekince de
     ayn� oranda ve Drag de�erine ba�l� olarak yava�lay�p duracak.
     ��te k�saca en basit �rnekle b�yle anlat�labilir aradaki farklar�.
     
      
    Velocity> Anl�k tepkiler
    AddForce> Yava� yava� h�zlanarak uygulanan kuvvetler.

    addforce t�rk�esi kuvvet ekle
    addvelocity t�rk�esi h�z ekle
     */


    // Update is called once per frame
    void Update()
    {
        counter = pin1.count1 + pin2.count2 + pin3.count3 + pin4.count4 + pin5.count5 + pin6.count6;
        Debug.Log(counter);

        if (Input.GetKeyUp(KeyCode.W))
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, force));
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 3, 0), ForceMode.Impulse);//yava�lat�yo
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(1, 0, 0), ForceMode.Impulse);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 0), ForceMode.Impulse);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            var pins = GameObject.FindGameObjectsWithTag("Pin");

            for (int i = 0; i < pins.Length; i++)
            {
                //collision.gameObject.transform.parent.gameObject.tag;
                var pinPhysics = pins[i].GetComponent<Rigidbody>();

                pinPhysics.velocity = Vector3.zero;
                pinPhysics.position = pinPositions[i];
                pinPhysics.rotation = pinRotations[i];
                pinPhysics.velocity = Vector3.zero;
                pinPhysics.angularVelocity = Vector3.zero;

                var ball = GameObject.FindGameObjectWithTag("Ball");
                ball.transform.position = ballPosition;
                ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                // counter = 0;
            }
            //kullanici1[sira1] = counter;


            /*if (sira1 = 11)
            {
                sira1 = 0;

            }
            */
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            var ball = GameObject.FindGameObjectWithTag("Ball");
            ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            ball.transform.position = ballPosition;
            //counter1 += counter;
            //Debug.Log(counter1);
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }



        if (Input.GetKeyUp(KeyCode.S))
        {
            Debug.Log("max deger");
            Debug.Log(max);
        }
    }





    private void OnCollisionEnter(Collision collision)
    {

        /*MonoBehavior��n sa�lad��� fonksiyonlardan OnCollisionEnter
           * ilk �arp��ma oldu�u anda tetiklenir.
           * OnCollisionStay ise devaml�l�k durumunda tetiklenirken 
           * OnCollisionExit �arp��ma sonland���nda tetiklenir. 
           */
        /*   if (collision.gameObject.tag == "Pin")
           {
               GetComponent<AudioSource>().Play();
               counter++;

           }*/
    }

}


