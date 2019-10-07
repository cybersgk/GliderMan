using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour {

    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;
    public static event PlayerDelegate OnPlayerLife;

    public Sprite blackWings;
    public Sprite silverWings;
    public Sprite goldWings;
    public Sprite emeraldWings;
    public SpriteRenderer wingMan;

    public AudioSource TapAudio;
    public AudioSource DieAudio;
    public AudioSource ScoreAudio;

    public float tapForce = 10;
	public float tiltSmooth = 5;
    public int life;
	public Vector3 startPos, worldCoordinates;
    public Text lifeText;
    public float speed;

	Rigidbody2D rigidBody;
	Quaternion downRotation;
	Quaternion forwardRotation;

    GameManager game;
    void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
        GameManager.OnGameLife += OnGameLife;
        GameManager.OnChangeRigidBody += OnChangeRigidBody;
    }
    void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
        GameManager.OnGameLife -= OnGameLife;
        GameManager.OnChangeRigidBody -= OnChangeRigidBody;
    }
    void OnGameStarted()
    {
        rigidBody.velocity = Vector3.zero;
        rigidBody.simulated = true;
    }
    void OnGameOverConfirmed()
    {
        transform.localPosition = startPos;
        //transform.rotation = Quaternion.identity;
    }
    void OnGameLife()
    {
        transform.localPosition = startPos;
        //transform.rotation = Quaternion.identity;
    }
    void OnChangeRigidBody()
    {
        if (!PlayerPrefs.HasKey("Wings"))
        {
            PlayerPrefs.SetInt("Wings", 0);
        }
        int wings = PlayerPrefs.GetInt("Wings");
        life = wings;
        lifeText.text = "EXTRA LIVES - " + life;
        if (wings == 0)
        {
            wingMan.sprite = blackWings;
        }
        if (wings == 1)
        {
            wingMan.sprite = silverWings;
        }
        if (wings == 2)
        {
            wingMan.sprite = goldWings;
        }
        if (wings == 3)
        {
            wingMan.sprite = emeraldWings;
        }
    }

    void Start(){
        rigidBody = GetComponent<Rigidbody2D>();
        //wingMan = GetComponent<SpriteRenderer>();
        //downRotation = Quaternion.Euler (0, 0, -70);
		//forwardRotation = Quaternion.Euler (0, 0, 25);
        game = GameManager.Instance;
        rigidBody.simulated = false;
        OnChangeRigidBody();
    }
    void Update(){
        if (game.GameOver)
        {
            return;
        }
        /*if (Input.GetMouseButtonDown(0))
        {
            TapAudio.Play();
            transform.rotation = forwardRotation;
            rigidBody.velocity = Vector3.zero;
            rigidBody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
        */
        foreach (Touch touch in Input.touches)
        {
            worldCoordinates = Camera.main.ScreenToWorldPoint(touch.position);
            //transform.position = new Vector3(worldCoordinates.x, transform.position.y);
            Vector3 tempPos = new Vector3(worldCoordinates.x, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, tempPos, Time.deltaTime * speed);
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "ScoreZone")
        {
            OnPlayerScored();
            ScoreAudio.Play();
            collider.isTrigger = false;
        }
        if (collider.gameObject.tag == "DeadZone")
        {
            if (life <= 0)
            {
                life = PlayerPrefs.GetInt("Wings");
                OnPlayerDied();
                rigidBody.simulated = false;
            }
            else
            {
                rigidBody.simulated = false;
                OnPlayerLife();
            }
            if(life>0)
            life--;
            lifeText.text = "EXTRA LIVES - " + life;
            DieAudio.Play();
        }
    }
}
