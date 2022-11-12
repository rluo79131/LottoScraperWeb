using AngleSharp;
using LottoScraperWeb.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LottoScraperWeb.BLL
{
    public class LottoService
    {
        private readonly LottoContext LottoContext = new LottoContext();

        public List<Information> GetInformationList() => LottoContext.Informations.OrderByDescending(e => e.DrawTerm).ToList();

        public async Task Scraper()
        {
            var informationList = new List<Information>();

            //爬取-樂透資料
            using (var client = new HttpClient())
            {
                var url = "https://www.taiwanlottery.com.tw/Lotto/Lotto649/history.aspx";
                var response = await client.GetAsync(url);
                var html = response.Content.ReadAsStringAsync().Result;
                var browsingContext = BrowsingContext.New(AngleSharp.Configuration.Default);
                var document = await browsingContext.OpenAsync(res => res.Content(html));
                var trTagList = document.QuerySelectorAll("#Lotto649Control_history_dlQuery > tbody > tr").ToList();

                for (var index = 0; index < trTagList.Count; index++)
                {
                    var sNoList = new List<string>();
                    for (var sIndex = 1; sIndex <= 6; sIndex++)
                    {
                        var sNo = document.QuerySelector($"#Lotto649Control_history_dlQuery_SNo{sIndex}_{index}").TextContent;
                        sNoList.Add(sNo);
                    }

                    informationList.Add(new Information()
                    {
                        DrawTerm = document.QuerySelector($"#Lotto649Control_history_dlQuery_L649_DrawTerm_{index}").TextContent,
                        DDate = document.QuerySelector($"#Lotto649Control_history_dlQuery_L649_DDate_{index}").TextContent,
                        EDate = document.QuerySelector($"#Lotto649Control_history_dlQuery_L649_EDate_{index}").TextContent,
                        SellAmount = document.QuerySelector($"#Lotto649Control_history_dlQuery_L649_SellAmount_{index}").TextContent,
                        Total = document.QuerySelector($"#Lotto649Control_history_dlQuery_Total_{index}").TextContent,
                        SNoList = string.Join(",", sNoList),
                        No = Convert.ToByte(document.QuerySelector($"#Lotto649Control_history_dlQuery_SNo_{index}").TextContent),
                    });
                }
            }

            //儲存-樂透資料(不重複儲存)
            foreach (Information data in informationList)
            {
                if (!LottoContext.Informations.Any(e => e.DrawTerm == data.DrawTerm))
                {
                    LottoContext.Informations.Add(data);
                }
            }
            await LottoContext.SaveChangesAsync();
        }
    }
}