using System.Reflection;

namespace Presentation;

public static class PresentationAssembly
{
    public static Assembly Assembly { get; private set; } = Assembly.GetExecutingAssembly();
}
