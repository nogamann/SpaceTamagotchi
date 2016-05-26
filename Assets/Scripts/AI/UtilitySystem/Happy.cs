using UnityEngine;
using System.Collections;
using System.Timers;
using System;

public class Happy : MoodObject
{
    public float joyUpdate;
    public float healthUpdate;
    public float hungerInterval;
    public float loveInterval;
    // TODO add another two love timers. One for each player.
    Creature creature;

    // Use this for initialization
    void Start()
    {
        joyUpdate = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        float realTime = Time.realtimeSinceStartup;

        // update joy meter
        if ((realTime - joyUpdate) > 1500)
        {
            creature.Joy = (-0.01f);
        }

        // update health meter
        if ((realTime - healthUpdate) > 1500)
        {
            creature.Health = (-0.01f);
        }

        // update hunger meter
        if ((realTime - hungerInterval) > 1500)
        {
            creature.Hunger = (-0.01f);
        }

        // update love meter
        if ((realTime - loveInterval) > 1500)
        {
            creature.LoveGeneral = (-0.01f);
        }
    }

    void Run()
    {

    }
}