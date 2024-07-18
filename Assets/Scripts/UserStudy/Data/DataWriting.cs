
using System.Collections;
using System.Collections.Generic;


namespace Content.Source.DataInformation
{
    public interface DataWriting
    {
        IEnumerator WriteTrialsIntoCsvFile(ListPrintable[] trialsToWrite, string postAddress);
        IEnumerator WritePositionLogIntoCSVFile(ListPrintable positionInfo);
       
        void WriteQuestionnaire(string userID, Dictionary<string, string> input, string nameOfQuestionnaire);
        
        IEnumerator WriteQuestionnaireAsCoroutine(string userID, Dictionary<string, string> input, string nameOfQuestionnaire);
    }
}