using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveBall : MonoBehaviour {

	Vector3 ballStartPosition;
	Rigidbody2D rb;
	float speed = 400;
	public AudioSource blip;
	public AudioSource blop;
    public float numMissed = 0;
    public float numMissed2 = 0;
    public static float elapsed = 0;


    public Text score1;
    public Text score2;

    void Start()
    {
		rb = this.GetComponent<Rigidbody2D>();
		ballStartPosition = this.transform.position;
		ResetBall();
        score1.text = numMissed.ToString();
        score2.text = numMissed2.ToString();

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        {


            if (collision.gameObject.tag == "backwall")
            {

                //yvel2 = 0;
                numMissed++;
                score1.text = numMissed.ToString();
                score2.text = numMissed2.ToString();
                blop.Play();
            }
            else if (collision.gameObject.tag == "backwall2")
            {

                numMissed2++;
                blop.Play();
                score1.text = numMissed.ToString();
                score2.text = numMissed2.ToString();
            }
            else
            {
                blip.Play();
            }
        }
    }
        
	public void ResetBall()
    {
		this.transform.position = ballStartPosition;
		rb.velocity = Vector3.zero;
		//if yoû've got the ball bouncing around the scene and reset it,
        //it will go back to it's starting position 
		//but it will still move with the same velocity
		//if you reset the ball you need to stop it from moving
		Vector3 dir = new Vector3(Random.Range(100, 300), Random.Range(-100, 100), 0).normalized;
		//normalized: Vector3 değişkenini alıp, x,y,z bileşen değerlerini vektörün boyutu 1 birim olacak şekilde hesaplar.
        //Vectorün sadece büyüklüğünü değiştirir, yönünde bir değişiklik olmaz.
		rb.AddForce(dir * 400);
    }

	void Update()
    {
        elapsed += Time.deltaTime;
        
            speed += 3*(elapsed);
        


        if (Input.GetKeyDown("space"))
        {
			ResetBall();
        }
		
    }
    
    }

