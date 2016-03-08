using UnityEngine;

//[ExecuteInEditMode]
public class Underwater : MonoBehaviour
{
    public float m_UnderwaterCheckOffset = 0.001f;
    public Color envFogColor;
    public Color underwaterFogColor = Color.blue;
    public GameObject underwaterStuff = null;

    public float skyFogDensity = 0.005f;
    public float waterFogDensity = 0.05f;

    bool wasUnderwater = false;

    public bool IsUnderwater(Camera cam)
    {
        Player.instance.underwater = cam.transform.position.y + m_UnderwaterCheckOffset < transform.position.y;
        return cam.transform.position.y + m_UnderwaterCheckOffset < transform.position.y;
    }

    public void OnWillRenderObject()
    {
        Camera cam = Camera.current;

        if (IsUnderwater(cam))
        {
            if (Camera.main == cam && !cam.gameObject.GetComponent(typeof(UnderwaterEffect)))
                cam.gameObject.AddComponent(typeof(UnderwaterEffect));

            UnderwaterEffect effect = (UnderwaterEffect)cam.gameObject.GetComponent(typeof(UnderwaterEffect));
            if (effect)
                effect.enabled = true;

            //Ok some HACK's here
            GetComponent<Renderer>().sharedMaterial.shader.maximumLOD = 50;

            if (!wasUnderwater)
            {
                wasUnderwater = true;

                //Change fog a little
                RenderSettings.fogDensity = waterFogDensity;
                RenderSettings.fogColor = underwaterFogColor;

                //Enable caustic
                if (underwaterStuff != null)
                    underwaterStuff.SetActive(true);
            }
        }
        else
        {
            UnderwaterEffect effect = (UnderwaterEffect)cam.gameObject.GetComponent(typeof(UnderwaterEffect));
            if (effect && effect.enabled)
                effect.enabled = false;

            //Ok some HACK's here
            GetComponent<Renderer>().sharedMaterial.shader.maximumLOD = 100;

            if (wasUnderwater)
            {
                //Change fog a little
                RenderSettings.fogDensity = skyFogDensity;
                RenderSettings.fogColor = envFogColor;
                wasUnderwater = false;

                //Disable caustic
                if (underwaterStuff != null)
                    underwaterStuff.SetActive(false);
            }
        }
    }
}