using System.Text;
using BigO.Core.Extensions;

namespace BigO.Core;

internal class RandomStringBuilder
{
    private static readonly string[] GlobalCharacterPool =
    {
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
        "abcdefghijklmnopqrstuvqxyz",
        "1234567890",
        "!@#$%^&*()_-+=[{]};:>|./?"
    };

    private readonly bool _allowDigits;
    private readonly bool _allowLowerCaseCharacters;
    private readonly bool _allowSpecialCharacters;
    private readonly bool _allowUpperCaseCharacters;
    private readonly char[]? _charactersToExclude;
    private readonly StringBuilder _stringBuilder;
    private readonly int _stringSize;

    public RandomStringBuilder(int stringSize, bool allowLowerCaseCharacters = true,
        bool allowUpperCaseCharacters = true, bool allowDigits = true,
        bool allowSpecialCharacters = true, params char[]? charactersToExclude)
    {
        if (stringSize <= 1)
        {
            throw new ArgumentOutOfRangeException(nameof(stringSize), "The string size cannot be less than 1.");
        }

        if (!allowUpperCaseCharacters && !allowLowerCaseCharacters && !allowDigits && !allowSpecialCharacters)
        {
            throw new ArgumentException(
                "At least one character options to include int he string needs to be set to true!");
        }

        _stringSize = stringSize;
        _allowLowerCaseCharacters = allowLowerCaseCharacters;
        _allowUpperCaseCharacters = allowUpperCaseCharacters;
        _allowDigits = allowDigits;
        _allowSpecialCharacters = allowSpecialCharacters;
        _charactersToExclude = charactersToExclude;
        _stringBuilder = new StringBuilder();

        BuildCurrentPool();
    }

    private void BuildCurrentPool()
    {
        var options = BuildOptions();
        var numberOfOptions = options.Count;

        for (var size = 0; size < _stringSize; size++)
        {
            var optionIndex = size % numberOfOptions;
            var characterSet = options[optionIndex].CharacterSet;
            var characterSetSize = options[optionIndex].Size;
            var randomIndex = Random.Shared.Next(0, characterSetSize);
            _stringBuilder.Append(characterSet[randomIndex]);
        }
    }

    public string Build()
    {
        return _stringBuilder.ToString().Shuffle();
    }

    private List<CharacterPool> BuildOptions()
    {
        List<CharacterPool> output = new();

        var processExclusions = _charactersToExclude.IsNotNullOrEmpty();

        if (_allowUpperCaseCharacters)
        {
            var characters = GlobalCharacterPool[0];
            if (processExclusions)
            {
                characters = characters.RemoveCharacters(_charactersToExclude);
            }

            output.Add(new CharacterPool(characters.ToCharArray()));
        }

        if (_allowLowerCaseCharacters)
        {
            var characters = GlobalCharacterPool[1];
            if (processExclusions)
            {
                characters = characters.RemoveCharacters(_charactersToExclude);
            }

            output.Add(new CharacterPool(characters.ToCharArray()));
        }

        if (_allowDigits)
        {
            var characters = GlobalCharacterPool[2];
            if (processExclusions)
            {
                characters = characters.RemoveCharacters(_charactersToExclude);
            }

            output.Add(new CharacterPool(characters.ToCharArray()));
        }

        if (_allowSpecialCharacters)
        {
            var characters = GlobalCharacterPool[3];
            if (processExclusions)
            {
                characters = characters.RemoveCharacters(_charactersToExclude);
            }

            output.Add(new CharacterPool(characters.ToCharArray()));
        }

        return output;
    }

    private struct CharacterPool
    {
        public CharacterPool(char[] characterSet)
        {
            CharacterSet = characterSet;
            Size = characterSet.Length;
        }

        public char[] CharacterSet { get; }

        public int Size { get; }
    }
}