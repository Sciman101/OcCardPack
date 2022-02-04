using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace OcCardPack.Abilities
{
	public class Sticky : AbilityBehaviour
	{
		public static Ability ability;
		public override Ability Ability => ability;


		public static NewAbility Create()
		{
			return Utils.AddAbility<Sticky>("Sticky", "Cards opposing this creature are unable to move.");
		}
	}

	[HarmonyPatch(typeof(Strafe),nameof(Strafe.DoStrafe))]
	class StickyPatch
    {

		static IEnumerator Postfix(IEnumerator original, Strafe __instance)
        {
			PlayableCard thisCard = __instance.Card;
			PlayableCard opposingCard = thisCard.Slot.opposingSlot.Card;
			if (opposingCard != null && opposingCard.HasAbility(Sticky.ability))
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
