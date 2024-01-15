namespace DO;
/// <summary>
/// Classes for managing different types of exceptions
/// </summary>

/// <summary>
///Exception for when item's requested id does not exist
/// </summary>
[Serializable]
public class DalDoesNotExistException : Exception 
{
    public DalDoesNotExistException(string? message) : base(message) { }
}

/// <summary>
/// Exception for when item's requested id already exist
/// </summary>
[Serializable]
public class DalAlreadyExistsException : Exception
{
    public DalAlreadyExistsException(string? message) : base(message) { }
}
/// <summary>
/// Object T mustn't be deleted
/// </summary>
[Serializable]
public class DalDeletionImpossibleException : Exception
{
    public DalDeletionImpossibleException(string? message) : base(message) { }
}


