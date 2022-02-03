using System.Collections;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace OcCardPack.Abilities
{
    class JnfrFaceRandomizer : SpecialCardBehaviour
    {
        public static SpecialTriggeredAbility specialTriggeredAbility;

        // Get all applicable jnfr face sprites
        public static Texture2D[] jnfrFaceSprites = new Texture2D[0];

        public override bool RespondsToDrawn()
        {
            return true;
        }

        public override IEnumerator OnDrawn()
        {
            // Randomize portrait
            int index = Random.Range(0, jnfrFaceSprites.Length);

            Card.renderInfo.portraitOverride = Sprite.Create(jnfrFaceSprites[index],CardUtils.DefaultCardArtRect,CardUtils.DefaultVector2);
            Card.RenderCard();

            yield break;
        }

        public static void Create()
        {
            StatIconInfo info = ScriptableObject.CreateInstance<StatIconInfo>();
            NewSpecialAbility ability = new NewSpecialAbility(typeof(JnfrFaceRandomizer),SpecialAbilityIdentifier.GetID(OcCardPackPlugin.PluginGuid,"JnfrFaceRandomizer"),info);
            JnfrFaceRandomizer.specialTriggeredAbility = ability.specialTriggeredAbility;
        }

    }
}
