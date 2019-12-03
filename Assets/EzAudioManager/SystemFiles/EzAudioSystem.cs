using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EzAudio {

    public class EzAudioSystem : MonoBehaviour {
        [SerializeField][Range (5, 30)] private int m_MaxAudioSourceAllowed = 10;
        [SerializeField][Range (1, 5)] private int m_InitialAudioSourceAllowed = 1;

        List<AudioSource> mSourcePool = new List<AudioSource> ();
        EzAudioBook mEzBook;
        public static EzAudioSystem instance;

        private void Awake () {
            instance = this;
            for (int i = 0; i < m_InitialAudioSourceAllowed; i++) { AddAudioSource (); }
            mEzBook = Resources.Load<EzAudioBook> (EzAudioConstants.EZ_AUDIO_BOOK_FILENAME);
            if (mEzBook == null) {
                Debug.LogError ("No audio book found in Resources.");
                Debug.LogError ("To Generate audio book, please go to Window/EzAudioManager and click Generate button.");
                return;
            }
        }

        private void AddAudioSource () {
            if (mSourcePool.Count > m_MaxAudioSourceAllowed) {
                Debug.LogError ("Audio source pool Max Limit reached!");
                return;
            }

            GameObject gAudio = new GameObject ();
            gAudio.transform.SetParent (this.transform);
            mSourcePool.Add (gAudio.AddComponent<AudioSource> ());
        }

        private AudioSource GetFreeAudioSource () {
            //if there is a free source return it
            foreach (var item in mSourcePool) {
                if (!item.isPlaying) { return item; }
            }

            //if there is no free source, create new if not max reached
            if (mSourcePool.Count < m_MaxAudioSourceAllowed) {
                AddAudioSource ();
                return mSourcePool[mSourcePool.Count - 1];
            }

            //no free source found, and max limit reached, compromise random source.
            AudioSource randFreeSource = mSourcePool[UnityEngine.Random.Range (0, mSourcePool.Count)];
            randFreeSource.Stop ();
            return randFreeSource;
        }

        public void PlayClip (EzAudioFiles _filename, float _volume = 1) {
            GetFreeAudioSource ().PlayOneShot (mEzBook.GetClip (_filename), _volume);
        }
    }
}