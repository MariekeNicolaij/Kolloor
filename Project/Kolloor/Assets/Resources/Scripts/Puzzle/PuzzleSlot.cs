using UnityEngine;
using System.Collections;

public class PuzzleSlot : MonoBehaviour
{
    public PuzzleColors puzzleColor;
    Transform puzzleSlot;

    Vector3 positionInGround;

    public bool lerpToGround;
    float lerpTime = 0, lerpSpeed = 0.001f;

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
        lerpTime += lerpSpeed;
        puzzleSlot.position = Vector3.Lerp(puzzleSlot.position, positionInGround, lerpTime);

        if (Vector3.Distance(puzzleSlot.position, positionInGround) < maxLerpDistance)
        {
            ColorManager.instance.UnlockColor(puzzleColor, transform.position, 0.5f);       // Activate color
            AudioManager.instance.PlaySound(AudioCategory.PuzzleSlot);                      // Play sound
            lerpToGround = false;
            lerpTime = 0;
        }
    }
}