using APIPlugin;
using DiskCardGame;
using System.Collections;
using UnityEngine;

namespace OcCardPack.Abilities
{
	public class DestroyTerrain : AbilityBehaviour
	{
		public static Ability ability;
		public override Ability Ability => ability;

		public override bool RespondsToDealDamage(int amount, PlayableCard target)
		{
			return amount > 0 && target != null && !target.Dead && target.Info.traits.Contains(Trait.Terrain);
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x00043794 File Offset: 0x00041994
		public override IEnumerator OnDealDamage(int amount, PlayableCard target)
		{
			yield return base.PreSuccessfulTriggerSequence();
			yield return new WaitForSeconds(0.15f);
			target.Anim.LightNegationEffect();
			yield return new WaitForSeconds(0.15f);
			yield return target.Die(false, base.Card, true);
			yield return base.LearnAbility(0f);
			yield break;
		}

		public static NewAbility Create()
		{
			return Utils.AddAbility<DestroyTerrain>("Vandal", "When this card attacks a terrain card, it is destroyed immediately.");
		}
	}
}
