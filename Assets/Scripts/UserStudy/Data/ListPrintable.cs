using System.Collections.Generic;

namespace Content.Source.DataInformation
{
    public abstract class ListPrintable
    {
        public string UserId { get; set; }
        public abstract string ReturnRouteWay();

        /// <summary>
        /// Returns the Header of the File
        /// </summary>
        /// <returns></returns>
        public abstract string[] GetHeaderOfFile();
        
        /// <summary>
        /// Returns the recording of the Trial
        /// </summary>
        /// <returns></returns>
        public abstract List<string> GetResultsOfRecording();

    }
}