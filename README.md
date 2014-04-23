MonoBitcoind
============

**A Mono/C# wrapper for the bitcoind functions with examples for each function, following KISS: Keep It Simple, Stupid.**

Summary
-------
Primarily, this is a single class that allows you to use nearly all bitcoind calls from Mono/C# with the benefit of the code and the type system handling all the of the specific syntax for bitcoin core's JSON-RPC interface.  

This allows you to build on top of bitcoind by just including this class.  In addition, examples for every implemented call are provided in another class, making it stupid simple to pick up.

By the end of the month, there will be new features that include simplified hierarchical deterministic extended keys and multisignature transactions.

There are several other good C#/Mono bitcoin implementations on github that you should check out:
`BitcoinLib`, `BitSharp`, and `Bitcoin.NET`.


Please Do Not Make An Exchange With This Code
---------------------------------------------
This code is under heavy development, and digital currency security is a serious thing, so use it at your own risk.  

I'm eager to hear feedback from the core dev community, please email me at Noah@CoinBeyond.com.


Setup
-----

- Download and install Bitcoin Core
- Modify your `bitcoin.conf` file (if you're just starting see: https://en.bitcoin.it/wiki/Running_Bitcoin) and set:
```
rpcuser={your username you will make up}
rpcpassword={your password you will make up}
testnet=1 (if you want to run on the bitcoin testnet, or...)
regtest=1 (if you want to run in regtest mode, which allows you start with a fresh blockchain and mine your own blocks)
server=1
txindex=1 (if you want to be able to lookup any transaction, but this requires running bitcoind with -reindex once)
```
- Start a bitcoind server by running `bitcoind -printtoconsole` (and possibly -reindex) from the bitcoind location (in Windows, this is `C:\Program Files\Bitcoin\daemon`)
- From another command prompt in the same directory, you can run bitcoind manually.  

MonoBitcoind let's you automate processes that you'd have to do by hand in this command prompt.

- Download and install your editor of choice for Mono or C# programs.
- Create or import a project with these committed files.
- Add a reference to `Newtonsoft.Json.dll` that is included in the `libs` folder.
- At the top of `Bitcoind.cs`, change _rpcuser and _rpcpassword to be your username and password that you set in your `bitcoin.conf` file.
- If you run Main is `Program.cs`, it will run through all of the examples.  Whether or not it fails, the console it made will remain open until you close it, due to a sleep call in `Program.cs`.  The examples run-through will undoubtedly fail, as it has certain things unique to my instance of bitcoind, but by debugging it you might learn some more about bitcoind.

I know I did while making this.


License
-------

MonoBitcoind is released under the terms of the MIT license. See [LICENSE](LICENSE) for more information or see http://opensource.org/licenses/MIT.

