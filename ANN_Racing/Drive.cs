using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Drive : MonoBehaviour
{

   
    public float speed = 50.0f;
    public float rotationSpeed = 100.0f;
    public float visibleDistance = 200.0f;
    List<string> collectedTrainingData=new List<string>();
    //reason ý'm going to create a list is that ý want to store all of those string
    //that we're generating
    StreamWriter tdf;

    void Start()
    {
        string path = Application.dataPath + "/trainingData.txt";
        tdf = File.CreateText(path);
    }

    void OnApplicationQuit()
    {
        foreach(string td in collectedTrainingData)
        {
            tdf.WriteLine(td);
        }
        tdf.Close();
        //tam olarak ayný çýktýyý üreten bir sürü girdi verirseniz,
        //oldukça kafa karýþtýrýcý olabilir çünkü bildiðimizi hangisinin yapmasý
        //gerektiðini ayýrt edemiyoruz.
    }

    float Round(float x)
    {
        return (float)System.Math.Round(x, System.MidpointRounding.AwayFromZero) / 2.0f;
    }
    //input output sayýlalrý arttýkça 
    //nöral aðlar bu standart hatayý aþaðý indirmeye çalýþýrken çok zor zamanlar geçirecekler.
    //ancak ayný çýktýyý üreten bir sürü girdi verirseniz,
    //oldukça kafa karýþtýrýcý olabilir.
    //rounded to the nearest point five


    void Update()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translationInput = Input.GetAxis("Vertical") ;
        float rotationInput = Input.GetAxis("Horizontal")  ;

        //burada ihtiyacýmýz olan deðer sýrasý deðil,
        //sinir aðýnýn hesaplayabilmesini istediðimiz bu saf deðer,
        //böylece esas olarak hata durumunun kullanýmýný simüle edebilir.

        // Make it move 10 meters per second instead of 10 meters per frame...
        float translation = Time.deltaTime * speed * translationInput;
        float rotation = Time.deltaTime * rotationSpeed * rotationInput;

        // Move translation along the object's z-axis
        transform.Translate(0, 0, translation);

        // Rotate around our y-axis
        transform.Rotate(0, rotation, 0);

        Debug.DrawRay(transform.position, this.transform.forward * visibleDistance, Color.red);
        Debug.DrawRay(transform.position, this.transform.right * visibleDistance, Color.red);

        RaycastHit hit;
        float fDist = 0 ; float rDist = 0; float lDist  = 0; float r45Dist = 0; float l45Dist = 0;
        //mesafe maks iken 0 çarpmaya yaklaþtýkça 1e yaklaþýyor

        if(Physics.Raycast(transform.position,this.transform.forward,out hit, visibleDistance))
        { //visibleDistance ile bulunduðu konum arasý bi deðeri alabilir yani 0-200
          //starting position,going forward      ,   ,
            fDist = 1-Round(hit.distance/visibleDistance);//artýk oran hesaplanýr
        }
        if (Physics.Raycast(transform.position, this.transform.right, out hit, visibleDistance))//anything under the 200
        {
            rDist = 1 - Round(hit.distance / visibleDistance);
        }
        if (Physics.Raycast(transform.position, -this.transform.right, out hit, visibleDistance))//anything under the 200
        {
            lDist = 1 - Round(hit.distance / visibleDistance);
        }
        if (Physics.Raycast(transform.position,
                           Quaternion.AngleAxis(-45,Vector3.up)*this.transform.right, out hit, visibleDistance))//anything under the 200
        {
            r45Dist = 1 - Round(hit.distance / visibleDistance);
        }
        if (Physics.Raycast(transform.position,
                           Quaternion.AngleAxis(45, Vector3.up) * -this.transform.right, out hit, visibleDistance))//anything under the 200
        {
            l45Dist = 1 - Round(hit.distance / visibleDistance);
        }
        //activasyon fonksiyonuna giden deðerler 0 ve 1 aralýðýnda olduklarý için daha verimli iþleme yapýlabilir

        string td = fDist + "," + rDist + "," + lDist + "," +
            r45Dist + "," + l45Dist + "," +
            Round(translationInput) + "," + Round(rotationInput);
        //tihis is the inputs which is the distance values
        //and the outputs that the error or key pressed for the translation and the rotation

        if (!collectedTrainingData.Contains(td))
        {
            collectedTrainingData.Add(td);
            //ayný deðerleri eklemiyoruz
        }
        
         
    }
}
