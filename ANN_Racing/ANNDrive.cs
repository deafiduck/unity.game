using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;//file 

public class ANNDrive : MonoBehaviour
{
    ANN ann;
    public float visibleDistance = 200;
    public int epochs = 1000;//how many times we're going to run
    //declared at the top here a number of epochs
    //which is how many times we're going to run the training data
    //throw before testing to see how good we've done
    public float speed = 50;
    public float rotatitonSpeed = 100.0f;

    bool trainingDone = false;
    //new rider to run the training
    //so that we can see how far the training
    //has progress on the screen and therefore
    //will need to yield .we wont be able to
    //stop the update until training done has been set to true .
    float trainingProgress = 0;
    //how far throught the epocchs
    double sse = 0;
    //calculating errors
    double lastSEE = 1;
    //last some of squid errors

    public float rotation;
    public float translation;
    //show in inspector for debugging

    public bool loadFromFile=false;

    void Start()
    {
        ann = new ANN(5, 2, 1, 10, 0.5);

        if (loadFromFile)
        {
            LoadWeightsFromFile();
            trainingDone = true;
        }
        else
        {
            StartCoroutine(LoadTrainingSet());
        }

        StartCoroutine(LoadTrainingSet());
        //distances,
        //outputs moving us forward and rotation to turn to car
        //alpha value 0.5
        //way where we can adapt that learning value as a way of training

    }
    void OnGUI()
    {
        GUI.Label(new Rect(25, 25, 250, 30), "SSE: " + lastSEE);
        GUI.Label(new Rect(25, 40, 250, 30), "Alpha: " + ann.alpha);
        GUI.Label(new Rect(25, 55, 250, 30), "Trained: " + trainingProgress);
    }

