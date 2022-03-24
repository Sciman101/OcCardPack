using System.Collections;
using System.Linq;
using InscryptionAPI.Card;
using DiskCardGame;
using UnityEngine;
using APIPlugin;
using InscryptionAPI.Helpers;

namespace OcCardPack.Abilities
{
    class JnfrFaceRandomizerBehaviour : SpecialCardBehaviour
    {

        // Get all applicable jnfr face sprites
        public static Sprite[] jnfrFaceSprites;

        public override bool RespondsToDrawn()
        {
            return true;
        }

        public override IEnumerator OnDrawn()
        {
            // Randomize portrait
            int index = Random.Range(0, jnfrFaceSprites.Length);

            this.PlayableCard.renderInfo.portraitOverride = jnfrFaceSprites[index];
            PlayableCard.RenderCard();

            yield break;
        }

        public static void GetSprites()
        {
            // Load textures and convert to sprites
            jnfrFaceSprites = new Sprite[11];
            for (int i=0;i<11;i++)
            {
                jnfrFaceSprites[i] = TextureHelper.GetImageAsSprite("/Artwork/Cards/jnfr" + (i == 0 ? "":i.ToString()) + ".png",TextureHelper.SpriteType.CardPortrait);
            }
        }

    }
}
