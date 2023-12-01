using ExcelApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ImportTxt(IFormFile file)
        {
            // 註冊 Big5 編碼提供者
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "請選擇 TXT 檔案。";
                return View("Index");
            }

            List<Lm01dModelClass> records = new List<Lm01dModelClass>();

            using (var reader = new StreamReader(file.OpenReadStream(), Encoding.GetEncoding("big5")))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var fields = line.Split(';');

                    // 確認欄位數量符合預期
                    if (fields.Length >= 9)
                    {
                        var record = new Lm01dModelClass
                        {
                            Number = fields[0],
                            Id = fields[1],
                            Ldate = fields[2],
                            Edate = fields[3],
                            Vaccount = fields[4],
                            Totalamount = fields[5],
                            Totalbalance = fields[6],
                            Yesbalance = fields[7],
                            Nobalance = fields[8],

                        };

                        records.Add(record);
                    }
                    else
                    {
                        // 欄位數量不足，可能需要進行錯誤處理或日誌記錄
                        // 在這裡可以添加相應的處理邏輯
                    }
                }
            }

            ViewBag.Records = records;
            ViewBag.Message = $"成功匯入 {records.Count} 筆資料。";

            return View("Index");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
