
namespace BO;
/// <summary>
/// Classes for managing different types of exceptions
/// Some are initially caught in Dal 
/// </summary>



/// <summary>
///Exception for when item's requested id does not exist
/// </summary>
[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }
}

/// <summary>
/// Exception for when an item's requested id already exist 
/// </summary>
[Serializable]
public class BlAllreadyExistsException : Exception
{
    public BlAllreadyExistsException(string? message) : base(message) { }
    public BlAllreadyExistsException(string message, Exception innerException)
                : base(message, innerException) { }
}
/// <summary>
/// Object T mustn't be deleted 
/// </summary>
[Serializable]
public class BlDeletionImpossibleException : Exception
{
    public BlDeletionImpossibleException(string? message) : base(message) { }
    public BlDeletionImpossibleException(string message, Exception innerException)
                : base(message, innerException) { }
}
/// <summary>
/// Failed to create xml file
/// </summary>
[Serializable]
public class BlXMLFileLoadCreateException : Exception
{
    public BlXMLFileLoadCreateException(string? message) : base(message) { }
    public BlXMLFileLoadCreateException(string message, Exception innerException)
                : base(message, innerException) { }
}
/// <summary>
/// An exception for dates mistakes
/// </summary>
[Serializable]
public class BlWrongDateException : Exception
{
    public BlWrongDateException(string? message) : base(message) { }
}
/// <summary>
/// Exception for wrong input such as:negetive id/cost, wrong email format etc.
/// </summary>
public class BlWrongInputException : Exception
{
    public BlWrongInputException(string? message) : base(message) { }
}

/// <summary>
/// The agent with the given id doen't have a task with the requested id
/// </summary>
[Serializable]
public class BlWrongAgentForTaskException : Exception
{
    public BlWrongAgentForTaskException(string? message) : base(message) { }
}
/// <summary>
/// 
/// </summary>
[Serializable]
public class BlProjectStageException : Exception
{
    public BlProjectStageException(string? message) : base(message) { }
}

