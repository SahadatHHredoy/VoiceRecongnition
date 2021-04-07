using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace SpeechRecognitionApp
{
    public class QuestionToAnswer
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }
    class Program
    {
        #region Combine Program
        public static List<QuestionToAnswer> questionToAnswers = new List<QuestionToAnswer>();
        public static SpeechSynthesizer ss = new SpeechSynthesizer();
        static void Main(string[] args)
        {

            AssignAnswers();

            // Create an in-process speech recognizer for the en-US locale.  
            using (SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US")))
            {
                //Choices 
                Choices choices = new Choices(questionToAnswers.Select(s => s.Question).ToArray());
                //grammer builder
                GrammarBuilder grammarBuilder = new GrammarBuilder();
                grammarBuilder.Append(choices);
                //Grammer
                Grammar grammar = new Grammar(grammarBuilder);

                // Create and load a dictation grammar.  
                recognizer.LoadGrammarAsync(grammar);

                // Add a handler for the speech recognized event.  
                recognizer.SpeechRecognized +=
                    new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);

                // Configure input to the speech recognizer.  
                recognizer.SetInputToDefaultAudioDevice();

                // Start asynchronous, continuous speech recognition.  
                recognizer.RecognizeAsync(RecognizeMode.Multiple);

                // Keep the console window open.  
                while (true)
                {
                    Console.ReadLine();
                }
            }
        }



        // Handle the SpeechRecognized event.  
        static void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            var question = e.Result.Text.Trim();
            var answer = questionToAnswers.FirstOrDefault(s => s.Question.Contains(question));
            if (answer != null)
            {
                ss.Speak(answer.Answer);
                //Console.WriteLine(answer.Answer);
            }
        }
        #endregion
        private static void AssignAnswers()
        {
           
            questionToAnswers.Add(new QuestionToAnswer() { Question = "hi", Answer = "hello" });
            questionToAnswers.Add(new QuestionToAnswer() { Question = "what's your name", Answer = "homo sapiens" });
            questionToAnswers.Add(new QuestionToAnswer() { Question = "what's about you", Answer = "I am machine" });
        }
    }
}