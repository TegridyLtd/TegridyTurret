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
using Tegridy.Ordinance;
using System.Collections.Generic;
namespace Tegridy.Turret
{
    [System.Serializable] public class TurretConfig
    {
        [Header("Base Rotation config")]
        [Range(0.01f, 100)] public float baseSpeed;
        [Range(1,359)] public float minBearing;
        [Range(1, 359)] public float maxBearing;

        [Header("Pivot Rotation Config")]
        [Range(0.01f, 100)] public float pivotSpeed;
        [Range(1, 359)] public float minInclination;
        [Range(1, 359)] public float maxInclination;
    }
    [System.Serializable] public class TurretAudio
    {
        [Header("MovementAudio")]
        public AudioClip baseRotate;
        public AudioClip pivotRotate;
    }
    [System.Serializable] public class PodAudio
    {
        [Header("PodAudio")]
        public AudioClip[] fire;
        public AudioClip[] reloading;
        public AudioClip[] reloaded;
        public AudioClip[] noAmmo;
        public AudioClip[] changeAmmo;
        public AudioClip[] eject;
    }
    [System.Serializable] public class projectilePod
    {
        [Header("General Config")]
        public Transform barrel;
        public Transform launchPoint;
        public float power; //used as range for ray
        public float accuracy;
        public int ammoType;

        [Header("Animations")]
        public Animator animator;
        public AnimationClip fireAnim;
        public AnimationClip reloadAnim;

        [Header("Reload Config")]
        public bool eject;
        public Transform ejectPos;
        public float ejectPower;
        public float ejectDelay;
        public float reloadTime;


        public PodAudio audioClip;

        [HideInInspector] public bool reloading;
    }
    [System.Serializable] public class Ammo
    {
        public int ammoType;
        public int ammoQuty;
    }
    [System.Serializable] public class MissilePod
    {
        public Transform[] missileSalvo;
        public Transform[] casingEject;

        public int power;
        public int ammoType = 0;
        public bool singleShot = true;
        public int singleCount = 1;
        public int curSalvo;

        [Header("Animations")]
        public Animator animator;
        public AnimationClip fireAnim;
        public AnimationClip reloadAnim;

        [Header("Reload Config")]
        public bool eject;
        public float ejectPower;
        public float ejectDelay;
        public float reloadTime;

        public PodAudio audioClips;

        [Header("Info")]
        public TegridyOrdinanceMissile[] missiles;
    }

}