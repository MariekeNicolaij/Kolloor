using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    public void Update()
    {
        Move();
        Jump();
        Sprint();
        Interact();
        Pause();
    }

    void Move()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            Player.instance.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
            Player.instance.Jump();
    }

    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
            Player.instance.Sprint(true);
        else if (Input.GetButtonUp("Sprint"))
            Player.instance.Sprint(false);
    }

    void Interact()
    {
        if (Input.GetButtonDown("Interact"))
            Player.instance.Shoot();
    }

    void Pause()
    {
        if (Input.GetButtonDown("Pause"))
            Player.instance.Pause();
    }
}