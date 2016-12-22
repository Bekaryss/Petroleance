// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

namespace Petroleance.Services
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Petroleance.Models;
    using Microsoft.AspNet.SignalR;
    using Petroleance.Hubs;
    using System.Web;
    using Microsoft.AspNet.Identity;
    public class StatisticsService
    {
        private TriviaContext db;

        public StatisticsService(TriviaContext db)
        {
            this.db = db;
        }

        public async Task<StatisticsViewModel> GenerateStatistics()
        {
            string User = HttpContext.Current.User.Identity.GetUserId();
            var correctAnswers = await this.db.TriviaAnswers.CountAsync(a => a.TriviaOption.IsCorrect && a.UserId == User);
            var totalAnswers = await this.db.TriviaAnswers.CountAsync(a => a.UserId == User);
            var totalUsers = (float)await this.db.TriviaAnswers.GroupBy(a => a.UserId == User).CountAsync();

            var incorrectAnswers = totalAnswers - correctAnswers;

            return new StatisticsViewModel
            {
                CorrectAnswers = correctAnswers,
                IncorrectAnswers = incorrectAnswers,
                TotalAnswers = totalAnswers,
                CorrectAnswersAverage = (totalUsers > 0) ? correctAnswers / totalUsers : 0,
                IncorrectAnswersAverage = (totalUsers > 0) ? incorrectAnswers / totalUsers : 0,
                TotalAnswersAverage = (totalUsers > 0) ? totalAnswers / totalUsers : 0,
            };
        }
        public async Task NotifyUpdates()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<StatisticsHub>();
            if (hubContext != null)
            {
                var stats = await this.GenerateStatistics();
                hubContext.Clients.All.updateStatistics(stats);
            }
        }
    }
}