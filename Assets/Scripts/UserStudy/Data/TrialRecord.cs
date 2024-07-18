using System.Collections.Generic;


namespace Content.Source.DataInformation
{
    public class TrialRecord : ListPrintable
    {
         public static readonly string[] Header = 
        {
            "UserId",
            "Session",
            "Round",
            "Condition",
            "UseCustomizedAvatar",
            "TrialCounter",
            "StopSignalDelay", 
            "InterTrialInterval",
            "TrialType",
            "ReactionTime",
            "Was_Successful",
            "Was_Timeout",
            "TimeStamp",
            "PressedKey",
            "ShownSprite"
            
        };
         
         
        public bool UseCustomizedAvatar { get; set; }

        public int RoundId { get; set; }

        public string UserId { get; set; }

        public string Condition { get; set;}

        public string SessionId { get; set; }

        public int TrialCounter { get; set; }

        public float StopSignalDelay { get; set; }

        public float InterTrialInterval { get; set; }

        public string TrialType { get; set; }

        public bool WasSuccessful { get; set; }

        public bool WasTimeOut { get; set; }

        public float ReactionTime { get; set; }

        public string TimeStamp { get; set; }

        public string KeyCode { get; set; }

        public string ShownSprite { get; set; }
        

        public string[] GetHeader()
        {
            return Header;
        }

        public string GetUserId()
        {
            return UserId;
        }

        public string GetSessionId()
        {
            return SessionId;
        }

        public override string ReturnRouteWay()
        {
            throw new System.NotImplementedException();
        }

        public override string[] GetHeaderOfFile()
        {
            return Header;
        }
        
        public override List<string> GetResultsOfRecording()
        {
            
            List<string> results = new List<string>();
            
            results.Add(UserId);
            results.Add(SessionId);
            results.Add(RoundId.ToString());
            results.Add(Condition);
            results.Add(UseCustomizedAvatar.ToString());
            results.Add(TrialCounter.ToString());
            
            
            // Only Record the StopSignalDelay if the Task was a Stop Signal one
            //if (TrialType == TaskDirections.StopSignal.ToString())
            //{
            //    results.Add(StopSignalDelay.ToString());
            //}
            //else
            //{
            //    results.Add("NAN");
            //}


            results.Add(InterTrialInterval.ToString());
            results.Add(TrialType);
            
            // if it was a Timeout, set recording time to NAN
            if (WasTimeOut)
            {
                results.Add("NAN");
            }
            else
            {
                
                results.Add(ReactionTime.ToString());
            }

            results.Add(WasSuccessful.ToString());
            results.Add(WasTimeOut.ToString()); 
            results.Add(TimeStamp);
            results.Add(KeyCode);
            results.Add(ShownSprite);
            
            return results;
            
        }

    }
}