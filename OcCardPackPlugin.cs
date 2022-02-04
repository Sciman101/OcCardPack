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
using OcCardPack.Cards;

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

            PotionSeller.Create();
            TonicEffect.Create();

            Sticky.Create();

            VinesOnAttack.Create();
            DarueAttackTrigger.Create();

            JnfrFaceRandomizer.Create();
            #endregion

            #region Add Cards

            NewCard.Add(
                "Iekika",
                "Imp",
                3, 3,
                new List<CardMetaCategory> { CardMetaCategory.Rare },
                CardComplexity.Simple,
                CardTemple.Nature,
                description: "The otherworldy Imp. Easy to excite, it shall strike at anything.",
                bloodCost: 3,
                appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance> { CardAppearanceBehaviour.Appearance.RareCardBackground },
                defaultTex: AllCardArt.Single(t => t.name.Equals("imp")),
                emissionTex: AllCardArt.Single(t => t.name.Equals("imp_emission")),
                abilities: new List<Ability> { StrikeBackrow.ability }
                );

            NewCard.Add(
                "Root",
                "Kobold",
                1, 2,
                new List<CardMetaCategory> { CardMetaCategory.ChoiceNode },
                CardComplexity.Simple,
                CardTemple.Nature,
                description: "A strange creature. No wall or monument shall hinder this beast.",
                tribes: new List<Tribe> { Tribe.Reptile }, // maybe not lmao
                bonesCost: 3,
                defaultTex: AllCardArt.Single(t => t.name.Equals("root")),
                emissionTex: AllCardArt.Single(t => t.name.Equals("root_emission")),
                abilities: new List<Ability> { DestroyTerrain.ability }
                );

            NewCard.Add(
                "JNFR",
                "JNFR",
                1, 1,
                new List<CardMetaCategory> { CardMetaCategory.ChoiceNode },
                CardComplexity.Simple,
                CardTemple.Nature,
                description: "It bears a strong resemblance to... him, only.... more insufferable.",
                bloodCost: 1,
                defaultTex: AllCardArt.Single(t => t.name.Equals("jnfr")),
                emissionTex: AllCardArt.Single(t => t.name.Equals("jnfr_emission")),
                abilities: new List<Ability> { Ability.BuffEnemy, Ability.DrawCopyOnDeath },
                specialAbilities: new List<SpecialTriggeredAbility> { SpecialTriggeredAbility.TalkingCardChooser, JnfrFaceRandomizer.specialTriggeredAbility },
                appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance> { CardAppearanceBehaviour.Appearance.AnimatedPortrait }
                );
            NewTalkingCard.Add<JNFRTalkingCard>("JNFR", JNFRTalkingCard.GetDictionary());

            NewCard.Add(
                "Jamie",
                "Kobold",
                1, 2,
                new List<CardMetaCategory> { CardMetaCategory.Rare },
                CardComplexity.Simple,
                CardTemple.Nature,
                description: "A strange creature. It offers its wares to you, excitedly.",
                tribes: new List<Tribe> { Tribe.Reptile }, // maybe not lmao
                bloodCost: 2,
                defaultTex: AllCardArt.Single(t => t.name.Equals("jamie")),
                emissionTex: AllCardArt.Single(t => t.name.Equals("jamie_emission")),
                abilities: new List<Ability> { PotionSeller.ability }
                );
            AddPotionCard(TonicEffect.POT_HEALTH,"Health Tonic");
            AddPotionCard(TonicEffect.POT_POWER, "Power Tonic");
            AddPotionCard(TonicEffect.POT_FLIGHT, "Flight Tonic");
            AddPotionCard(TonicEffect.POT_GROWTH, "Growth Tonic");
            AddPotionCard(TonicEffect.POT_BLOOD, "Enriched Blood Vial");

            NewCard.Add(
                "Uma",
                "Slime",
                0, 8,
                new List<CardMetaCategory> { CardMetaCategory.ChoiceNode },
                CardComplexity.Simple,
                CardTemple.Nature,
                description: "The vast slime. It will hold your enemies close.",
                bloodCost: 3,
                defaultTex: AllCardArt.Single(t => t.name.Equals("uma")),
                emissionTex: AllCardArt.Single(t => t.name.Equals("uma_emission")),
                abilities: new List<Ability> { Sticky.ability }
                );

            NewCard.Add(
                "Darue",
                "Plantfolk",
                0, 3,
                new List<CardMetaCategory> { CardMetaCategory.ChoiceNode },
                CardComplexity.Simple,
                CardTemple.Nature,
                description: "The timid wallflower. ",
                bonesCost: 4,
                specialAbilities: new List<SpecialTriggeredAbility> { DarueAttackTrigger.specialTriggeredAbility },
                defaultTex: AllCardArt.Single(t => t.name.Equals("darue_inactive")),
                altTex: AllCardArt.Single(t => t.name.Equals("darue")),
                abilities: new List<Ability> { VinesOnAttack.ability }
                );

            NewCard.Add(
               "Thorn_Vines",
               "Thorned Vines",
               1, 1,
               new List<CardMetaCategory> { },
               CardComplexity.Simple,
               CardTemple.Nature,
               traits: new List<Trait> { Trait.Terrain },
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance> { CardAppearanceBehaviour.Appearance.TerrainBackground, },
               defaultTex: AllCardArt.Single(t => t.name.Equals("darue_vines")),
               abilities: new List<Ability> { Ability.Sharp }
               );
            #endregion

        }

        private void LoadAssets()
        {
            LoadTexturesFrom("Abilities",ref AllAbilityIcons);
            LoadTexturesFrom("Cards", ref AllCardArt);
        }

        private void AddPotionCard(string id, string name)
        {
            CardInfo info = ScriptableObject.CreateInstance<CardInfo>();
            info.name = id;
            info.displayedName = name;
            info.baseHealth = 1;
            info.baseAttack = 0;
            info.cardComplexity = CardComplexity.Simple;
            info.temple = CardTemple.Nature;
            info.abilities = new List<Ability> { TonicEffect.ability };
            info.portraitTex = Sprite.Create(AllCardArt.Single(t => t.name.Equals(id,System.StringComparison.OrdinalIgnoreCase)),CardUtils.DefaultCardArtRect,CardUtils.DefaultVector2);
            NewCard.Add(info);
            PotionSeller.potionCardPool.Add(info);
        }

        private void LoadTexturesFrom(string subdir, ref Texture2D[] output)
        {
            string path = Path.Combine(this.Info.Location.Replace("OcCardPack.dll", ""),"Artwork",subdir);
            string[] allFiles = Directory.GetFiles(path,"*.png",SearchOption.TopDirectoryOnly);
            output = new Texture2D[allFiles.Length];
            // load all images
            for (int i=0;i<allFiles.Length;i++) {
                Texture2D tex = new Texture2D(2,2);
                tex.filterMode = FilterMode.Point;
                tex.name = Path.GetFileNameWithoutExtension(allFiles[i]);
                byte[] imgBytes = File.ReadAllBytes(allFiles[i]);
                tex.LoadImage(imgBytes);

                output[i] = tex;
            }
        }

    }
}
