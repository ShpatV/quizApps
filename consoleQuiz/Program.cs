using System;
using System.Collections.Generic;
using System.Threading;
using quizConsole.Models;

namespace quizConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Pershendetje nga PolymathQuiz, Ju Lutem Shkruani Emrin Tuaj Per Pjesmarrje Ne Kuiz");
            string emri = GetValidatedName();
            // Krijoni pyetjet
            IEnumerable<Question> questions = GenerateQuestions(); 

            // Krijo participantin
            Participant participant = new Participant
            {
                ParticipantId = 1,
                Name = emri,
                Score = 0,
                TimeTaken = 0
            };

            Console.WriteLine($"Pershendetje {participant.Name}! Miresevini Ne Quiz.");
            Console.WriteLine("Quiz fillon! Përgjigju Me Numrin E Opsionit Të Saktë.");

            DateTime startTime = DateTime.Now;

            bool isTimeUp = false;

            Thread timeThread = new Thread(() =>   //loop i pafund qe kontrollon kohen
            {
                while (true)
                {
                    TimeSpan elapsed = DateTime.Now - startTime;
                    if (elapsed.TotalMinutes > 5)
                    {
                        Console.WriteLine("Keni vonuar mbi 1 minutë në quiz.");
                        isTimeUp = true;
                        Environment.Exit(0);
                        break;
                    }
                    Thread.Sleep(1000); // Prit 1 sekondë para se të kontrollojë përsëri
                }
            });

            // Fillo thread-in e kontrollit të kohës
            timeThread.Start();

            // Përcaktohuni nëpër pyetjet
            foreach (Question question in questions)
            {
                // Kontrollo kohën
                if (isTimeUp)
                    break;

                Console.WriteLine(question.QnInWords);
                Console.WriteLine("1. " + question.Option1);
                Console.WriteLine("2. " + question.Option2);
                Console.WriteLine("3. " + question.Option3);
                Console.WriteLine("4. " + question.Option4);

                int userAnswer = GetUserAnswer();

                if (userAnswer == question.Answer)
                {
                    Console.WriteLine("Përgjigja juaj është e saktë!");
                    participant.Score++;
                }
                else
                {
                    Console.WriteLine("Përgjigja juaj është e gabuar!");
                }
                Console.WriteLine();
            }

            DateTime endTime = DateTime.Now;
            TimeSpan totalTime = endTime - startTime;
            if (!isTimeUp)
            {
                Console.WriteLine("Quiz-i ka përfunduar!");

                Console.WriteLine("Pikët: " + participant.Score);
                Console.WriteLine("Koha e marrë: " + totalTime.Minutes + " minuta");
            }
            else
            {
                Console.WriteLine("Quiz-i ka përfunduar sepse koha ka skaduar!");
            }

            Console.WriteLine("Faleminderit për pjesëmarrjen!");

            // Mbyll aplikacionin
            Environment.Exit(0);
        }

        static IEnumerable<Question> GenerateQuestions()
        {
            // Krijo listën e pyetjeve
            List<Question> questions = new List<Question>
            {
                new Question
                {
                    QuestionId = 1,
                    QnInWords = "Cila Nga Këto Nuk Eshte Një Sistem Operativ?",
                    Option1 = "Windows",
                    Option2 = "Linux",
                    Option3 = "Android",
                    Option4 = "Microsoft Office",
                    Answer = 4
                },
                new Question
                {
                    QuestionId = 2,
                    QnInWords = "Cili Prej Këtyre Nuk Eshtë Një Tip i Sistemeve Operative?",
                    Option1 = "DOS",
                    Option2 = "MacOS",
                    Option3 = "iOS",
                    Option4 = "HTML",
                    Answer = 4
                }
            };

            return questions;
        }

        static int GetUserAnswer()
        {
           
            int userAnswer = 0;
            bool isValidAnswer = false;
            while (!isValidAnswer)
            {
                Console.Write("Përgjigja juaj: ");
                string answerInput = Console.ReadLine();

                bool isNumeric = int.TryParse(answerInput, out userAnswer); //rezultati ruhet te useranswer
                try
                {
                    if (!isNumeric)
                    {
                        Console.WriteLine("Përgjigja juaj duhet të jetë një numër të tërë. Ju lutemi, provoni përsëri.");
                    }
                    else if (userAnswer < 1 || userAnswer > 4)
                    {
                        Console.WriteLine("Përgjigja juaj është e pavlefshme. Ju lutemi, jepni një numër nga 1 deri në 4.");
                    }
                    else
                    {
                        isValidAnswer = true;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Përgjigja juaj duhet të jetë një numër të tërë. Ju lutemi, provoni përsëri.");
                }
            }
            return userAnswer;

            
        }
        static string GetValidatedName()
            {
                string name = string.Empty;
                bool isValidName = false;
                while (!isValidName)
                {
                    Console.Write("Emri juaj: ");
                    name = Console.ReadLine();

                    if (name.Length < 3)
                    {
                        Console.WriteLine("Emri Duhet Të Përmbajë Të Paktën 3 Shkronja.");
                    }
                    else if (name.Any(char.IsDigit))
                    {
                        Console.WriteLine("Emri Nuk Duhet Të Përmbajë Numra.");
                    }
                    else
                    {
                        isValidName = true;
                    }
                }
                return name;
            }
    }
}


