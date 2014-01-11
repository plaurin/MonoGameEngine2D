The code in the *.TiledSharp namespace is a modified version of the original found at https://github.com/marshallward/TiledSharp

List of changes:
- Removed dependencies not supported by Portable Class Library
 - Added a reference to the Zlib.Portable Nuget package (http://www.nuget.org/packages/Zlib.Portable/) 
   instead of embedding the source code directly.
 - Replaced usage of System.IO.Compression.GZipStream with Zlib.Portable implementation
 - Removed usage of System.Reflection.Assembly
- Updated to the StyleCop code standard

LICENSE and NOTICE files are the originals from the TiledSharp project