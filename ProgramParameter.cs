using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudreve_FileSynchronizer
{
    public class ProgramParameter
    {
        private string sourceDirectory;
        private string target;
        private string userName;
        private string connectionString;
        private bool showHelp;
        private int storagePolicy;

        public string SourceDirectory { get => sourceDirectory; set => sourceDirectory = value?.Trim(); }
        public string Target { get => target; set => target = value?.Trim(); }
        public string UserName { get => userName; set => userName = value?.Trim(); }
        public string ConnectionString { get => connectionString; set => connectionString = value?.Trim(); }
        public bool ShowHelp { get => showHelp; set => showHelp = value; }
        public int StoragePolicy { get => storagePolicy; set => storagePolicy = value; }
    }
}
