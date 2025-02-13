using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Pathoschild.Stardew.LookupAnything.Framework.Fields;
using StardewValley;
using StardewValley.Locations;

namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.Tiles;

/// <summary>Describes the <see cref="IslandSouthEast"/> mermaid music puzzle.</summary>
internal class IslandMermaidPuzzleSubject : TileSubject
{
    /*********
    ** Fields
    *********/
    /// <summary>Whether to show puzzle solutions.</summary>
    private readonly bool ShowPuzzleSolutions;


    /*********
    ** Public methods
    *********/
    /// <summary>Construct an instance.</summary>
    /// <param name="gameHelper">Provides utility methods for interacting with the game code.</param>
    /// <param name="location">The game location.</param>
    /// <param name="position">The tile position.</param>
    /// <param name="showRawTileInfo">Whether to show raw tile info like tilesheets and tile indexes.</param>
    /// <param name="showPuzzleSolutions">Whether to show puzzle solutions.</param>
    public IslandMermaidPuzzleSubject(GameHelper gameHelper, GameLocation location, Vector2 position, bool showRawTileInfo, bool showPuzzleSolutions)
        : base(gameHelper, location, position, showRawTileInfo)
    {
        this.Name = I18n.Puzzle_IslandMermaid_Title();
        this.Description = null;
        this.Type = null;
        this.ShowPuzzleSolutions = showPuzzleSolutions;
    }

    /// <inheritdoc />
    public override IEnumerable<ICustomField> GetData()
    {
        // mermaid puzzle
        {
            IslandSouthEast location = (IslandSouthEast)this.Location;
            bool complete = location.mermaidPuzzleFinished.Value;

            if (!this.ShowPuzzleSolutions && !complete)
                yield return new GenericField(I18n.Puzzle_Solution(), I18n.Puzzle_Solution_Hidden());
            else
            {
                int[] sequence = this.GameHelper.Metadata.PuzzleSolutions.IslandMermaidFluteBlockSequence;
                int songIndex = location.songIndex;

                var checkboxes = sequence
                    .Select((pitch, i) => CheckboxListField.Checkbox(text: this.Stringify(pitch), value: complete || songIndex >= i))
                    .ToArray();

                yield return new CheckboxListField(I18n.Puzzle_Solution(), checkboxes)
                    .AddIntro(complete ? I18n.Puzzle_Solution_Solved() : I18n.Puzzle_IslandMermaid_Solution_Intro());
            }
        }

        // raw map data
        foreach (ICustomField field in base.GetData())
            yield return field;
    }
}
