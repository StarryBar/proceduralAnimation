using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum PlayerState
{
    [Description("Idle")]
    Idle,
    [Description("Walking")]
    Walking,
    [Description("Climbing")]
    Climbing,
    [Description("Playing")]
    Playing,
    [Description("Error")]
    Error
}

