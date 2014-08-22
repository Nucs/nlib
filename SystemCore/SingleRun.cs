﻿#if NET_4_5 || NET_4_51
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using nucs.SystemCore.Boolean;

namespace nucs.SystemCore {
    public static class SingleRun {
        /// <summary>
        /// Already fired methods will be listed here in the following style:  'MethodName:lineInt:FilePath'. e.g. 'ShotOnlyOnce:44:C:\\hacking'
        /// </summary>
        public static string[] Fired {get { return shots.ToArray(); }} 
        private static HashSet<string> shots = null;

        /// <summary>
        /// Allows the method to be called/fired only once. if method was already fired, it will return true.
        /// </summary>
        /// <returns>If the method that this function is being called inside, has been called already.</returns>
        public static bool HasStaticAlreadyRan(
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0) {
            if (shots == null) shots = new HashSet<string>();
            var id = memberName+"|"+sourceLineNumber+"|"+sourceFilePath;
            if (shots.Contains(id)) return true;
            shots.Add(id);
            return false;
        }

        public static bool ResetAnyAlreadyRan(string id) {
            if (shots == null) return false;
            return shots.Remove(id);
        }
        public static bool HasAlreadyRan(int hashcode,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (shots == null) shots = new HashSet<string>();
            var id = memberName + "|" + sourceLineNumber + "|" + sourceFilePath + "|" + hashcode;
            if (shots.Contains(id)) return true;
            shots.Add(id);
            return false;
        }



        public static bool HasntRanBool(this Bool condition) {
            if (condition == true)
                return false;
            condition.value = true;
            return true;
        }
    }

    

}
#endif