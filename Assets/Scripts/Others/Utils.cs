using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoxScripts {
    public class TransformData {
        public Vector3 position { get; }
        public Vector3 eulerAngles { get; }

        public TransformData (Transform copy) {
            position = copy.position;
            eulerAngles = copy.eulerAngles;
        }

        public TransformData (Vector3 pos, Vector3 rot) {
            position = pos;
            eulerAngles = rot;
        }

        public Vector3 LerpDistance(TransformData other, float timer)
        {
            return Vector3.Lerp(position, other.position, timer);
        }

        public Vector3 LerpAngle(TransformData other, float timer)
        {
            return new Vector3(
                Mathf.LerpAngle(eulerAngles.x, other.eulerAngles.x, timer),
                Mathf.LerpAngle(eulerAngles.y, other.eulerAngles.y, timer),
                Mathf.LerpAngle(eulerAngles.z, other.eulerAngles.z, timer));
        }

        
    }

    public class NotebookPage {
        public int reqID { get; }
        public string text { get; }
        public NotebookPage (string dText)
        {
            reqID = -1;
            text = dText;
        }

        public NotebookPage (int rqId, string dText)
        {
            reqID = rqId;
            text = dText;
        }
    }

    public class DiaryController : MonoBehaviour
    {

    }

    public enum NotebookType{
        Diary,
        Notes,
        None
    }

    public enum RadioSound{
        Beep,
        Peep,
        None
    }

    public enum PinSelect{
        Red,
        Blue,
        None
    }

    public enum GameState {
        TITLESCREEN,
        LOADGAME,
        PLAYING,
        INTERACTING,
        ENDINTERACTING,
        LOOKITEM,
        ENDLOOKITEM,
        TARGETINGPICTURE,
        TARGETING,
        MOVINGPICTURE,
        OPENNOTEBOOK,
        CLOSENOTEBOOK,
        ENDGAME,
        MOVINGCAMERA
    }

    public enum TextPosition
    {
        TOPLEFT,
        TOP,
        TOPRIGHT,
        BOTTOMLEFT,
        BOTTOM,
        BOTTOMRIGHT,
        NONE
    }

    // https://forum.unity.com/threads/change-gameobject-layer-at-run-time-wont-apply-to-child.10091/
    
     
    public static class IListExtensions {
        /// <summary>
        /// Shuffles the element order of the specified list.
        /// </summary>
        public static void Shuffle<T>(this IList<T> ts) {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i) {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }
    }

    public static class BoxUtils{

        public static int ConvertTo01 (int value)
        {
            if(value == 0) return 0;
            if(value > 0) return 1;
            return -1;
        }
        public static void SetLayerRecursively(
            GameObject obj,
            int newLayer)
        {
            if (null == obj)
                return;
        
            obj.layer = newLayer;
        
            foreach (Transform child in obj.transform)
            {
                if (null == child)
                {
                    continue;
                }
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
        public static void SetTagRecursively(
            GameObject obj,
            string newTag)
        {
            if (null == obj)
                return;
        
            obj.tag = newTag;
        
            foreach (Transform child in obj.transform)
            {
                if (null == child)
                {
                    continue;
                }
                SetTagRecursively(child.gameObject, newTag);
            }
        }
    }
}
