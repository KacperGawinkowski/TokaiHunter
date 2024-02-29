using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IntroSkip : MonoBehaviour
{
    Animator anim;
    VideoPlayer videoPlayer;

    private void Start()
    {
        videoPlayer = FindObjectOfType<VideoPlayer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            videoPlayer.Stop();
            anim.CrossFade("MainMenu", 0f, 0, 1f);
        }
    }
}
