using System;

namespace nucs.Emailing {
    /// <summary>
    ///     Acts as a void generic
    /// </summary>
    public sealed class TVoid {
        private bool Equals(TVoid other) {
            return true;
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj) {
            if (obj is Type && ((Type) obj).Name.Equals("Void", StringComparison.InvariantCultureIgnoreCase))
                return true;
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj is TVoid && Equals((TVoid) obj);
        }

        /// <summary>Serves as the default hash function. </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() {
            return 0;
        }

        public static bool operator ==(TVoid left, TVoid right) {
            return Equals(left, right);
        }

        public static bool operator !=(TVoid left, TVoid right) {
            return !Equals(left, right);
        }
        
        public static bool operator ==(TVoid left, Type right) {
            return Equals(left, right);
        }

        public static bool operator !=(TVoid left, Type right) {
            return !Equals(left, right);
        }
    }
}