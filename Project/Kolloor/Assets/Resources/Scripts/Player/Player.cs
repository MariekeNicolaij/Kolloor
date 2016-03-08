using UnityEngine;
using System.Collections;
using Managers;

public class Player : MonoBehaviour
{
    public LaserGun MajorLazer;

    public static Player instance;
    PuzzleObject puzzleObject;
    CharacterController characterController;

    Vector3 startPosition;
    Vector3 moveDirection = Vector3.zero;

    public bool underwater;

    public bool pickedUp;
    bool canPickup;
    bool interact;
    bool pause;
    bool sprint;

    float maxFallDepth = -10;
    float underwaterSpeed = 3;
    float moveSpeed = 6;
    float sprintSpeed = 12;
    float jumpSpeed = 4;
    float gravity = 9.81f;


    void Start()
    {
        instance = this;
        characterController = GetComponent<CharacterController>();
        startPosition = transform.position;
        gravity = Physics.gravity.y;

        MajorLazer.Start(this);
    }

    void Update()
    {
        Gravity();
        Respawn();
        MajorLazer.Update();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Puzzle Object")
            PuzzleObjectTriggerEnter(other);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Puzzle Object")
            PuzzleObjectTriggerExit(other);
    }

    void PuzzleObjectTriggerEnter(Collider other)
    {
        if (puzzleObject || !other.GetComponent<PuzzleObject>())
            return;
        puzzleObject = other.GetComponent<PuzzleObject>();

        if (puzzleObject.active)
        {
            canPickup = true;
        }
    }

    void PuzzleObjectTriggerExit(Collider other)
    {
        if (pickedUp)
            return;

        puzzleObject = null;
        canPickup = false;
    }

    void Respawn()
    {
        if (transform.position.y < maxFallDepth)
            transform.position = startPosition;
    }

    void Gravity()
    {
        moveDirection.y += gravity * Time.smoothDeltaTime;
        characterController.Move(moveDirection * Time.smoothDeltaTime);
    }

    public void Move(float horizontalAxis, float verticalAxis)
    {
        float speed;

        if (underwater)
            speed = underwaterSpeed;
        else if (sprint || OnIce())
            speed = sprintSpeed;
        else
            speed = moveSpeed;

        transform.Translate(Vector3.right * horizontalAxis * Time.smoothDeltaTime * speed / 2);
        transform.Translate(Vector3.forward * verticalAxis * Time.smoothDeltaTime * speed);
    }

    public void Jump()
    {
        if (characterController.isGrounded)
            moveDirection.y = jumpSpeed;
    }

    public void Sprint(bool sprint)
    {
        this.sprint = sprint;
    }

    public void Interact()
    {

        AudioManager.instance.PlaySound(AudioCategory.Pickup, false, true);
        if (!puzzleObject)
            return;

        MajorLazer.Shoot();

        //if (!puzzleObject)
        //    return;


        //if (canPickup && !pickedUp)
        //    Pickup();
        //else if (pickedUp)
        //    Drop(true);
    }

    public void Pause()
    {
        pause = System.Convert.ToBoolean(Time.timeScale);
        PauseMenu.instance.Pause(pause);
    }

    bool OnIce()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.tag == "Ice")
                return true;
        } return false;
    }

    void Pickup()
    {
        puzzleObject.GetComponent<Rigidbody>().useGravity = false;
        puzzleObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        puzzleObject.GetComponent<SphereCollider>().radius = 0.5f;
        puzzleObject.transform.parent = transform;
        puzzleObject.lerpToPlayer = true;
        pickedUp = true;
    }

    public void Drop(bool playerDrop)
    {
        if (playerDrop)
        {
            puzzleObject.GetComponent<Rigidbody>().useGravity = true;
            puzzleObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            puzzleObject.GetComponent<SphereCollider>().radius = 3;
            puzzleObject.transform.parent = null;
        }
        pickedUp = false;
        puzzleObject.lerpToPlayer = false;
        puzzleObject = null;
    }
}