    IEnumerator LoadTrainingSet()
    {
        string path = Application.dataPath + "/trainingData.txt";
        string line;
        if (File.Exists(path))
        {
            int lineCount = File.ReadAllLines(path).Length;
            StreamReader tdf = File.OpenText(path);
            List<double> calcOutputs = new List<double>();
            List<double> inputs = new List<double>();
            List<double> outputs = new List<double>();

            for (int i = 0; i < epochs; i++)
            {
                sse = 0;//initially the standard error will be set to zero because we want to start calculating it
                tdf.BaseStream.Position = 0;
                string currentWeights = ann.PrintWeights();
                while ((line = tdf.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                    float thisError = 0;
                    if (System.Convert.ToDouble(data[5]) != 0 && System.Convert.ToDouble(data[6]) !=0)
                    {
                        inputs.Clear();
                        outputs.Clear();
                        inputs.Add(System.Convert.ToDouble(data[0]));
                        inputs.Add(System.Convert.ToDouble(data[1]));
                        inputs.Add(System.Convert.ToDouble(data[2]));
                        inputs.Add(System.Convert.ToDouble(data[3]));
                        inputs.Add(System.Convert.ToDouble(data[4]));

                        double o1 = Map(0, 1, -1, 1, System.Convert.ToSingle(data[5]));
                        outputs.Add(o1);
                        double o2 = Map(0, 1, -1, 1, System.Convert.ToSingle(data[6]));
                        outputs.Add(o2);
                        //sinir aðýnýn denemesini istediðimiz çýktý deðerleri,
                        //kullanýcýnýn veri 5 ve veri 6 olan öteleme ve döndürme deðerleri için koyduðu þeydir.

                        calcOutputs = ann.Train(inputs, outputs);
                        thisError = ((Mathf.Pow((float)(outputs[0] - calcOutputs[0]), 2) +
                    Mathf.Pow((float)(outputs[1] - calcOutputs[1]), 2))) / 2.0f;

                    }
                    sse += thisError;
                }
                trainingProgress = (float)i / (float)epochs;
                sse /= lineCount;
               if( lastSEE < sse)
                {
                    //we couldn't get any better with that training set
                    /*bundan sonra sinir aðýmýzýn eðitim aðýrlýklarýnýn
                     * alfa deðerini güncelleyeceðiz. ve bu durumda,
                     * en düþük SSE'yi elde etmek için bir tür optimal çözüme odaklanýp
                     * odaklanamayacaðýmýzý görmek için onu 0 1 noktasýna kadar azaltacaðýz.*/
                    ann.LoadWeights(currentWeights);
                    ann.alpha = Mathf.Clamp((float)ann.alpha - 0.001f, 0.01f, 0.9f);
                    //: Verilen deðerin, belirtilen minimum ve maksimum deðerlerini aþmadýðýndan emin olmamýzý saðlar.
                    //Bu fonksiyona verdiðimiz deðer eðer belirttiðimiz sýnýrdan az ise,
                    //minimum deðeri, sýnýrdan fazla ise de, belirttiðimiz maksimum deðeri bize döndürür.
                }
                else
                {
                    ann.alpha = Mathf.Clamp((float)ann.alpha + 0.001f, 0.01f, 0.9f);
                    //we can take bigger leaps down that hole
                    lastSEE = sse;
                }
                yield return null;
            }
        }
        trainingDone = true;
        SaveWeightsToFile();
    }
    
    void SaveWeightsToFile()
    {
        string path = Application.dataPath + "/weights.txt";
        StreamWriter wf = File.CreateText(path);
        wf.WriteLine(ann.PrintWeights());
        wf.Close();
    }

    void LoadWeightsFromFile()
    {
        string path = Application.dataPath + "/weights.txt";
        StreamReader wf = File.OpenText(path);

        if (File.Exists(path))
        {
            string line = wf.ReadLine();
            ann.LoadWeights(line);
        }
    }
    //we want to save value 


    float Map(float newfrom,float newto,float origfrom,float origto,float value)
    {
        if (value <= origfrom)
        {
            return newfrom;
        }
        else if (value >= origto)
        {
            return newto;
        }
        else
        {
            return (newto - newfrom) * ((value - origfrom) / (origto - origfrom)) + newfrom;
        }

    }
    float Round(float x)
    {
        return (float)System.Math.Round(x, System.MidpointRounding.AwayFromZero) / 2.0f;
    }
    void Update()
    {
        if (!trainingDone) return;

        List<double> calcOutputs = new List<double>();
        List<double> inputs = new List<double>();
        List<double> outputs = new List<double>();

        RaycastHit hit;
        
        float fDist = 0; float rDist = 0; float lDist = 0; float r45Dist = 0; float l45Dist = 0;
        if (Physics.Raycast(transform.position, this.transform.forward, out hit, visibleDistance))
        { //visibleDistance ile bulunduðu konum arasý bi deðeri alabilir yani 0-200
          //starting position,going forward      ,   ,
            fDist = 1 - Round(hit.distance / visibleDistance);//artýk oran hesaplanýr
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
                           Quaternion.AngleAxis(-45, Vector3.up) * this.transform.right, out hit, visibleDistance))//anything under the 200
        {
            r45Dist = 1 - Round(hit.distance / visibleDistance);
        }
        if (Physics.Raycast(transform.position,
                           Quaternion.AngleAxis(45, Vector3.up) * -this.transform.right, out hit, visibleDistance))//anything under the 200
        {
            l45Dist = 1 - Round(hit.distance / visibleDistance);
        }

        inputs.Add(fDist);
        inputs.Add(rDist);
        inputs.Add(lDist);
        inputs.Add(r45Dist);
        inputs.Add(l45Dist);
        //before when we had a human player
        //we were just writing meant to file 
        outputs.Add(0);
        outputs.Add(0);
        calcOutputs = ann.CalcOutput(inputs, outputs);
        float translationInput = Map(-1, 1, 0, 1, (float)calcOutputs[0]);
        float rotationInput = Map(-1, 1, 0, 1, (float)calcOutputs[1]);
        translation = translationInput * speed * Time.deltaTime;
        rotation = rotationInput * rotatitonSpeed * Time.deltaTime;
        this.transform.Translate(0, 0, translation);
        this.transform.Rotate(0, rotation, 0);
    }


    //training iþlemi yapýlýrken en düþük deðere sahip yerler vardýr bu yerlere
    //local optima denir  neural networkler bu verilerin buralara girip çýkamamasýndan
    //muzdaripler.Ancak gerçekte küçük olaný bulursak ve buraya geri dönemeyeceðimzie emin olursal
    //en küçüðü bulur ve o deðere bi daha dönmeyiz
}
