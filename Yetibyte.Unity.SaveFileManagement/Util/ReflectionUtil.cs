using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yetibyte.Unity.SaveFileManagement.Util {

    /// <summary>
    /// Contains a collection of utility methods associated with reflection.
    /// </summary>
    public static class ReflectionUtil {

        /// <summary>
        /// Checks whether or not the given type is decorated with an attribute of the type specified by the type parameter <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the attribute to check for. Must inherit from <see cref="Attribute"/>.</typeparam>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the given type is decorated with the target attribute; false otherwise.</returns>
        public static bool HasAttribute<T>(Type type) where T : Attribute {

            return type != null && type.GetCustomAttributes(typeof(T), false).Any();

        }

    }

}
