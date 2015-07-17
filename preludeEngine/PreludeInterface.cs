/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 05.12.2004
 * Time: 11:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using SpeechLib;
using System.Threading;
using System.Timers;
using NLog;

namespace PreludeEngine
{
	/// <summary>
	/// TODO: check speech engine -- if too fast it stops
	/// that never happens when using msagent speechlib
	/// 2.) incorporate speechrecognition
	/// </summary>
	public class PreLudeInterface
	{
		private Mind mindInstance = null;	
		public string loadedMind 	= "mind.mdu";
		public bool isContributable = false;
		public bool isSpeaking      = false;
        public bool quantumRandomness = false;
		private System.Timers.Timer timer 	= null;
		public delegate string AutoSpeakHandler(string boredString);
		public AutoSpeakHandler reportBoredom;
        public PreludeEngine.Mind.MatchingAlgorithm initializedAssociater = Mind.MatchingAlgorithm.Basic;
        private static Logger logger = LogManager.GetCurrentClassLogger();
		
		public void initializeEngine()
		{
			mindInstance = new Mind(loadedMind, false);
			mindInstance.analyzeShortTermMemory();
            mindInstance.associater = initializedAssociater;
		}
		
		public string chatWithPrelude(string input)
		{
			if(mindInstance == null) return "Error: Mind not initialized";

			if(timer != null)
				timer.Stop();
			
            if (quantumRandomness)
                mindInstance.quantumRandomness = true;

            if (avoidLearnByRepeating)
                mindInstance.avoidLearnByRepeating = true;

			string output = "";
			output = mindInstance.listenToInput(input);
			if(isSpeaking)
				speak(output);

			SetTimer();
			return output;	
		}

        /// <summary>
        /// sets the interval to auto save.
        /// </summary>
        private void SetTimer()
        {
			timer = new System.Timers.Timer(300000);
			timer.Elapsed += new System.Timers.ElapsedEventHandler(autoSaving);
			timer.Start();
        }
		
		public void autoSaving(object sender, System.Timers.ElapsedEventArgs e)
		{
            try
            {
				forcedSaveMindFile();
            }
            catch (System.Exception ex)
            {
                ;
            }
		}
	
		
		public void speak(string a)
		{
			if(mindInstance == null) return;
			try
            {
            	SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
                SpVoice speech = new SpVoice();
                if (isSpeaking)
                {
                    speech.Speak(a, SpFlags);    
                }
        	}
        	catch
            {
            	;
            }
		}
		
		public void stopPreludeEngine()
		{
			if(mindInstance == null) return;
			mindInstance.prepareCurrentMemoryForDisc();
			if(isContributable)
			   mindInstance.contributeClientMind();
		}
		
		public void forcedContribution()
		{	
			if(mindInstance == null) return;
			if(isContributable)
			   mindInstance.contributeClientMind();
		}
		//save current mind to disc
		public void forcedSaveMindFile()
		{
			if(mindInstance == null) return;
			mindInstance.prepareCurrentMemoryForDisc();
		}
		
		//save current mind to disc there is another way too!
		public void forcedSaveMindFile(string a)
		{
			if(mindInstance == null) return;
			mindInstance.prepareCurrentMemoryForDisc(a);
		}		
		
		//count currently loaded bot memory
		public int countMindMemory()
		{
			if(mindInstance == null) return -1;
			int i = 0;
			i = mindInstance.memorySize;
			return i;
		}
		
		public string getVersionInfo()
		{
			return "Prelude@# Engine, version 1.2.7, 2004-2015(c) by Lennart Lopin ";
		}
		
		public bool setPreludeClient(string server, int port)
		{
			if(mindInstance != null) return false;
			PreLudeClient.port = port;
			PreLudeClient.server = server;
			return true;
		}

        public bool avoidLearnByRepeating { get; set; }
    }
}
