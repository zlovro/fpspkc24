using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.Libraries.KBLib2
{
    public abstract class Kb2Behaviour : MonoBehaviour
    {
        [HideInInspector]
        public Transform tf;

        protected virtual void Awake()
        {
            tf = transform;
        }
        
        public List<Transform> GetChildren()
        {
            return transform.Cast<Transform>().ToList();
        }
    }
}