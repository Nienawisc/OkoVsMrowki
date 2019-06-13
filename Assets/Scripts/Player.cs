using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]float speed = 5;
    [SerializeField] float cooldownSpeed = 10;
    [SerializeField] Tilemap map;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject explosion;
    [SerializeField] Text textpoint;
    [SerializeField] TextMeshProUGUI textbomb;
    [SerializeField] RectTransform deadPanel;
    [SerializeField] float bulletSpeed = 500;
    [SerializeField] Sprite deadsprite;
    [SerializeField] RectTransform PauseScreen;
    int points = 0;

    int bombs = 0;

    float baseSpeed;
    float baseCooldownSpeed;

    float timeStamp=0;
    bool piercing = false;
    float boostTime = 0;

    bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        baseSpeed = speed;
        baseCooldownSpeed = cooldownSpeed;
        updateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            mouseTracking();
            movement();
            cameraTacking();
            shooting();
            if(Input.GetKeyDown(KeyCode.Space))
            {
                explode();
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                pauseScreen();
            }
        }
        else deadScreen();
    }
    float mouseTracking()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        return angle;
    }
    void movement()
    {
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");

        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis("Vertical");

        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
        transform.GetComponent<Rigidbody2D>().AddForce(movement * speed);
    }
    void cameraTacking()
    {
        Vector3 temp = transform.position;
        temp.z = Camera.main.transform.position.z;
        Camera.main.transform.position = temp;
    }
    void shooting()
    {
        if (Input.GetMouseButton(0)&& timeStamp <= Time.time)
        {
            GameObject b = Instantiate(bullet);
            Vector3 pos = transform.position;
            Vector2 angle = (Vector2)(Quaternion.Euler(0, 0, mouseTracking()) * Vector2.right);
            Vector3 offset = new Vector3(angle.x,angle.y);
            b.transform.position = pos + offset;
            b.GetComponent<Rigidbody2D>().AddForce(angle * bulletSpeed);
            b.GetComponent<bullet>().pierce = piercing;
            b.GetComponent<bullet>().owner = gameObject;
            timeStamp = Time.time + cooldownSpeed;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(boostTime>0)
        {
            if(Time.time>=boostTime)
            {
                boostTime = 0;
                resetStats();
            }
        }
        if (collision.gameObject.tag == "Boost")
        {
            resetStats();
            speed += collision.gameObject.GetComponent<Bust>().increasedSpeed;
            cooldownSpeed = collision.gameObject.GetComponent<Bust>().newCooldown;
            piercing = collision.gameObject.GetComponent<Bust>().piercing;
            boostTime = Time.time + collision.gameObject.GetComponent<Bust>().time;
            bombs += collision.gameObject.GetComponent<Bust>().addBomb;
            updateUI();
            Destroy(collision.gameObject);
        }
    }
    private void resetStats()
    {
        speed = baseSpeed;
        cooldownSpeed = baseCooldownSpeed;
        piercing = false;
    }
    public void increasePoints(int value)
    {
        points += value;
        updateUI();
    }
    private void updateUI()
    {
        textpoint.text = string.Format("Points: {0}", points);
        textbomb.text = string.Format("X {0}", bombs);
    }
    public void deadset(bool value)
    {
        dead = value;
        gameObject.GetComponent<SpriteRenderer>().sprite = deadsprite;
    }
    private void deadScreen()
    {
        textpoint.gameObject.SetActive(false);
        deadPanel.gameObject.SetActive(true);
        deadPanel.transform.Find("Points").GetComponent<Text>().text = string.Format("Points: {0}", points);
        Time.timeScale = 0;
    }
    public void resetLvl()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void saveToRating()
    {
        string save = string.Format("{0}-{1}-{2}", System.DateTime.Now, deadPanel.transform.Find("Input").Find("Text").GetComponent<Text>().text, points);
        string path = Application.persistentDataPath + "/rating.dat";
        Debug.Log(path);
        using (StreamWriter sw = new StreamWriter(path,true))
        {
            sw.WriteLine(save);
        }
        SceneManager.LoadScene("Menu");
    }
    private void explode()
    {
        if(bombs>0)
        { 
            GameObject exp = Instantiate(explosion);
            exp.transform.GetComponent<bullet>().owner = gameObject;
            exp.transform.position = transform.position;
            bombs--;
            updateUI();
        }
    }
    public void pauseScreen()
    {
        if(!PauseScreen.gameObject.activeSelf)
        {
            Time.timeScale = 0;
            PauseScreen.gameObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            PauseScreen.gameObject.SetActive(false);
        }
    }
    public void exitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
