using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using System.Collections;

namespace OcCardPack.Abilities
{
    class StrikeBackrow : AbilityBehaviour
    {
        public static Ability ability;
        public override Ability Ability => ability;

        public override bool RespondsToDealDamageDirectly(int amount)
        {
            return true;
        }

        public override IEnumerator OnDealDamageDirectly(int amount)
        {
            PlayableCard otherCard = BoardManager.Instance.GetCardQueuedForSlot(Card.Slot.opposingSlot);
            if (otherCard != null)
            {
                // A backline card exists! Fuck it up!!
                yield return otherCard.TakeDamage(Card.Info.Attack, Card);
            }

            yield break;
        }

        public static NewAbility Create()
        {
            return Utils.AddAbility<StrikeBackrow>("Spearhead", "When this card would attack an empty space, it will try to attack the back row instead.");
        }
    }

    [HarmonyPatch]
    public class PatchesForStrikeBackrow
    {



    }
}
