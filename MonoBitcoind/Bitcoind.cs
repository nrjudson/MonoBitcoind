using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MonoBitcoind
{
    class Bitcoind
    {
		private static string _rpcuser = "type-your-username-here";
		private static string _rpcpassword = "type-your-password-here";

		private static bool _print = true;

		private static string qt = "\"";

		private static string BITCOIND_ERROR = "Failed with bitcoind: ";

        private static void PrintIf(string s)
        {
            if (_print)
            {
                System.Console.WriteLine(s);
            }
        }

		private static JObject RequestRaw(string request)
        {
            PrintIf("");
            PrintIf("");
            PrintIf(request);
            PrintIf("");

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://localhost.:8332");
			webRequest.Credentials = new NetworkCredential(_rpcuser, _rpcpassword);
            webRequest.ContentType = "application/json-rpc";
            webRequest.Method = "POST";

            byte[] byteArray = Encoding.UTF8.GetBytes(request);
            webRequest.ContentLength = byteArray.Length;
            Stream dataStream = webRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse webResponse = webRequest.GetResponse();

            string response = null;
            using (StreamReader sr = new StreamReader(webResponse.GetResponseStream()))
            {
                response = sr.ReadToEnd();
            }

            JObject joeResponse = JsonConvert.DeserializeObject<JObject>(response);

            PrintIf("JSON RPC RESPONSE: " + response);

            return joeResponse;
        }


		public static JObject Request(string method, string args)
        {
            string request = "{\"jsonrpc\":\"1.0\",\"id\":\"1\",\"method\":\"" + method + "\",\"params\":[" + args + "]}";
            return RequestRaw(request);
        }


		public static JObject Request(string method)
        {
            string request = "{\"jsonrpc\":\"1.0\",\"id\":\"1\",\"method\":\"" + method + "\"}";
            return RequestRaw(request);
        }


		public static string SimpleStringRequest(string method)
		{
			return SimpleStringRequest (method, null);
		}


		public static string SimpleStringRequest(string method, string parameter, bool surroundParameterWithQuotes = true)
		{
			return SimpleJTokenRequest(method, parameter, surroundParameterWithQuotes).ToString ();
		}


		public static JToken SimpleJTokenRequest(string method)
		{
			return SimpleJTokenRequest (method, null);
		}


		public static JToken SimpleJTokenRequest(string method, string parameter, bool surroundParameterWithQuotes = true)
		{
			JObject result;
			if (parameter == null)
				result = Request (method);
			else {
				if (surroundParameterWithQuotes)
					parameter = qt + parameter + qt;
				result = Request (method, parameter);
			}
			string error = result["error"].ToString ();
			if (error != "")
				throw new Exception (BITCOIND_ERROR + result ["error"].ToString ());
			return result ["result"];
		}


		private static string CheckAndCreateMultiSigArgs(int nRequired, string[] pubKeys)
		{
			if (pubKeys == null || pubKeys.Length == 0) {
				throw new Exception ("Must pass pubKeys!");
			}
			if (nRequired > 15) {
				throw new Exception ("Because PUSHDATA can only push up to 520 bytes on the stack, there's a max of 15 pubKeys to use a multisig address!");
			}
			if (nRequired > pubKeys.Length) {
				throw new Exception ("The number of requried private keys is more than the number of public keys assigned to this address!");
			}

			string arguments = nRequired + ", [";
			for (int i = 0; i < pubKeys.Length; i++) {
				if (i != 0) {
					arguments += ",";
				}
				arguments += qt + pubKeys [i] + qt;
			}
			arguments += "]";
			return arguments;
		}


		private static string ConvertBoolToString(bool value)
		{
			if (value)
				return "true";
			return "false";
		}


		private static string ConvertTxsToString(RawTxInput[] inputs)
		{
			string txInfo = "[";
			bool firstOne = true;
			foreach (RawTxInput input in inputs) {
				if (firstOne)
					firstOne = false;
				else
					txInfo += ",";

				if (input.txID == null || input.txID.Length == 0)
					throw new Exception ("A txID is necessary for input for a new transaction!");
				if (input.vout == null || input.vout.Length == 0)
					throw new Exception ("A vout needs to be specified for which part of the previous transaction's output to use as input!");
				txInfo += "{" + qt + "txid" + qt + ":" + qt + input.txID + qt + "," + qt + "vout" + qt + ":" + input.vout;

				// Optional
				if (input.scriptPubKey != null && input.scriptPubKey.Length > 0)
					txInfo += "," + qt + "scriptPubKey" + qt + ":" + qt + input.scriptPubKey + qt;
				if (input.redeemScript != null && input.redeemScript.Length > 0)
					txInfo += "," + qt + "redeemScript" + qt + ":" + qt + input.redeemScript + qt;

				txInfo += "}";
			}
			txInfo += "]";
			return txInfo;
		}


		private static string ConvertPayToAddressesToString(PayToAddress[] payToAddresses)
		{
			string txOutputInfo = "{";
			bool firstOne = true;
			foreach (PayToAddress payToAddress in payToAddresses) {
				if (firstOne)
					firstOne = false;
				else
					txOutputInfo += ",";

				if (payToAddress.address == null || payToAddress.address.Length == 0)
					throw new Exception ("An address to send the payment to must not be blank!");
				if (payToAddress.amount == null || payToAddress.amount.Length == 0)
					throw new Exception ("A payment amount must not be blank!");
				txOutputInfo += qt + payToAddress.address + qt + ":" + payToAddress.amount;
			}
			txOutputInfo += "}";
			return txOutputInfo;
		}


		private static string ConvertSigHashTypeToString(SigHashType sigHashType)
		{
			if (sigHashType.Equals (SigHashType.All))
				return "ALL";
			if (sigHashType.Equals (SigHashType.None))
				return "NONE";
			if (sigHashType.Equals (SigHashType.Single))
				return "SINGLE";
			if (sigHashType.Equals (SigHashType.AllOrAnyoneCanPay))
				return "ALL|ANYONECANPAY";
			if (sigHashType.Equals (SigHashType.NoneOrAnyoneCanPay))
				return "NONE|ANYONECANPAY";
			if (sigHashType.Equals (SigHashType.SingleOrAnyoneCanPay))
				return "SINGLE|ANYONECANPAY";
			throw new Exception ("SigHashType " + sigHashType.ToString () + " is unsupported in ConvertSigHashTypeToString!");
		}




		////////////////////////////////////////////////////////////////////////////////////////////
		/// All bitcoind methods in alphabetical order
		////////////////////////////////////////////////////////////////////////////////////////////

		/*
		 * Creates a multisig address and adds it to your wallet as an account.
		 * The pubKeys can be addresses or hex-encoded public keys.
		 * Returns the multisig address.
		 * If it already exists as an account, it will overwrite the account name (balance is the same).
		 * Will throw an exception on error.
		 */
		public static string AddMultiSigAddress(int nRequired, string[] pubKeys, string account = null)
		{
			string arguments = CheckAndCreateMultiSigArgs (nRequired, pubKeys);

			if (account != null) {
				arguments += ", " + qt + account + qt;
			}

			//JObject multiSigResult = null;
//			try {
//				multiSigResult = Request("addmultisigaddress", arguments);
//			}
//			catch (Exception e) {
//				throw new Exception ("The request failed, " + e.Message);
//			}
//
//			if (multiSigResult == null) {
//				throw new Exception ("The request failed, the returned object is null.");
//			}

			return SimpleStringRequest ("addmultisigaddress", arguments, false);
		}


		public enum AddNodeAction {
			add,
			remove,
			onetry
		}


		/**
		 * If it returns without throwing an exception, it went well.
		 */
		public static void AddNode(string ipAddress, string port, AddNodeAction type)
		{
			SimpleStringRequest ("addnode", qt + ipAddress + ":" + port + qt + ", " + qt + type.ToString () + qt, false);
		}


		/**
		 * Creates a backup of your wallet at the path specified.
		 * This can be either a directory or a full path including file name.
		 * If it returns without throwing an exception, it went well.
		*/
		public static void BackupWallet(string path)
		{
			SimpleStringRequest ("backupwallet", path);
		}


		public struct CreateMultiSigResult {
			public string address;
			public string redeemScript;
		}

		/*
		 * Creates a multisig address.
		 * The pubKeys can be addresses or hex-encoded public keys.
		 * Returns the CreateMultiSigResult object that contains the address and redeemScript.
		 * Will throw an exception on error.
		 */
		public static CreateMultiSigResult CreateMultiSig(int nRequired, string[] pubKeys)
		{
			string arguments = CheckAndCreateMultiSigArgs(nRequired, pubKeys);

			JToken multiSigResult = SimpleJTokenRequest ("createmultisig", arguments, false);

			CreateMultiSigResult result = new CreateMultiSigResult ();

			result.address = multiSigResult ["address"].ToString ();
			result.redeemScript = multiSigResult ["redeemScript"].ToString ();
			return result;
		}


		public struct PayToAddress {
			public string address;
			public string amount;
		}


		public struct RawTxInput {
			public string txID; // The txID to use for the input of a new transaction
			public string vout; // The index of the previous tx's vout to use, starting from 0.
			public string scriptPubKey; // Necessary for multisig
			public string redeemScript; // Necessary for multisig
		}


		public static string CreateRawTransaction(RawTxInput[] inputs, PayToAddress[] payToAddresses)
		{
			string txInputInfo = ConvertTxsToString (inputs);
			string txOutputInfo = ConvertPayToAddressesToString (payToAddresses);
			return SimpleStringRequest ("createrawtransaction", txInputInfo + ", " + txOutputInfo, false);
		}


		public static JToken DecodeRawTransaction(string rawTxHex)
		{
			return SimpleStringRequest ("decoderawtransaction", rawTxHex);
		}


		public static string DumpPrivKey(string address)
		{
			return SimpleStringRequest ("dumpprivkey", address);
		}


		/**
		 * Encrypts the wallet with 'passphrase'. This is for first time encryption.
		 * After this, any calls that interact with private keys such as sending or signing
		 * will require the passphrase to be set prior the making these calls.
		 * Use the walletpassphrase call for this, and then walletlock call.
		 * If the wallet is already encrypted, use the walletpassphrasechange call.
		 * 
		 * Note that this will shutdown the server!
		 */
		public static void EncryptWallet(string passphrase)
		{
			SimpleJTokenRequest ("encryptwallet", passphrase);
		}


		public static string GetAccount(string address)
		{
			return SimpleStringRequest ("getaccount", address);
		}


		public static string GetAccountAddress(string account)
		{
			return SimpleStringRequest ("getaccountaddress", account);
		}


		public static JToken GetAddedNodeInfo(bool dns, string node = null)
		{
			string arguments = ConvertBoolToString (dns);
			if (node != null)
				arguments += ", " + qt + node + qt;

			return SimpleJTokenRequest ("getaddednodeinfo", arguments, false);
		}


		public static JToken GetAddressesByAccount(string account)
		{
			return SimpleJTokenRequest ("getaddressesbyaccount", account);
		}


		public static string GetBalance(string account, string minimumNumberOfConfirmations = "1")
		{
			return SimpleStringRequest ("getbalance", qt + account + qt + ", " + minimumNumberOfConfirmations, false);
		}


		public static string GetBestBlockHash()
		{
			return SimpleStringRequest ("getbestblockhash");
		}


		public static string GetBlock(string blockHash)
		{
			return SimpleStringRequest ("getblock", blockHash);
		}


		public static string GetBlockCount()
		{
			return SimpleStringRequest ("getblockcount");
		}


		public static string GetBlockHash(string index)
		{
			return SimpleStringRequest ("getblockhash", index, false);
		}


		public static string GetBlockTemplate()
		{
			return null; //////////////////////////////////////////////////////////////////////////////////////////// !!!
		}


		public static string GetConnectionCount()
		{
			return SimpleStringRequest ("getconnectioncount");
		}


		public static string GetDifficulty()
		{
			return SimpleStringRequest ("getdifficulty");
		}


		public static bool GetGenerate()
		{
			string boolStr = SimpleStringRequest ("getgenerate");
			if (boolStr.ToLower () == "false")
				return false;
			else if (boolStr.ToLower () == "true")
				return true;
			throw new Exception ("GetGenerate didn't return a 'false' or 'true'!");
		}


		public static string GetHashesPerSec()
		{
			return SimpleStringRequest ("gethashespersec");
		}


		public static JToken GetInfo()
		{
			return SimpleJTokenRequest ("getinfo");
		}


		public static JToken GetMiningInfo()
		{
			return SimpleJTokenRequest ("getmininginfo");
		}


		public static string GetNewAddress(string account)
		{
			return SimpleStringRequest ("getnewaddress", account);
		}


		public static JToken GetPeerInfo()
		{
			return SimpleJTokenRequest ("getpeerinfo");
		}


		/**
		 * Warning, the wiki says this method is NOT for normal use!
		 */
		public static string GetRawChangeAddress(string account)
		{
			return SimpleStringRequest ("getrawchangeaddress");
		}


		public static JToken GetRawMemPool()
		{
			return SimpleJTokenRequest ("getrawmempool");
		}


		public static JToken GetRawTransaction(string txID, bool showAll = false)
		{
			if (showAll)
				return SimpleJTokenRequest ("getrawtransaction", qt + txID + qt + ", 1", false);
			return SimpleJTokenRequest ("getrawtransaction", txID);
		}


		public static JToken GetReceivedByAccount(string account)
		{
			return SimpleJTokenRequest ("getreceivedbyaccount", account);
		}


		public static string GetReceivedByAddress(string address, string minimumNumberOfConfirmations = "1")
		{
			return SimpleStringRequest ("getreceivedbyaddress", qt + address + qt + ", " + minimumNumberOfConfirmations, false);
		}


		public static JToken GetTransaction(string txID)
		{
			return SimpleJTokenRequest ("gettransaction", txID);
		}


		public static JToken GetTxOut(string txID, string n, bool includeMemPool = true)
		{
			return SimpleJTokenRequest ("gettxout", qt + txID + qt + ", " + n + ", " + ConvertBoolToString (includeMemPool), false);
		}


		public static JToken GetTxOutSetInfo()
		{
			return SimpleJTokenRequest ("gettxoutsetinfo");
		}


		public static JToken GetWork(string data = null)
		{
			if (data == null)
				return SimpleJTokenRequest ("getwork");
			return SimpleJTokenRequest ("getwork", data);
		}


		public static string ImportPrivKey(string privateKey)
		{
			return SimpleStringRequest ("importprivkey", privateKey);
		}


		public static string ImportPrivKey(string privateKey, string label = null, bool rescan = true)
		{
			if (label == null || label == "") {
				// Test that this is for sure the case.
				throw new Exception ("The label for importing a private key cannot be blank, or else your rescan parameter will be the label!");
			}
			return SimpleStringRequest ("importprivkey", qt + privateKey + qt + ", " + qt + label + qt + ", " + ConvertBoolToString(rescan), false);
		}


		public static void KeyPoolRefill()
		{
			Request ("keypoolrefill");
		}


		public static JToken ListAccounts(string minimumNumberOfConfirmations = "1")
		{
			return SimpleJTokenRequest ("listaccounts", minimumNumberOfConfirmations, false);
		}


		public static JToken ListAddressGroupings()
		{
			return SimpleJTokenRequest ("listaddressgroupings");
		}


		public static JToken ListReceivedByAccount(string minimumNumberOfConfirmations = "1", bool includeEmpty = false)
		{
			return SimpleJTokenRequest ("listreceivedbyaccount", minimumNumberOfConfirmations + ", " + ConvertBoolToString(includeEmpty), false);
		}


		public static JToken ListReceivedByAddress(string minimumNumberOfConfirmations = "1", bool includeEmpty = false)
		{
			return SimpleJTokenRequest ("listreceivedbyaddress", minimumNumberOfConfirmations + ", " + ConvertBoolToString(includeEmpty), false);
		}


		public static JToken ListSinceBlock(string blockHash = null, string targetConfirmations = null)
		{
			if (targetConfirmations != null && blockHash == null)
				throw new Exception ("If blockHash is null, so must be targetConfirmations!");
			if (blockHash == null)
				return SimpleJTokenRequest ("listsinceblock");
			if (targetConfirmations == null)
				return SimpleJTokenRequest ("listsinceblock", blockHash);
			return SimpleJTokenRequest ("listsinceblock", qt + blockHash + qt + ", " + targetConfirmations, false);
		}


		/**
		 * Returns up to [count] most recent transactions skipping the first [from] transactions for account [account]. 
		 * If [account] not provided it'll return recent transactions from all accounts.
		 */
		public static JToken ListTransactions(string account = null, string count = "10", string from = "0")
		{
			if (account == null)
				return SimpleJTokenRequest ("listtransactions");
			return SimpleJTokenRequest ("listtransactions", qt + account + qt + ", " + count + ", " + from, false);
		}


		public static JToken ListUnspent(string minimumNumberOfConfirmations = "1", string maximumConfirmations = "999999")
		{
			return SimpleJTokenRequest ("listunspent", minimumNumberOfConfirmations + ", " + maximumConfirmations, false);
		}


		public static JToken ListLockUnspent()
		{
			return SimpleJTokenRequest ("listlockunspent");
		}


		public static bool LockUnspent(bool toUnlock, RawTxInput[] lockThese = null)
		{
			string result;
			if (lockThese == null)
				result = SimpleStringRequest ("lockunspent", ConvertBoolToString (toUnlock), false);
			else {
				string txs = ConvertTxsToString (lockThese);
				result = SimpleStringRequest ("lockunspent", ConvertBoolToString (toUnlock) + ", " + txs, false);
			}
			if (result.ToLower () == "true")
				return true;
			else
				return false;
		}


		public static string Move(string fromAccount, string toAccount, string amount, string minimumNumberOfConfirmations = "1", string comment = null)
		{
			if (comment == null)
				return SimpleStringRequest ("move", qt + fromAccount + qt + ", " + qt + toAccount + qt + ", " + amount + ", " + minimumNumberOfConfirmations, false);
			return SimpleStringRequest ("move", qt + fromAccount + qt + ", " + qt + toAccount + qt + ", " + amount + ", " + minimumNumberOfConfirmations + ", " + qt + comment + qt, false);
		}


		public static string SendFrom(string fromAccount, string toAddress, string amount, string minimumNumberOfConfirmations = "1", string comment = null, string commentTo = null)
		{
			if (comment == null && commentTo != null)
				throw new Exception ("Comment can't be null when commentTo is not null!");
			if (comment != null && commentTo != null)
				return SimpleStringRequest ("sendfrom", qt + fromAccount + qt + ", " + qt + toAddress + qt + ", " + amount 
						+ ", " + minimumNumberOfConfirmations + ", " + qt + comment + qt + ", " + qt + commentTo + qt, false);
			if (comment != null && comment == null)
				return SimpleStringRequest ("sendfrom", qt + fromAccount + qt + ", " + qt + toAddress + qt + ", " + amount 
						+ ", " + minimumNumberOfConfirmations + ", " + qt + comment + qt, false);
			return SimpleStringRequest ("sendfrom", qt + fromAccount + qt + ", " + qt + toAddress + qt + ", " + amount + ", " + minimumNumberOfConfirmations, false);
		}


		public static string SendMany(string fromAccount, PayToAddress[] payToAddresses, string minimumNumberOfConfirmations = "1", string comment = null)
		{
			string txOutputInfo = ConvertPayToAddressesToString (payToAddresses);
			if (comment == null || comment.Length == 0)
				return SimpleStringRequest ("sendmany", qt + fromAccount + qt + ", " + txOutputInfo + ", " + minimumNumberOfConfirmations, false);
			return SimpleStringRequest ("sendmany", qt + fromAccount + qt + ", " + txOutputInfo + ", " + minimumNumberOfConfirmations + ", " + qt + comment + qt, false);
		}


		public static string SendRawTransaction(string rawTxHex)
		{
			return SimpleStringRequest ("sendrawtransaction", rawTxHex);
		}


		public static string SendToAddress(string address, string amount, string comment = null, string commentTo = null)
		{
			if (comment == null && commentTo != null)
				throw new Exception ("Comment can't be null when commentTo is not null!");
			if (comment != null && commentTo != null)
				return SimpleStringRequest ("sendtoaddress", qt + address + qt + ", " + amount + ", " + qt + comment + qt + ", " + qt + commentTo + qt, false);
			if (comment != null && comment == null)
				return SimpleStringRequest ("sendtoaddress", qt + address + qt + ", " + amount + ", " + qt + comment + qt, false);
			return SimpleStringRequest ("sendtoaddress", qt + address + qt + ", " + amount, false);
		}


		public static void SetAccount(string address, string account)
		{
			SimpleStringRequest ("setaccount", qt + address + qt + ", " + qt + account + qt, false);
		}


		/*
		 * <generate> is true or false to turn generation on or off.
		 * Generation is limited to [genproclimit] processors, -1 is unlimited.
		 * Note: in -regtest mode, genproclimit controls how many blocks are generated immediately.
		 */
		public static void SetGenerate(bool generate, string genproclimit = null)
		{
			if (genproclimit == null)
				SimpleStringRequest ("setgenerate", ConvertBoolToString(generate), false);
			else
				SimpleStringRequest ("setgenerate", ConvertBoolToString(generate) + ", " + genproclimit, false);
		}


		public static bool SetTxFee(string amount)
		{
			string result = SimpleStringRequest ("settxfee", amount, false);
			if (result.ToLower () == "true")
				return true;
			return false;
		}


		/*
		 * Only works for addresses in your wallet.
		 */
		public static string SignMessage(string address, string message)
		{
			return SimpleStringRequest ("signmessage", qt + address + qt + ", " + qt + message + qt, false);
		}
			

		public enum SigHashType
		{
			All,
			None,
			Single,
			AllOrAnyoneCanPay,
			NoneOrAnyoneCanPay,
			SingleOrAnyoneCanPay
		}


		public static string SignRawTransaction(String txHex, RawTxInput[] inputs = null, string[] privateKeys = null, SigHashType sigHashType = SigHashType.All)
		{
			if (inputs == null)
				return SimpleStringRequest ("signrawtransaction", qt + txHex + qt, false);

			string inputInfo = ConvertTxsToString (inputs);
			if (privateKeys == null)
				return SimpleStringRequest ("signrawtransaction", qt + txHex + qt + ", " + inputInfo, false);

			string privateKeysFormatted = "[";
			bool firstOne = true;
			foreach (string key in privateKeys) {
				if (firstOne)
					firstOne = false;
				else
					privateKeysFormatted += ",";
				privateKeysFormatted += qt + key + qt;
			}
			privateKeysFormatted += "]";
			return SimpleStringRequest ("signrawtransaction", qt + txHex + qt + ", " + inputInfo + ", " + privateKeysFormatted + ", " + qt + ConvertSigHashTypeToString(sigHashType) + qt, false);
		}


		public static void Stop()
		{
			Request ("stop");
		}


		public static string SubmitBlock()
		{
			return null;
		}


		public static JToken ValidateAddress(string address)
		{
			return SimpleJTokenRequest ("validateaddress", address);
		}


		public static bool VerifyMessage(string address, string signature, string message)
		{
			string result = SimpleStringRequest ("verifymessage", qt + address + qt + ", " + qt + signature + qt + ", " + qt + message + qt, false);
			if (result.ToLower () == "true")
				return true;
			return false;
		}


		/**
		 * Relocks a wallet
		 * Must call EncryptWallet first?
		 */
		public static void WalletLock()
		{
			SimpleStringRequest("walletlock");
		}


		/**
		 * Unlock a wallet for timeout seconds
		 */
		public static void WalletPassPhrase(string passPhrase, string timeout)
		{
			SimpleStringRequest("walletpassphrase", qt + passPhrase + qt + ", " + timeout, false);
		}


		/*
		 * Changes the wallet passphrase
		 */
		public static void WalletPassPhraseChange(string oldPassPhrase, string newPassPhrase)
		{
			SimpleStringRequest("walletpassphrasechange", qt + oldPassPhrase + qt + ", " + qt + newPassPhrase + qt, false);
		}
    }
}
