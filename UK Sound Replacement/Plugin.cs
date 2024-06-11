using BepInEx;
using HarmonyLib;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Reflection;

[BepInPlugin("tempy.soundreplacement", "UKSoundReplacement", "1.2.5")]
public class Plugin : BaseUnityPlugin
{

    private static Dictionary<string, Dictionary<string, string>> savedData = new Dictionary<string, Dictionary<string, string>>();
    private static FileInfo SaveFile = null;

    public static Plugin instance { get; private set; }
    private static Harmony harmony;
    public string modFolder { get; internal set; }

    void SetPersistentModData(string key, string value) {
        SetModData("mod", key, value);
    }

    string RetrieveStringPersistentModData(string key) {
        return RetrieveModData(key, "mod");
    }

    void Awake()
    {
        string path = Assembly.GetExecutingAssembly().Location;
        path = path.Substring(0, path.LastIndexOf(Path.DirectorySeparatorChar)) + Path.DirectorySeparatorChar + "save.json";
        SaveFile = new FileInfo(path);
        if (SaveFile.Exists)
        {
            using (StreamReader jFile = SaveFile.OpenText())
            {
                savedData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(jFile.ReadToEnd());
                if (savedData == null)
                    savedData = new Dictionary<string, Dictionary<string, string>>();
                jFile.Close();
            }
        }
        else
        {
            SaveFile.Create();
            Debug.Log("Making save!");
        }
        
        modFolder = System.IO.Directory.GetCurrentDirectory();

        Debug.Log(modFolder);
        instance = this;
        harmony = new Harmony("tempy.soundreplacement");
        //Assembly.Load(modFolder + "\\TagLibSharp.dll");
        harmony.PatchAll();
        SoundPackController.CreateNewSoundPack("Stock");

        Debug.Log("Searching " + this.modFolder + " for .uksr files");
        //StartCoroutine(SoundPackController.LoadCgMusic(modFolder, this));
        foreach (FileInfo file in new DirectoryInfo(this.modFolder).GetFiles("*.uksr", SearchOption.AllDirectories))
        {
            using (StreamReader jFile = file.OpenText())
            {
                Dictionary<string, string> jValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(jFile.ReadToEnd());
                string name = "No Name";
                if (jValues.ContainsKey("name"))
                    name = jValues["name"];
                if (name != "Template")
                {
                    Debug.Log("Found .uksr " + name + " at path " + file.FullName);
                    SoundPackController.SoundPack newPack = SoundPackController.CreateNewSoundPack(name);
                    if (newPack != null)
                        StartCoroutine(newPack.LoadFromDirectory(file.Directory, this));
                }
                jFile.Close();
            }
        }

        object rev = RetrieveStringPersistentModData("rev");
        if (rev != null)
            SoundPackController.SetCurrentSoundPack(rev.ToString(), SoundPackController.SoundPackType.Revolver);
        object sg = RetrieveStringPersistentModData("sg");
        if (sg != null)
            SoundPackController.SetCurrentSoundPack(sg.ToString(), SoundPackController.SoundPackType.Shotgun);
        object ng = RetrieveStringPersistentModData("ng");
        if (ng != null)
            SoundPackController.SetCurrentSoundPack(ng.ToString(), SoundPackController.SoundPackType.Nailgun);
        object rc = RetrieveStringPersistentModData("rc");
        if (rc != null)
            SoundPackController.SetCurrentSoundPack(rc.ToString(), SoundPackController.SoundPackType.Railcannon);
        object rl = RetrieveStringPersistentModData("rl");
        if (rl != null)
            SoundPackController.SetCurrentSoundPack(rl.ToString(), SoundPackController.SoundPackType.RocketLauncher);

        /*
        object cgLoop = RetrieveStringPersistentModData("cgLoop");
        if (cgLoop != null)
            SoundPackController.persistentLoopName = cgLoop.ToString();
        object cgIntro = RetrieveStringPersistentModData("cgIntro");
        if (cgIntro != null)
            SoundPackController.persistentIntroName = cgIntro.ToString();
        */
    }

    public void SetSoundPackPersistent(string name, SoundPackController.SoundPackType type)
    {
        Debug.Log("Setting persistent sound pack to " + name + " for type " + type);
        switch (type)
        {
            case SoundPackController.SoundPackType.Revolver:
                SetPersistentModData("rev", name);
                return;
            case SoundPackController.SoundPackType.Shotgun:
                SetPersistentModData("sg", name);
                return;
            case SoundPackController.SoundPackType.Nailgun:
                SetPersistentModData("ng", name);
                return;
            case SoundPackController.SoundPackType.Railcannon:
                SetPersistentModData("rc", name);
                return;
            case SoundPackController.SoundPackType.RocketLauncher:
                SetPersistentModData("rl", name);
                return;
            case SoundPackController.SoundPackType.All:
                SetPersistentModData("rev", name);
                SetPersistentModData("sg", name);
                SetPersistentModData("ng", name);
                SetPersistentModData("rc", name);
                SetPersistentModData("rl", name);
                return;
        }
    }

    public static string RetrieveModData(string key, string modName)
            {
                if (savedData.ContainsKey(modName))
                {
                    if (savedData[modName].ContainsKey(key))
                        return savedData[modName][key];
                        Debug.Log(savedData);
                }
                return null;
            }

    public static void SetModData(string modName, string key, string value)
    {
        if (!savedData.ContainsKey(modName))
        {
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            newDict.Add(key, value);
            savedData.Add(modName, newDict);
            Debug.Log(savedData);
        }
        else if (!savedData[modName].ContainsKey(key))
            savedData[modName].Add(key, value);
        else
            savedData[modName][key] = value;
    }
}

