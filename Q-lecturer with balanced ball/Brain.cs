using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

 public class Replay
{
    public List<double> states;
    public double reward;

    public Replay(double xr,double ballz,double ballvz,double r)
    {
        states = new List<double>();
        states.Add(xr);//x direction of our platform
        states.Add(ballz); //ball z position of platfor
        states.Add(ballvz);//ball velocitys iz z direction
        reward = r;
    }
}


public class Brain : MonoBehaviour
{
    public GameObject ball;
    ANN ann;

    float reward = 0.0f;
    List<Replay> replayMemory = new List<Replay>();
    int mCapacity = 10000; //memory capacity

    float discount = 0.99f;//gelecekteki ödülleri satýn alýrken ne kadar indirim yapacaðýmýzdýr
    float exploreRate = 100.0f;//chance of picking random action
    float maxExploreRate = 100.0f;
    float minExploreRate = 00.01f;
    float exploreDecay = 0.0001f;
    /*bu nedenle, uygulama eðitiminin tamamýnda,
     * Keþfetme Oraný düþmeye baþlar ve her seferinde
     * buradaki azalma deðerinize göre düþer ve 
     * bu hesaplamayý biraz daha aþaðýda göreceksiniz*/

    Vector3 ballStartPos;
    int failCount = 0;
    float tiltSpeed = 0.5f;//eðim hýzý



    float timer = 0;
    float maxBalanceTime = 0;

    void Start()
    {
        ann = new ANN(3, 2, 1, 6, 0.2f);
        ballStartPos = ball.transform.position;
        Time.timeScale = 5.0f;
    }

    GUIStyle guiStyle = new GUIStyle();
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

    }
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            ResetBall();
        }
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;
        List<double> states = new List<double>();//rotx ballz velx= inputs
        List<double> qs = new List<double>();//q values

        states.Add(this.transform.rotation.x);
        states.Add(ball.transform.position.z);
        states.Add(ball.GetComponent<Rigidbody>().angularVelocity.x);

        qs = SoftMax(ann.CalcOutput(states));
        //qs = ann.CalcOutput(states);
        //olasýlýðý negatiften kurtarma ve olasýlýklar toplamýnýn [0,1]arasýnda olmasýný saðlar
        double maxQ = qs.Max();
        //for example tiltR 0.8  tiltR 0.2 -->their position then maxQ is 0.8 and index value is set the zero
        int maxQIndex = qs.ToList().IndexOf(maxQ);
        exploreRate = Mathf.Clamp(exploreRate - exploreDecay, minExploreRate, maxExploreRate);
        //random chance of doing random action

       /* if (Random.Range(0, 100) < exploreRate)
        {
            maxQIndex = Random.Range(0, 2);
        }*/
            if (maxQIndex == 0)
            {
                this.transform.Rotate(Vector3.right, tiltSpeed * (float)qs[maxQIndex]);
            }
            else if (maxQIndex == 1)
            {
                this.transform.Rotate(Vector3.right, -tiltSpeed * (float)qs[maxQIndex]);
            }

        if (ball.GetComponent<BallState>().dropped)
        {
            reward = -1.0f;

        }
        else
        {
            reward = 0.1f;
        }

        Replay lastMemory = new Replay(this.transform.rotation.x,
                                     ball.transform.position.z,
                                     ball.GetComponent<Rigidbody>().angularVelocity.x,
                                     reward);
        if (replayMemory.Count > mCapacity)
        {
            replayMemory.RemoveAt(0);
        }

        replayMemory.Add(lastMemory);

        if (ball.GetComponent<BallState>().dropped)
        {
            for(int i = replayMemory.Count - 1; i >= 0; i--)
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
                    maxQ = toutputsNew.Max();
                    feedback = (replayMemory[i].reward +
                        discount * maxQ);
                }

                toutputsOld[action] = feedback;
                ann.Train(replayMemory[i].states, toutputsOld);
            }
            if (timer > maxBalanceTime)
            {
                maxBalanceTime = timer;
            }
            timer = 0;
            ball.GetComponent<BallState>().dropped = false;
            this.transform.rotation = Quaternion.identity;
            ResetBall();//her þeyi sýfýrlar
            replayMemory.Clear();
            failCount++;        
        }

    }

    void ResetBall()
    {
        ball.transform.position = ballStartPos;
        ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        ball.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
    }

    List<double> SoftMax(List<double> values)//outputs values on the list 
    {
        //softmax is used quite often as a calculation on the output layer of neural nets
        double max = values.Max();

        float scale = 0.0f;
        for(int i = 0; i < values.Count; ++i)
        {
            scale += Mathf.Exp((float)(values[i] - max));
            //how far each value in the array is away from the maximum value that's in the array
        }
        List<double> result = new List<double>();  
        for(int i = 0; i < values.Count; i++)
        {
            result.Add(Mathf.Exp((float)(values[i] - max)) / scale);
        }
        return result;

    }
}
