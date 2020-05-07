using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace Dependencies.Viewer.UnitTests.Extensions
{
    [ExcludeFromCodeCoverage]
    internal static class AssertExtensions
    {
        public static void DeepEqual<T>(this Assert _, T expected, T actual)
        {
            var compareLogic = new CompareLogic();

            var result = compareLogic.Compare(expected, actual);

            if (!result.AreEqual)
                throw new AssertFailedException(result.DifferencesString);
        }
    }
}
