using System.Collections.Generic;


namespace Content.Source.DataInformation
{
    public class TrialRecord : ListPrintable
    {
         public static readonly string[] Header = 
        {
            "UserId",
            "Condition", //cosy or non
            "TrialType", //round 0 for pre, round 1 for post
            "TensionAnxietyScore",
            "DepressionDejectionScore",
            "VigorActivityScore",
            "AngerHostilityScore",
            "FatigueInertiaScore",
            "ConfusionBewildermentScore",
            "TotalMoodDisturbanceScore"
        };
         

        //public string UserId { get; set; }

        public string Condition { get; set;}

        public int TrialType { get; set; }

        public float TensionAnxietyScore { get; set; }

        public float DepressionDejectionScore { get; set; }

        public float AngerHostilityScore { get; set; }

        public float FatigueInertiaScore { get; set; }

        public float ConfusionBewildermentScore { get; set; }

        public float TotalMoodDisturbanceScore { get; set; }
        public float VigorActivityScore { get; set; }


        public string[] GetHeader()
        {
            return Header;
        }

        public string GetUserId()
        {
            return UserId;
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
            results.Add(Condition);
            results.Add(TrialType.ToString());
            results.Add(Condition);

            results.Add(TensionAnxietyScore.ToString());
            results.Add(DepressionDejectionScore.ToString());
            results.Add(AngerHostilityScore.ToString());
            results.Add(FatigueInertiaScore.ToString());
            results.Add(ConfusionBewildermentScore.ToString());
            results.Add(VigorActivityScore.ToString());
            results.Add(TotalMoodDisturbanceScore.ToString());


            // Only Record the StopSignalDelay if the Task was a Stop Signal one
            //if (TrialType == TaskDirections.StopSignal.ToString())
            //{
            //    results.Add(StopSignalDelay.ToString());
            //}
            //else
            //{
            //    results.Add("NAN");
            //}


            //results.Add(InterTrialInterval.ToString());
            //results.Add(TrialType);
            
            //// if it was a Timeout, set recording time to NAN
            //if (WasTimeOut)
            //{
            //    results.Add("NAN");
            //}
            //else
            //{
                
            //    results.Add(ReactionTime.ToString());
            //}

            //results.Add(WasSuccessful.ToString());
            //results.Add(WasTimeOut.ToString()); 
            //results.Add(TimeStamp);
            //results.Add(KeyCode);
            //results.Add(ShownSprite);
            
            return results;
            
        }

    }
}