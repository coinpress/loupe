using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nethereum.RPC.Eth.DTOs;

namespace loupe.Pages
{
    public class Last50Model : PageModel
    {
        public List<BlockWithTransactionHashes> BlockList = new List<BlockWithTransactionHashes>();
        Utilities.Web3Client w3client = new Utilities.Web3Client();
        public void OnGet()
        {

            var block = w3client.BlockWithTransactionHashes().GetAwaiter().GetResult();
            int startBlock = Math.Max((int)block.Number.Value - 10, 0);

            BlockList.Clear();
            BlockList.TrimExcess();

            for (int i = startBlock; i < block.Number.Value; i++)
            {
                var returned = w3client.BlockWithTransactionHashes(i).GetAwaiter().GetResult();
                BlockList.Add(returned);
            }
        }
    }
}