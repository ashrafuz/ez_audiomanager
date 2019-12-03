using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EzAudio {

    public class EzAudioConstants {
        public const string EZ_AUDIO_MANAGER_DIR = "Assets/EzAudioManager/";
        public const string EZ_AUDIO_BOOK_FILENAME = "EzAudioBook";
    }

    [System.Serializable]
    public class AudioPack {
        public int index;
        public EzAudioFiles rawEnum;
        public string fullPath;
        public string fileNameWithoutExtension;
        public AudioClip audioClip;
        public AudioPack (int _val, string _fp, AudioClip _ac) {
            index = _val;
            audioClip = _ac;
            fullPath = _fp;

            rawEnum = (EzAudioFiles) _val;
            fileNameWithoutExtension = Path.GetFileNameWithoutExtension (_fp);
        }
    }

    public class EzAudioBook : ScriptableObject {
        public List<AudioPack> audioBook = new List<AudioPack> ();

        public void AddBook (List<string> _allFiles) {
            audioBook.Clear ();
            for (int i = 0; i < _allFiles.Count; i++) {
                AudioClip ac = (AudioClip) AssetDatabase.LoadAssetAtPath (_allFiles[i], typeof (AudioClip));
                audioBook.Add (new AudioPack (i, _allFiles[i], ac));
            }
        }

        public AudioClip GetClip (EzAudioFiles _file) {
            int index = (int) _file;
            if (index < audioBook.Count) {
                return audioBook[index].audioClip;
            }

            Debug.LogError ("can't find audio file " + _file);
            return null;
        }
    }
}