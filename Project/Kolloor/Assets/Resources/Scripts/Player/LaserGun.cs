using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using AI;
using Managers;

[Serializable]
public class LaserGun
{
    public GameObject ObjectHolder;
    public float LaserLength = 5;
    [Range(0, 10)]
    public float DropForce = 1;


    private float dropforceBuilder;
    public float DropforceBuilderDefault = 1;
    public float MaxDropForce = 50;

    private GameObject HoldingObject;

    private Player player;

    #region layers
    public Layers[] layersToPickUp;
    #endregion

    private bool lerpObject = false;

    private Material currentMat;

    private GameObject RaycastObject;

    public void Start(Player player)
    {
        dropforceBuilder = DropforceBuilderDefault;

        this.player = player;

        if (ObjectHolder == null)
        {
            Debug.LogError("the ObjectHolder of the laser gun is not set");
        }
    }

    public void Update()
    {
        if (lerpObject)
        {
            LerpObject();
        }
        if (HoldingObject == null)
            Raycast();
    }

    private void LerpObject()
    {
        HoldingObject.transform.localPosition = Vector3.Lerp(HoldingObject.transform.localPosition, ObjectHolder.transform.localPosition, Time.smoothDeltaTime);
        HoldingObject.transform.localRotation = Quaternion.Lerp(HoldingObject.transform.localRotation, ObjectHolder.transform.localRotation, Time.smoothDeltaTime);

        if (!(dropforceBuilder > MaxDropForce))
            dropforceBuilder += DropForce;

        if (Vector3.Distance(HoldingObject.transform.position, ObjectHolder.transform.position) <= 0.05)
        {
            HoldingObject.transform.localPosition = ObjectHolder.transform.localPosition;
            HoldingObject.transform.localRotation = ObjectHolder.transform.localRotation;
            lerpObject = false;
        }
    }

    public void Shoot()
    {
        if (HoldingObject == null && RaycastObject != null)
        {
            TreatGameObject();
        }
        else if (HoldingObject != null)
        {
            DropCurrentObject();
        }
    }

    public void DropCurrentObject(bool DropedBySlot = false, bool DropWithForce = true)
    {
        if (HoldingObject == null)
            return;

        if (lerpObject)
            lerpObject = false;

        Rigidbody rigidBody = HoldingObject.GetComponent<Rigidbody>();

        if (HoldingObject.layer == (int)Layers.PuzzleObject)
        {
            HoldingObject.GetComponent<SphereCollider>().radius = 3;
            HoldingObject.GetComponent<PuzzleObject>().lerpToPlayer = false;

            // start timer from when last interacted with puzzleobject
            //AIManager.instance.SetTimer(true);
        }
        else if (HoldingObject.layer == (int)Layers.AI)
        {
            BaseAI AI = HoldingObject.GetComponent<BaseAI>();

            AI.stateManager.SwitchToDefault();
            AI.Holded = false;

            SetChilderen(HoldingObject, new Color(0, 0, 0, 0));
        }

        currentMat.SetColor("_OutlineColor", new Color(0, 0, 0, 0));

        if (!DropedBySlot)
        {
            HoldingObject.transform.parent = null;
            rigidBody.useGravity = true;
            rigidBody.constraints = RigidbodyConstraints.None;

            Debug.Log(dropforceBuilder);

            if (DropWithForce)
                rigidBody.AddForce(Camera.main.ViewportPointToRay(new Vector3(.5f, .5f)).direction * dropforceBuilder, ForceMode.Impulse);
        }
        dropforceBuilder = DropforceBuilderDefault;


        HoldingObject = null;
    }

    private void LaserEffect()
    {

    }

    private void TreatGameObject()
    {
        Rigidbody rigidBody = RaycastObject.GetComponent<Rigidbody>();

        Debug.Log(rigidBody);

        if (RaycastObject.layer == (int)Layers.PuzzleObject && rigidBody.GetComponent<PuzzleObject>().active)
        {
            AudioManager.instance.PlaySound(AudioCategory.Pickup, false, true);
            RaycastObject.GetComponent<SphereCollider>().radius = 0.5f;
            RaycastObject.transform.parent = ObjectHolder.transform.parent;
            HoldingObject = RaycastObject;

            currentMat.SetColor("_OutlineColor", Color.green);

            // stop timer from when last interacted with puzzleobject
            //AIManager.instance.SetTimer(false);
        }
        else if (RaycastObject.layer == (int)Layers.AI)
        {
            GameObject originalObject = RaycastObject;

            if (!RaycastObject.GetComponent<BaseAI>())
                originalObject = RaycastObject.transform.parent.gameObject;

            BaseAI AI = originalObject.GetComponent<BaseAI>();
            AI.stateManager.ChangeState(new AI.States.IdleState());
            originalObject.transform.parent = ObjectHolder.transform.parent;
            AI.Holded = true;
            HoldingObject = originalObject;

            SetChilderen(originalObject, Color.red);
            currentMat.SetColor("_OutlineColor", Color.red);
        }
        else
            RaycastObject.transform.parent = ObjectHolder.transform.parent;

        rigidBody.useGravity = false;
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;

        lerpObject = true;
    }

    private void Raycast()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f));

        Debug.DrawRay(ray.origin, ray.direction, Color.red, 2);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, maxDistance: LaserLength))
        {
            if (layersToPickUp.Contains((Layers)hit.collider.gameObject.layer))
            {
                if (RaycastObject != hit.collider.gameObject)
                {
                    if (RaycastObject != null && RaycastObject.layer == (int)Layers.AI)
                    {
                        SetChilderen(RaycastObject, new Color(0, 0, 0, 0));
                    }

                    RaycastObject = hit.collider.gameObject;


                    if (currentMat != null)
                        currentMat.SetColor("_OutlineColor", new Color(0, 0, 0, 0));


                    currentMat = RaycastObject.GetComponent<MeshRenderer>().materials[0];

                    currentMat.SetColor("_OutlineColor", Color.yellow);

                    if (RaycastObject.layer == (int)Layers.AI)
                    {
                        SetChilderen(RaycastObject, Color.yellow);
                    }
                }
                return;
            }
        }

        if (currentMat != null)
        {
            currentMat.SetColor("_OutlineColor", new Color(0, 0, 0, 0));
            currentMat = null;

            if (RaycastObject.layer == (int)Layers.AI)
            {
                SetChilderen(RaycastObject, new Color(0, 0, 0, 0));
            }
        }

        RaycastObject = null;
    }

    /// <summary>
    /// sets the ouline collor for al childeren
    /// </summary>
    /// <param name="obj"> the parent object of the childeren </param>
    /// <param name="color"> the color to set the outline to </param>
    private void SetChilderen(GameObject obj, Color color)
    {
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            obj.transform.GetChild(i).GetComponent<MeshRenderer>().materials[0].SetColor("_OutlineColor", color);
        }
    }
}