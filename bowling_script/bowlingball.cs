

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
    public float force;//topun atýlýþ hýzý
    // Use this for initialization
    private List<Vector3> pinPositions;//dubalarýn baþlangýç konumlarýný taip etmek 
    private List<Quaternion> pinRotations;// quaternion 3 vektor degerinin çakýþma ihtimaller için kullanýlýr 
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


    /*Siz mesela W tuþuna basýldýðýnda gidecek olan bir araba yaptýnýz diyelim. 
      Siz bu arabaya velocity ile 200 birimlik bir kuvvet uyguladýnýz diyelim.
      W tuþuna bastýðýnýz anda araba birden 200 birim ileri ok gibi fýrlayacak 
      ve elinizi tuþtan çektiðiniz anda ayarladýðýnýz direnç 
      (Drag, Türkçesi ile hava sürtünmesi de denebilir)
      etkisiyle yavaþlayýp duracak.
      Mantýken bu çok saçma bir durum.
      Bu örnekte AddForce kullanýrsak, W tuþuna bastýðýmýz anda
      araba 0'dan 200'e birden deðil de yavaþ yavaþ ivmelenerek
      veyahut hýzlanarak çýkacak ve siz elinizi W'den çekince de
     ayný oranda ve Drag deðerine baðlý olarak yavaþlayýp duracak.
     Ýþte kýsaca en basit örnekle böyle anlatýlabilir aradaki farklarý.
     
      
    Velocity> Anlýk tepkiler
    AddForce> Yavaþ yavaþ hýzlanarak uygulanan kuvvetler.

    addforce türkçesi kuvvet ekle
    addvelocity türkçesi hýz ekle
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
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 3, 0), ForceMode.Impulse);//yavaþlatýyo
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

        /*MonoBehavior’ýn saðladýðý fonksiyonlardan OnCollisionEnter
           * ilk çarpýþma olduðu anda tetiklenir.
           * OnCollisionStay ise devamlýlýk durumunda tetiklenirken 
           * OnCollisionExit çarpýþma sonlandýðýnda tetiklenir. 
           */
        /*   if (collision.gameObject.tag == "Pin")
           {
               GetComponent<AudioSource>().Play();
               counter++;

           }*/
    }

}


