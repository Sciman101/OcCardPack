using APIPlugin;
using DiskCardGame;
using System.Collections;

namespace OcCardPack.Abilities
{
    class PotionPower : AbilityBehaviour
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
            BoardManager.Instance.currentSacrificeDemandingCard.AddTemporaryMod(new CardModificationInfo(1,0));
            yield break;
        }

        public static NewAbility Create()
		{
			return Utils.AddAbility<PotionPower>("Power Tonic", "When this creature is sacrificed, the card it was sacrificed for gains 1 power.");
		}
	}
}
