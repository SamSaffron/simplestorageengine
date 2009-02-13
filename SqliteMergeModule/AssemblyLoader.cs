using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO.Compression;
using System.IO;

namespace SqliteMergeModule {
    public class AssemblyLoader {
        private static bool loaded = false;
        private static object syncObj = new object();
        private static Assembly sqliteAssembly;

        public static void LoadSqlite() {
            lock (syncObj) {
                if (!loaded) {
                    string path = Path.GetTempPath();
                    if (Is64Bit) {
                        path = Path.Combine(path, "SqliteDll");
                        LoadCompressedAssembly("SqliteMergeModule.System.Data.SQLite.X64.gz",path, "System.Data.SQLite.DLL");
                    } else {
                        path = Path.Combine(path, "Sqlite64Dll");
                        LoadCompressedAssembly("SqliteMergeModule.System.Data.SQLite.gz",path, "System.Data.SQLite.DLL");
                    }
                    AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
                    loaded = true;
                }
            }
        }

       static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args) {
            if (args.Name.StartsWith("System.Data.SQLite")) {
                return sqliteAssembly;
            }
            return null;
        }

        private static void LoadCompressedAssembly(string name, string path, string filename)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name)) {
                GZipStream unzippedStream = new GZipStream(stream, CompressionMode.Decompress, true);
                
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path); 
                }
                string fullpath = Path.Combine(path, filename);
                try {
                    if (File.Exists(fullpath)) {
                        // this is a hard one, we need to support engine upgrades 
                        File.Delete(fullpath);
                    }

                    File.WriteAllBytes(fullpath, ReadStream(unzippedStream, 0));
                } catch { 
                    // nothing we can do, just try to load whats there
                }
                sqliteAssembly = Assembly.LoadFrom(fullpath);
            }
        }

        // from http://www.yoda.arachsys.com/csharp/readbinary.html
        private static byte[] ReadStream(Stream stream, int initialLength) {

            // If we've been passed an unhelpful initial length, just
            // use 32K.
            if (initialLength < 1) {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0) {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length) {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1) {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }

        private static bool Is64Bit {
            get {
                return IntPtr.Size == 8;
            }
        }
    }
}
