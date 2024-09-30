//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RepoScribe.Core.Utilities
//{
//    internal class SetupManager
//    {
//        public SetupManager()
//        {
//            // Check if the user has a program data directory in their home directory '~/.reposcribe'
//            var homeDirDataPath = "%USERPROFILE%\\.reposcribe\\appsettings.json";

//            // Check if the user has a application installation directory 'C:\Program Files\RepoScribe'
//            // Check if the user has a CLI Executable directory in their PATH
//            // If not, create them all

//            // Check if the user has a configuration file in their home directory, use that
//            var homeDirConfigPath = "%USERPROFILE%\\Documents\\RepoScribe\\appsettings.json";
//        }
//    }

//    internal enum ProgramDataPaths
//    {
//        HomeDirDataPath,
//        AppDirDataPath,
//        CliDirDataPath
//    }

//    internal enum ConfigPaths
//    {
//        HomeDirConfigPath,
//        AppDirConfigPath,
//        EnvVarConfigPath
//    }

//    internal struct PathData
//    {
//        public ProgramDataPaths ProgramDataPath { get; set; }
//        public ConfigPaths ConfigPath { get; set; }
//    }

//    internal struct PathDataPair
//    {
//        public PathData HomeDirDataPath { get; set; }
//        public PathData AppDirDataPath { get; set; }
//        public PathData CliDirDataPath { get; set; }


//        internal string GetPath(PathDataPair pathDataPair)
//        {
//            // Check if the user has a program data directory in their home directory '~/.reposcribe'
//            var homeDirDataPath = "%USERPROFILE%\\.reposcribe\\appsettings.json";
//            // Check if the user has a application installation directory 'C:\Program Files\RepoScribe'
//            // Check if the user has a CLI Executable directory in their PATH
//            // If not, create them all
//            // Check if the user has a configuration file in their home directory, use that
//            var homeDirConfigPath = "%USERPROFILE%\\Documents\\RepoScribe\\appsettings.json";
//            return homeDirDataPath;
//        }
//    }
//}
