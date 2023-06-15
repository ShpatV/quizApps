using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quizConsole.Models
{
    public class Participant
    {
        public int ParticipantId {get; set;}

        public string Name{get; set;}

        public int Score {get; set;}

        public int TimeTaken { get; set; }
    }
}