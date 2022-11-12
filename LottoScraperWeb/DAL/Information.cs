using System;
using System.ComponentModel.DataAnnotations;

namespace LottoScraperWeb.DAL
{
    public class Information
    {
        [Key]
        public string DrawTerm { get; set; }
        public string DDate { get; set; }
        public string EDate { get; set; }
        public string SellAmount { get; set; }
        public string Total { get; set; }
        public string SNoList { get; set; }
        public byte No { get; set; }
    }
}