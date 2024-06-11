using HarmonyLib;
using UnityEngine;
using System.Reflection;
using System.Linq;

[HarmonyPatch(typeof(Shotgun), "Start")]
public static class Inject_ShotgunSounds
{
    public static void Postfix(Shotgun __instance)
    {
        SoundPackController.SetAudioSourceClip(__instance.pumpChargeSound.GetComponent<AudioSource>(), "ShotgunCharge", SoundPackController.SoundPackType.Shotgun);
        SoundPackController.SetAudioSourceClip(__instance.warningBeep.GetComponent<AudioSource>(), "OverCharged", SoundPackController.SoundPackType.Shotgun);
        SoundPackController.SetAudioSourceClip(__instance.chargeSoundBubble.GetComponent<AudioSource>(), "ShotgunChargeLoop", SoundPackController.SoundPackType.Shotgun);
        AudioSource heatSinkAud = (AudioSource)Traverse.Create(__instance).Field("heatSinkAud").GetValue();
        if (heatSinkAud == null)
            heatSinkAud = __instance.heatSinkSMR.GetComponent<AudioSource>();
        SoundPackController.SetAudioSourceClip(heatSinkAud, "HeatHiss", SoundPackController.SoundPackType.Shotgun);
        SoundPackController.SetAudioClip(ref __instance.clickChargeSound, "CoreEjectFlick", SoundPackController.SoundPackType.Shotgun);
        SoundPackController.SetAudioClip(ref __instance.smackSound, "CoreEjectReload", SoundPackController.SoundPackType.Shotgun);
        SoundPackController.SetAudioClip(ref __instance.clickSound, "MainShotReload", SoundPackController.SoundPackType.Shotgun);
    }
}

[HarmonyPatch(typeof(Shotgun), "Shoot")]
public static class Inject_ShotgunShootSounds
{
    public static bool Prefix(Shotgun __instance)
    {
        SoundPackController.SetAudioClip(ref __instance.shootSound, "ShotgunShootSounds" + __instance.variation, SoundPackController.SoundPackType.Shotgun);
        return true;
    }
}

[HarmonyPatch(typeof(Shotgun), "ShootSinks")]
public static class Inject_ShotgunShootHeatSinkSounds
{
    public static bool Prefix(Shotgun __instance)
    {
        SoundPackController.SetAudioSourceClip(__instance.grenadeSoundBubble.GetComponent<AudioSource>(), "CoreEject", SoundPackController.SoundPackType.Shotgun);
        SoundPackController.SetAudioClip(ref __instance.shootSound, "ShotgunShootSounds" + __instance.variation, SoundPackController.SoundPackType.Shotgun);
        return true;
    }
}

[HarmonyPatch(typeof(Shotgun), "Pump1Sound")]
public static class Inject_ShotgunPump1Sounds
{
    public static bool Prefix(Shotgun __instance)
    {
        SoundPackController.SetAudioClip(ref __instance.pump1sound, "ShotgunPump1", SoundPackController.SoundPackType.Shotgun);
        return true;
    }
}

[HarmonyPatch(typeof(Shotgun), "Pump2Sound")]
public static class Inject_ShotgunPump2Sounds
{
    public static bool Prefix(Shotgun __instance)
    {
        SoundPackController.SetAudioClip(ref __instance.pump2sound, "ShotgunPump2", SoundPackController.SoundPackType.Shotgun);
        return true;
    }
}

// Jackhamer

[HarmonyPatch(typeof(ShotgunHammer), "Awake")]
public static class Inject_ShotgunHammerSounds
{
    public static string alt = "JH";

    public static void Postfix(ShotgunHammer __instance)
    {
        // Access private fields using reflection
        var pump1Sound = typeof(ShotgunHammer).GetField("pump1Sound", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance) as AudioSource;
        Debug.LogError("why");
        var pump2Sound = typeof(ShotgunHammer).GetField("pump2Sound", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance) as AudioSource;
        Debug.LogError("why2");
        var pumpExplosion = typeof(ShotgunHammer).GetField("pumpExplosion", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance) as GameObject;
        Debug.LogError("why3");
        var overPumpExplosion = typeof(ShotgunHammer).GetField("overPumpExplosion", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance) as GameObject;
        Debug.LogError("why4");
        var tempChargeSound = typeof(ShotgunHammer).GetField("tempChargeSound", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance) as GameObject;
        Debug.LogError("???");

        SoundPackController.SetAudioSourceClip(pump1Sound, "ShotgunPump1" + alt, SoundPackController.SoundPackType.Shotgun);
        Debug.LogError("define1");
        SoundPackController.SetAudioSourceClip(pump2Sound, "ShotgunPump2" + alt, SoundPackController.SoundPackType.Shotgun);
        Debug.LogError("define2");
        SoundPackController.SetAudioSourceClip(pumpExplosion.GetComponent<AudioSource>(), "PumpExplosion" + __instance.variation + alt, SoundPackController.SoundPackType.Shotgun);
        Debug.LogError("define3");
        SoundPackController.SetAudioSourceClip(overPumpExplosion.GetComponent<AudioSource>(), "OverCharged" + alt, SoundPackController.SoundPackType.Shotgun);
        Debug.LogError("define4");
        //SoundPackController.SetAudioSourceClip(tempChargeSound.GetComponent<AudioSource>(), "ShotgunChargeLoop" + __instance.variation + alt, SoundPackController.SoundPackType.Shotgun);
        Debug.LogError("define5");

        /*AudioSource heatSinkAud = (AudioSource)Traverse.Create(__instance).Field("overheatAud").GetValue();
        if (heatSinkAud == null)
            heatSinkAud = heatSinkSMR.GetComponent<AudioSource>();
        SoundPackController.SetAudioSourceClip(heatSinkAud, "HeatHiss" + alt, SoundPackController.SoundPackType.Shotgun);*/
    }
}

[HarmonyPatch(typeof(ShotgunHammer), "Update")]
public static class Inject_ShotgunHammer
{
    public static void Postfix(ShotgunHammer __instance)
    {
        
    }
}
