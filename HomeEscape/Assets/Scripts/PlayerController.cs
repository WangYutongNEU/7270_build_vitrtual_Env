using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;

    public GameObject bulletPrefab;

    public AudioClip collectClip;
    public AudioClip hitClip;
    public AudioClip launchClip;
    public AudioClip deadClip;
    public AudioClip successClip;

    public GameObject dialogImage1;
    public GameObject dialogImage2;
    public Text winText;

    // 以下都为private，提交前修改
    private float moveX;
    private float moveY;

    private Rigidbody2D rb;

    private int maxLife = 5;
    private int curLife;
    private int maxMagic = 5;
    private int curMagic;

    private float timeInvincible = 1.0f;
    private bool isInvincible;
    private float invincibleTimer;

    private Animator animator;
    private Vector2 lookDirection = new Vector2(1,0);

    private float showTime1 = 4f;
    private float showTime2 = 8f;
    private float showTimer1;
    private float showTimer2;
    private int clipRound = 1;
    private bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        winText.text = "";
        curLife = maxLife;
        curMagic = maxMagic;
        animator = GetComponent<Animator>();
        dialogImage1.SetActive(false);
        dialogImage2.SetActive(false);
        showTimer1 = -1;
        showTimer2 = -1;
        
    }

    // Update is called once per frame
    void Update()
    {

        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        // set looking direction
        Vector2 move = new Vector2(moveX, moveY);

        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);


        // set when attacked
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        // launch bullet
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if (curMagic != 0){
                Launch();
            }
            
        }

        showTimer1 -= Time.deltaTime;
        showTimer2 -= Time.deltaTime;
        if (showTimer1 < 0) {
            dialogImage1.SetActive(false);
        }
        if (showTimer2 < 0) {
            dialogImage2.SetActive(false);
        }

        if (curLife == 0) {
            winText.text = "YOU ARE DEAD : (";
            
            if (clipRound != 0){
                AudioManager.instance.AudioPlay(deadClip);
                clipRound -= 1;
            }
            Destroy(gameObject);
        }

    }

    void FixedUpdate() {
        Vector2 position = rb.position;
        position.x = position.x + speed * moveX * Time.deltaTime;
        position.y = position.y + speed * moveY * Time.deltaTime;

        rb.MovePosition(position);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //deactivate once collide
        if (other.gameObject.CompareTag("Red"))
        {
            if (curLife < maxLife){
                other.gameObject.SetActive(false);
                ChangeLife(1);
                AudioManager.instance.AudioPlay(collectClip);
                // Debug.Log("Red" + curLife + "/" + maxLife);
            }

        }
        else if (other.gameObject.CompareTag("Blue"))
        {
            if (curMagic < maxMagic){
                other.gameObject.SetActive(false);
                ChangeMagic(1);
                AudioManager.instance.AudioPlay(collectClip);
                // Debug.Log("Blue: " + curMagic + "/" + maxMagic);
            }

        }
        else if (other.gameObject.CompareTag("dialog1")){
            showTimer1 = showTime1;
            dialogImage1.SetActive(true);
            
        }
        else if (other.gameObject.CompareTag("dialog2")){
            showTimer2 = showTime2;
            dialogImage2.SetActive(true);
        }
        else if (other.gameObject.CompareTag("Key")){
            other.gameObject.SetActive(false);
            speed = 0;
            winText.text = "YOU SUCCESSFULLY ESCAPED!!!";
            AudioManager.instance.AudioPlay(successClip);
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other){
        if (other.gameObject.CompareTag("Trap")){
            ChangeLife(-1);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            AudioManager.instance.AudioPlay(hitClip);
            ChangeLife(-1);
        }
    }

    void Launch()
    {
        GameObject bullet = Instantiate(bulletPrefab, rb.position, Quaternion.identity);

        BulletController bc = bullet.GetComponent<BulletController>();
        bc.Launch(lookDirection, 300);
        AudioManager.instance.AudioPlay(launchClip);
        animator.SetTrigger("Launch");
        ChangeMagic(-1);
    }

    public void ChangeLife(int amount){
        // 每一帧都call ChangeLife 并且amount小于0，为什么isInvicible不会反复重置？
        if (isAlive == false){
            return;
        }
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
                return;
            
            isInvincible = true;
            AudioManager.instance.AudioPlay(hitClip);
            invincibleTimer = timeInvincible;
        }
        // limit the player's life b/w 0~10
        curLife = Mathf.Clamp(curLife + amount, 0, maxLife);
        // update health bar
        UImanager.instance.UpdateHealth(curLife, maxLife);
        Debug.Log("Red" + curLife + "/" + maxLife);
    }
    public void ChangeMagic(int amount){
        // limit the player's magic b/w 0~10
        curMagic = Mathf.Clamp(curMagic + amount, 0, maxMagic);
        UImanager.instance.UpdateMagic(curMagic, maxMagic);
        Debug.Log("Blue" + curMagic + "/" + maxMagic);
    }
    

}
