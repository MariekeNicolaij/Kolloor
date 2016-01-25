using UnityEngine;
using System.Collections;

public class PuzzleSlot : MonoBehaviour
{
    public PuzzleColors puzzleColor;
    Transform puzzleSlot;

    Vector3 positionInGround;

    public bool lerpToGround;

    float slotHeight = 3.05f;
    float maxLerpDistance = 0.025f;


    void Start()
    {
        puzzleSlot = transform.parent;
        positionInGround = puzzleSlot.position;
        positionInGround.y = puzzleSlot.position.y - slotHeight;
    }

    void Update()
    {
        if (lerpToGround)
            LerpToGround();
    }

    void LerpToGround()
    {
        puzzleSlot.position = Vector3.Lerp(puzzleSlot.position, positionInGround, Time.smoothDeltaTime);

        if (Vector3.Distance(puzzleSlot.position, positionInGround) < maxLerpDistance)
        {
            ColorManager.instance.UnlockColor(puzzleColor, transform.position, 0.5f);       // Activate color
            lerpToGround = false;
        }
    }
}