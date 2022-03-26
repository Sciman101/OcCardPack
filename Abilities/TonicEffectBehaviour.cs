using APIPlugin;
using DiskCardGame;
using System.Collections;
using UnityEngine;

namespace OcCardPack.Abilities
{
    class TonicEffectBehaviour : AbilityBehaviour
    {
        public const string POT_HEALTH = "PotionHealth";
        public const string POT_POWER = "PotionPower";
        public const string POT_GROWTH = "PotionGrowth";
        public const string POT_FLIGHT = "PotionFlight";
        public const string POT_BLOOD = "PotionBlood";

        public static Ability ability;
		public override Ability Ability => ability;

        public override bool RespondsToSacrifice()
        {
            return true;
        }

        public override IEnumerator OnSacrifice()
        {
            CardModificationInfo cardMod;

            string cardName = Card.Info.name.Substring(7); // Magic numbers yaaaay
            Debug.Log(cardName);
            switch (cardName)
            {
                case POT_HEALTH:
                    cardMod = new CardModificationInfo(0, 2);
                    break;

               case POT_POWER:
                    cardMod = new CardModificationInfo(1, 0);
                    break;

                case POT_GROWTH:
                    cardMod = new CardModificationInfo(Ability.Evolve);
                    break;

                case POT_FLIGHT:
                    cardMod = new CardModificationInfo(Ability.Flying);
                    break;

                case POT_BLOOD:
                    cardMod = new CardModificationInfo(Ability.TripleBlood);
                    break;

                default:
                    cardMod = new CardModificationInfo(1, 1);
                    break;
            }

            // Give a bonus to the card we were sacrificed for
            PlayableCard target = BoardManager.Instance.currentSacrificeDemandingCard;
            yield return new WaitForSeconds(0.15f);
            target.Anim.PlayTransformAnimation();
            yield return new WaitForSeconds(0.15f);
            target.AddTemporaryMod(cardMod);


            yield break;
        }
	}
}
