using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace JustManageMarkers.Commands;

public readonly struct Argument
{
    public string? Value { get; }
    public bool IsPresent { get; }
    public bool StringArgument { get; }
    public bool ListArgument { get; }

    public Argument(string value, bool wasQuoted, bool wasBracketed)
    {
        this.Value = value != "" ? value : null;
        this.IsPresent = value != "";
        this.StringArgument = wasQuoted;
        this.ListArgument = wasBracketed;
    }

    public override string ToString()
    {
        return "'" + this.Value + "'";
    }
}

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public partial class ArgumentStruct
{
    private const string QUOTED_ARGUMENT = "%QUOTED_ARGUMENT%";
    private const string BRACKETED_ARGUMENT = "%BRACKETED_ARGUMENT%";
    private const string QUOTED_PATTERN = "\"[^\"]*\"";
    private const string BRACKETED_PATTERN = @"\[[^\]]*\]";

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private string _arguments { get; }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private List<string> _quotedArguments { get; } = new();

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private List<string> _bracketedArguments { get; } = new();

    public List<List<string>> AcceptedArguments { get; }
    public string OriginalArguments { get; }
    public Argument Argument1 { get; set; }
    public Argument Argument2 { get; set; }
    public Argument Argument3 { get; set; }
    public Argument Argument4 { get; set; }
    public Argument Argument5 { get; set; }
    public int Count { get; } = 0;

    public ArgumentStruct(List<List<string>>? acceptedArguments, string? arguments)
    {
        acceptedArguments ??= new List<List<string>>();
        arguments ??= "";

        this.OriginalArguments = arguments;
        this._arguments = arguments;
        this.AcceptedArguments = acceptedArguments;

        // Save list arguments
        if (arguments.Contains('['))
        {
            (this._arguments, this._bracketedArguments) = _saveBracketedArguments(this._arguments);
        }

        // Save quoted arguments
        if (arguments.Contains('"'))
        {
            (this._arguments, this._quotedArguments) = _saveQuotedArguments(this._arguments);
        }

        // Break down the arguments
        var argumentNumber = 0;
        var argumentsArray = this._arguments.Split(" ");

        // Fail out if there are too many arguments
        if (argumentsArray.Length > 5)
        {
            throw new InvalidArgumentsException(
                arguments,
                "Too many arguments, please see the help command"
            );
        }

        // Set each argument into its own property
        foreach (var argument in argumentsArray)
        {
            var actualArgument = argument;
            var wasQuoted = false;
            var wasBracketed = false;

            // Replace placeholder with actual argument
            switch (argument)
            {
                case BRACKETED_ARGUMENT:
                    actualArgument = this._bracketedArguments[argumentNumber];
                    wasBracketed = true;
                    break;
                case QUOTED_ARGUMENT:
                    actualArgument = this._quotedArguments[argumentNumber];
                    wasQuoted = true;
                    break;
            }

            // Create argument, add it to count
            var argumentativeArgument = new Argument(
                actualArgument,
                wasQuoted,
                wasBracketed
            );

            if (argumentativeArgument.IsPresent)
            {
                this.Count++;
            }

            argumentNumber++;

            // Save each argument to property
            var property = this.GetType().GetProperty("Argument" + argumentNumber)!;
            property.SetValue(
                this,
                argumentativeArgument
            );
        }
    }

    public Argument getArgument(int index)
    {
        var property = this.GetType().GetProperty("Argument" + index)!;
        return (Argument) property.GetValue(this)!;
    }

    [GeneratedRegex(QUOTED_PATTERN)]
    private static partial Regex _quotedPattern();

    private static (string, List<string>) _saveQuotedArguments(string arguments)
    {
        List<string> argumentList = new();

        // Save quoted arguments and replace them with a placeholder
        arguments = _quotedPattern().Replace(
            arguments,
            match =>
            {
                argumentList.Add(match.Value.Trim());
                return QUOTED_ARGUMENT;
            }
        );

        return (arguments, argumentList);
    }

    [GeneratedRegex(BRACKETED_PATTERN)]
    private static partial Regex _bracketedPattern();

    private static (string, List<string>) _saveBracketedArguments(string arguments)
    {
        List<string> argumentList = new();

        // Save quoted arguments and replace them with a placeholder
        arguments = _bracketedPattern().Replace(
            arguments,
            match =>
            {
                argumentList.Add(match.Value.Trim());
                return QUOTED_ARGUMENT;
            }
        );

        return (arguments, argumentList);
    }
}
