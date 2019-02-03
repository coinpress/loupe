using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth;
using Nethereum.Web3;
using Nethereum.Hex.HexTypes;
using System.Diagnostics;

namespace loupe.Utilities
{
    public class Web3Client
    {
        //Ethereum MainNet RPC server

        //Web3 web3Connect = new Web3("http://gasprice.poa.network:8545"); 

        //Local instance of a CoinPress coin node set to default RPC port 3907
        //Note that you must change your ServerConfig.json file to allow RPC. I used the following (but beware this opens up most everything)
        //{"AdditionalCommandLineArgs":"--shh --rpc --rpcaddr \"0.0.0.0\" --rpcapi \"db, eth, net, web3, personal\" --rpccorsdomain \"*\"","RPCPort":3907,"CacheSize":128,"DataDirectory":"","Port":3906}

        //Web3 web3Connect = new Web3("http://localhost:3907"); 

        //Note that you must change your ServerConfig.json file to allow RPC. I used the following (but beware this opens up most everything)
        //{"AdditionalCommandLineArgs":"--shh --rpc --rpcaddr \"0.0.0.0\" --rpcapi \"db, eth, net, web3, personal\" --rpccorsdomain \"*\"","RPCPort":3907,"CacheSize":128,"DataDirectory":"","Port":3906}

        //Local instance of a CoinPress wallet set to default port

        Web3 web3Connect = new Web3("http://localhost:8545"); 


        public async Task<BlockWithTransactions> BlockWithTransactions(int requested = -1)
        {
            try
            {
                HexBigInteger blockNo;

                if(requested == -1)
                {
                    blockNo = await web3Connect.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                }
                else
                {
                    var requestedBI = (BigInteger)requested;
                    blockNo = new HexBigInteger(new BigInteger(requested));
                }

                return await web3Connect.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(blockNo);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Updating block info failed: {e}");

                return null;
            }
        }

        public async Task<BlockWithTransactionHashes> BlockWithTransactionHashes(int requested = -1)
        {
            try
            {
                BlockWithTransactionHashes block;

                if (requested == -1)//Send back the current block
                {
                    var blockNo = await web3Connect.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                    block = await web3Connect.Eth.Blocks.GetBlockWithTransactionsHashesByNumber.SendRequestAsync(blockNo);
                }
                else
                {
                    block = await web3Connect.Eth.Blocks.GetBlockWithTransactionsHashesByNumber.SendRequestAsync(new HexBigInteger(new BigInteger(requested)));
                }
                return block;
            }
            catch (Exception)
            {
                Debug.WriteLine("Crashed getting block with transaction hashes");
                return null;
            }
        }

        public async Task<BlockWithTransactions> BlockByHash(string hash)
        {
            try
            {
                return await web3Connect.Eth.Blocks.GetBlockWithTransactionsByHash.SendRequestAsync(hash);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public async Task<string> DeciperTypeOfHash(string hash)
        {
            var trans = await GetTransaction(hash);

            try
            {
                if (!string.IsNullOrEmpty(trans.From))
                {
                    return "Transaction";
                }
            }
            catch (Exception)
            {
                
            }
            return "Block";
        }

        public async Task<decimal> GetAccountBalance(string hash)
        {
            try
            {
                var account = await web3Connect.Eth.GetBalance.SendRequestAsync(hash);
                return Nethereum.Web3.Web3.Convert.FromWei(account.Value);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error getting account balance: {e}");

                return 0;
            }
        }

        public async Task<Transaction> GetTransaction(string hash)
        {
            try
            {
                var transaction = await web3Connect.Eth.Transactions.GetTransactionByHash.SendRequestAsync(hash);

                return transaction;
            }
            catch (Exception)
            {
                return null;
            }
        }


    }
}
