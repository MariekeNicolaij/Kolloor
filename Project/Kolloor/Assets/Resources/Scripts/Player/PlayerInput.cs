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
        DropObject();
    }

    void Move()
    {
            Player.instance.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
            Player.instance.Jump();
    }

    void Sprint()
    {
            Player.instance.Sprint(Input.GetButton("Sprint"));
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

    void DropObject()
    {
        if (Input.GetButtonDown("Drop"))
            Player.instance.MajorLazer.DropCurrentObject(false);
    }
}