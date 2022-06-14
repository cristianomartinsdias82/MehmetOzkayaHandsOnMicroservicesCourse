using System;

namespace Catalog.API
{
    public static class ExceptionHelper
    {
        public static Exception NullArgumentException(string argName)
            => throw new ArgumentNullException($"Argument {argName} cannot be null.");
    }
}
