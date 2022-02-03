using APIPlugin;
using DiskCardGame;
using System.Collections;

namespace OcCardPack.Abilities
{
    class PotionFlight : AbilityBehaviour
    {
		public static Ability ability;
		public override Ability Ability => ability;

        public override bool RespondsToSacrifice()
        {
            return true;
        }

        public override IEnumerator OnSacrifice()
        {
            // Give a bonus to the card we were sacrificed for
            BoardManager.Instance.currentSacrificeDemandingCard.AddTemporaryMod(new CardModificationInfo(Ability.Flying));
            yield break;
        }

        public static NewAbility Create()
		{
			return Utils.AddAbility<PotionFlight>("Flight Tonic", "When this creature is sacrificed, the card it was sacrificed for gains the Flying sigil.");
		}
	}
}
