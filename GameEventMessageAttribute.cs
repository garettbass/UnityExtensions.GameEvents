using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace UnityExtensions
{

    [AttributeUsage(
        AttributeTargets.Class |
        AttributeTargets.Enum |
        AttributeTargets.Struct)]
    public class GameEventMessageAttribute : Attribute { }

}