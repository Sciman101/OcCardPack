using BepInEx;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using APIPlugin;
using HarmonyLib;
using System.Reflection;
using OcCardPack.Abilities;
using DiskCardGame;
using System.Linq;

namespace OcCardPack
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
    class OcCardPackPlugin : BaseUnityPlugin
    {
        public const string PluginGuid = "sciman.inscryption.ocpack";
        public const string PluginName = "Sciman's OC Card Pack";
        public const string PluginVersion = "1.0.0";

        public static UnityEngine.Texture2D[] AllAbilityIcons;
        public static UnityEngine.Texture2D[] AllCardArt;

        private void Awake()
        {
            LoadAssets();

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);

            #region Add Abilities
            StrikeBackrow.Create();
            DestroyTerrain.Create();
            #endregion

            #region Add Cards

            NewCard.Add(
                "Imp",
                "Imp",
                3, 3,
                new List<CardMetaCategory> { CardMetaCategory.ChoiceNode, CardMetaCategory.Rare },
                CardComplexity.Simple,
                CardTemple.Nature,
                description: "An excitable hunter who will strike at anything.",
                bloodCost: 3,
                appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance> { CardAppearanceBehaviour.Appearance.RareCardBackground },
                defaultTex: AllCardArt.Single(t => t.name.Equals("imp")),
                emissionTex: AllCardArt.Single(t => t.name.Equals("imp_emission")),
                abilities: new List<Ability> { StrikeBackrow.ability }
                );

            NewCard.Add(
                "Raccoon",
                "Raccoon",
                1, 2,
                new List<CardMetaCategory> { CardMetaCategory.ChoiceNode },
                CardComplexity.Simple,
                CardTemple.Nature,
                description: "No wall or monument shall hinder this beast.",
                tribes: new List<Tribe> { Tribe.Reptile }, // maybe not lmao
                bloodCost: 1,
                defaultTex: AllCardArt.Single(t => t.name.Equals("root")),
                emissionTex: AllCardArt.Single(t => t.name.Equals("root_emission")),
                abilities: new List<Ability> { DestroyTerrain.ability }
                );

            #endregion

        }

        private void LoadAssets()
        {
            LoadTexturesFrom("Abilities",ref AllAbilityIcons);
            LoadTexturesFrom("Cards",ref AllCardArt);
        }

        private void LoadTexturesFrom(string subdir, ref Texture2D[] output)
        {
            string path = Path.Combine(this.Info.Location.Replace("OcCardPack.dll", ""),"Artwork",subdir);
            string[] allFiles = Directory.GetFiles(path,"*.png",SearchOption.TopDirectoryOnly);
            output = new Texture2D[allFiles.Length];
            // load all images
            for (int i=0;i<allFiles.Length;i++) {
                Texture2D tex = new Texture2D(2,2);
                tex.name = Path.GetFileNameWithoutExtension(allFiles[i]);
                byte[] imgBytes = File.ReadAllBytes(allFiles[i]);
                tex.LoadImage(imgBytes);

                output[i] = tex;
            }
        }

    }
}
