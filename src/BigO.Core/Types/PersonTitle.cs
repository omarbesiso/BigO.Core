using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BigO.Core.Types;

/// <summary>
///     Represents the title of a person.
/// </summary>
[PublicAPI]
public enum PersonTitle : byte
{
    /// <summary>
    ///     Represents an unknown title.
    /// </summary>
    [Display(Name = "Unknown")] [Description("Represents an unknown title.")]
    Unknown = 0,

    /// <summary>
    ///     Represents the title for a man.
    /// </summary>
    [Display(Name = "Mr.")] [Description("Represents the title for a man.")]
    Mr = 1,

    /// <summary>
    ///     Represents the title for a doctor, regardless of gender.
    /// </summary>
    [Display(Name = "Dr.")] [Description("Represents the title for a doctor, regardless of gender.")]
    Dr = 2,

    /// <summary>
    ///     Represents the title for a married woman.
    /// </summary>
    [Display(Name = "Mrs.")] [Description("Represents the title for a married woman.")]
    Mrs = 3,

    /// <summary>
    ///     Represents the title for a woman, regardless of marital status.
    /// </summary>
    [Display(Name = "Ms.")] [Description("Represents the title for a woman, regardless of marital status.")]
    Ms = 4,

    /// <summary>
    ///     Represents the title for an unmarried woman.
    /// </summary>
    [Display(Name = "Miss")] [Description("Represents the title for an unmarried woman.")]
    Miss = 5,

    /// <summary>
    ///     Represents the title for a knight.
    /// </summary>
    [Display(Name = "Sir")] [Description("Represents the title for a knight.")]
    Sir = 6,

    /// <summary>
    ///     Represents the title for a woman who has been given a dame-hood, the female equivalent of a knighthood.
    /// </summary>
    [Display(Name = "Dame")]
    [Description(
        "Represents the title for a woman who has been given a damehood, the female equivalent of a knighthood.")]
    Dame = 7
}