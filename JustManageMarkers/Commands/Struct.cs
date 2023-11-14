using System.Collections.Generic;

namespace JustManageMarkers.Commands;

public struct Command
{
    public string Name { get; }
    public HandlerDelegate Handler { get; }
    public string ShortDescription { get; }
    public List<List<string>>? Arguments { get; } // If a string ends with ?, it is optional
    public ArgumentParserDelegate? ArgumentParser { get; }
    public string? Description { get; }
    public bool SpaceAbove { get; } = false;
    public bool IncludeInHelp { get; } = true;


    public delegate void HandlerDelegate(
        JustManageMarkers plugin,
        ArgumentStruct args,
        int parserResult
    );

    public delegate int ArgumentParserDelegate(
        JustManageMarkers plugin,
        ArgumentStruct arguments
    );


    #region Constructors

    #region No arguments, no description

    public Command(string name, HandlerDelegate handler, string shortDescription) : this()
    {
        this.Name = name;
        this.Handler = handler;
        this.ShortDescription = shortDescription;
    }

    public Command(
        string name,
        HandlerDelegate handler,
        string shortDescription,
        bool includeInHelp
    ) : this()
    {
        this.Name = name;
        this.Handler = handler;
        this.ShortDescription = shortDescription;
        this.IncludeInHelp = includeInHelp;
    }

    public Command(
        string name,
        HandlerDelegate handler,
        string shortDescription,
        bool spaceAbove,
        bool includeInHelp
    ) : this()
    {
        this.Name = name;
        this.Handler = handler;
        this.ShortDescription = shortDescription;
        this.SpaceAbove = spaceAbove;
        this.IncludeInHelp = includeInHelp;
    }

    #endregion

    #region No arguments, with description

    public Command(
        string name,
        HandlerDelegate handler,
        string shortDescription,
        string description
    ) : this()
    {
        this.Name = name;
        this.Handler = handler;
        this.ShortDescription = shortDescription;
        this.Description = description;
    }

    public Command(
        string name,
        HandlerDelegate handler,
        string shortDescription,
        string description,
        bool includeInHelp
    ) : this()
    {
        this.Name = name;
        this.Handler = handler;
        this.ShortDescription = shortDescription;
        this.IncludeInHelp = includeInHelp;
        this.Description = description;
    }

    public Command(
        string name,
        HandlerDelegate handler,
        string shortDescription,
        string description,
        bool spaceAbove,
        bool includeInHelp
    ) : this()
    {
        this.Name = name;
        this.Handler = handler;
        this.ShortDescription = shortDescription;
        this.SpaceAbove = spaceAbove;
        this.IncludeInHelp = includeInHelp;
        this.Description = description;
    }

    #endregion

    #region Arguments, no description

    public Command(
        string name,
        HandlerDelegate handler,
        string shortDescription,
        string description,
        List<List<string>> arguments,
        ArgumentParserDelegate argumentParser
    ) : this()
    {
        this.Name = name;
        this.Handler = handler;
        this.ShortDescription = shortDescription;
        this.Description = description;
        this.Arguments = arguments;
        this.ArgumentParser = argumentParser;
    }

    public Command(
        string name,
        HandlerDelegate handler,
        string shortDescription,
        string description,
        List<List<string>> arguments,
        ArgumentParserDelegate argumentParser,
        bool includeInHelp
    ) : this()
    {
        this.Name = name;
        this.Handler = handler;
        this.ShortDescription = shortDescription;
        this.Description = description;
        this.Arguments = arguments;
        this.ArgumentParser = argumentParser;
        this.IncludeInHelp = includeInHelp;
    }

    public Command(
        string name,
        HandlerDelegate handler,
        string shortDescription,
        string description,
        List<List<string>> arguments,
        ArgumentParserDelegate argumentParser,
        bool spaceAbove,
        bool includeInHelp
    ) : this()
    {
        this.Name = name;
        this.Handler = handler;
        this.ShortDescription = shortDescription;
        this.Description = description;
        this.Arguments = arguments;
        this.ArgumentParser = argumentParser;
        this.SpaceAbove = spaceAbove;
        this.IncludeInHelp = includeInHelp;
    }

    #endregion

    #region Arguments, with description

    public Command(
        string name,
        HandlerDelegate handler,
        string shortDescription,
        List<List<string>> arguments,
        ArgumentParserDelegate argumentParser
    ) : this()
    {
        this.Name = name;
        this.Handler = handler;
        this.ShortDescription = shortDescription;
        this.Arguments = arguments;
        this.ArgumentParser = argumentParser;
    }

    public Command(
        string name,
        HandlerDelegate handler,
        string shortDescription,
        List<List<string>> arguments,
        ArgumentParserDelegate argumentParser,
        bool includeInHelp
    ) : this()
    {
        this.Name = name;
        this.Handler = handler;
        this.ShortDescription = shortDescription;
        this.Arguments = arguments;
        this.ArgumentParser = argumentParser;
        this.IncludeInHelp = includeInHelp;
    }

    public Command(
        string name,
        HandlerDelegate handler,
        string shortDescription,
        List<List<string>> arguments,
        ArgumentParserDelegate argumentParser,
        bool spaceAbove,
        bool includeInHelp
    ) : this()
    {
        this.Name = name;
        this.Handler = handler;
        this.ShortDescription = shortDescription;
        this.Arguments = arguments;
        this.ArgumentParser = argumentParser;
        this.SpaceAbove = spaceAbove;
        this.IncludeInHelp = includeInHelp;
    }

    #endregion

    #endregion
}
