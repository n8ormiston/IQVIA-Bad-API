using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace BadApiCodingChallenge.Models
{
    public class BadApiIntegrationModel
    {
        private static HttpClient client = new HttpClient();
        public List<TweetModel> AllTweets = new List<TweetModel>();
        private const int MAX_DAYS_TO_ADD = 6;
        private const int MAX_RECORDS_RETURNED = 100;

        public BadApiIntegrationModel()
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void GetTweetsInTimeSpan(DateTime startDate, DateTime endDate)
        {
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);
            List<TweetModel> partialList = new List<TweetModel>();
            DateTime tempStartDate = startDate;
            int daysToAdd = MAX_DAYS_TO_ADD;
            DateTime tempEndDate = startDate.AddDays(daysToAdd).AddSeconds(-1);

            while (tempEndDate <= endDate)
            {
                partialList = GetTweetsInternal(tempStartDate, tempEndDate);

                if (partialList.Count() < MAX_RECORDS_RETURNED)
                {
                    AllTweets.AddRange(partialList);

                    if (tempEndDate == endDate)
                        break;

                    tempStartDate = tempEndDate.AddSeconds(1);
                    tempEndDate = tempEndDate.AddDays(daysToAdd);
                    daysToAdd = MAX_DAYS_TO_ADD;
                }
                else
                {
                    tempEndDate = tempEndDate.AddDays(-1);
                    daysToAdd--;
                }

                if (tempEndDate > endDate)
                    tempEndDate = endDate;
            }

            AllTweets = AllTweets.DistinctBy(t => t.Id).ToList();

        }

        private List<TweetModel> GetTweetsInternal(DateTime startDate, DateTime endDate)
        {
            string startDateUTC = startDate.ToUniversalTime().ToString();
            string endDateUTC = endDate.ToUniversalTime().ToString();
            string requestUrl = $"https://badapi.iqvia.io/api/v1/Tweets?startDate={startDateUTC}&endDate={endDateUTC}";
            string response = client.GetStringAsync(new Uri(requestUrl)).Result;

            List<TweetModel> responseTweets = JsonConvert.DeserializeObject<List<TweetModel>>(response);
            return responseTweets;
        }
    }
}