
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
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string message, Exception innerException)
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
/// Property is null and can't be used
/// </summary>
[Serializable]
public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }
}
/// <summary>
/// 
/// </summary>
[Serializable]
public class BlWrongCategoryException : Exception
{
    public BlWrongCategoryException(string? message) : base(message) { }
}

/// <summary>
/// The date given is logically wrong
/// </summary>
[Serializable]
public class BlWrongDateOrderException : Exception
{
    public BlWrongDateOrderException(string? message) : base(message) { }
}