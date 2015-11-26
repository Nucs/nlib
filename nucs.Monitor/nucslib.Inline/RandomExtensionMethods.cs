using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace nucs.SystemCore.Boolean {
    internal static class RandomExtensionMethods {
        /// <summary>
        ///     Creates all directories and subdirectories in the specified @this if the directory doesn't already exists.
        ///     This methods is the same as FileInfo.CreateDirectory however it's less ambigues about what happen if the
        ///     directory already exists.
        /// </summary>
        /// <param name="this">The directory @this to create.</param>
        /// <returns>An object that represents the directory for the specified @this.</returns>
        /// ###
        /// <exception cref="T:System.IO.IOException">
        ///     The directory specified by <paramref name="this" /> is a file.-or-The
        ///     network name is not known.
        /// </exception>
        /// ###
        /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// ###
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="this" /> is a zero-length string, contains only
        ///     white space, or contains one or more invalid characters as defined by
        ///     <see
        ///         cref="F:System.IO.Path.InvalidPathChars" />
        ///     .-or-<paramref name="this" /> is prefixed with, or contains only a colon character (:).
        /// </exception>
        /// ###
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="this" /> is null.
        /// </exception>
        /// ###
        /// <exception cref="T:System.IO.PathTooLongException">
        ///     The specified @this, file name, or both exceed the system-
        ///     defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file
        ///     names must be less than 260 characters.
        /// </exception>
        /// ###
        /// <exception cref="T:System.IO.DirectoryNotFoundException">
        ///     The specified @this is invalid (for example, it is on
        ///     an unmapped drive).
        /// </exception>
        /// ###
        /// <exception cref="T:System.NotSupportedException">
        ///     <paramref name="this" /> contains a colon character (:) that
        ///     is not part of a drive label ("C:\").
        /// </exception>
        public static DirectoryInfo EnsureDirectoryExists(this DirectoryInfo @this)
        {
            if (@this.Exists == false)
                return Directory.CreateDirectory(@this.FullName);
            return @this;
        }

        /// <summary>
        ///     A string extension method that removes the number described by @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A string.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_String_RemoveNumber
        ///               {
        ///                   [TestMethod]
        ///                   public void RemoveNumber()
        ///                   {
        ///                       // Type
        ///                       string @this = &quot;Fizz1Buzz2&quot;;
        ///           
        ///                       // Exemples
        ///                       string result = @this.RemoveNumber(); // return &quot;FizzBuzz&quot;;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(&quot;FizzBuzz&quot;, result);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static string RemoveNumber(this string @this)
        {
            return new string(@this.ToCharArray().Where(x => !Char.IsNumber(x)).ToArray());
        }

        /// <summary>
        ///     Concatenates all the elements of a IEnumerable, using the specified separator between each element.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">An IEnumerable that contains the elements to concatenate.</param>
        /// <param name="separator">
        ///     The string to use as a separator. separator is included in the returned string only if
        ///     value has more than one element.
        /// </param>
        /// <returns>
        ///     A string that consists of the elements in value delimited by the separator string. If value is an empty array,
        ///     the method returns String.Empty.
        /// </returns>
        public static string StringJoin<T>(this IEnumerable<T> @this, string separator)
        {
            return string.Join(separator, @this.Select(j => j.ToString()).ToArray());
        }

                public static string DeleteDuplicateCharsMultiple(this string target, string samples) {
            if (string.IsNullOrEmpty(target) || string.IsNullOrEmpty(samples))
                return target;
            var indexes = new List<long>();
            for (var i = 0; i < target.Length; i++) {
                for (int j = 0; j < samples.Length; j++) {
                    if (target[i] == samples[j])
                        for (var l = 1;/*true*/; l++) {
                            if (i + l < target.Length && target[i + l] == samples[j]) {
                                indexes.Add(i+l);
                            } else
                                break;
                        }
                }
            }
            return target.DeleteIndexes(indexes);
        }

        /// <summary>
        /// Given "01234lol" as target and indexes in an array {0, 1, 3} will return "24lol".
        /// </summary>
        /// <param name="target"></param>
        /// <param name="indexes">Which indexes to delete</param>
        /// <returns></returns>
        public static string DeleteIndexes(this string target, ICollection<long> indexes)
        {
            if (string.IsNullOrEmpty(target) || indexes.Count == 0)
                return target;
            var tar = target.ToCharArray();
            const char deletechar = '♣';
            foreach (var i in indexes)
                if (i < tar.Length)
                    tar[i] = deletechar;
            return new string(tar).Replace(deletechar.ToString(CultureInfo.InvariantCulture), string.Empty);
        }
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var j in list)
            {
                action.Invoke(j);
                yield return j;
            }
        }

        public static void EvaluateLinq<T>(this IEnumerable<T> list)
        {
            var e = list.GetEnumerator();
            while (e.MoveNext()) ;
        }

    }

        internal static class StringGenerator {
        private static Random rand=null;
        public static string Generate(int len = 10) {
            return Generate(rand ?? (rand=new Random()), len);
        } 

        
        public static string Generate(Random rand, int len = 10) {
            if (len <= 0) return "";
            char ch;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rand.NextDouble() + 65)));
                builder.Append(ch);
            }
            for (int i = 0; i < len; i++) {
                if (rand.Next(1, 3) == 1)
                    builder[i] = char.ToLowerInvariant(builder[i]);
            }
            return builder.ToString();
        }
        public static System.Boolean HasExtension(this FileInfo @this)
        {
            return Path.HasExtension(@this.FullName);
        }
        public static String GetFileNameWithoutExtension(this FileInfo @this)
        {
            return Path.GetFileNameWithoutExtension(@this.FullName);
        }
        public static System.Boolean IsLetter(this String s, Int32 index)
        {
            return Char.IsLetter(s, index);
        }
        public static bool IsDigit(this Char c)
        {
            return Char.IsDigit(c);
        }
        public static decimal ToDecimal(this string toConvert)
        {
            try { return Convert.ToDecimal(toConvert); } catch { return 0; }
        }

        public static FileInfo ChangeExtension(this FileInfo @this, String extension)
        {
            return new FileInfo(Path.ChangeExtension(@this.FullName, extension));
        }
        /// <summary>
        /// Simple way to convert an single item to an IEnumerable and eventually any type of collection.
        /// </summary>
        public static IEnumerable<T> ToEnumerable<T>(this T obj)
        {
            yield return obj;
        }

        public static FileInfo[] GetFiles(this DirectoryInfo @this, String[] searchPatterns)
        {
            return searchPatterns.SelectMany(x => @this.GetFiles(x)).Distinct().ToArray();
        }

    }




}