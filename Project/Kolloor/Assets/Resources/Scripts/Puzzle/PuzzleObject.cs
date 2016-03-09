using UnityEngine;
using Managers;

public class PuzzleObject : MonoBehaviour
{
    public PuzzleColors puzzleColor;

    public PuzzleObjectManager Manager;

    PuzzleSlot puzzleSlot;

    Vector3 startPosition;
    Vector3 positionInPlayer = new Vector3(1, -0.5f, 2);
    Vector3 positionInSlot = new Vector3(0, 0, 0);
    Vector3 scaleInSlot = new Vector3(2, 2, 2);

    [HideInInspector]
    public bool lerpToPlayer, lerpToSlot;
    [HideInInspector]
    public bool active = true;

    float lerpTime = 0, lerpSpeed = 0.001f;
    float maxFallDepth = -10;
    float maxLerpDistance = 0.025f;

    int ID;

    void Start()
    {
        if (Manager == null)
            Manager = PuzzleObjectManager.instance;

        ID = Manager.Register(this);

        startPosition = transform.position;
    }

    void Update()
    {
        Respawn();
        if (lerpToPlayer)
            LerpToPlayer();
        if (lerpToSlot)
            LerpToSlot();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Puzzle Slot")
            PuzzleSlotTriggerEnter(other);
    }

    void PuzzleSlotTriggerEnter(Collider other)
    {
        if (puzzleSlot || !other.GetComponent<PuzzleSlot>())
            return;
        puzzleSlot = other.GetComponent<PuzzleSlot>();

        if (puzzleColor == puzzleSlot.puzzleColor)
            ActivateAndLerpObject();
        else
            puzzleSlot = null;
    }

    void Respawn()
    {
        if (transform.position.y < maxFallDepth)
        {
            transform.position = startPosition;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    void LerpToSlot()
    {
        lerpTime += lerpSpeed;
        transform.localPosition = Vector3.Lerp(transform.localPosition, positionInSlot, lerpTime);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(Vector3.zero), lerpTime);

        if (Vector3.Distance(transform.localPosition, positionInSlot) < maxLerpDistance)
        {
            transform.localPosition = positionInSlot;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.Lerp(transform.localScale, scaleInSlot, lerpTime);
        }
        if (Vector3.Distance(transform.localScale, scaleInSlot) < maxLerpDistance)
        {
            AudioManager.instance.PlaySound(AudioCategory.InSlot);
            transform.localScale = scaleInSlot;
            puzzleSlot.lerpToGround = true;
            lerpToSlot = false;
            lerpTime = 0;
        }
    }

    void LerpToPlayer()
    {
        lerpTime += lerpSpeed;
        transform.localPosition = Vector3.Lerp(transform.localPosition, positionInPlayer, lerpTime);
        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, Vector3.zero, lerpTime);

        if (Vector3.Distance(transform.localPosition, positionInPlayer) < maxLerpDistance)
        {
            transform.localPosition = positionInPlayer;
            transform.localRotation = Quaternion.identity;
            lerpToPlayer = false;
            lerpTime = 0;
        }
    }

    void ActivateAndLerpObject()
    {
        Player.instance.Drop(false);
        transform.SetParent(puzzleSlot.transform);
        active = false;
        lerpToSlot = true;
    }
}