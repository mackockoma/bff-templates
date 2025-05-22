using System.Reflection;

namespace {{SolutionName}}.Application;

public static class Reference
{
    static Reference()
    {
        var referenceType = typeof(Reference);

        Assembly = referenceType.Assembly;
    }

    public static Assembly Assembly { get; }
}
