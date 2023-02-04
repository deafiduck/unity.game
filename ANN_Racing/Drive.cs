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
    //reason �'m going to create a list is that � want to store all of those string
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
        //tam olarak ayn� ��kt�y� �reten bir s�r� girdi verirseniz,
        //olduk�a kafa kar��t�r�c� olabilir ��nk� bildi�imizi hangisinin yapmas�
        //gerekti�ini ay�rt edemiyoruz.
    }

    float Round(float x)
    {
        return (float)System.Math.Round(x, System.MidpointRounding.AwayFromZero) / 2.0f;
    }
    //input output say�lalr� artt�k�a 
    //n�ral a�lar bu standart hatay� a�a�� indirmeye �al���rken �ok zor zamanlar ge�irecekler.
    //ancak ayn� ��kt�y� �reten bir s�r� girdi verirseniz,
    //olduk�a kafa kar��t�r�c� olabilir.
    //rounded to the nearest point five


    void Update()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translationInput = Input.GetAxis("Vertical") ;
        float rotationInput = Input.GetAxis("Horizontal")  ;

        //burada ihtiyac�m�z olan de�er s�ras� de�il,
        //sinir a��n�n hesaplayabilmesini istedi�imiz bu saf de�er,
        //b�ylece esas olarak hata durumunun kullan�m�n� sim�le edebilir.

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
        //mesafe maks iken 0 �arpmaya yakla�t�k�a 1e yakla��yor

        if(Physics.Raycast(transform.position,this.transform.forward,out hit, visibleDistance))
        { //visibleDistance ile bulundu�u konum aras� bi de�eri alabilir yani 0-200
          //starting position,going forward      ,   ,
            fDist = 1-Round(hit.distance/visibleDistance);//art�k oran hesaplan�r
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
        //activasyon fonksiyonuna giden de�erler 0 ve 1 aral���nda olduklar� i�in daha verimli i�leme yap�labilir

        string td = fDist + "," + rDist + "," + lDist + "," +
            r45Dist + "," + l45Dist + "," +
            Round(translationInput) + "," + Round(rotationInput);
        //tihis is the inputs which is the distance values
        //and the outputs that the error or key pressed for the translation and the rotation

        if (!collectedTrainingData.Contains(td))
        {
            collectedTrainingData.Add(td);
            //ayn� de�erleri eklemiyoruz
        }
        
         
    }
}
