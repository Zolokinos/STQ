using System;
using JetBrains.Annotations;

namespace DefaultNamespace
{
    public static class Bus<T> where T : Delegate
    {
        [CanBeNull] public static T? Event;
    }
}