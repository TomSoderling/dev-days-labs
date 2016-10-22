using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using System.Collections.Generic;
using System.Text;

namespace DevDaysSpeakers
{
    public class EmotionService
    {
        private static async Task<Emotion[]> GetEmotionsAsync(string url)
        {
            var client = new HttpClient();
            var stream = await client.GetStreamAsync(url);
            var emotionClient = new EmotionServiceClient(Config.TOMS_COGNITIVE_EMOTION_SERVICE_KEY);

            var emotionResults = await emotionClient.RecognizeAsync(stream);

            if (emotionResults == null || emotionResults.Count() == 0)
            {
                throw new Exception("Can't detect face");
            }

            return emotionResults;
        }

        // Average happiness calculation in case of multiple people
        public static async Task<float> GetAverageHappinessScoreAsync(string url)
        {
            Emotion[] emotionResults = await GetEmotionsAsync(url);

            float score = 0;
            foreach (var emotionResult in emotionResults)
                score = score + emotionResult.Scores.Happiness;

            return score / emotionResults.Count();
        }


        public static async Task<List<float>> GetAverageEmotionScoresAsync(string url)
        {
            Emotion[] emotionResults = await GetEmotionsAsync(url);

            var happinessScore = 0f;
            foreach (var emotion in emotionResults)
                happinessScore += emotion.Scores.Happiness;

            var sadnessScore = 0f;
            foreach (var emotion in emotionResults)
                sadnessScore += emotion.Scores.Sadness;

            var surpriseScore = 0f;
            foreach (var emotion in emotionResults)
                surpriseScore += emotion.Scores.Surprise;


            var emotionScores = new List<float>();
            var total = emotionResults.Count();

            emotionScores.Add(happinessScore / total);
            emotionScores.Add(sadnessScore / total);
            emotionScores.Add(surpriseScore / total);


            return emotionScores;
        }



        public static string GetHappinessMessage(float score)
        {
            score = score * 100;
            double result = Math.Round(score, 2);

            //if (score >= 50) // not happy enough!
            if (score >= Config.HAPPINESS_LEVEL_THRESHOLD)
                return result + " % :-)";
            else
                return result + " % :-(";
        }


        public static string GetEmotionsMessage(List<float> scores)
        {
            for (var i = 0; i < scores.Count(); i++)
            {
                scores[i] *= 100; // convert to percentage
                scores[i] = (float)Math.Round(scores[i], 2); // round to 2 decimal places
            }

            var str = new StringBuilder();

            if (scores[0] >= Config.HAPPINESS_LEVEL_THRESHOLD)
                str.AppendLine($"Happiness: {scores[0]} % :-)");
            else
                str.AppendLine($"Happiness: {scores[0]} % :-(");

            if (scores[1] >= Config.SADNESS_LEVEL_THRESHOLD)
                str.AppendLine($"Sadness: {scores[1]} % :-(");
            else
                str.AppendLine($"Sadness: {scores[1]} % :-)");

            if (scores[2] >= Config.SURPRISE_LEVEL_THRESHOLD)
                str.AppendLine($"Surprise: {scores[2]} % :-O");
            else
                str.AppendLine($"Surprise: {scores[2]} % :-|");


            return str.ToString();
        }
    }
}
