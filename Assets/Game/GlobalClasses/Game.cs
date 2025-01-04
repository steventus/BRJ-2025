using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Game
{
    public static readonly Events events = new Events();
}

public class Events {
    public UnityAction<float> OnParry;
    public UnityAction OnBulletTimeStart;
    public UnityAction OnBulletTimeEnd;
}
