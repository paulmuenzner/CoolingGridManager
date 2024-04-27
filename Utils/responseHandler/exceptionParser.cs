using System;
using System.Text.RegularExpressions;

public class ExceptionDetails
{

    public ExceptionDetails()
    {
        // Initialize non-nullable properties
        ExceptionType = "";
        ErrorMessage = "";
        FileName = "";
        MethodName = "";
    }

    public string ExceptionType { get; set; }
    public string ErrorMessage { get; set; }
    public string FileName { get; set; }
    public int LineNumber { get; set; }
    public string MethodName { get; set; }
}

public class ExceptionParser
{
    public static ExceptionDetails ParseException(Exception exception)
    {
        var exceptionDetails = new ExceptionDetails();


        // Split the exception message into lines
        var lines = exception.ToString().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        // Extract exception type and error message from the first line
        var firstLineParts = lines[0].Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
        if (firstLineParts.Length >= 2)
        {
            exceptionDetails.ExceptionType = firstLineParts[0];
            exceptionDetails.ErrorMessage = firstLineParts[1];
        }

        // Extract file name, line number, and method name from the last line (stack trace)
        var lastLine = lines[lines.Length - 1];
        var match = Regex.Match(lastLine, @"(.+) in (.+):line (\d+)");

        if (match.Success)
        {
            exceptionDetails.MethodName = match.Groups[1].Value;
            exceptionDetails.FileName = match.Groups[2].Value.Replace("\\", "/");
            int.TryParse(match.Groups[3].Value, out int lineNumber);
            exceptionDetails.LineNumber = lineNumber;
        }

        return exceptionDetails;
    }
}
