using System;
using NLog;

namespace PreludeEngine
{
	class MainClass
	{
		private static string ind = "";
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public static void Main (string[] args)
		{
			//string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
			//Console.WriteLine("Prelude@# ("+version+") command line version, welcome user!");
			//Console.WriteLine("if you want to stop chatting, enter: 'exit'");
			//initialize interface
			PreLudeInterface pi = new PreLudeInterface();

			//configure prelude
			//define path to mind file
			pi.loadedMind = "mind.mdu";
			//decide whether you want true randomness
			pi.quantumRandomness = false;
			pi.isSpeaking = true;
			pi.avoidLearnByRepeating = true;

			pi.initializedAssociater = Mind.MatchingAlgorithm.Dice;

			//start your engine ...
			pi.initializeEngine();
			//here we go:
			while(true)
			{
				Console.Write("Human: ");
				ind = Console.ReadLine();
				if (ind.Equals("q")) {
					break;
				}
				logger.Trace("Human: " + ind);
				string answer = pi.chatWithPrelude(ind);
				Console.WriteLine("Robot: " + answer);
				logger.Trace("Robot: " + answer);
			}
			pi.stopPreludeEngine();
		}
	}
}