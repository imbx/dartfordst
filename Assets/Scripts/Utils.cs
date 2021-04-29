using System;
using System.Collections;
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
}
