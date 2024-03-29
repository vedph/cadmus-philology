﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cadmus.Core;
using Cadmus.Core.Layers;
using Fusi.Tools.Configuration;

namespace Cadmus.Philology.Parts;

/// <summary>
/// Critical apparatus layer fragment.
/// Tag: <c>fr.it.vedph.apparatus</c>.
/// </summary>
/// <seealso cref="T:Cadmus.Core.Layers.ITextLayerFragment" />
[Tag("fr.it.vedph.apparatus")]
public sealed class ApparatusLayerFragment : ITextLayerFragment
{
    /// <summary>
    /// Gets or sets the location of this fragment.
    /// </summary>
    /// <remarks>
    /// The location can be expressed in different ways according to the
    /// text coordinates system being adopted. For instance, it might be a simple
    /// token-based coordinates system (e.g. 1.2=second token of first block), or
    /// a more complex system like an XPath expression.
    /// </remarks>
    public string Location { get; set; }

    /// <summary>
    /// Gets or sets the tag, an optional arbitrary string representing a
    /// categorization of some sort for that fragment, e.g. "margin",
    /// "interlinear", etc. This can be overridden by variants tag.
    /// </summary>
    public string? Tag { get; set; }

    /// <summary>
    /// Gets or sets the entries.
    /// </summary>
    public List<ApparatusEntry> Entries { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApparatusLayerFragment"/>
    /// class.
    /// </summary>
    public ApparatusLayerFragment()
    {
        Location = "";
        Entries = new List<ApparatusEntry>();
    }

    /// <summary>
    /// Get all the pins exposed by the implementor.
    /// </summary>
    /// <param name="item">The optional item. The item with its parts
    /// can optionally be passed to this method for those parts requiring
    /// to access further data.</param>
    /// <returns>Pins: <c>fr.tot-count</c> and a collection of pins with
    /// keys: <c>fr.variant</c>=variant (normalized if any, else just
    /// the variant), <c>fr.witness</c>=witness (filtered, with digits),
    /// <c>fr.author</c>=author (filtered, with digits).</returns>
    public IEnumerable<DataPin> GetDataPins(IItem? item = null)
    {
        DataPinBuilder builder = new(DataPinHelper.DefaultFilter);

        builder.Set(PartBase.FR_PREFIX + "tot", Entries?.Count ?? 0, false);

        if (Entries?.Count > 0)
        {
            foreach (ApparatusEntry entry in Entries)
            {
                // fr.variant
                if (entry.Type != ApparatusEntryType.Note)
                {
                    string variant = entry.NormValue ?? entry.Value ?? "";
                    builder.AddValue(PartBase.FR_PREFIX + "variant", variant);
                }

                // fr.witness
                if (entry.Witnesses?.Count > 0)
                {
                    builder.AddValues(
                        PartBase.FR_PREFIX + "witness",
                        from w in entry.Witnesses select w.Value,
                        filter: true, filterOptions: true);
                }

                // fr.author
                if (entry.Authors?.Count > 0)
                {
                    builder.AddValues(
                        PartBase.FR_PREFIX + "author",
                        from w in entry.Authors select w.Value,
                        filter: true, filterOptions: true);
                }
            }
        }

        return builder.Build(null);
    }

    /// <summary>
    /// Gets the definitions of data pins used by the implementor.
    /// </summary>
    /// <returns>Data pins definitions.</returns>
    public IList<DataPinDefinition> GetDataPinDefinitions()
    {
        return new List<DataPinDefinition>(new[]
        {
            new DataPinDefinition(DataPinValueType.Integer,
                PartBase.FR_PREFIX + "tot-count",
                "The total count of apparatus entries in fragment."),
            new DataPinDefinition(DataPinValueType.String,
                PartBase.FR_PREFIX + "variant",
                "The list of variants (using their normalized form if any).",
                "M"),
            new DataPinDefinition(DataPinValueType.String,
                PartBase.FR_PREFIX + "witness",
                "The list of witnesses.",
                "Mf"),
            new DataPinDefinition(DataPinValueType.String,
                PartBase.FR_PREFIX + "author",
                "The list of authors.",
                "Mf")
        });
    }

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <returns>
    /// A <see cref="string" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        StringBuilder sb = new($"[Apparatus] {Location}");

        switch (Entries?.Count ?? 0)
        {
            case 1:
                sb.Append(' ').Append(Entries![0].ToString());
                break;
            case 0:
                break;
            default:
                sb.Append(' ').Append(Entries!.Count);
                break;
        }

        return sb.ToString();
    }
}
