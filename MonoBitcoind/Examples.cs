using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MonoBitcoind
{
	public class Examples
	{
		private static string _rawTx1To1;
		private static string _rawTx1To2;
		private static string _rawTx2To1;
		private static string _rawTx2To2;

		private static string _helloSigned;
		private static string _genesisSigned;

		private static string _walletPassPhrase = "MyThuperThecurePathword";
		private static string _walletPassPhraseTimeout = "300"; // 5 minutes

		public static void RunExamples()
		{
			Bitcoind.WalletPassPhrase (_walletPassPhrase, _walletPassPhraseTimeout);

			if (!AddMultiSigAddress ())
				Console.WriteLine ("Failed AddMultiSigAddress tests!");
			if (!AddNode ())
				Console.WriteLine ("Failed AddNode tests!");
			if (!BackupWallet ())
				Console.WriteLine ("Failed BackupWallet tests!");
			if (!CreateMultiSig ())
				Console.WriteLine ("Failed CreateMultiSig tests!");
			if (!CreateRawTransaction ())
				Console.WriteLine ("Failed CreateRawTransaction tests!");
			if (!DecodeRawTransaction ())
				Console.WriteLine ("Failed DecodeRawTransaction tests!");
			if (!DumpPrivKey ())
				Console.WriteLine ("Failed DumpPrivKey tests!");
//			if (!EncryptWallet ())
//				Console.WriteLine ("Failed EncryptWallet tests!");
			if (!GetAccount ())
				Console.WriteLine ("Failed GetAccount tests!");
			if (!GetAddedNodeInfo ())
				Console.WriteLine ("Failed GetAddedNodeInfo!");
			if (!GetBalance ())
				Console.WriteLine ("Failed GetBalance!");
			if (!GetBestBlockHash ())
				Console.WriteLine ("Failed GetBestBlockHash!");
			if (!GetBlock ())
				Console.WriteLine ("Failed GetBlock!");
			if (!GetBlockCount ())
				Console.WriteLine ("Failed GetBlockCount!");
			if (!GetBlockHash ())
				Console.WriteLine ("Failed GetBlockHash!");
			if (!GetBlockTemplate ())
				Console.WriteLine ("Failed GetBlockTemplate!");
			if (!GetConnectionCount ())
				Console.WriteLine ("Failed GetConnectionCount!");
			if (!GetDifficulty ())
				Console.WriteLine ("Failed GetDifficulty!");
			if (!GetGenerate ())
				Console.WriteLine ("Failed GetGenerate!");
			if (!GetHashesPerSec ())
				Console.WriteLine ("Failed GetHashesPerSec!");
			if (!GetInfo ())
				Console.WriteLine ("Failed GetInfo!");
			if (!GetMiningInfo ())
				Console.WriteLine ("Failed GetMiningInfo!");
			if (!GetNewAddress ())
				Console.WriteLine ("Failed GetNewAddress!");
			if (!GetPeerInfo ())
				Console.WriteLine ("Failed GetPeerInfo!");
			if (!GetRawChangeAddress ())
				Console.WriteLine ("Failed GetRawChangeAddress!");
			if (!GetRawMemPool ())
				Console.WriteLine ("Failed GetRawMemPool!");
			if (!GetRawTransaction ())
				Console.WriteLine ("Failed GetRawTransaction!");
			if (!GetReceivedByAccount ())
				Console.WriteLine ("Failed GetReceivedByAccount!");
			if (!GetReceivedByAddress ())
				Console.WriteLine ("Failed GetReceivedByAddress!");
			if (!GetTransaction ())
				Console.WriteLine ("Failed GetTransaction!");
			if (!GetTxOut ())
				Console.WriteLine ("Failed GetTxOut!");
			if (!GetTxOutSetInfo ())
				Console.WriteLine ("Failed GetTxOutSetInfo!");
			if (!GetWork ())
				Console.WriteLine ("Failed GetWork!");
			if (!ImportPrivKey ())
				Console.WriteLine ("Failed ImportPrivKey!");
			if (!KeyPoolRefill ())
				Console.WriteLine ("Failed KeyPoolRefill!");
			if (!ListAccounts ())
				Console.WriteLine ("Failed ListAccounts!");
			if (!ListAddressGroupings ())
				Console.WriteLine ("Failed ListAddressGroupings!");
			if (!ListReceivedByAccount ())
				Console.WriteLine ("Failed ListReceivedByAccount!");
			if (!ListReceivedByAddress ())
				Console.WriteLine ("Failed ListReceivedByAddress!");
			if (!ListSinceBlock ())
				Console.WriteLine ("Failed ListSinceBlock!");
			if (!ListTransactions ())
				Console.WriteLine ("Failed ListTransactions!");
			if (!ListUnspent ())
				Console.WriteLine ("Failed ListUnspent!");
			if (!ListLockUnspent ())
				Console.WriteLine ("Failed ListLockUnspent!");
			if (!LockUnspent ())
				Console.WriteLine ("Failed LockUnspent!");
			if (!Move ())
				Console.WriteLine ("Failed Move!");
			if (!SendFrom ())
				Console.WriteLine ("Failed SendFrom!");
			if (!SendMany ())
				Console.WriteLine ("Failed SendMany!");
			if (!SendRawTransaction ())
				Console.WriteLine ("Failed SendRawTransaction!");
			if (!SendToAddress ())
				Console.WriteLine ("Failed SendToAddress!");
			if (!SetAccount ())
				Console.WriteLine ("Failed SetAccount!");
			if (!SetGenerate ())
				Console.WriteLine ("Failed SetGenerate!");
			if (!SetTxFee ())
				Console.WriteLine ("Failed SetTxFee!");
			if (!SignMessage ())
				Console.WriteLine ("Failed SignMessage!");
			if (!SignRawTransaction ())
				Console.WriteLine ("Failed SignRawTransaction!");
			if (!Stop ())
				Console.WriteLine ("Failed Stop!");
			if (!SubmitBlock ())
				Console.WriteLine ("Failed SubmitBlock!");
			if (!ValidateAddress ())
				Console.WriteLine ("Failed ValidateAddress!");
			if (!VerifyMessage ())
				Console.WriteLine ("Failed VerifyMessage!");
			if (!WalletLock ())
				Console.WriteLine ("Failed WalletLock!");
			if (!WalletPassPhrase ())
				Console.WriteLine ("Failed WalletPassPhrase!");
			if (!WalletPassPhraseChange ())
				Console.WriteLine ("Failed WalletPassPhraseChange!");
		}


		public static bool AddMultiSigAddress()
		{
			Console.WriteLine ("Starting AddMultiSigAddress tests...");
			// Note that you can use pubKeys or addresses.
			string[] pubKeys3 = {"03be117fab1593c1f5aa43353df54a0920fc699f089e872f36a40599688f566a47",
				"02558f523c7b6c97e10d89348c9b93b7ee7d860f62027f932eb150d4ba6e41c278",
				"032973a0a39b810f1b3b351241e1ebe71ab21159e2068ddc352bb19d1c4f6f3d95"
			};
			string address = Bitcoind.AddMultiSigAddress (2, pubKeys3);//, "testMultiSigAcct1");
			if (address.Length != 35) {
				Console.WriteLine ("(addMultiSig - No acct) Invalid address: " + address);
				return false;
			}
			address = Bitcoind.AddMultiSigAddress (2, pubKeys3, "testMultiSigAcct1");
			if (address.Length != 35) {
				Console.WriteLine ("(addMultiSig - With acct) Invalid address: " + address);
				return false;
			}
			return true;
		}


		public static bool AddNode()
		{
			Console.WriteLine ("Starting AddNode tests...");
			// Make sure this node is not already added
			//Bitcoind.AddNode ("67.212.92.167", "8333", Bitcoind.AddNodeAction.remove);
			Bitcoind.AddNode ("67.212.92.167", "8333", Bitcoind.AddNodeAction.add);
			Bitcoind.AddNode ("67.212.92.167", "8333", Bitcoind.AddNodeAction.remove);
			Bitcoind.AddNode ("67.212.92.167", "8333", Bitcoind.AddNodeAction.onetry);
			return true;
		}


		public static bool BackupWallet()
		{
			Console.WriteLine ("Starting BackupWallet tests...");
			Bitcoind.BackupWallet ("C:/Test/backup.dat");
			Bitcoind.BackupWallet ("C:/Test/backup.dat.ass");
			return true;
		}


		public static bool CreateMultiSig()
		{
			Console.WriteLine ("Starting CreateMultiSig tests...");
			// Note that you can use pubKeys or addresses.
			string[] pubKeys3 = {"mpNT4J4PEpTFab68QvH7tMQ8hYS4JhunFK",
				"n3ZGGJ1Mhqq39LUaP2umiqnZu7GVpqVh1r",
				"mtMS6D2y4wbEiKZa6qJuXbibnQ26HdtY7J"
			};
			Bitcoind.CreateMultiSigResult multiSig1 = Bitcoind.CreateMultiSig (2, pubKeys3);
			if (multiSig1.address.Length != 35)
				return false;
			if (multiSig1.redeemScript.Length < 40)
				return false;
			Bitcoind.CreateMultiSigResult multiSig2 = Bitcoind.CreateMultiSig (3, pubKeys3);
			if (multiSig2.address.Length != 35)
				return false;
			if (multiSig2.redeemScript.Length < 40)
				return false;
			return true;
		}


		public static bool CreateRawTransaction()
		{
			Console.WriteLine ("Starting CreateRawTransaction tests...");

			// NOTE: scriptPubKey and redeemScript are not being tested in this case.

			Bitcoind.RawTxInput[] inputs1 = new Bitcoind.RawTxInput[1];
			Bitcoind.RawTxInput input1 = new Bitcoind.RawTxInput ();
			input1.txID = "fff17832d351c12476dc3d6f902fd2e0666992135ffe49ac343d9db15dfcddf7";
			input1.vout = "0";
			inputs1 [0] = input1;

			Bitcoind.RawTxInput[] inputs2 = new Bitcoind.RawTxInput[2];
			Bitcoind.RawTxInput input2 = new Bitcoind.RawTxInput ();
			input2.txID = "e69899e5bd2020b99c67b258e5d56ca5c091c9b065f412381ce44a9e438721c1";
			input2.vout = "1";
			inputs2 [0] = input1;
			inputs2 [1] = input2;

			Bitcoind.PayToAddress[] outputs1 = new Bitcoind.PayToAddress[1];
			Bitcoind.PayToAddress output1 = new Bitcoind.PayToAddress ();
			output1.address = "mjkTKcq27UHMoydoQPrq74fCgyBZ5DCAwf";
			output1.amount = "1.5";
			outputs1 [0] = output1;

			Bitcoind.PayToAddress[] outputs2 = new Bitcoind.PayToAddress[2];
			Bitcoind.PayToAddress output2 = new Bitcoind.PayToAddress ();
			output2.address = "miKXJCSnAYyMNPX1emfd7QGXdCFTJRm9Bi";
			output2.amount = "10.123456";
			outputs2 [0] = output1;
			outputs2 [1] = output2;

			// Test 1 tx with 1 output
			_rawTx1To1 = Bitcoind.CreateRawTransaction (inputs1, outputs1);
			if (_rawTx1To1 == null || _rawTx1To1.Length == 0)
				return false;
			// Test 1 tx with 2 output
			_rawTx1To2 = Bitcoind.CreateRawTransaction (inputs1, outputs2);
			if (_rawTx1To2 == null || _rawTx1To2.Length == 0)
				return false;
			// Test 2 tx with 1 output
			_rawTx2To1 = Bitcoind.CreateRawTransaction (inputs2, outputs1);
			if (_rawTx2To1 == null || _rawTx2To1.Length == 0)
				return false;
			// Test 2 tx with 2 output
			_rawTx2To2 = Bitcoind.CreateRawTransaction (inputs2, outputs2);
			if (_rawTx2To2 == null || _rawTx2To2.Length == 0)
				return false;
			return true;
		}


		public static bool DecodeRawTransaction()
		{
			Console.WriteLine ("Starting DecodeRawTransaction tests...");

			// Note that right now, CreateRawTransaction in this class needs to be called first to fill the rawTx strings.

			Bitcoind.DecodeRawTransaction (_rawTx1To1);
			Bitcoind.DecodeRawTransaction (_rawTx1To2);
			Bitcoind.DecodeRawTransaction (_rawTx2To1);
			Bitcoind.DecodeRawTransaction (_rawTx2To2);

			return true;
		}


		public static bool DumpPrivKey()
		{
			Console.WriteLine ("Starting DumpPrivKey tests...");

			Bitcoind.DumpPrivKey ("n1dPhwHRC8jNwUEJD7jThf4RkFVvcYLqmz");
			Bitcoind.DumpPrivKey ("msvqm9b8qV3cFLGXDifiEpPunZunwKwDaZ");
			Bitcoind.DumpPrivKey ("mmx8vJ6F4sBcBuhbEF1UAGevAMmyaSfLxx");

			return true;
		}


		public static bool EncryptWallet()
		{
			Console.WriteLine ("Starting DumpPrivKey tests...");

			Bitcoind.EncryptWallet (_walletPassPhrase); // Locks
			Bitcoind.WalletPassPhrase (_walletPassPhrase, _walletPassPhraseTimeout); // Unlocks

			return true;
		}


		public static bool GetAccount()
		{
			Console.WriteLine ("Starting GetAccount tests...");

			Bitcoind.GetAccount ("n1dPhwHRC8jNwUEJD7jThf4RkFVvcYLqmz");
			Bitcoind.GetAccount ("msvqm9b8qV3cFLGXDifiEpPunZunwKwDaZ");
			Bitcoind.GetAccount ("mmx8vJ6F4sBcBuhbEF1UAGevAMmyaSfLxx");

			return true;
		}


		public static bool GetAddedNodeInfo()
		{
			Console.WriteLine ("Starting GetAddedNodeInfo tests...");

			Bitcoind.GetAddedNodeInfo (false);
			Bitcoind.GetAddedNodeInfo (true);

			Bitcoind.AddNode ("212.8.63.158", "8546", Bitcoind.AddNodeAction.add);

			Bitcoind.GetAddedNodeInfo (false, "212.8.63.158:8546");
			Bitcoind.GetAddedNodeInfo (true, "212.8.63.158:8546");

			Bitcoind.AddNode ("212.8.63.158", "8546", Bitcoind.AddNodeAction.remove);

			return true;
		}


		public static bool GetBalance()
		{
			Console.WriteLine ("Starting GetBalance tests...");

			Console.WriteLine("GetBalance: " + Bitcoind.GetBalance ("cs"));
			Console.WriteLine("GetBalance: " + Bitcoind.GetBalance ("cs", "6"));

			return true;
		}


		public static bool GetBestBlockHash()
		{
			Console.WriteLine ("Starting GetBestBlockHash tests...");

			Console.WriteLine("GetBestBlockHash: " + Bitcoind.GetBestBlockHash ());

			return true;
		}


		public static bool GetBlock()
		{
			Console.WriteLine ("Starting GetBlock tests...");

			Console.WriteLine("GetBlock: " + Bitcoind.GetBlock ("00008a3534145aad5d0dfd04cb0da07e8d87eeb4dfc7f54680a05f0f75094922"));

			return true;
		}


		public static bool GetBlockCount()
		{
			Console.WriteLine ("Starting GetBlockCount tests...");

			Console.WriteLine("GetBlockCount: " + Bitcoind.GetBlockCount ());

			return true;
		}


		public static bool GetBlockHash()
		{
			Console.WriteLine ("Starting GetBlockHash tests...");

			Console.WriteLine("GetBlockHash: " + Bitcoind.GetBlockHash ("0"));

			return true;
		}


		public static bool GetBlockTemplate()
		{
			Console.WriteLine ("Starting GetBlockTemplate tests...");

			//////////////////////////////////////////////////////////////////////////////////////////////////// !!!

			return true;
		}


		public static bool GetConnectionCount()
		{
			Console.WriteLine ("Starting GetConnectionCount tests...");

			Console.WriteLine("ConnectionCount: " + Bitcoind.GetConnectionCount ());

			return true;
		}


		public static bool GetDifficulty()
		{
			Console.WriteLine ("Starting GetDifficulty tests...");

			Console.WriteLine("Difficulty: " + Bitcoind.GetDifficulty ());

			return true;
		}


		public static bool GetGenerate()
		{
			Console.WriteLine ("Starting GetGenerate tests...");

			Console.WriteLine("GetGenerate: " + Bitcoind.GetGenerate ());

			return true;
		}


		public static bool GetHashesPerSec()
		{
			Console.WriteLine ("Starting GetHashesPerSec tests...");

			Console.WriteLine("GetHashesPerSec: " + Bitcoind.GetHashesPerSec ());

			return true;
		}


		public static bool GetInfo()
		{
			Console.WriteLine ("Starting GetInfo tests...");

			Console.WriteLine("GetInfo: " + Bitcoind.GetInfo ());

			return true;
		}


		public static bool GetMiningInfo()
		{
			Console.WriteLine ("Starting GetMiningInfo tests...");

			Console.WriteLine("GetMiningInfo: " + Bitcoind.GetMiningInfo ());

			return true;
		}


		public static bool GetNewAddress()
		{
			Console.WriteLine ("Starting GetNewAddress tests...");

			Console.WriteLine("GetNewAddress: " + Bitcoind.GetNewAddress ("cs"));

			return true;
		}


		public static bool GetPeerInfo()
		{
			Console.WriteLine ("Starting GetPeerInfo tests...");

			Console.WriteLine("GetPeerInfo: " + Bitcoind.GetPeerInfo ());

			return true;
		}


		public static bool GetRawChangeAddress()
		{
			Console.WriteLine ("Starting GetRawChangeAddress tests...");

			Console.WriteLine("GetRawChangeAddress: " + Bitcoind.GetRawChangeAddress ("cs"));

			return true;
		}


		public static bool GetRawMemPool()
		{
			Console.WriteLine ("Starting GetRawMemPool tests...");

			Console.WriteLine("GetRawMemPool: " + Bitcoind.GetRawMemPool ());

			return true;
		}


		public static bool GetRawTransaction()
		{
			Console.WriteLine ("Starting GetRawTransaction tests...");

			Console.WriteLine ("GetRawTransaction 1: " + Bitcoind.GetRawTransaction ("fff17832d351c12476dc3d6f902fd2e0666992135ffe49ac343d9db15dfcddf7"));
			Console.WriteLine ("GetRawTransaction 2 verbose: " + Bitcoind.GetRawTransaction ("e69899e5bd2020b99c67b258e5d56ca5c091c9b065f412381ce44a9e438721c1", true));

			return true;
		}


		public static bool GetReceivedByAccount()
		{
			Console.WriteLine ("Starting GetReceivedByAccount tests...");

			Console.WriteLine("GetReceivedByAccount: " + Bitcoind.GetReceivedByAccount ("cs"));

			return true;
		}


		public static bool GetReceivedByAddress()
		{
			Console.WriteLine ("Starting GetReceivedByAddress tests...");

			Console.WriteLine("GetReceivedByAddress: " + Bitcoind.GetReceivedByAddress ("mhm2QuUhVaNMFcehWN5P9KiSNh1QxVunKk"));
			Console.WriteLine("GetReceivedByAddress: " + Bitcoind.GetReceivedByAddress ("mhm2QuUhVaNMFcehWN5P9KiSNh1QxVunKk", "6"));

			return true;
		}


		public static bool GetTransaction()
		{
			Console.WriteLine ("Starting GetTransaction tests...");

			Console.WriteLine("GetTransaction: " + Bitcoind.GetTransaction ("fff17832d351c12476dc3d6f902fd2e0666992135ffe49ac343d9db15dfcddf7"));
			Console.WriteLine("GetTransaction: " + Bitcoind.GetTransaction ("e69899e5bd2020b99c67b258e5d56ca5c091c9b065f412381ce44a9e438721c1"));

			return true;
		}


		public static bool GetTxOut()
		{
			Console.WriteLine ("Starting GetTxOut tests...");

			Console.WriteLine("GetTxOut: " + Bitcoind.GetTxOut ("fff17832d351c12476dc3d6f902fd2e0666992135ffe49ac343d9db15dfcddf7", "0"));
			Console.WriteLine("GetTxOut: " + Bitcoind.GetTxOut ("fff17832d351c12476dc3d6f902fd2e0666992135ffe49ac343d9db15dfcddf7", "0"));

			return true;
		}


		public static bool GetTxOutSetInfo()
		{
			Console.WriteLine ("Starting GetTxOutSetInfo tests...");

			Console.WriteLine("GetTxOutSetInfo: " + Bitcoind.GetTxOutSetInfo ());

			return true;
		}


		public static bool GetWork()
		{
			Console.WriteLine ("Starting GetWork tests...");

			////////////////////////////////////////////////////////////// Must be connected to real bitcoin network?

//			JToken work = Bitcoind.GetWork ();
//			string data = work ["data"].ToString ();
//			Console.WriteLine("GetWork: " + work.ToString ());
//
//			JToken dataResult = Bitcoind.GetWork ();
//			Console.WriteLine("GetWorkOnData: " + dataResult.ToString ());

			return true;
		}


		public static bool ImportPrivKey()
		{
			Console.WriteLine ("Starting ImportPrivKey tests...");

			// Test when there's a rescan and not a label!
			Console.WriteLine("ImportPrivKey: " + Bitcoind.ImportPrivKey ("cToctw27wEp97fUzQ1VHih4qAzyAkp2Nq8MiQYABbnpkm36f6m1F"));
			ListAccounts ();
			Console.WriteLine("ImportPrivKey: " + Bitcoind.ImportPrivKey ("cToctw27wEp97fUzQ1VHih4qAzyAkp2Nq8MiQYABbnpkm36f6m1F", "test"));
			ListAccounts ();
			Console.WriteLine("ImportPrivKey: " + Bitcoind.ImportPrivKey ("cToctw27wEp97fUzQ1VHih4qAzyAkp2Nq8MiQYABbnpkm36f6m1F", "test2", false));
			ListAccounts ();
			Console.WriteLine("ImportPrivKey: " + Bitcoind.ImportPrivKey ("cToctw27wEp97fUzQ1VHih4qAzyAkp2Nq8MiQYABbnpkm36f6m1F", "cs2", true));
			ListAccounts ();

			return true;
		}


		public static bool KeyPoolRefill()
		{
			Console.WriteLine ("Starting KeyPoolRefill tests...");

			Bitcoind.KeyPoolRefill ();

			return true;
		}


		public static bool ListAccounts()
		{
			Console.WriteLine ("Starting ListAccounts tests...");

			Console.WriteLine("ListAccounts: " + Bitcoind.ListAccounts ());
			Console.WriteLine("ListAccounts with 50 confirmations: " + Bitcoind.ListAccounts ("50"));

			return true;
		}


		public static bool ListAddressGroupings()
		{
			Console.WriteLine ("Starting ListAddressGroupings tests...");

			Console.WriteLine("ListAddressGroupings: " + Bitcoind.ListAddressGroupings ());

			return true;
		}


		public static bool ListReceivedByAccount()
		{
			Console.WriteLine ("Starting ListReceivedByAccount tests...");

			Console.WriteLine("ListReceivedByAccount: " + Bitcoind.ListReceivedByAccount ());
			Console.WriteLine("ListReceivedByAccount with 50 confirmations: " + Bitcoind.ListReceivedByAccount ("50"));
			Console.WriteLine("ListReceivedByAccount with 50 confirmations showing empties: " + Bitcoind.ListReceivedByAccount ("50", true));

			return true;
		}


		public static bool ListReceivedByAddress()
		{
			Console.WriteLine ("Starting ListReceivedByAddress tests...");

			Console.WriteLine("ListReceivedByAddress: " + Bitcoind.ListReceivedByAddress ());
			Console.WriteLine("ListReceivedByAddress with 50 confirmations: " + Bitcoind.ListReceivedByAddress ("50"));
			Console.WriteLine("ListReceivedByAddress with 50 confirmations showing empties: " + Bitcoind.ListReceivedByAddress ("50", true));

			return true;
		}


		public static bool ListSinceBlock()
		{
			Console.WriteLine ("Starting ListSinceBlock tests...");

			Console.WriteLine("ListSinceBlock: " + Bitcoind.ListSinceBlock ());
			Console.WriteLine("ListSinceBlock: " + Bitcoind.ListSinceBlock ("0f9188f13cb7b2c71f2a335e3a4fc328bf5beb436012afca590b1a11466e2206"));
			Console.WriteLine("ListSinceBlock: " + Bitcoind.ListSinceBlock ("0f9188f13cb7b2c71f2a335e3a4fc328bf5beb436012afca590b1a11466e2206", "50"));

			return true;
		}


		public static bool ListTransactions()
		{
			Console.WriteLine ("Starting ListTransactions tests...");

			Console.WriteLine("ListTransactions: " + Bitcoind.ListTransactions ());
			Console.WriteLine("ListTransactions: " + Bitcoind.ListTransactions ("cb2"));
			Console.WriteLine("ListTransactions: " + Bitcoind.ListTransactions ("cb2", "3"));
			Console.WriteLine("ListTransactions: " + Bitcoind.ListTransactions ("cb2", "3", "3"));

			return true;
		}


		public static bool ListUnspent()
		{
			Console.WriteLine ("Starting ListUnspent tests...");

			Console.WriteLine("ListUnspent: " + Bitcoind.ListUnspent ());
			Console.WriteLine("ListUnspent: " + Bitcoind.ListUnspent (_walletPassPhraseTimeout));
			Console.WriteLine("ListUnspent: " + Bitcoind.ListUnspent (_walletPassPhraseTimeout, "15"));

			return true;
		}

		public static bool ListLockUnspent()
		{
			Console.WriteLine ("Starting ListLockUnspent tests...");

			Console.WriteLine("ListLockUnspent: " + Bitcoind.ListLockUnspent ());

			return true;
		}

		public static bool LockUnspent()
		{
			Console.WriteLine ("Starting LockUnspent tests...");

			Bitcoind.RawTxInput[] inputs1 = new Bitcoind.RawTxInput[1];
			Bitcoind.RawTxInput input1 = new Bitcoind.RawTxInput ();
			input1.txID = "fff17832d351c12476dc3d6f902fd2e0666992135ffe49ac343d9db15dfcddf7";
			input1.vout = "0";
			inputs1 [0] = input1;

			Bitcoind.RawTxInput[] inputs2 = new Bitcoind.RawTxInput[2];
			Bitcoind.RawTxInput input2 = new Bitcoind.RawTxInput ();
			input2.txID = "f7d882aea4204b2dbe2d3941c1ee280c7c34551f005f5e7ef52f4c19cfea3db8";
			input2.vout = "0";
			inputs2 [0] = input1;
			inputs2 [1] = input2;

			Console.WriteLine("LockUnspent: " + Bitcoind.LockUnspent (false));
			Console.WriteLine("LockUnspent: " + Bitcoind.LockUnspent (true));
			Console.WriteLine("LockUnspent: " + Bitcoind.LockUnspent (false, inputs1));
			Console.WriteLine("LockUnspent: " + Bitcoind.LockUnspent (true, inputs1));
			Console.WriteLine("LockUnspent: " + Bitcoind.LockUnspent (false, inputs2));
			Console.WriteLine("LockUnspent: " + Bitcoind.LockUnspent (true, inputs2));

			return true;
		}


		public static bool Move()
		{
			Console.WriteLine ("Starting Move tests...");

			Console.WriteLine("Move: " + Bitcoind.Move ("cust2", "cb2", "1.0"));
			Console.WriteLine("Move: " + Bitcoind.Move ("cust2", "cb2", "0.0123", "3"));
			Console.WriteLine("Move: " + Bitcoind.Move ("cb2", "cust2", "1.0123", "3", "refund"));

			return true;
		}


		public static bool SendFrom()
		{
			Console.WriteLine ("Starting SendFrom tests...");

			Console.WriteLine("SendFrom: " + Bitcoind.SendFrom ("cust2", "msvqm9b8qV3cFLGXDifiEpPunZunwKwDaZ", "1.0"));
			Console.WriteLine("SendFrom: " + Bitcoind.SendFrom ("cust2", "msvqm9b8qV3cFLGXDifiEpPunZunwKwDaZ", "0.0123", "3"));
			Console.WriteLine("SendFrom: " + Bitcoind.SendFrom ("cb2", "n1dPhwHRC8jNwUEJD7jThf4RkFVvcYLqmz", "1.0", "3", "refund"));
			Console.WriteLine("SendFrom: " + Bitcoind.SendFrom ("cb2", "n1dPhwHRC8jNwUEJD7jThf4RkFVvcYLqmz", "0.0123", "3", "refund", "refund2"));

			return true;
		}


		public static bool SendMany()
		{
			Console.WriteLine ("Starting SendMany tests...");

			Bitcoind.PayToAddress[] outputsToCB2 = new Bitcoind.PayToAddress[1];
			Bitcoind.PayToAddress outputToCB2 = new Bitcoind.PayToAddress ();
			outputToCB2.address = "msvqm9b8qV3cFLGXDifiEpPunZunwKwDaZ";
			outputToCB2.amount = "0.1";
			outputsToCB2 [0] = outputToCB2;

			Bitcoind.PayToAddress[] outputsToCust2 = new Bitcoind.PayToAddress[1];
			Bitcoind.PayToAddress outputToCust2 = new Bitcoind.PayToAddress ();
			outputToCust2.address = "n1dPhwHRC8jNwUEJD7jThf4RkFVvcYLqmz";
			outputToCust2.amount = "0.1";
			outputsToCust2 [0] = outputToCust2;

			Bitcoind.PayToAddress[] outputs2 = new Bitcoind.PayToAddress[2];
			outputs2 [0] = outputToCB2;
			outputs2 [1] = outputToCust2;

			Console.WriteLine("SendMany: " + Bitcoind.SendMany ("cust2", outputsToCB2));
			Console.WriteLine("SendMany: " + Bitcoind.SendMany ("cb2", outputsToCust2));
			Console.WriteLine("SendMany: " + Bitcoind.SendMany ("cust2", outputs2, "3"));
			Console.WriteLine("SendMany: " + Bitcoind.SendMany ("cb2", outputs2, "3", "refund"));

			return true;
		}


		public static bool SendRawTransaction()
		{
			Console.WriteLine ("Starting SendRawTransaction tests...");

			///////////////////////////////////////////////////////////////////////////////////////////////// !!!

			return true;
		}


		public static bool SendToAddress()
		{
			Console.WriteLine ("Starting SendToAddress tests...");

			Console.WriteLine("SendToAddress: " + Bitcoind.SendToAddress ("mmx8vJ6F4sBcBuhbEF1UAGevAMmyaSfLxx", "0.1"));
			Console.WriteLine("SendToAddress: " + Bitcoind.SendToAddress ("mmx8vJ6F4sBcBuhbEF1UAGevAMmyaSfLxx", "0.001", "test"));
			Console.WriteLine("SendToAddress: " + Bitcoind.SendToAddress ("mmx8vJ6F4sBcBuhbEF1UAGevAMmyaSfLxx", "0.0001", "test", "test2"));

			return true;
		}


		public static bool SetAccount()
		{
			Console.WriteLine ("Starting SetAccount tests...");

			Bitcoind.SetAccount ("mmx8vJ6F4sBcBuhbEF1UAGevAMmyaSfLxx", "cs3");
			Bitcoind.SetAccount ("mmx8vJ6F4sBcBuhbEF1UAGevAMmyaSfLxx", "cs2");

			return true;
		}


		public static bool SetGenerate()
		{
			Console.WriteLine ("Starting SetGenerate tests...");

			Bitcoind.SetGenerate (true);
			Bitcoind.SetGenerate (false);
			Bitcoind.SetGenerate (true, "1");
			Bitcoind.SetGenerate (false);

			return true;
		}


		public static bool SetTxFee()
		{
			Console.WriteLine ("Starting SetTxFee tests...");

			if (!Bitcoind.SetTxFee ("0.0005"))
				return false;
			if (!Bitcoind.SetTxFee ("0.0001"))
				return false;

			return true;
		}


		public static bool SignMessage()
		{
			Console.WriteLine ("Starting SignMessage tests...");

			_helloSigned = Bitcoind.SignMessage ("mmx8vJ6F4sBcBuhbEF1UAGevAMmyaSfLxx", "Hello world");
			_genesisSigned = Bitcoind.SignMessage ("n1dPhwHRC8jNwUEJD7jThf4RkFVvcYLqmz", "The Times 03/Jan/2009 Chancellor on brink of second bailout for banks");

			Console.WriteLine("SignMessage: " + _helloSigned);
			Console.WriteLine("SignMessage: " + _genesisSigned);

			return true;
		}


		public static bool SignRawTransaction()
		{
			Console.WriteLine ("Starting SignRawTransaction tests...");

			// Note that this depends on CreateRawTransaction having been run.

			// Was originally going to use this, but signrawtransaction works so well with multisig...
//			Bitcoind.RawTxInput[] inputs1 = new Bitcoind.RawTxInput[1];
//			Bitcoind.RawTxInput input1 = new Bitcoind.RawTxInput ();
//			input1.txID = "fff17832d351c12476dc3d6f902fd2e0666992135ffe49ac343d9db15dfcddf7";
//			input1.vout = "0";
//			inputs1 [0] = input1;
//
//			Bitcoind.RawTxInput[] inputs2 = new Bitcoind.RawTxInput[2];
//			Bitcoind.RawTxInput input2 = new Bitcoind.RawTxInput ();
//			input2.txID = "e69899e5bd2020b99c67b258e5d56ca5c091c9b065f412381ce44a9e438721c1";
//			input2.vout = "1";
//			inputs2 [0] = input1;
//			inputs2 [1] = input2;
//
//			string[] inputs2PrivKeys = new string[2];
//			inputs2PrivKeys[0] = "cQ1nxdyfCY19WqoQQctArRga77xsneJPieRzuku4d7rjyTXT6Cmt";
//			inputs2PrivKeys[1] = "cVJpaLpFVceG3WERxShJGs81YhcSunucwd7j1Xh6nRPhMKF6tkAd";

			string multiSigTxHex = "0100000001cd13a17b390f450bdebd546095e9b8cb9e5985285933ab171379372686d807610000000000ffffffff0140420f00000000001976a91488260a05398e905853eb103825538d4380e07a2388ac00000000";
			Bitcoind.RawTxInput[] multiSigInputs = new Bitcoind.RawTxInput[1];
			Bitcoind.RawTxInput multiSigInput = new Bitcoind.RawTxInput ();
			multiSigInput.txID = "6107d8862637791317ab33592885599ecbb8e9956054bdde0b450f397ba113cd";
			multiSigInput.vout = "0";
			multiSigInput.scriptPubKey = "a914bc9fd4193f6bff24df8450d7f326ea8b04bc1cb387";
			multiSigInput.redeemScript = "522103be117fab1593c1f5aa43353df54a0920fc699f089e872f36a40599688f566a472102558f523c7b6c97e10d89348c9b93b7ee7d860f62027f932eb150d4ba6e41c27821032973a0a39b810f1b3b351241e1ebe71ab21159e2068ddc352bb19d1c4f6f3d9553ae";
			multiSigInputs [0] = multiSigInput;

			string[] multiSigPrivKeys = {
				"cToctw27wEp97fUzQ1VHih4qAzyAkp2Nq8MiQYABbnpkm36f6m1F",
				"cUjMgRmsRp9s5fSqXzyjZiTFddxa62hLdeJdWceD4BTm6gQY2geQ"
			};

			string signedTx1To1 = Bitcoind.SignRawTransaction (_rawTx1To1);
			string signedTx1To2 = Bitcoind.SignRawTransaction (multiSigTxHex, multiSigInputs);
			string signedTx2To1 = Bitcoind.SignRawTransaction (multiSigTxHex, multiSigInputs, multiSigPrivKeys);
			string signedTx2To2 = Bitcoind.SignRawTransaction (multiSigTxHex, multiSigInputs, multiSigPrivKeys, Bitcoind.SigHashType.AllOrAnyoneCanPay);

			return true;
		}


		public static bool Stop()
		{
			Console.WriteLine ("Starting Stop tests...");

			// Yeah right...
			//Bitcoind.Stop ();

			return true;
		}


		public static bool SubmitBlock()
		{
			Console.WriteLine ("Starting SubmitBlock tests...");

			//////////////////////////////////////////////////////////////////////////////////////////////////////// !!!

			return true;
		}


		public static bool ValidateAddress()
		{
			Console.WriteLine ("Starting ValidateAddress tests...");

			Console.WriteLine ("ValidateAddress: " + Bitcoind.ValidateAddress ("msvqm9b8qV3cFLGXDifiEpPunZunwKwDaZ"));
			Console.WriteLine ("ValidateAddress: " + Bitcoind.ValidateAddress ("n1dPhwHRC8jNwUEJD7jThf4RkFVvcYLqmz"));

			return true;
		}


		public static bool VerifyMessage()
		{
			Console.WriteLine ("Starting VerifyMessage tests...");

			// Note that this depends on SignMessage having been run...

			Console.WriteLine ("VerifyMessage: " + Bitcoind.VerifyMessage("mmx8vJ6F4sBcBuhbEF1UAGevAMmyaSfLxx", _helloSigned, "Hello world"));
			Console.WriteLine ("VerifyMessage: " + Bitcoind.VerifyMessage("n1dPhwHRC8jNwUEJD7jThf4RkFVvcYLqmz", _genesisSigned, "The Times 03/Jan/2009 Chancellor on brink of second bailout for banks"));

			return true;
		}


		public static bool WalletLock()
		{
			Console.WriteLine ("Starting WalletLock tests...");

			Bitcoind.WalletLock (); // relocks
			Bitcoind.WalletPassPhrase (_walletPassPhrase, _walletPassPhraseTimeout); 

			return true;
		}


		public static bool WalletPassPhrase()
		{
			Console.WriteLine ("Starting WalletPassPhrase tests...");

			//Bitcoind.EncryptWallet (walletPassPhrase); // Locks
			Bitcoind.WalletLock (); // relocks
			Bitcoind.WalletPassPhrase (_walletPassPhrase, _walletPassPhraseTimeout);

			return true;
		}


		public static bool WalletPassPhraseChange()
		{
			Console.WriteLine ("Starting WalletPassPhraseChange tests...");

			Bitcoind.WalletLock (); // relocks
			Bitcoind.WalletPassPhraseChange (_walletPassPhrase, "newPassword"); 
			Bitcoind.WalletPassPhrase ("newPassword", _walletPassPhraseTimeout); 
			Bitcoind.WalletLock (); // relocks
			Bitcoind.WalletPassPhraseChange ("newPassword", _walletPassPhrase);
			Bitcoind.WalletPassPhrase (_walletPassPhrase, _walletPassPhraseTimeout);

			return true;
		}
	}
}

