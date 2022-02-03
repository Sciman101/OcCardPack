using APIPlugin;
using DiskCardGame;
using System.Collections;

namespace OcCardPack.Abilities
{
    class PotionHealth : AbilityBehaviour
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
            BoardManager.Instance.currentSacrificeDemandingCard.AddTemporaryMod(new CardModificationInfo(0,2));
            yield break;
        }

        public static NewAbility Create()
		{
			return Utils.AddAbility<PotionHealth>("Health Tonic", "When this creature is sacrificed, the card it was sacrificed for gains 2 health.");
		}
	}
}
