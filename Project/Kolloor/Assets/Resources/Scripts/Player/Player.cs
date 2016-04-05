using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public static Player instance;

    public LaserGun MajorLazer;
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
            canPickup = true;
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

    bool CanMoveForward()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1))
            if (hit.collider && hit.collider.tag != "Next Level Portal")
                return false;
        return true;
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

        if (CanMoveForward())
            transform.Translate(Vector3.forward * verticalAxis * Time.smoothDeltaTime * speed);
        transform.Translate(Vector3.right * horizontalAxis * Time.smoothDeltaTime * speed / 2);
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

    public void Shoot()
    {
        if (Time.timeScale > 0)
            AudioManager.instance.PlaySound(AudioCategory.Shoot, false, true);
        StartCoroutine(MajorLazer.LaserEffect(MajorLazer.Shoot()));

        if (!puzzleObject)
            return;
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
            if (hit.collider.tag == "Ice")
                return true;
        return false;
    }
}