using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace EzAudio {

    public class EzAudioManagerWindow : EditorWindow {
        static List<string> allAudioAssetName = new List<string> ();
        static List<AudioClip> allAudioClip = new List<AudioClip> ();

        static StringBuilder consoleLogText = new StringBuilder ();

        [MenuItem ("Window/EzAudioManager")]
        static void OpenWindow () {
            EzAudioManagerWindow window = (EzAudioManagerWindow) GetWindow (typeof (EzAudioManagerWindow));
            window.minSize = new Vector2 (300, 300);
            window.Show ();
        }

        private void OnGUI () {
            GUILayout.Space (5);
            GUILayout.Label ("Howdy User! Welcome to Ez Audio Manager!");
            GUILayout.BeginHorizontal ();
            //GUILayout.Label ("Click here to generate all audio >> ");
            if (GUILayout.Button ("Generate Audio Book", GUILayout.Height (40))) {
                GetAllAudioFileNames ();
                CreateAudioBook ();
            }

            GUILayout.EndHorizontal ();
            GUILayout.Space (10);
            GUILayout.Label (consoleLogText.ToString ());
        }

        static void GetAllAudioFileNames () {
            allAudioAssetName.Clear ();
            allAudioClip.Clear ();
            consoleLogText.Clear ();

            string[] guids = AssetDatabase.FindAssets ("t:audioClip");
            foreach (string guid in guids) {
                string fname = AssetDatabase.GUIDToAssetPath (guid);
                allAudioAssetName.Add (fname);
                consoleLogText.Append ("Found File >> " + fname + "\n");
            }

            new AudioEnumCreator<EzAudioFiles> (allAudioAssetName);
        }

        static void CreateAudioBook () {
            try {
                EzAudioBook ezbook = ScriptableObject.CreateInstance<EzAudioBook> ();
                ezbook.AddBook (allAudioAssetName);
                if (!Directory.Exists ("Assets/Resources")) {
                    consoleLogText.Append ("Created Resources Directory. \n");
                    Directory.CreateDirectory ("Assets/Resources");
                }

                string fullName = "Assets/Resources/" + EzAudioConstants.EZ_AUDIO_BOOK_FILENAME + ".asset";
                consoleLogText.Append ("Created Audio Book at :: " + fullName + "\n");
                AssetDatabase.CreateAsset (ezbook, fullName);
                AssetDatabase.SaveAssets ();
                AssetDatabase.Refresh ();

                consoleLogText.Append ("\n\nOperation Successful!! \nNow you can use EzAudioFiles.FILE_NAME to get all sound files.Please see EzAudioSample.scene to see how to use it.\n\n");

                //EditorUtility.FocusProjectWindow ();
                Selection.activeObject = ezbook;
            } catch (System.Exception _ex) {
                consoleLogText.Append ("\nERROR:: " + _ex.Message );
                throw;
            }
        }

    }

    /*==================*/

    public class AudioEnumCreator<T> {
        public List<string> audioBundle = new List<string> ();
        public AudioEnumCreator (List<string> allNames) {
            string classPath = EzAudioConstants.EZ_AUDIO_MANAGER_DIR + typeof (T) + ".cs";
            int invalidClipNameCounter = 0;
            if (File.Exists (classPath)) { File.Delete (classPath); } // delete previous file
            using (StreamWriter outfile = new StreamWriter (classPath)) {
                outfile.WriteLine ("public enum " + typeof (T) + " { ");

                for (int i = 0; i < allNames.Count; i++) {
                    outfile.Write ("\t\t");

                    string clipname = Path.GetFileNameWithoutExtension (allNames[i]).ToUpper ();
                    clipname = clipname.Replace (' ', '_'); // converting whitespaces with underscore

                    //Debug.Log("checking :: " + clipname);

                    if (!IsIdentifier (clipname)) {
                        clipname = "INVALID_NAME_" + invalidClipNameCounter + " /* Invalidated name => " + allNames[i] + " */";
                        invalidClipNameCounter++;
                    } else if (audioBundle.Contains (clipname)) {
                        clipname = "INVALID_NAME_" + invalidClipNameCounter + " /* Same name found for=> " + allNames[i] + " */";
                        invalidClipNameCounter++;
                    }

                    outfile.Write (clipname + " = " + i);
                    audioBundle.Add (clipname);
                    if (i != allNames.Count - 1) { outfile.Write (", \n"); }
                }

                outfile.WriteLine ("\n}");
            } //file writtten

            AssetDatabase.Refresh ();
        }

        public static bool IsIdentifier (string text) {
            if (string.IsNullOrEmpty (text))
                return false;
            if (!char.IsLetter (text[0]) && text[0] != '_')
                return false;
            for (int ix = 1; ix < text.Length; ++ix)
                if (!char.IsLetterOrDigit (text[ix]) && text[ix] != '_')
                    return false;
            return true;
        }

    }
}