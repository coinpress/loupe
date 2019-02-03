using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace loupe.Pages
{
    public class AddressModel : PageModel
    {
        public string Account;
        public decimal AccountBalance;

        public void OnGet()
        {
            var query = Request.QueryString.Value.Replace("?", "");

            if (query.Length == 42)//Verify it's an address sized string that was fed in
            {
                Utilities.Web3Client w3client = new Utilities.Web3Client();
                AccountBalance = w3client.GetAccountBalance(query).GetAwaiter().GetResult();
                Account = query;
            }
        }
    }
}