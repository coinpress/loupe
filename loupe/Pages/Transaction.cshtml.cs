using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nethereum.RPC.Eth.DTOs;

namespace loupe.Pages
{
    public class TransactionModel : PageModel
    {
        public Transaction Transaction;
        public void OnGet()
        {
            var query = Request.QueryString.Value.Replace("?", "");
            Utilities.Web3Client w3client = new Utilities.Web3Client();
            Transaction = w3client.GetTransaction(query).GetAwaiter().GetResult();
        }
    }
}