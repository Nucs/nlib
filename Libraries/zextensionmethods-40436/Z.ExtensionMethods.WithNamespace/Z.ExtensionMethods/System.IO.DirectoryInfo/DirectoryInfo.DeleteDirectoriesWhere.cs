// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.IO;
using System.Linq;

namespace Z.ExtensionMethods
{
    public static partial class DirectoryInfoExtension
    {
        /// <summary>
        ///     A DirectoryInfo extension method that deletes the directories where.
        /// </summary>
        /// <param name="obj">The obj to act on.</param>
        /// <param name="predicate">The predicate.</param>
        /// <example>
        ///     <code>
        ///           using System;
        ///           using System.IO;
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_IO_DirectoryInfo_DeleteDirectoriesWhere
        ///               {
        ///                   [TestMethod]
        ///                   public void DeleteDirectoriesWhere()
        ///                   {
        ///                       // Type
        ///                       var root = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;System_IO_DirectoryInfo_GetDirectories&quot;));
        ///                       Directory.CreateDirectory(root.FullName);
        ///                       root.CreateSubdirectory(&quot;DirFizz123&quot;);
        ///                       root.CreateSubdirectory(&quot;DirBuzz123&quot;);
        ///                       root.CreateSubdirectory(&quot;DirNotFound123&quot;);
        ///           
        ///                       // Exemples
        ///                       root.DeleteDirectoriesWhere(x =&gt; x.Name.StartsWith(&quot;DirFizz&quot;));
        ///                       int result = root.GetDirectories().Length;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(2, result);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static void DeleteDirectoriesWhere(this DirectoryInfo obj, Func<DirectoryInfo, bool> predicate)
        {
            obj.GetDirectories().Where(predicate).ForEach(x => x.Delete());
        }

        /// <summary>
        ///     A DirectoryInfo extension method that deletes the directories where.
        /// </summary>
        /// <param name="obj">The obj to act on.</param>
        /// <param name="searchOption">The search option.</param>
        /// <param name="predicate">The predicate.</param>
        /// <example>
        ///     <code>
        ///           using System;
        ///           using System.IO;
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_IO_DirectoryInfo_DeleteDirectoriesWhere
        ///               {
        ///                   [TestMethod]
        ///                   public void DeleteDirectoriesWhere()
        ///                   {
        ///                       // Type
        ///                       var root = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;System_IO_DirectoryInfo_GetDirectories&quot;));
        ///                       Directory.CreateDirectory(root.FullName);
        ///                       root.CreateSubdirectory(&quot;DirFizz123&quot;);
        ///                       root.CreateSubdirectory(&quot;DirBuzz123&quot;);
        ///                       root.CreateSubdirectory(&quot;DirNotFound123&quot;);
        ///           
        ///                       // Exemples
        ///                       root.DeleteDirectoriesWhere(x =&gt; x.Name.StartsWith(&quot;DirFizz&quot;));
        ///                       int result = root.GetDirectories().Length;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(2, result);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static void DeleteDirectoriesWhere(this DirectoryInfo obj, SearchOption searchOption, Func<DirectoryInfo, bool> predicate)
        {
            obj.GetDirectories("*.*", searchOption).Where(predicate).ForEach(x => x.Delete());
        }
    }
}