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
    public float MaxDropforce = 50;

    public GameObject Laser;

    [Range(0, 5)]
    public float LaserOnFor = 0.4f;

    private GameObject HoldingObject;

    private Player player;

    #region layers
    public Layers[] layersToPickUp;
    #endregion

    private bool lerpObject = false;

    private ParticleSystem currentParticleSystem;

    private GameObject RaycastObject;

    private ParticleSystem laserparticle;

    private bool showLaser = false;

    private Vector3 StandartLaserRotation;

    public void Start(Player player)
    {
        dropforceBuilder = DropforceBuilderDefault;

        this.player = player;

        if (!ObjectHolder)
        {
            Debug.LogError("the ObjectHolder of the laser gun is not set");
        }

        if (!Laser)
            Debug.LogError("add an laser gameobject to the player in the laser gun");

        if (!Laser.GetComponent<ParticleSystem>())
            Debug.LogError("Laser is not set, it should be a gameobject with only a line renderer");
        else {
            laserparticle = Laser.GetComponent<ParticleSystem>();
            if (Laser.activeSelf)
                Laser.SetActive(false);
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

        if (!(dropforceBuilder > MaxDropforce))
            dropforceBuilder += DropForce;

        if (Vector3.Distance(HoldingObject.transform.position, ObjectHolder.transform.position) <= 0.05)
        {
            HoldingObject.transform.localPosition = ObjectHolder.transform.localPosition;
            HoldingObject.transform.localRotation = ObjectHolder.transform.localRotation;
            lerpObject = false;
            showLaser = false;
        }
    }


    public void LaserGunStartUp()
    {
        StandartLaserRotation = Laser.transform.rotation.eulerAngles;
    }

    public bool Shoot()
    {
        if (HoldingObject == null && RaycastObject != null)
        {
            showLaser = true;
            TreatGameObject();
            return true;
        }
        else if (HoldingObject != null)
        {
            DropCurrentObject();
        }

        return false;
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

            //start timer from when last interacted with puzzleobject
            AIManager.instance.SetTimer(true);
        }
        else if (HoldingObject.layer == (int)Layers.AI)
        {
            BaseAI AI = HoldingObject.GetComponent<BaseAI>();
            AI.DropDown();
        }

        currentParticleSystem.gameObject.SetActive(false);

        if (!DropedBySlot)
        {
            HoldingObject.transform.parent = null;
            rigidBody.useGravity = true;
            rigidBody.constraints = RigidbodyConstraints.None;

            if (DropWithForce)
                rigidBody.AddForce(Camera.main.ViewportPointToRay(new Vector3(.5f, .5f)).direction * dropforceBuilder, ForceMode.Impulse);
        }
        dropforceBuilder = DropforceBuilderDefault;

        if (showLaser)
            showLaser = false;

        HoldingObject = null;
    }

    public IEnumerator LaserEffect(bool ShootSomeThing)
    {
        Laser.SetActive(true);

        float lifeTime = laserparticle.startLifetime;

        if (ShootSomeThing)
        {
            while (showLaser)
            {
                Laser.transform.LookAt(HoldingObject.transform.position);

                laserparticle.startLifetime -= Time.deltaTime * .1f;

                yield return null;
            }

            laserparticle.startLifetime = lifeTime;
        }
        else
        {
            float timer = 0;

            while (timer <= LaserOnFor)
            {

                timer += Time.deltaTime;

                Laser.transform.Rotate(StandartLaserRotation);

                yield return null;
            }
        }

        Laser.SetActive(false);
    }

    private void TreatGameObject()
    {
        Rigidbody rigidBody = RaycastObject.GetComponent<Rigidbody>();

        if (RaycastObject.layer == (int)Layers.PuzzleObject && rigidBody.GetComponent<PuzzleObject>().active)
        {
            AudioManager.instance.PlaySound(AudioCategory.Pickup, false, true);
            RaycastObject.GetComponent<SphereCollider>().radius = 0.5f;
            RaycastObject.transform.parent = ObjectHolder.transform.parent;
            HoldingObject = RaycastObject;

            currentParticleSystem.startColor = Color.green;

            //stop timer from when last interacted with puzzleobject
            AIManager.instance.SetTimer(false);
        }
        else if (RaycastObject.layer == (int)Layers.AI)
        {
            GameObject originalObject = RaycastObject;

            if (!RaycastObject.GetComponent<BaseAI>())
                originalObject = RaycastObject.transform.parent.gameObject;

            BaseAI AI = originalObject.GetComponent<BaseAI>();
            AI.PickUp();
            originalObject.transform.parent = ObjectHolder.transform.parent;
            HoldingObject = originalObject;

            currentParticleSystem.startColor = Color.red;

            rigidBody = AI.GetComponentInParent<Rigidbody>();
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

                    RaycastObject = hit.collider.gameObject;


                    if (currentParticleSystem != null)
                        currentParticleSystem.gameObject.SetActive(false);

                    currentParticleSystem = RaycastObject.GetComponentInChildren<ParticleSystem>(true);
                    if (currentParticleSystem != null)
                    {
                        currentParticleSystem.gameObject.SetActive(true);

                        currentParticleSystem.startColor = Color.yellow;
                    }
                }
                return;
            }
        }

        if (currentParticleSystem != null)
        {
            currentParticleSystem.startColor = new Color(0, 0, 0, 0);
            currentParticleSystem.gameObject.SetActive(false);
            currentParticleSystem = null;
        }

        RaycastObject = null;
    }
}