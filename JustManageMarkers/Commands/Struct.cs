using System.Collections.Generic;

namespace JustManageMarkers.Commands;

public struct Command
{
    public string Name { get; }
    public string ShortDescription { get; }
    public List<List<string>>? Arguments { get; } // If a string ends with ?, it is optional
    public string? Description { get; }
    public bool SpaceAbove { get; } = false;
    public bool IncludeInHelp { get; } = true;

    #region Constructors

    #region No arguments, no description

    public Command(string name, string shortDescription) : this()
    {
        this.Name = name;
        this.ShortDescription = shortDescription;
    }

    public Command(string name, string shortDescription, bool includeInHelp) : this()
    {
        this.Name = name;
        this.ShortDescription = shortDescription;
        this.IncludeInHelp = includeInHelp;
    }

    public Command(string name, string shortDescription, bool spaceAbove, bool includeInHelp) : this()
    {
        this.Name = name;
        this.ShortDescription = shortDescription;
        this.SpaceAbove = spaceAbove;
        this.IncludeInHelp = includeInHelp;
    }

    #endregion

    #region No arguments, with description

    public Command(string name, string shortDescription, string description) : this()
    {
        this.Name = name;
        this.ShortDescription = shortDescription;
        this.Description = description;
    }

    public Command(string name, string shortDescription, string description, bool includeInHelp) : this()
    {
        this.Name = name;
        this.ShortDescription = shortDescription;
        this.IncludeInHelp = includeInHelp;
        this.Description = description;
    }

    public Command(string name, string shortDescription, string description, bool spaceAbove, bool includeInHelp) : this()
    {
        this.Name = name;
        this.ShortDescription = shortDescription;
        this.SpaceAbove = spaceAbove;
        this.IncludeInHelp = includeInHelp;
        this.Description = description;
    }

    #endregion

    #region Argments, no description

    public Command(string name, string shortDescription, string description, List<List<string>> arguments) : this()
    {
        this.Name = name;
        this.ShortDescription = shortDescription;
        this.Description = description;
        this.Arguments = arguments;
    }

    public Command(string name, string shortDescription, string description, List<List<string>> arguments, bool includeInHelp) : this()
    {
        this.Name = name;
        this.ShortDescription = shortDescription;
        this.Description = description;
        this.Arguments = arguments;
        this.IncludeInHelp = includeInHelp;
    }

    public Command(string name, string shortDescription, string description, List<List<string>> arguments, bool spaceAbove, bool includeInHelp) : this()
    {
        this.Name = name;
        this.ShortDescription = shortDescription;
        this.Description = description;
        this.Arguments = arguments;
        this.SpaceAbove = spaceAbove;
        this.IncludeInHelp = includeInHelp;
    }

    #endregion

    #region Arguments, with description

    public Command(string name, string shortDescription, List<List<string>> arguments) : this()
    {
        this.Name = name;
        this.ShortDescription = shortDescription;
        this.Arguments = arguments;
    }

    public Command(string name, string shortDescription, List<List<string>> arguments, bool includeInHelp) : this()
    {
        this.Name = name;
        this.ShortDescription = shortDescription;
        this.Arguments = arguments;
        this.IncludeInHelp = includeInHelp;
    }

    public Command(string name, string shortDescription, List<List<string>> arguments, bool spaceAbove, bool includeInHelp) : this()
    {
        this.Name = name;
        this.ShortDescription = shortDescription;
        this.Arguments = arguments;
        this.SpaceAbove = spaceAbove;
        this.IncludeInHelp = includeInHelp;
    }

    #endregion

    #endregion
}
