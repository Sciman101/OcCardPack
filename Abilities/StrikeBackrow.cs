using APIPlugin;
using DiskCardGame;
using HarmonyLib;

namespace OcCardPack.Abilities
{
    class StrikeBackrow : AbilityBehaviour
    {
        public static Ability ability;
        public override Ability Ability => ability;

        public static NewAbility Create()
        {
            return Utils.AddAbility<StrikeBackrow>("Spearhead", "When this card would attack an empty space, it will try to attack the back row instead.");
        }
    }

    [HarmonyPatch(typeof(CombatPhaseManager),nameof(CombatPhaseManager.SlotAttackSlot))]
    class StrikeBackrowPatch
    {
    }
  
}
