using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

// Not Found
public class NotFoundException : Exception
{
    // Additional properties to provide more context about the exception
    public string ResourceName { get; } // Table or model name
    public object QueryParameter { get; }

    // Timestamp property to record the time when the exception occurred
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    // Constructor with additional parameters to initialize the properties
    public NotFoundException(string message, string resourceName, object queryParameter) : base(message)
    {
        ResourceName = resourceName;
        QueryParameter = queryParameter;
    }


    // Override the ToString method to include the additional properties in the exception message
    public override string ToString()
    {
        return $"[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] Resource '{ResourceName}' with query parameter '{QueryParameter}' not found. Message: {Message}\n{StackTrace}";
    }
}

// Format
public class FormatException : Exception
{

    public DateTime Timestamp { get; } = DateTime.UtcNow;
    public FormatException(string message) : base(message)
    {
    }

    // Override the ToString method to include the additional properties in the exception message
    public override string ToString()
    {
        return $"Format Exception on [{Timestamp:yyyy-MM-dd HH:mm:ss.fff}]. Message: {Message}\n{StackTrace}";
    }
}

/// //////////////////////
// VALIDATION
public class ValidationException : Exception
{
    // Additional properties to provide more context about the exception
    // Source eg api or controller name
    public string SourceRequested { get; }

    // Timestamp property to record the time when the exception occurred
    public DateTime Timestamp { get; } = DateTime.UtcNow;
    public ValidationException(string message, string sourceRequested) : base(message)
    {
        SourceRequested = sourceRequested;
    }

    // Override the ToString method to include the additional properties in the exception message
    public override string ToString()
    {
        return $"[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] Requested resource '{SourceRequested}' with validation exception. Message: {Message}\n{StackTrace}";
    }
}

public class UnauthorizedAccessException : Exception
{
    // Additional properties to provide more context about the exception
    public string SourceRequested { get; }


    // Timestamp property to record the time when the exception occurred
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    public UnauthorizedAccessException(string message, string sourceRequested) : base(message)
    {
        SourceRequested = sourceRequested;
    }
}



public class TryCatchException : Exception
{
    // Additional properties to provide more context about the exception
    public string SourceRequested { get; }

    // Timestamp property to record the time when the exception occurred
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    public TryCatchException(string message, string sourceRequested) : base(message)
    {
        SourceRequested = sourceRequested;
    }

    public override string ToString()
    {
        return $"[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] TryCatch exception using '{SourceRequested}'. Message: {Message}\n{StackTrace}";
    }
}

