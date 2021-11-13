/////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2021 Tegridy Ltd                                          //
// Author: Darren Braviner                                                 //
// Contact: db@tegridygames.co.uk                                          //
/////////////////////////////////////////////////////////////////////////////
//                                                                         //
// This program is free software; you can redistribute it and/or modify    //
// it under the terms of the GNU General Public License as published by    //
// the Free Software Foundation; either version 2 of the License, or       //
// (at your option) any later version.                                     //
//                                                                         //
// This program is distributed in the hope that it will be useful,         //
// but WITHOUT ANY WARRANTY.                                               //
//                                                                         //
/////////////////////////////////////////////////////////////////////////////
//                                                                         //
// You should have received a copy of the GNU General Public License       //
// along with this program; if not, write to the Free Software             //
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,              //
// MA 02110-1301 USA                                                       //
//                                                                         //
/////////////////////////////////////////////////////////////////////////////

using UnityEngine;
namespace Tegridy.Ordinance
{
    [System.Serializable] public class ProjectilePool
    {
        [Header("Round Config")]
        public GameObject shell;
        public GameObject casing;
        public DamageConfig damage;
        public ProjectileConfig config;

        [HideInInspector] public int curShell = 0;
        [HideInInspector] public GameObject[] shellPool;
        [HideInInspector] public GameObject[] casingPool;
    }
    [System.Serializable] public class MissilePool
    {
        [Header("Missile Config")]
        public GameObject shell;
        public GameObject casing;
        public DamageConfig damage;
        public MissileConfig config;
    }
    [System.Serializable] public class MissileConfig
    {
        [Header("Obect Config")]
        public RigidConfig rbConfig;
        public AudioConfig audioConfig;

        [Header("Flight Stats")]
        public float maxVelocity;
        public float turnRate;
        public float thrustForce;

        [Header("Fuel Config")]
        public float fuel;
        public float fuelBurn; //burn rate
        public float burnDelay; //do wewait after launching before firing the engines?
        public bool fuelOutDestroy; //detonate the object regardless when out of fuel?
        public float fuelOutDelay; // time to detonate

        [Header("Warhead Config")]
        public float trackingRange; // max range the missile can track towards the target
        public float minProximinity; // if zero only detonate on inpact
        public bool waitTillImpact; //if we have fuel wait till we hit the target
        public bool detonateOnIncrease; //if target distance increases after reaching min distance detonate
    }
    public class ProjectileConfig
    {
        [Header("Projectile Config")]
        public GameObject warhead;
        public GameObject travelFX;
        public float impactDelay;
        public float detonationDelay;
        public float minProximinity; // if zero only detonate on inpact
    }
    [System.Serializable] public class DamageConfig
    {
        public float critical;
        public float heat;
        public float explosive;
        public float impact;
        public float piercing;
    }
    [System.Serializable] public class FireCommand
    {
        public string playerName;
        public int podType;
        public int group;
        public Transform launchPos;
        public Transform ejectPos;
        public float power;
        public bool eject;
        public float ejectDelay;
        public float ejectPower;
    }
    [System.Serializable] public class MissileLaunch
    {
        public string playerName;
        public Transform target;
        public Transform launchPos;
        public Transform ejectPos;
        public float power;
        public bool eject;
        public float ejectDelay;
        public float ejectPower;
        public Vector3 finalPosition;
    }
    [System.Serializable] public class AudioConfig
    {
        //spacial
        //volume
        //3d doppler
        //etc

    }
    [System.Serializable] public class RigidConfig
    {
        public float mass;
        public float drag;
        public float angularDrag;
        public bool useGravity;
        public bool isKinematic;
        public CollisionDetectionMode colMode;
        public RigidbodyInterpolation interpolMode;
    }
}
