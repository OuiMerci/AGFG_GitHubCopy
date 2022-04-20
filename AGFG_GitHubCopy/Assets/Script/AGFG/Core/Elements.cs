using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AGFG.Core
{
    [Serializable]
    public struct ElementalMultipliers
    {
        public int Fire;
        public int Ice;
        public int Thunder;
        public int Light;
        public int Dark;

        public int GetResistance(Elements element) => element switch
        {
            Elements.None => 0,
            Elements.Fire => Fire,
            Elements.Ice => Ice,
            Elements.Thunder => Thunder,
            Elements.Light => Light,
            Elements.Dark => Dark,
            _ => throw new ArgumentOutOfRangeException(nameof(element), $"Unhandled element value: {element}"),
        };
    }

    public enum Elements
    {
        None,
        Fire,
        Ice,
        Thunder,
        Light,
        Dark
    } 
}