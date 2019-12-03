using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSound : MonoBehaviour {

    void Start () {
        EzAudio.EzAudioSystem.instance.PlayClip(EzAudioFiles.XIT);
    }
}