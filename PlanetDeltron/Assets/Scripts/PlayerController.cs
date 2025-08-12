using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private CharacterController myCharacterController;
    public TMP_Text distanceRan;
    public TMP_Text finalScore;
    public GameObject gameOverScreen;
    private Animator playerAnim;

    public int distanceUnit = 0;
    public int speed = 1;

    private bool turnLeft, turnRight, jump, slide;
    private bool canAddSpeed = false;
    private bool canJumpOrSlide = true;
    public bool isGameOver = false;

    // Jump variables
    private float jumpSpeed = 10;
    private float gravity = 20;
    private Vector3 moveDirection = new Vector3 (0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        // Get components of player
        playerAnim = GetComponent<Animator>();
        myCharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            // Get inputs as booleans
            turnLeft = Input.GetKeyDown(KeyCode.LeftArrow);
            turnRight = Input.GetKeyDown(KeyCode.RightArrow);
            jump = Input.GetKeyDown(KeyCode.UpArrow);
            slide = Input.GetKeyDown(KeyCode.DownArrow);

            if (turnLeft)
                // Turn 90 left
                transform.Rotate(new Vector3(0f, -90f, 0f));
            else if (turnRight)
                // Turn 90 right
                transform.Rotate(new Vector3(0f, 90f, 0f));

            if (myCharacterController.isGrounded)
            {
                // If player is on ground, and the jump or slide button is pressed whilst it's not on cooldown do:
                if (jump && canJumpOrSlide)
                {
                    playerAnim.SetTrigger("Jump_Trig");
                    // Add vertical velocity (jump)
                    moveDirection.y = jumpSpeed;
                    // Start jump or slide cooldown
                    canJumpOrSlide = false;
                    StartCoroutine(jumpOrSlideCooldown());
                }
                else if (slide && canJumpOrSlide)
                {
                    // Change players height and center to allow it to slide under obstacle
                    myCharacterController.height = 0.5f;
                    myCharacterController.center = new Vector3(0, 0.25f, 0);
                    StartCoroutine(waitForSlide());
                    playerAnim.SetTrigger("Slide_Trig");
                    canJumpOrSlide = false;
                    StartCoroutine(jumpOrSlideCooldown());
                }
            }

            // Add gravity to player
            if (moveDirection.y > -10)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            // Call distance method (adding score)
            distance();

            // Move player (gravity/jump/speed over time)
            myCharacterController.Move(moveDirection * Time.deltaTime);
            myCharacterController.Move(transform.forward * speed * Time.deltaTime);
        }
        // Check if player has fell off
        playerFellOff();
    }

    public void distance()
    {
        distanceRan.SetText("Score: " + distanceUnit.ToString());
        // if distance unit is dividable by 50 and speed is less than 9 add speed
        if ((distanceUnit > 0 && distanceUnit % 50 == 0) && canAddSpeed && speed < 9)
        {
            speed++;
            canAddSpeed = false;
        }
        else if (distanceUnit % 10 != 0)
        {
            canAddSpeed = true;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Player collides with obstacle
        if (hit.gameObject.CompareTag("Obstacle") && !isGameOver)
        {
            playerAnim.SetTrigger("Death_Trig");
            gameOver();
        }
    }

    // Method to check if player has fallen off
    private void playerFellOff()
    {
        if (myCharacterController.transform.position.y < -10)
        {
            playerAnim.SetTrigger("Death_Trig");
            gameOver();
        }
    }

    // Game over method
    private void gameOver()
    {
        isGameOver = true;
        distanceRan.enabled = false;
        finalScore.SetText("Final score: " + distanceUnit);
        gameOverScreen.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player enters Tile box
        if (other.CompareTag("Tile"))
        {
            // Increase points
            distanceUnit++;
            // Destroy tile after 5 seconds
            StartCoroutine(destroyTile(other.gameObject));
        }
    }
    IEnumerator destroyTile(GameObject tileToDestroy)
    {
        // Removing tiles that were stepped on
        yield return new WaitForSeconds(5);
        Destroy(tileToDestroy);
    }

    // Cooldown for jump or slide
    IEnumerator jumpOrSlideCooldown()
    {
        yield return new WaitForSeconds(1.5f);
        canJumpOrSlide = true;
    }

    // After slide animation setting back character controller height, centre
    IEnumerator waitForSlide()
    {
        yield return new WaitForSeconds(1.15f);
        myCharacterController.height = 2;
        myCharacterController.center = new Vector3(0, 0.98f, 0);

    }
}
