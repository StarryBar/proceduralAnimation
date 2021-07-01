using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Kinematica;

namespace HelloWorld
{

    [RequireComponent(typeof(Kinematica))]
    public class SimpleKinematicaScript : MonoBehaviour
    {
        void OnEnable()
        {
            var kinematica = gameObject.GetComponent<Kinematica>();
            ref MotionSynthesizer ms = ref kinematica.Synthesizer.Ref;
            ms.PlayFirstSequence(ms.Query.Where(IdleTrait.Trait));
        }
    }
}
