using System.Collections.Generic;
using System.IO;
using nucs.Filesystem;

namespace nucs.Startup {
    public interface IStartupMethod {
        /// <summary>
        ///     Attach the file to startup, alias is optional and used for avoiding collisions with other attacheds.
        /// </summary>
        /// <param name="file">The file that will start on startup</param>
        /// <param name="alias">The name inwhich the startup will be registered under, used to avoid collisions and can be null.</param>
        bool Attach(FileCall file, string alias = null);

        /// <summary>
        ///     Disattach the file from startup.
        /// </summary>
        /// <returns></returns>
        bool Disattach(FileInfo fc);

        /// <summary>
        ///     Is this file attached under the alias name?
        /// </summary>
        bool IsAttached(string alias);

        /// <summary>
        ///     Is this file attached?
        /// </summary>
        bool IsAttached(FileInfo file);

        /// <summary>
        ///     Is this startup method attachable with current credentials.
        /// </summary>
        bool IsAttachable { get; }

        /// <summary>
        ///     Returns the attached files to this startup.
        /// </summary>
        IEnumerable<FileCall> Attached { get; }

        /// <summary>
        ///     Provides a priority to each method. which is best, 0 is first to be tested, the larger the less prioritezed.
        /// </summary>
        uint Priority { get; }
    }
}