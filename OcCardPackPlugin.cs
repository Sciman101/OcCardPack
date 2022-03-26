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

        public const string PluginPrefix = "sciman";

        public static UnityEngine.Texture2D[] AllAbilityIcons;
        public static UnityEngine.Texture2D[] AllCardArt;

        public static AbilityInfo VinesOnAttack { get; private set; }
        public static AbilityInfo DestroyTerrain { get; private set; }
        public static AbilityInfo PotionSeller { get; private set; }
        public static AbilityInfo Sticky { get; private set; }
        public static AbilityInfo TonicEffect { get; private set; }

        private void Awake()
        {

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);

            #region Add Abilities

            VinesOnAttack = AbilityManager.New(PluginGuid, "Vine Grower", "When a card bearing this sigil is played, Thorny Vines are created on each empty adjacent space. Thorny Vines are defined as: 1 Power, 1 Health, Thorns", typeof(VinesOnAttackBehaviour), "Sciman101-OcCardPack/Artwork/Abilities/vinesonattack.png").AddMetaCategories(AbilityMetaCategory.Part1Rulebook);
            DestroyTerrain = AbilityManager.New(PluginGuid, "Vandal", "When this card attacks a terrain card, it is destroyed immediately.", typeof(DestroyTerrainBehaviour), "Sciman101-OcCardPack/Artwork/Abilities/destroyterrain.png").AddMetaCategories(AbilityMetaCategory.Part1Rulebook);
            PotionSeller = AbilityManager.New(PluginGuid, "Potion Seller", "When this card is played, choose a random tonic to enter your hand.", typeof(PotionSellerBehaviour), "Sciman101-OcCardPack/Artwork/Abilities/potionseller.png").AddMetaCategories(AbilityMetaCategory.Part1Rulebook);
            Sticky = AbilityManager.New(PluginGuid, "Sticky", "Cards opposing this creature are unable to move to other spaces on the board.", typeof(StickyBehaviour), "Sciman101-OcCardPack/Artwork/Abilities/sticky.png").AddMetaCategories(AbilityMetaCategory.Part1Rulebook);
            TonicEffect = AbilityManager.New(PluginGuid, "Tonic", "When this card is sacrificed, the card it was sacrificed for drinks the contents of the bottle.", typeof(TonicEffectBehaviour), "Sciman101-OcCardPack/Artwork/Abilities/toniceffect.png").AddMetaCategories(AbilityMetaCategory.Part1Rulebook);

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
                .SetPortrait("Sciman101-OcCardPack/Artwork/Cards/imp.png")
                .SetEmissivePortrait("Sciman101-OcCardPack/Artwork/Cards/imp_emission.png");

            CardManager.New(PluginPrefix,"Kobol_Root","Kobold",1,2, "A mischevious creature. No wall or monument shall hinder this beast.")
                .SetDefaultPart1Card()
                .SetCost(bonesCost: 3)
                .AddAbilities(DestroyTerrain.ability)
                .SetPortrait("Sciman101-OcCardPack/Artwork/Cards/root.png")
                .SetEmissivePortrait("Sciman101-OcCardPack/Artwork/Cards/root_emission.png")
                .AddTribes(Tribe.Reptile);

            CardManager.New(PluginPrefix, "Robot_Jnfr", "Robot?", 1, 1, "It bears a strong resemblance to... him, only.... more insufferable.")
                .SetDefaultPart1Card()
                .SetCost(bloodCost: 1)
                .AddAbilities(Ability.BuffEnemy, Ability.DrawCopyOnDeath)
                .SetPortrait("Sciman101-OcCardPack/Artwork/Cards/static_jnfr.png")
                .SetEmissivePortrait("Sciman101-OcCardPack/Artwork/Cards/jnfr_emission.png")
                .AddSpecialAbilities(jnfrFaceRandomizer);

            CardManager.New(PluginPrefix, "Alchemist_Jamie", "Alchemist", 1, 2, "Smelling of sulfur and ashes, it offers its wares to you, excitedly.")
                .SetDefaultPart1Card()
                .SetCost(bloodCost: 2)
                .AddAbilities(PotionSeller.ability)
                .SetPortrait("Sciman101-OcCardPack/Artwork/Cards/jamie.png")
                .SetEmissivePortrait("Sciman101-OcCardPack/Artwork/Cards/jamie_emission.png")
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
                .SetPortrait("Sciman101-OcCardPack/Artwork/Cards/uma.png")
                .SetEmissivePortrait("Sciman101-OcCardPack/Artwork/Cards/uma_emission.png")
                .AddTribes(Tribe.Reptile);

            CardManager.New(PluginPrefix, "Plant_Darue", "Plantfolk", 0, 3, "A wallflower, but a loyal one.")
               .SetDefaultPart1Card()
               .SetCost(bonesCost: 4)
               .AddAbilities(VinesOnAttack.ability)
               .SetPortrait("Sciman101-OcCardPack/Artwork/Cards/darue_inactive.png")
               .SetAltPortrait("Sciman101-OcCardPack/Artwork/Cards/darue.png")
               .SetEmissiveAltPortrait("Sciman101-OcCardPack/Artwork/Cards/darue_emission.png")
               .AddSpecialAbilities(darueAttackTrigger);

            CardManager.New(PluginPrefix, "Thorn_Vines", "Thorned Vines", 1, 1)
               .SetTerrain()
               .AddAbilities(Ability.Sharp)
               .SetPortrait("Sciman101-OcCardPack/Artwork/Cards/darue_vines.png");
            #endregion
        }

        private void AddPotionCard(string id, string name, string portrait)
        {
            CardInfo potionCard = CardManager.New(PluginPrefix, id, name, 0, 1).AddAbilities(TonicEffect.ability).SetPortrait("Sciman101-OcCardPack/Artwork/Cards/"+portrait+".png");
            PotionSellerBehaviour.potionCardPool.Add(potionCard);
        }

    }
}
