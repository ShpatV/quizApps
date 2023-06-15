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
            // Krijoni pyetjet
            IEnumerable<Question> questions = GenerateQuestions();

            // Krijo participantin
            Participant participant = new Participant
            {
                ParticipantId = 1,
                Name = "Emri i pjesëmarrësit",
                Score = 0,
                TimeTaken = 0
            };

            Console.WriteLine("Quiz fillon! Përgjigju me numrin e opsionit të saktë.");

            DateTime startTime = DateTime.Now;

            bool isTimeUp = false;

            Thread timeThread = new Thread(() =>
            {
                while (true)
                {
                    TimeSpan elapsed = DateTime.Now - startTime;
                    if (elapsed.TotalMinutes > 1)
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
                    QnInWords = "Pyetja 1",
                    Option1 = "Përgjigja 1",
                    Option2 = "Përgjigja 2",
                    Option3 = "Përgjigja 3",
                    Option4 = "Përgjigja 4",
                    Answer = 2
                },
                new Question
                {
                    QuestionId = 2,
                    QnInWords = "Pyetja 2",
                    Option1 = "Përgjigja 1",
                    Option2 = "Përgjigja 2",
                    Option3 = "Përgjigja 3",
                    Option4 = "Përgjigja 4",
                    Answer = 3
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

                bool isNumeric = int.TryParse(answerInput, out userAnswer);
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
    }
}


