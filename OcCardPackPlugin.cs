using BepInEx;
using System.IO;
using UnityEngine;
using InscryptionAPI;
using InscryptionAPI.Card;
using HarmonyLib;
using System.Reflection;
using DiskCardGame;
using voidSigils;
using OcCardPack.Abilities;
using static InscryptionAPI.Card.SpecialTriggeredAbilityManager;

namespace OcCardPack
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("extraVoid.inscryption.voidSigils", BepInDependency.DependencyFlags.HardDependency)]
    class OcCardPackPlugin : BaseUnityPlugin
    {
        public const string PluginGuid = "sciman.inscryption.ocpack";
        public const string PluginName = "Sciman's OC Card Pack";
        public const string PluginVersion = "1.0.0";

        public const string PluginPrefix = "sciman_ocs";

        public static UnityEngine.Texture2D[] AllAbilityIcons;
        public static UnityEngine.Texture2D[] AllCardArt;

        public AbilityInfo VinesOnAttack { get; private set; }
        public AbilityInfo DestroyTerrain { get; private set; }
        public AbilityInfo PotionSeller { get; private set; }
        public AbilityInfo Sticky { get; private set; }
        public AbilityInfo TonicEffect { get; private set; }

        private void Awake()
        {
            LoadAssets();

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);

            #region Add Abilities

            VinesOnAttack = AbilityManager.New(PluginGuid, "Vine Grower", "When a card bearing this sigil is played, Thorny Vines are created on each empty adjacent space. Thorny Vines are defined as: 1 Power, 1 Health, Thorns", typeof(VinesOnAttackBehaviour), "/Artwork/Abilities/vinesonattack.png");
            DestroyTerrain = AbilityManager.New(PluginGuid, "Vandal", "When this card attacks a terrain card, it is destroyed immediately.", typeof(DestroyTerrainBehaviour), " /Artwork/Abilities/destroyterrain.png");
            PotionSeller = AbilityManager.New(PluginGuid, "Potion Seller", "When this card is played, choose a random tonic to enter your hand.", typeof(PotionSellerBehaviour), "/Artwork/Abilities/potionseller.png");
            Sticky = AbilityManager.New(PluginGuid, "Sticky", "Cards opposing this creature are unable to move to other spaces on the board.", typeof(StickyBehaviour), "/Artwork/Abilities/sticky.png");
            TonicEffect = AbilityManager.New(PluginGuid, "Tonic", "When this card is sacrificed, the card it was sacrificed for drinks the contents of the bottle.", typeof(TonicEffectBehaviour), "/Artwork/Abilities/toniceffect.png");

            JnfrFaceRandomizerBehaviour.GetSprites();
            SpecialTriggeredAbility jnfrFaceRandomizer = SpecialTriggeredAbilityManager.Add(PluginGuid, "JnfrFaceChanger", typeof(JnfrFaceRandomizerBehaviour)).Id;
            SpecialTriggeredAbility darueAttackTrigger = SpecialTriggeredAbilityManager.Add(PluginGuid, "DarueAttackTrigger", typeof(DarueAttackTriggerBehaviour)).Id;


            #endregion

            #region Add Cards
            CardInfo card_imp = CardManager.New(
                    PluginPrefix,
                    "Imp_Iekika",
                    "Imp",
                    3, 3,
                    "The otherworldy Imp. Easy to excite, it shall strike at anything.")
                .SetDefaultPart1Card()
                .SetCost(bloodCost: 3)
                .AddAbilities(void_Pierce.ability)
                .SetPortrait("/Artwork/Cards/imp")
                .SetEmissivePortrait("/Artwork/Cards/imp_emission.png");

            CardManager.New(PluginPrefix,"Kobol_Root","Kobold",1,2, "A mischevious creature. No wall or monument shall hinder this beast.")
                .SetDefaultPart1Card()
                .SetCost(bonesCost: 3)
                .AddAbilities(DestroyTerrain.ability)
                .SetPortrait("/Artwork/Cards/root")
                .SetEmissivePortrait("/Artwork/Cards/root_emission.png")
                .AddTribes(Tribe.Reptile);

            CardManager.New(PluginPrefix, "Robot_Jnfr", "Robot?", 1, 1, "It bears a strong resemblance to... him, only.... more insufferable.")
                .SetDefaultPart1Card()
                .SetCost(bloodCost: 1)
                .AddAbilities(Ability.BuffEnemy, Ability.DrawCopyOnDeath)
                .SetPortrait("/Artwork/Cards/static_jnfr")
                .SetEmissivePortrait("/Artwork/Cards/jnfr_emission.png")
                .AddSpecialAbilities(jnfrFaceRandomizer);

            CardManager.New(PluginPrefix, "Alchemist_Jamie", "Alchemist", 1, 2, "Smelling of sulfur and ashes, it offers its wares to you, excitedly.")
                .SetDefaultPart1Card()
                .SetCost(bloodCost: 2)
                .AddAbilities(PotionSeller.ability)
                .SetPortrait("/Artwork/Cards/jamie")
                .SetEmissivePortrait("/Artwork/Cards/jamie_emission.png")
                .AddTribes(Tribe.Reptile);
            AddPotionCard(TonicEffectBehaviour.POT_HEALTH,"Health Tonic","potionhealth");
            AddPotionCard(TonicEffectBehaviour.POT_POWER, "Power Tonic","potionpower");
            AddPotionCard(TonicEffectBehaviour.POT_FLIGHT, "Flight Tonic","potionflight");
            AddPotionCard(TonicEffectBehaviour.POT_GROWTH, "Growth Tonic","potiongrowth");
            AddPotionCard(TonicEffectBehaviour.POT_BLOOD, "Enriched Blood Vial","potionblood");


            CardManager.New(PluginPrefix, "Slime_Uma", "Slime", 1, 8, "The vast slime. It will hold your enemies close.")
                .SetDefaultPart1Card()
                .SetCost(bloodCost: 3)
                .AddAbilities(Sticky.ability)
                .SetPortrait("/Artwork/Cards/uma")
                .SetEmissivePortrait("/Artwork/Cards/uma_emission.png")
                .AddTribes(Tribe.Reptile);

            CardManager.New(PluginPrefix, "Plant_Darue", "Plantfolk", 0, 3, "A wallflower, but a loyal one.")
               .SetDefaultPart1Card()
               .SetCost(bonesCost: 4)
               .AddAbilities(VinesOnAttack.ability)
               .SetPortrait("/Artwork/Cards/darue_inactive")
               .SetAltPortrait("/Artwork/Cards/darue_inactive")
               .SetEmissiveAltPortrait("/Artwork/Cards/darue_emission.png")
               .AddSpecialAbilities(darueAttackTrigger);

            CardManager.New(PluginPrefix, "Thorn_Vines", "Thorned Vines", 1, 1)
               .SetDefaultPart1Card()
               .SetTerrain()
               .AddAbilities(Ability.Sharp)
               .SetPortrait("/Artwork/Cards/darue_vines.png");
            #endregion
        }

        private void LoadAssets()
        {
            LoadTexturesFrom("Abilities",ref AllAbilityIcons);
            LoadTexturesFrom("Cards", ref AllCardArt);
        }

        private void AddPotionCard(string id, string name, string portrait)
        {
            CardInfo potionCard = CardManager.New(PluginPrefix, id, name, 0, 1).AddAbilities(TonicEffect.ability).SetPortrait("/Artwork/Cards/"+portrait+".png");
            PotionSellerBehaviour.potionCardPool.Add(potionCard);
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
