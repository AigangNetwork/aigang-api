using System;

namespace Aigang.Platform.Utils
{
    public static class GuidGenerator
    {
        public static string Generate()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}