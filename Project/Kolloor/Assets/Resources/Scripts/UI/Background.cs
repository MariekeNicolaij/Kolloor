using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Background : MonoBehaviour
{
    public List<Image> background;
    [Range(0, 10)]
    public List<float> speed;

    Vector2 beginPos;
    Vector2 lerpPos;
    Vector2 mousePos;

    float offSet = 50;


    void Start()
    {
        beginPos = background[0].transform.position;

        SpeedCheck();
    }

    void SpeedCheck()
    {
        if (speed.Count != background.Count)
            for (int i = 0; i < background.Count; i++)
                speed.Add(i);
    }

    void Update()
    {
        FollowCursor();
    }

    void FollowCursor()
    {
        mousePos = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);

        lerpPos = beginPos + (mousePos.normalized * offSet);

        for (int i = 0; i < background.Count; i++)
            background[i].transform.position = Vector3.Lerp(background[i].transform.position, lerpPos, Time.smoothDeltaTime * speed[i]);
    }
}