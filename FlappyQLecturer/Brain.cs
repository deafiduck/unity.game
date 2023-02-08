using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Replay
{

    public List<double> states;
    public double reward;

    public Replay(double dtop, double dbot,double reward)
    {
        states = new List<double>();
        states.Add(dtop);
        states.Add(dbot);
        this.reward = reward;
    }
}

public class Brain : MonoBehaviour
{

    public GameObject topBeam;
    public GameObject bottomBeam;
    ANN ann;

    float reward = 0.0f;
    List<Replay> replayMemory = new List<Replay>();
    int mCapacity = 1000;

    float discount = 0.99f;//gelecekteki ödülleri satýn alýrken ne kadar indirim yapacaðýmýzdýr
    float exploreRate = 100.0f;//chance of picking random action
    float maxExploreRate = 100.0f;
    float minExploreRate = 00.01f;
    float exploreDecay = 0.0001f;

    Vector3 birdStartPosition;

    int failCount = 0;
    float moveForce = 0.5f;

    float timer = 0;
    float maxBalanceTime = 0;

    bool crashed = false;
    Rigidbody2D rb;
    void Start()
    {
        ann = new ANN(2, 2, 1, 6, 0.2f);
        birdStartPosition = this.transform.position;
        Time.timeScale = 5.0f;
        rb = this.GetComponent<Rigidbody2D>();
    }

    /*GUIStyle guiStyle = new GUIStyle();
    void OnGUI()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 600, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
        GUI.Label(new Rect(10, 25, 500, 30), "Fails" + failCount, guiStyle);
        GUI.Label(new Rect(10, 50, 500, 30), "Decay Rate" + exploreRate, guiStyle);
        GUI.Label(new Rect(10, 75, 500, 30), "Last Best Balance" + maxBalanceTime, guiStyle);
        GUI.Label(new Rect(10, 100, 500, 30), "This Balance" + timer, guiStyle);
        GUI.EndGroup();

    }*/

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            ResetBird();
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        crashed = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        crashed = false;
    }
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        List<double> states = new List<double>();
        List<double> qs = new List<double>();

        states.Add(Vector3.Distance(this.transform.position, topBeam.transform.position));
        states.Add(Vector3.Distance(this.transform.position, bottomBeam.transform.position));

        qs = SoftMax(ann.CalcOutput(states));
        double MaxQ = qs.Max();

        int maxQIndex = qs.ToList().IndexOf(MaxQ);
        exploreRate = Mathf.Clamp(exploreRate - exploreDecay, minExploreRate, maxExploreRate);


        if (maxQIndex == 0)
        {
            rb.AddForce(Vector3.up * moveForce * (float)qs[maxQIndex]);
        }
        else if (maxQIndex == 1)
        {
            rb.AddForce(Vector3.up * -moveForce * (float)qs[maxQIndex]);
        }

        if (crashed)
        {
            reward = -1.0f;

        }
        else
        {
            reward = 0.1f;
        }


        Replay lastMemory = new Replay(Vector3.Distance(this.transform.position, topBeam.transform.position),
                                Vector3.Distance(this.transform.position, bottomBeam.transform.position),
                                reward);


        if (replayMemory.Count > mCapacity)
        {
            replayMemory.RemoveAt(0);
        }

        replayMemory.Add(lastMemory);

        if (crashed)
        {

            for (int i = replayMemory.Count - 1; i >= 0; i--)
            {
                List<double> toutputsOld = new List<double>();//what our q values with the current memory
                List<double> toutputsNew = new List<double>();
                toutputsOld = SoftMax(ann.CalcOutput(replayMemory[i].states));
                //toutputsOld = ann.CalcOutput(replayMemory[i].states);
                double maxQold = toutputsOld.Max();
                int action = toutputsOld.ToList().IndexOf(maxQold);

                double feedback;
                if (i == replayMemory.Count - 1 || replayMemory[i].reward == -1)
                {
                    //if we are last memory on the list  there is no nxt memory to get any maximum values
                    feedback = replayMemory[i].reward;
                }
                else
                {
                    toutputsNew = SoftMax(ann.CalcOutput(replayMemory[i + 1].states));
                    //toutputsNew = ann.CalcOutput(replayMemory[i + 1].states);
                    MaxQ = toutputsNew.Max();
                    feedback = (replayMemory[i].reward +
                        discount * MaxQ);
                }

                toutputsOld[action] = feedback;
                ann.Train(replayMemory[i].states, toutputsOld);
            }
            if (timer > maxBalanceTime)
            {
                maxBalanceTime = timer;
            }
            timer = 0;
            crashed = false;
            
            ResetBird();//her þeyi sýfýrlar
            replayMemory.Clear();
            failCount++;
        }
    }
    void ResetBird()
    {
        this.transform.position = birdStartPosition;
        this.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
       // bird.GetComponent<Rigidbody2D>().angularVelocity = new Vector3(0, 0, 0);
    }

    List<double> SoftMax(List<double> values)//outputs values on the list 
    {
        //softmax is used quite often as a calculation on the output layer of neural nets
        double max = values.Max();

        float scale = 0.0f;
        for (int i = 0; i < values.Count; ++i)
        {
            scale += Mathf.Exp((float)(values[i] - max));
            //how far each value in the array is away from the maximum value that's in the array
        }
        List<double> result = new List<double>();
        for (int i = 0; i < values.Count; i++)
        {
            result.Add(Mathf.Exp((float)(values[i] - max)) / scale);
        }
        return result;

    }

}


