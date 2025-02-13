using Pathoschild.Stardew.FastAnimations.Framework;
using StardewValley;
using StardewValley.Menus;

namespace Pathoschild.Stardew.FastAnimations.Handlers;

/// <summary>Handles the Stardew Valley Fair wheel spin minigame animation.</summary>
/// <remarks>See game logic in <see cref="WheelSpinGame.update"/>.</remarks>
internal sealed class WheelSpinHandler : BaseAnimationHandler
{
    /*********
    ** Public methods
    *********/
    /// <summary>Construct an instance.</summary>
    /// <param name="multiplier">The animation speed multiplier to apply.</param>
    public WheelSpinHandler(float multiplier)
        : base(multiplier) { }

    /// <inheritdoc />
    public override bool TryApply(int playerAnimationId)
    {
        return
            Game1.activeClickableMenu is WheelSpinGame { arrowRotationVelocity: > 0 } menu
            && this.ApplySkips(() =>
                menu.update(Game1.currentGameTime)
            );
    }
}
