using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pathoschild.Stardew.Common;
using StardewValley;

namespace Pathoschild.Stardew.LookupAnything.Framework.Fields;

/// <summary>A metadata field which shows a list of linked item names with icons.</summary>
internal class ItemIconListField : GenericField
{
    /*********
    ** Fields
    *********/
    /// <summary>The items to draw.</summary>
    private readonly Tuple<Item, SpriteInfo?>[] Items;

    /// <summary>Get the name to show for an item, or <c>null</c> to use the item's display name.</summary>
    private readonly Func<Item, string?>? FormatItemName;

    /// <summary>Whether to draw the stack size on the item icon.</summary>
    private readonly bool ShowStackSize;


    /*********
    ** Public methods
    *********/
    /// <summary>Construct an instance.</summary>
    /// <param name="gameHelper">Provides utility methods for interacting with the game code.</param>
    /// <param name="label">A short field label.</param>
    /// <param name="items">The items to display.</param>
    /// <param name="showStackSize">Whether to draw the stack size on the item icon.</param>
    /// <param name="formatItemName">Get the name to show for an item, or <c>null</c> to use the item's display name.</param>
    public ItemIconListField(GameHelper gameHelper, string label, IEnumerable<Item?>? items, bool showStackSize, Func<Item, string?>? formatItemName = null)
        : base(label, hasValue: items != null)
    {
        this.Items = items?.WhereNotNull().Select(item => Tuple.Create(item, gameHelper.GetSprite(item))).ToArray() ?? [];
        this.HasValue = this.Items.Any();
        this.ShowStackSize = showStackSize;
        this.FormatItemName = formatItemName;
    }

    /// <inheritdoc />
    public override Vector2? DrawValue(SpriteBatch spriteBatch, SpriteFont font, Vector2 position, float wrapWidth)
    {
        // get icon size
        float textHeight = font.MeasureString("ABC").Y;
        Vector2 iconSize = new Vector2(textHeight);

        // draw list
        const int padding = 5;
        int topOffset = 0;
        foreach ((Item item, SpriteInfo? sprite) in this.Items)
        {
            // draw icon
            spriteBatch.DrawSpriteWithin(sprite, position.X, position.Y + topOffset, iconSize);
            if (this.ShowStackSize && item.Stack > 1)
            {
                float scale = 2f; //sprite.SourceRectangle.Width / iconSize.X;
                Vector2 sizePos = position + new Vector2(iconSize.X - Utility.getWidthOfTinyDigitString(item.Stack, scale), iconSize.Y + topOffset - 6f * scale);
                Utility.drawTinyDigits(item.Stack, spriteBatch, sizePos, scale: scale, layerDepth: 1f, Color.White);
            }

            // draw text
            string displayText = this.FormatItemName?.Invoke(item) ?? item.DisplayName;
            Vector2 textSize = spriteBatch.DrawTextBlock(font, displayText, position + new Vector2(iconSize.X + padding, topOffset), wrapWidth);

            topOffset += (int)Math.Max(iconSize.Y, textSize.Y) + padding;
        }

        // return size
        return new Vector2(wrapWidth, topOffset + padding);
    }
}
