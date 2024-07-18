using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

namespace Content.Source.DataInformation
{
    public class BofWriter:DataWriting
    {
        private string serverUrl = "";
        public static string postURLTrialInformation = "/sassignment_recordtrial_";
        public static string postURLTiralCharacter = "/sassignment_recordtrialCharacter_";
        public static string postURLMatchingTrial = "/_matchingResult";
        private string postURLPosition = "/sassignment_recordpositon_";
        
        public IEnumerator WriteTrialsIntoCsvFile(ListPrintable[] trialsToWrite, string postAddress)
        {
            
            WWWForm trialDataToCsv = new WWWForm();
            string[] variables = trialsToWrite[0].GetHeaderOfFile();
            
            
            for (int i = 0; i < trialsToWrite[0].GetHeaderOfFile().Length; i++)
            {
                Debug.Log(variables[i]+" :" +trialsToWrite[0].GetResultsOfRecording()[i]);
                trialDataToCsv.AddField(variables[i],trialsToWrite[0].GetResultsOfRecording()[i]);
            }
            
            
            var request = UnityWebRequest.Post(String.Concat(serverUrl,postAddress), trialDataToCsv);
            request.SendWebRequest();

            while (request.isDone == false)
            {
                yield return null;
            }
        }
        
        public IEnumerator WritePositionLogIntoCSVFile(ListPrintable trialRecording)
        {
            WWWForm trialDataToCsv = new WWWForm();
            string[] variables = trialRecording.GetHeaderOfFile();
            
            for (int i = 0; i < trialRecording.GetHeaderOfFile().Length; i++)
            {
                trialDataToCsv.AddField(variables[i],trialRecording.GetResultsOfRecording()[i]);
            }
            
            
            var request = UnityWebRequest.Post(String.Concat(serverUrl,postURLPosition), trialDataToCsv);
            request.SendWebRequest();

            while (request.isDone == false)
            {
                yield return null;
            }
        }

        /// <summary>
        /// Returns the condition for CharacterEditor
        /// </summary>
        /// <returns></returns>
        public int GetConditionFromServer()
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="input"></param>
        /// <param name="nameOfQuestionnaire"></param>
        public void WriteQuestionnaire(string userID, Dictionary<string, string> input, string nameOfQuestionnaire)
        {
            WWWForm trialDataToCsv = new WWWForm();
            
            trialDataToCsv.AddField("participantID",userID);
            //trialDataToCsv.AddField("sessionID",ExperimentController.Instance().GetModelOfExperiment().GetExperimentSettings().SessionId.ToString());
            trialDataToCsv.AddField("currentTime",DateTime.Now.ToString(CultureInfo.InvariantCulture));
            //trialDataToCsv.AddField("condition",ExperimentController.Instance().GetExperimentConditionByName());
            foreach (var keypair in input)
            {
                trialDataToCsv.AddField(keypair.Key,keypair.Value);
            }
            // We send the trial to write to bof
            try
            {
                string uri = String.Concat(serverUrl, postURLTrialInformation, nameOfQuestionnaire);
                var request = UnityWebRequest.Post(uri, trialDataToCsv);
                Debug.Log("ServerRequest:" +uri);
                request.SendWebRequest();
            }
            catch (Exception ex) {
                Debug.Log("Error in SaveTrial(): " + ex.Message);
            }
        }

        public IEnumerator WriteQuestionnaireAsCoroutine(string userID, Dictionary<string, string> input, string nameOfQuestionnaire)
        {
            WWWForm trialDataToCsv = new WWWForm();
            
            
            trialDataToCsv.AddField("participantID",userID);
            //trialDataToCsv.AddField("sessionID",ExperimentController.Instance().GetModelOfExperiment().GetExperimentSettings().SessionId.ToString());
            trialDataToCsv.AddField("currentTime",DateTime.Now.ToString(CultureInfo.InvariantCulture));
            //trialDataToCsv.AddField("condition",ExperimentController.Instance().GetExperimentConditionByName());

            foreach (var keypair in input)
            {
                trialDataToCsv.AddField(keypair.Key,keypair.Value);
            }
            // We send the trial to write to bof

            string uri = String.Concat(serverUrl, postURLTrialInformation, nameOfQuestionnaire);
            var request = UnityWebRequest.Post(uri, trialDataToCsv);
            Debug.Log("ServerRequest:" +uri);

            request.SendWebRequest();

            while (request.isDone == false)
            {
                yield return null;
            }
            
            Debug.Log("Request is Done:" + request.isDone);
        }


        public BofWriter(string serverUrl)
        {
            Debug.Log("Setup BOF WRITER");
            this.serverUrl = serverUrl;
        }
    }


}