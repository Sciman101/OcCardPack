using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace OcCardPack.Abilities
{
	public class StickyBehaviour : AbilityBehaviour
	{
		public static Ability ability;
		public override Ability Ability => ability;
	}

	[HarmonyPatch(typeof(Strafe),nameof(Strafe.DoStrafe))]
	class StickyStrafePatch
    {

		static IEnumerator Postfix(IEnumerator original, Strafe __instance)
        {
			PlayableCard thisCard = __instance.Card;
			PlayableCard opposingCard = thisCard.Slot.opposingSlot.Card;
			if (opposingCard != null && opposingCard.HasAbility(OcCardPackPlugin.Sticky.ability))
            {
				thisCard.Anim.StrongNegationEffect();
				yield return new WaitForSeconds(0.15f);
            }
            else
            {
				yield return original;
			}
        }
    }

	[HarmonyPatch(typeof(TailOnHit), nameof(TailOnHit.OnCardGettingAttacked))]
	class StickyTailPatch
	{

		static IEnumerator Postfix(IEnumerator original, TailOnHit __instance)
		{
			PlayableCard thisCard = __instance.Card;
			PlayableCard opposingCard = thisCard.Slot.opposingSlot.Card;
			if (opposingCard != null && opposingCard.HasAbility(OcCardPackPlugin.Sticky.ability))
			{
				thisCard.Anim.StrongNegationEffect();
				yield return new WaitForSeconds(0.15f);
			}
			else
			{
				yield return original;
			}
		}

	}
}
